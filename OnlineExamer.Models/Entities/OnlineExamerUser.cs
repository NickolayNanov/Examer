using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace OnlineExamer.Models.Entities
{
    public class OnlineExamerUser : IdentityUser<string>
    {
        public OnlineExamerUser()
        {

        }

        public OnlineExamerUser(string email, string fullName)
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserName = fullName;

            this.UserExams = new HashSet<UserExam>();
        }

        public double? AverageGrade { get; set; }

        public ICollection<UserExam> UserExams { get; set; }
    }
}
