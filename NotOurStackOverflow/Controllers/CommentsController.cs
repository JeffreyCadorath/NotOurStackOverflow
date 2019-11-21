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

namespace NotOurStackOverflow.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db;
        private DBDataAccess dataAccess;
        private BusinessLogic businessLogic;

        public CommentsController()
        {
            db = new ApplicationDbContext();
            dataAccess = new DBDataAccess(db);
            businessLogic = new BusinessLogic(dataAccess);
        }

        // GET: Comments
        public ActionResult Index()
        {
            var posts = db.Comments.Include(c => c.User).Include(c => c.Post);
            return View(posts.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(int qId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Register", "Account");
            }

            var post = businessLogic.GetPost(qId);
            if(post is Question)
            {
                var question = (Question)post;
                ViewBag.PTitle = question.Title;
            }
            ViewBag.Post = post;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(int qId, [Bind(Include = "Id,Body,PostId")] Comment comment)
        {
            comment.UserId = User.Identity.GetUserId();
            comment.DatePosted = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Posts.Add(comment);
                db.SaveChanges();
                var commentPost = businessLogic.GetPost(comment.PostId);
                if (commentPost is Answer)
                {
                    Answer answer = (Answer)commentPost;
                    answer.Comments.Add(comment);
                }
                else
                {
                    Question question = (Question)commentPost;
                    question.Comments.Add(comment);
                }
                db.SaveChanges();
                if(comment.Post is Answer)
                {
                    Answer answer = (Answer)comment.Post;

                    return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
                } else
                {
                    return RedirectToAction("Details", "Questions", new { id = comment.PostId });
                }
            }

            var post = businessLogic.GetPost(qId);
            if (post is Question)
            {
                var question = (Question)post;
                ViewBag.PTitle = question.Title;
            }
            ViewBag.Post = post;
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Include(x => x.Votes).FirstOrDefault(p => p.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", comment.UserId);
            ViewBag.PostId = new SelectList(db.Posts, "Id", "UserId", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,DatePosted,Body,PostId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", comment.UserId);
            ViewBag.PostId = new SelectList(db.Posts, "Id", "UserId", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Posts.Remove(comment);
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
