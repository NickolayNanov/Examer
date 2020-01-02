namespace OnlineExamer.Core
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using OnlineExamer.Models.Entities;


    public class UserRolesSeeder
    {
        private readonly UserManager<OnlineExamerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesSeeder(UserManager<OnlineExamerUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (!_userManager.Users.Any() && !_roleManager.Roles.Any())
            {
                IdentityRole adminRole = new IdentityRole("Admin");
                IdentityRole userRole = new IdentityRole("User");

                await _roleManager.CreateAsync(adminRole);
                await _roleManager.CreateAsync(userRole);

                OnlineExamerUser admin = new OnlineExamerUser("nickolaynanov17@gmail.com");
                admin.UserName = "admin";

                await _userManager.CreateAsync(admin, "fr3s7ed23");
                await _userManager.AddToRoleAsync(admin, "Admin");
            } 
        }
    }
}
