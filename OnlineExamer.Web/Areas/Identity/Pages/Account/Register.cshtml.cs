using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OnlineExamer.Models.Entities;

namespace OnlineExamer.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<OnlineExamerUser> _signInManager;
        private readonly UserManager<OnlineExamerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<OnlineExamerUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<OnlineExamerUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Е-Поща")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Потребителско име")]
            public string Username { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} трябва да бъде най-малко {2} и най-много {1} символа.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърди парола")]
            [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!_userManager.Users.Any() && !_roleManager.Roles.Any())
            {
                await Seed();
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        private async Task Seed()
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

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new OnlineExamerUser { UserName = Input.Username, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, "User");
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
