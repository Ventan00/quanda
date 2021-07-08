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
        }

        public int IdUser { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public int Points { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public string Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationDate { get; set; }
        public int? IdService { get; set; }
        public string ServiceToken { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<RatingAnswer> RatingAnswers { get; set; }
        public virtual Service IdServiceNavigation { get; set; }
        public virtual TempUser IdTempUserNavigation { get; set; }
    }
}
