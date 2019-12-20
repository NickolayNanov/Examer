namespace OnlineExamer.Models.Entities
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class OnlineExamerUser : IdentityUser
    {
        public OnlineExamerUser()
        {
        }

        public OnlineExamerUser(string email)
        {
            this.Email = email;

            this.UserExams = new HashSet<UserExam>();
        }

        public double? AverageGrade { get; set; }

        public ICollection<UserExam> UserExams { get; set; }
    }
}
