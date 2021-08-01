using System;
using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            Questions = new HashSet<Question>();
            Notifications = new HashSet<Notification>();
            Answers = new HashSet<Answer>();
            RatingAnswers = new HashSet<RatingAnswer>();
            ReceiverThreads = new HashSet<Thread>();
            SenderThreads = new HashSet<Thread>();
            TagUsers = new HashSet<TagUser>();
            Reports = new HashSet<Report>();
            Messages = new HashSet<Message>();
        }

        public int IdUser { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public string Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationDate { get; set; }
        public int? IdService { get; set; }
        public string ServiceToken { get; set; }
        public bool IsDarkModeOff { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<RatingAnswer> RatingAnswers { get; set; }
        public virtual ICollection<Thread> ReceiverThreads { get; set; }
        public virtual ICollection<Thread> SenderThreads { get; set; }
        public virtual ICollection<TagUser> TagUsers { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public virtual Service IdServiceNavigation { get; set; }
        public virtual TempUser IdTempUserNavigation { get; set; }
        public virtual RecoveryUser IdRecoveryUserNavigation { get; set; }
    }
}
