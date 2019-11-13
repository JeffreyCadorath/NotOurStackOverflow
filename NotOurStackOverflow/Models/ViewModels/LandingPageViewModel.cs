using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotOurStackOverflow.Models.ViewModels
{
    public class LandingPageViewModel
    {
        public ApplicationUser CurrentUser { get; set; }
        public ICollection<Question> AllQuestions { get; set; }
        public ICollection<Question> CurrentUserQuestions { get; set; }
        public int Page { get; set; }

        public LandingPageViewModel()
        {

        }
    }
}