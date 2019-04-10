using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EcoloWeb.Data.Entity.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EcoloWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;


        //public string Validar1 { get; set; }





        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {

            [Required]
            [StringLength(60, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Nombre")]
            public string Name { get; set; }

            [Required]
            [StringLength(60, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Apellidos")]
            public string LastName { get; set; }

            [Display(Name = "Fecha Nacimiento")]
            public DateTime DOB { get; set; }

            [Required]
            [Display(Name = "Telefono")]
            public string Telefono { get; set; }


            public string TipoRegistro { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet([FromQuery] string tipoRegistro, string returnUrl = null)
        {
            ViewData["tipoRegistro"] = tipoRegistro;

            //La primera vez que carga si toma el valor 

            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync(string tipoRegistro, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");


            var esInstitucional = (tipoRegistro == "Institucional");


            if (ModelState.IsValid)
            {


                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Name = Input.Name, Lastname = Input.LastName, PhoneNumber = Input.Telefono, DOB = Input.DOB };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");



                    switch (tipoRegistro)
                    {

                        case "Institucional":
                            esInstitucional = true; await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Institucional", $"{esInstitucional} "));
                            break;

                        default:
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Personal", $"{esInstitucional} "));
                            break;
                    }


                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", $"{user.Name} {user.Lastname}"));
                    

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
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
