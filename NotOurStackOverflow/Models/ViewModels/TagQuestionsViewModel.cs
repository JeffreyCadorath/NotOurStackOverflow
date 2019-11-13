using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotOurStackOverflow.Models.ViewModels
{
    public class TagQuestionsViewModel
    {
        public ICollection<Question> AllQuestions { get; set; }

        public TagQuestionsViewModel()
        {

        }
    }
}