namespace NotOurStackOverflow.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using NotOurStackOverflow.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NotOurStackOverflow.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NotOurStackOverflow.Models.ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);

            if (!context.Users.Any(u => u.Email == "Ted@email.com"))
            {
                //Created Users
                var Ted = new ApplicationUser()
                {
                    UserName = "TedDansen",
                    Email = "Ted@email.com"
                };
                var Kyle = new ApplicationUser()
                {
                    UserName = "KlyeElyk",
                    Email = "Instructor1@email.com"
                };
                var Stan = new ApplicationUser()
                {
                    UserName = "StanDarsh",
                    Email = "Instructor2@email.com"
                };
                var BigDan = new ApplicationUser()
                {
                    UserName = "DanBiglittle",
                    Email = "Employer1@email.com"
                };
                var LittleDan = new ApplicationUser()
                {
                    UserName = "DanLittlebig",
                    Email = "Employer2@email.com"
                };

                //Seeded Users
                manager.Create(Ted, "Password1!");
                manager.Create(Kyle, "Password1!");
                manager.Create(Stan, "Password1!");
                manager.Create(BigDan, "Password1!");
                manager.Create(LittleDan, "Password1!");

                //Created Tags
                var Tag1 = new Tag
                {
                    Title = "C++",
                    Id = 1,
                };
                var Tag2 = new Tag
                {
                    Title = "Ears",
                    Id = 2,
                }; 
                var Tag3 = new Tag
                {
                    Title = "Animals",
                    Id = 3,
                };

                //Seeded Tags
                context.Tags.Add(Tag1);
                context.Tags.Add(Tag2);
                context.Tags.Add(Tag3);

                //Created Questions
                var Question1 = new Question
                {
                    Title = "WE MUST STOP THE DOGS",
                    Body = "Find All The Dogs With The Biggest Ears",
                    DatePosted = DateTime.Now,
                    User = Stan,
                    UserId = Stan.Id,
                    Id = 1
                };
                var Question2 = new Question
                {
                    Title = "Do Rabbits really need those big of ears",
                    Body = "Why is it that Rabbits have ears so big if they can't even hear me coming from 10ft away",
                    DatePosted = DateTime.Now,
                    User = Ted,
                    UserId = Ted.Id,
                    Id = 2
                };
                var Question3 = new Question
                {
                    Title = "Birds, Where are their ears?",
                    Body = "I feel like Birds have great hearing but have you ever seen their ears?",
                    DatePosted = DateTime.Now,
                    User = Kyle,
                    UserId = Kyle.Id,
                    Id = 3
                };
                var Question4 = new Question
                {
                    Title = "Do you think Fish have ears?",
                    Body = "Fish must have ears but when I was looking I couldn't find them anywhere",
                    DatePosted = DateTime.Now,
                    User = LittleDan,
                    UserId = LittleDan.Id,
                    Id = 4
                    
                };
                var Question5 = new Question
                {
                    Title = "Reptiles are not normal but they must have ears right?",
                    Body = "I've encountered sooo many different types of reptiles but I cant find their ears anywhere",
                    DatePosted = DateTime.Now,
                    User = BigDan,
                    UserId = BigDan.Id,
                    Id = 5
                };

                //Add Tags to Questions
                Question1.Tags.Add(Tag1);
                Question2.Tags.Add(Tag2);
                Question3.Tags.Add(Tag3);
                Question4.Tags.Add(Tag1);
                Question5.Tags.Add(Tag2);

                //Seeded Questions
                context.Questions.AddOrUpdate(Question1);
                context.Questions.AddOrUpdate(Question2);
                context.Questions.AddOrUpdate(Question3);
                context.Questions.AddOrUpdate(Question4);
                context.Questions.AddOrUpdate(Question5);

                //Created Answers
                var Answer1 = new Answer()
                {
                    Question = Question1,
                    QuestionId = Question1.Id,
                    Body = "The Dog with the biggest Ears was a Coonhound, named Harbor. But Inherently Basset Hounds have the biggest ears",
                    DatePosted = DateTime.Now,
                    User = Kyle,
                    UserId = Kyle.Id,
                    Id = 1,
                };
                var Answer2 = new Answer()
                {
                    Question = Question2,
                    QuestionId = Question2.Id,
                    Body = "A rabbit's ears contain an extensive network of blood vessels that provide a large surface area for heat exchange. These vessels swell (vasodilation) when the rabbit is hot, and contract (vasoconstriction) when it is cool, so much so that they are barely visible in cold weather.",
                    DatePosted = DateTime.Now,
                    User = Stan,
                    UserId = Stan.Id,
                    Id = 2,
                };
                var Answer3 = new Answer()
                {
                    Question = Question3,
                    QuestionId = Question3.Id,
                    Body = "On most birds, the ears are covered by barbless feathers that protect them from turbulence while flying but still allow the bird to hear. Like human ears, bird ears have three sections: the outer ear, the middle ear, and the inner ear.",
                    DatePosted = DateTime.Now,
                    User = BigDan,
                    UserId = BigDan.Id,
                    Id = 3,
                };
                var Answer4 = new Answer()
                {
                    Question = Question4,
                    QuestionId = Question4.Id,
                    Body = "Fish don't have ears that we can see, but they do have ear parts inside their heads. They pick up sounds in the water through their bodies and in their internal ear, according to the National Wildlife Federation.",
                    DatePosted = DateTime.Now,
                    User = Ted,
                    UserId = Ted.Id,
                    Id = 4,
                };
                var Answer5 = new Answer()
                {
                    Question = Question5,
                    QuestionId = Question5.Id,
                    Body = "Lizards have middle and inner ears; the tympanic membrane covers the middle ear cavity which is linked to the pharynx and eustachian tube. In general, the inner boundary of the middle ear cavity has two openings: a round one, covered by a thin membrane, and an oval opening that is uncovered.",
                    DatePosted = DateTime.Now,
                    User = LittleDan,
                    UserId = LittleDan.Id,
                    Id = 5,
                };

                //Seeded Answers
                context.Answers.AddOrUpdate(Answer1);
                context.Answers.AddOrUpdate(Answer2);
                context.Answers.AddOrUpdate(Answer3);
                context.Answers.AddOrUpdate(Answer4);
                context.Answers.AddOrUpdate(Answer5);

                //Created Comments
                var comment1 = new Comment
                {
                    Body = "WHO CARES THIS MUCH ABOUT DOG EARS",
                    User = LittleDan,
                    UserId = LittleDan.Id,
                    Post = Question1,
                    PostId = Question1.Id,
                    DatePosted = DateTime.Now,
                    Id = 1
                };
                var comment2 = new Comment
                {
                    Body = "WHO CARES THIS MUCH ABOUT RABBIT EARS",
                    User = Kyle,
                    UserId = Kyle.Id,
                    Post = Question2,
                    PostId = Question2.Id,
                    DatePosted = DateTime.Now,
                    Id = 2
                };
                var comment3 = new Comment
                {
                    Body = "WHO CARES THIS MUCH ABOUT BIRD EARS",
                    User = BigDan,
                    UserId = BigDan.Id,
                    Post = Question3,
                    PostId = Question3.Id,
                    DatePosted = DateTime.Now,
                    Id = 3
                };
                var comment4 = new Comment
                {
                    Body = "WHO CARES THIS MUCH ABOUT FISH EARS",
                    User = Kyle,
                    UserId = Kyle.Id,
                    Post = Question4,
                    PostId = Question4.Id,
                    DatePosted = DateTime.Now,
                    Id = 4
                };
                var comment5 = new Comment
                {
                    Body = "WHO CARES THIS MUCH ABOUT REPTILE EARS",
                    User = Stan,
                    UserId = Stan.Id,
                    Post = Question5,
                    PostId = Question5.Id,
                    DatePosted = DateTime.Now,
                    Id = 5
                };

                //Seeded Comments
                context.Comments.AddOrUpdate(comment1);
                context.Comments.AddOrUpdate(comment2);
                context.Comments.AddOrUpdate(comment3);
                context.Comments.AddOrUpdate(comment4);
                context.Comments.AddOrUpdate(comment5);
            }
        }
    }
}
