using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotOurStackOverflow;
using NotOurStackOverflow.Controllers;
using NotOurStackOverflow.Models;

namespace NotOurStackOverflow.Tests.Controllers
{
    [TestClass]
    public class NonQueryTests
    {
        [TestInitialize]
        public void Setup()
        {
            var mockUserSet = new Mock<DbSet<ApplicationUser>>();
            var mockQuestionSet = new Mock<DbSet<Question>>();
            var mockCommentSet = new Mock<DbSet<Comment>>();
            var mockAnswerSet = new Mock<DbSet<Answer>>();
            var mockTagSet = new Mock<DbSet<Tag>>();
            var mockVoteSet = new Mock<DbSet<Vote>>();

            var mockContext = new Mock<DbContext>();

            // new setup will need to be added here depending on what we're looking to have returned
        }
    }
}
