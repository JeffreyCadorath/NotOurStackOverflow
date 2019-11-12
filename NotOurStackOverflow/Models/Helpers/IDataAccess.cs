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
            throw new NotImplementedException();
        }

        public ICollection<Question> GetAllUserQuestions(string Id)
        {
            return dbContext.Questions.Where(q => q.UserId == Id).ToList();
        }
    }
}