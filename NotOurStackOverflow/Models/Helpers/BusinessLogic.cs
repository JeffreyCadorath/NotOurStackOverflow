using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotOurStackOverflow.Models.Helpers
{
    public class BusinessLogic
    {
        IDataAccess dataAccess;

        public BusinessLogic(IDataAccess injectedAccess)
        {
            this.dataAccess = injectedAccess;
        }

        public List<Question> AllUserQuestions(string Id)
        {
            var questions = dataAccess.GetAllUserQuestions(Id).ToList();
            return questions;
        }

        public List<Question> GetQuestionsThatArentUsers(string Id)
        {
            var questions = dataAccess.GetQuestionsThatArentUsers(Id).ToList();
            return questions;
        }
        public List<Question> AllQuestions()
        {
            var allQuestions = dataAccess.GetAllQuestions().ToList();
            return allQuestions;
        }

        public int TabulateReputation(string id)
        {
            ApplicationUser user = dataAccess.GetUser(id);

            int positives = user.VoteRecieved.Where(v => v.IsUpVote).Count();
            int negatives = user.VoteRecieved.Where(v => !v.IsUpVote).Count();

            return (positives * 5) - (negatives * 5);
        }
    }
}