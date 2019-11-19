using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NotOurStackOverflow.Models;
using NotOurStackOverflow.Models.Helpers;
using NotOurStackOverflow.Models.ViewModels;

namespace NotOurStackOverflow.Controllers
{
    public class TagsController : Controller
    {
        private ApplicationDbContext db;
        private DBDataAccess dataAccess;
        private BusinessLogic businessLogic;

        public TagsController()
        {
            db = new ApplicationDbContext();
            dataAccess = new DBDataAccess(db);
            businessLogic = new BusinessLogic(dataAccess);
        }
        // GET: Tags
        public ActionResult Index()
        {
            return View(db.Tags.ToList());
        }

        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tag);
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
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

        public ActionResult TagsQuestions(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tag = db.Tags.Find(Id);
            if (tag == null)
            {
                return HttpNotFound();
            }

            var QuestionList = db.Questions.Include("Votes").Where(x => x.Tags.Any(i => i.Id == tag.Id)).ToList();

            TagQuestionsViewModel tagQuestionsViewModel = new TagQuestionsViewModel
            {
                AllQuestions = QuestionList,
                PageTag = tag,
            };
            return View(tagQuestionsViewModel);
        }

        [HttpPost]
        public ActionResult TagsQuestions(int postId, bool isPositive, int currentTagId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Register", "Account");
            }


            Post post = db.Posts.Include(x => x.Votes).FirstOrDefault(p => p.Id == postId);
            ApplicationUser votingUser = db.Users.Find(User.Identity.GetUserId());
            ApplicationUser votedUser = db.Users.Find(post.UserId);
            Tag currentTag = db.Tags.Find(currentTagId);

            if (post == null || votingUser == null || votedUser == null || currentTag == null)
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

            if (negatingVote != null)
            {
                db.Votes.Remove(negatingVote);
                db.SaveChanges();
            }

            votedUser.Reputation = businessLogic.TabulateReputation(votedUser.Id);
            db.SaveChanges();
            return RedirectToAction("TagsQuestions", new { Id = currentTag.Id });
        }
    }
}
