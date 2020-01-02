using Microsoft.AspNetCore.Identity;
using System;

namespace OnlineExamer.Data.Seeding
{
    public class RolesSeeder
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesSeeder(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public RolesSeeder()
        {

        }   
    }
}
