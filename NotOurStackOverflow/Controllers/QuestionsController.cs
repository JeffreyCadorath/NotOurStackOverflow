using Microsoft.AspNet.Identity;
using NotOurStackOverflow.Models;
using NotOurStackOverflow.Models.Helpers;
using NotOurStackOverflow.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NotOurStackOverflow.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db;
        private DBDataAccess dataAccess;
        private BusinessLogic businessLogic;

        public QuestionsController()
        {
            db = new ApplicationDbContext();
            dataAccess = new DBDataAccess(db);
            businessLogic = new BusinessLogic(dataAccess);
        }

        // GET: Questions
        public ActionResult Index()
        {
            var posts = db.Questions.Include(q => q.User);
            return View(posts.ToList());
        }

        // add a GET to alter the ordering
        public ActionResult LandingPage(int? page, string sort)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            List<Question> usersQuestions = new List<Question>();
            List<Question> allQuestions = new List<Question>();

            if (user != null)
            {
                usersQuestions = businessLogic.AllUserQuestions(user.Id);
                allQuestions = businessLogic.GetQuestionsThatArentUsers(user.Id);
            }
            else
            {
                allQuestions = businessLogic.AllQuestions();
            }
            int viewPage = page == null ? 1 : (int)page;
            string Sort = sort == null ? "new" : sort;
            if (Sort == "Popular")
            {
                allQuestions = allQuestions.OrderByDescending(q => q.Answers.Count + q.Comments.Count).ToList();
            }
            else if (Sort == "OfDay")
            {
                var today = DateTime.Today;

                allQuestions = allQuestions.OrderByDescending(
                    q => q.Answers.Where(a => a.DatePosted.Value.Date == today).ToList().Count +
                    q.Comments.Where(c => c.DatePosted.Value.Date == today).ToList().Count).ToList();
            }
            else
            {
                allQuestions = allQuestions.OrderByDescending(q => q.DatePosted.Value).ToList();
            }
            ViewBag.AllQNum = allQuestions.Count;
            LandingPageViewModel viewModel = new LandingPageViewModel
            {
                CurrentUser = user,
                AllQuestions = allQuestions.Skip((viewPage - 1) * 10).Take(10).ToList(),
                CurrentUserQuestions = usersQuestions,
                Page = viewPage,
                sortMethod = Sort,
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult LandingPage(int postId, bool isPositive)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Register", "Account");
            }

            Post post = db.Posts.Include(x => x.Votes).FirstOrDefault(p => p.Id == postId);
            ApplicationUser votingUser = db.Users.Find(User.Identity.GetUserId());
            ApplicationUser votedUser = db.Users.Find(post.UserId);

            if (post == null || votingUser == null || votedUser == null)
            {
                return HttpNotFound();
            }
            
            Vote newVote = new Vote
            {
                IsUpVote = isPositive,
                Post = post,
                PostId = post.Id,
                VotingUser = votingUser,
                VotingUserId = votingUser.Id,
                PostUser = votedUser,
                PostUserId = votedUser.Id,
            };

            var negatingVote = post.Votes.Where(v => v.VotingUserId == votingUser.Id && v.IsUpVote != newVote.IsUpVote).FirstOrDefault();
            db.Votes.Add(newVote);
            db.SaveChanges();

            post.Votes.Add(newVote);
            votingUser.VoteMade.Add(newVote);
            votedUser.VoteRecieved.Add(newVote);
            db.SaveChanges();

            votedUser.Reputation = businessLogic.TabulateReputation(votedUser.Id);
            db.SaveChanges();

            if (negatingVote != null)
            {
                db.Votes.Remove(negatingVote);
                db.SaveChanges();
            }

            return RedirectToAction("LandingPage");
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        [HttpPost]
        public ActionResult Details(int? id, int postId, bool isPositive)
        {
            // create a vote the the accepted parameters
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Register", "Account");
            }

            Question currentQuestion = db.Questions.Find(id);

            Post post = db.Posts.Include(x => x.Votes).FirstOrDefault(p => p.Id == postId);
            ApplicationUser votingUser = db.Users.Find(User.Identity.GetUserId());
            ApplicationUser votedUser = db.Users.Find(post.UserId);

            if (post == null || votingUser == null || votedUser == null)
            {
                return HttpNotFound();
            }

            Vote newVote = new Vote
            {
                IsUpVote = isPositive,
                Post = post,
                PostId = post.Id,
                VotingUser = votingUser,
                VotingUserId = votingUser.Id,
                PostUser = votedUser,
                PostUserId = votedUser.Id,
            };

            var negatingVote = post.Votes.Where(v => v.VotingUserId == votingUser.Id && v.IsUpVote != newVote.IsUpVote).FirstOrDefault();
            db.Votes.Add(newVote);
            db.SaveChanges();

            post.Votes.Add(newVote);
            votingUser.VoteMade.Add(newVote);
            votedUser.VoteRecieved.Add(newVote);
            db.SaveChanges();

            votedUser.Reputation = businessLogic.TabulateReputation(votedUser.Id);
            db.SaveChanges();

            if (negatingVote != null)
            {
                db.Votes.Remove(negatingVote);
                db.SaveChanges();
            }

            return RedirectToAction("Details", currentQuestion);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (user == null)
            {
                return RedirectToAction("Register", "Account");
            }

            ViewBag.TagId = new MultiSelectList(db.Tags, "Id", "Title");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<int> TagId, string body, string Title)
        {
            Question question = new Question
            {
                Body = body,
                Tags = new List<Tag>(),
                Title = Title,
            };
            foreach (var tagId in TagId)
            {
                question.Tags.Add(db.Tags.Find(tagId));
            }
            question.UserId = User.Identity.GetUserId();
            question.DatePosted = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Posts.Add(question);
                db.SaveChanges();
                return RedirectToAction("LandingPage");
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", question.UserId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,DatePosted,Body,Title")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", question.UserId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Posts.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
