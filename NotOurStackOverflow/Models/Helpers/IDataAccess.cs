using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotOurStackOverflow.Models.Helpers
{
    public interface IDataAccess
    {
        ICollection<Question> GetAllQuestions();
        ICollection<Question> GetAllUserQuestions(string Id);
        ICollection<Question> GetQuestionsThatArentUsers(string Id);
        ICollection<Comment> GetAllComments();
        ICollection<Comment> GetAllUserComments(string Id);
        ICollection<Answer> GetAllAnswers();
        ICollection<Answer> GetAllUserAnswers(string Id);
        ApplicationUser GetUser(string id);

    }

    public class DBDataAccess : IDataAccess
    {
        ApplicationDbContext dbContext;

        public ApplicationUser GetUser(string id)
        {
            return dbContext.Users.Find(id);
        }
        public DBDataAccess(ApplicationDbContext db)
        {
            dbContext = db;   
        }

        public ICollection<Question> GetAllQuestions()
        {
            return dbContext.Questions.Include("Votes").ToList();
        }
        public ICollection<Question> GetAllUserQuestions(string Id)
        {
            return dbContext.Questions.Include("Votes").Where(q => q.UserId == Id).ToList();
        }
        public ICollection<Question> GetQuestionsThatArentUsers(string Id)
        {
            return dbContext.Questions.Include("Votes").Where(q => q.UserId != Id).ToList();
        }
        public ICollection<Comment> GetAllComments()
        {
            return dbContext.Comments.Include("Votes").ToList();
        }

        public ICollection<Comment> GetAllUserComments(string Id)
        {
            return dbContext.Comments.Include("Votes").Where(c => c.UserId == Id).ToList();
        }
        public ICollection<Answer> GetAllAnswers()
        {
            return dbContext.Answers.Include("Votes").ToList();
        }

        public ICollection<Answer> GetAllUserAnswers(string Id)
        {
            return dbContext.Answers.Include("Votes").Where(a => a.UserId == Id).ToList();
        }
    }
}