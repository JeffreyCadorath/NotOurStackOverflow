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
            return dataAccess.GetAllUserQuestions(Id).ToList();
        }
    }
}