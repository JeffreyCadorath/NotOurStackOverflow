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
    public class AnswersController : Controller
    {
        private ApplicationDbContext db;
        private DBDataAccess dataAccess;
        private BusinessLogic businessLogic;

        public AnswersController()
        {
            db = new ApplicationDbContext();
            dataAccess = new DBDataAccess(db);
            businessLogic = new BusinessLogic(dataAccess);
        }

        // GET: Answers
        public ActionResult Index()
        {
            var posts = db.Answers.Include(a => a.User).Include(a => a.Question);
            return View(posts.ToList());
        }

        // GET: Answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: Answers/Create
        public ActionResult Create(int qId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Register", "Account");
            }

            ViewBag.Question = businessLogic.GetQuestion(qId);
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(int qId, [Bind(Include = "Id,Body,QuestionId")] Answer answer)
        {
            answer.UserId = User.Identity.GetUserId();
            answer.DatePosted = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Posts.Add(answer);
                db.SaveChanges();
                var question = businessLogic.GetQuestion(answer.QuestionId);
                question.Answers.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
            }

            ViewBag.Question = businessLogic.GetQuestion(qId);
            return View(answer);
        }

        // GET: Answers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", answer.UserId);
            ViewBag.QuestionId = new SelectList(db.Posts, "Id", "UserId", answer.QuestionId);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,DatePosted,Body,QuestionId")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", answer.UserId);
            ViewBag.QuestionId = new SelectList(db.Posts, "Id", "UserId", answer.QuestionId);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answers.Find(id);
            db.Posts.Remove(answer);
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
