using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace OnlineExamer.Models.Entities
{
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
