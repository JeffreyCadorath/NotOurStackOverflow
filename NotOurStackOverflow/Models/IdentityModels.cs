using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NotOurStackOverflow.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty("VotingUser")]
        public virtual ICollection<Vote> VoteMade { get; set; }
        [InverseProperty("PostUser")]
        public virtual ICollection<Vote> VoteRecieved { get; set; }
        public int Reputation { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public abstract class Post
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime? DatePosted { get; set; }
        public string Body { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }

    public class Question : Post
    {
        public Question()
        {
            Tags = new HashSet<Tag>();
            Answers = new HashSet<Answer>();
            Comments = new HashSet<Comment>();
        }
        public string Title { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
    public class Tag
    {
        public Tag()
        {
            Questions = new HashSet<Question>();
        }
        public int Id { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public string Title { get; set; }
    }
    public class Comment : Post
    {
        public virtual Post Post { get; set; }
        public int PostId { get; set; }
    }
    public class Answer : Post
    {
        public Answer()
        {
            Comments = new HashSet<Comment>();
        }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Question Question { get; set; }
        public int QuestionId { get; set; }
    }
    public class Vote
    {
        public int Id { get; set; }
        public virtual ApplicationUser VotingUser { get; set; }
        [ForeignKey("VotingUser")]
        public string VotingUserId { get; set; }
        public virtual ApplicationUser PostUser { get; set; }
        [ForeignKey("PostUser")]
        public string PostUserId { get; set; }
        public bool IsUpVote { get; set; }
        public virtual Post Post { get; set; }
        public int PostId { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<NotOurStackOverflow.Models.Post> Posts { get; set; }
    }
}