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
        private readonly char[] forbidenUsernameCharacters = { '.', ',', '!', '?', '@', '#', '$', '%', '^', '&', '*', '(', ')', '=', '+', '_', '-', ';' };

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
            [Required(ErrorMessage = "Е-Пощата е задължителна!")]
            [EmailAddress]
            [Display(Name = "Е-Поща")]
            public string Email { get; set; }

            [Display(Name = "Потребителско име")]
            [Required(ErrorMessage = "Потребителското име е задължително!")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Паролата е задължителна!")]
            [StringLength(100, ErrorMessage = "{0} трябва да бъде най-малко {2} и най-много {1} символа.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърди парола")]
            [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
            [Required(ErrorMessage = "Това поле е задължително!")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (Input.Username.Any(x => forbidenUsernameCharacters.Contains(x)))
                {
                    ModelState.AddModelError("Невалидно потребителско име", "Потребителското име може да съдържа само латински букви, цифри");
                    return Page();
                }

                var user = new OnlineExamerUser { UserName = Input.Username, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, "User");
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
