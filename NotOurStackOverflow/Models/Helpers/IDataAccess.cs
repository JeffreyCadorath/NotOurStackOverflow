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
        ICollection<Comment> GetAllComments();
        ICollection<Comment> GetAllUserComments(string Id);
        ICollection<Answer> GetAllAnswers();
        ICollection<Answer> GetAllUserAnswers(string Id);

    }

    public class DBDataAccess : IDataAccess
    {
        ApplicationDbContext dbContext;
        public DBDataAccess(ApplicationDbContext db)
        {
            dbContext = db;   
        }

        public ICollection<Question> GetAllQuestions()
        {
            return dbContext.Questions.ToList();
        }

        public ICollection<Question> GetAllUserQuestions(string Id)
        {
            return dbContext.Questions.Where(q => q.UserId == Id).ToList();
        }
        public ICollection<Comment> GetAllComments()
        {
            return dbContext.Comments.ToList();
        }

        public ICollection<Comment> GetAllUserComments(string Id)
        {
            return dbContext.Comments.Where(c => c.UserId == Id).ToList();
        }
        public ICollection<Answer> GetAllAnswers()
        {
            return dbContext.Answers.ToList();
        }

        public ICollection<Answer> GetAllUserAnswers(string Id)
        {
            return dbContext.Answers.Where(a => a.UserId == Id).ToList();
        }
    }
}