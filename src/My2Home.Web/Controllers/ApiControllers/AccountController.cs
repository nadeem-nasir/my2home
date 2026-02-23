//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using My2Home.Core.Interfaces;
using My2Home.Web.Identity;
using My2Home.Web.Services;
using My2Home.Web.ViewModels.Account;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using entity = My2Home.Core.Entities;
using dto = My2Home.Web.ApiModels;
using My2Home.Core.CommonEnums;
using System.Web;

namespace My2Home.Web.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseApiController<AccountController>
    {
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
       // private readonly ISmsSender _smsSender;
        IServiceScopeFactory _scopeFactory;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IHostelRepository _hostelRepository;

        public AccountController(SignInManager<ApplicationIdentityUser> signInManager,
            IEmailSender emailSender,
            //ISmsSender smsSender,
            RoleManager<IdentityRole> roleManager,
            IServiceScopeFactory scopeFactory,
            IOrganizationRepository organizationRepository,
            IHostelRepository hostelRepository,
            IOrganizationUserRepository organizationUserRepository)
        {
            _signInManager = signInManager;
            _emailSender = emailSender;
            //_smsSender = smsSender;
            _roleManager = roleManager;
            _scopeFactory = scopeFactory;

            _organizationRepository = organizationRepository;
            _hostelRepository = hostelRepository;

            _organizationUserRepository= organizationUserRepository;
        }

        //
       // GET: /Account/Login
       [HttpGet]
       [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            //await CreateRoles();
            // ViewData["ReturnUrl"] = returnUrl;
            //return View();
            return Ok();
        }

        //
        // POST: /Account/Login
        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
           
            //EnsureDatabaseCreated(_applicationDbContext);
            //ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    return Ok("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Ok(model);
                }
            }
            // If we got this far, something failed, redisplay form
            return Ok(model);
        }

        //
        // GET: /Account/Register
        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            return Ok();
        }




        //
        // POST: /Account/Register
        [HttpPost("register")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            /*
            using (var scope = _scopeFactory.CreateScope())
            {
                AddOpenIdConnectOptions(scope, CancellationToken.None).GetAwaiter().GetResult();
            }
                await CreateRoles();
            */
            
            
            //EnsureDatabaseCreated(_applicationDbContext);
            //ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationIdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    CountryId = model.CountryId,
                    EmailConfirmed = true,
                    ExpiryDateTime = System.DateTime.UtcNow.AddDays(30),
                    CreatedDate = System.DateTime.UtcNow,
                    FirstName = model.FullName,
                    PlanTextPassword = model.Password,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.organizationadmin.ToString());
                    //Create organizations
                    //Create first hostel 
                    //send welcome confirmations email 
                   var tempOrg =  await  _organizationRepository.InsertAsync(new entity.OrganizationEntity
                    {
                        OrganizationName = model.FullName
                    });
                    if (tempOrg != null)
                    {
                        await _hostelRepository.InsertAsync(new entity.HostelEntity
                        {
                            HostelOrganizationId = tempOrg.GetValueOrDefault(),
                            HostelName = model.FullName,

                        });

                        await _organizationUserRepository.InsertAsync(new entity.OrganizationUserEntity
                            {
                              OrganizationId = tempOrg.Value,
                              OrganizationIdentityUserId = user.Id
                            });
                    }

                    //Send welcome email
                    await _emailSender.SendWelcomeEmail(model.Email, model.FullName);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Context.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(model);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return Ok(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost("logoff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost("externallogin")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            //EnsureDatabaseCreated(_applicationDbContext);
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet("externallogincallback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return Ok("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                // ViewData["ReturnUrl"] = returnUrl;
                // ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return Ok(new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost("externalloginconfirmation")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return Ok("ExternalLoginFailure");
                }
                var user = new ApplicationIdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            //ViewData["ReturnUrl"] = returnUrl;
            return Ok(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet("confirmemail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Ok("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Ok("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return Ok(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet("forgotpassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
           // await _emailSender.SendWelcomeEmail("m.nadeem.nasir@gmail.com", "Nadeem");

            //await _emailSender.SendResetPasswordEmail("m.nadeem.nasir@gmail.com", "Nadeem" , "test");

            return Ok();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {        
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Ok();
                }
                               

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/index.html#/auth/reset-password?userId={user.Id}&code={HttpUtility.UrlEncode(code)}";

                /*
                var callbackUrl = Url.Action("ResetPassword", "Account", 
                    new { userId = user.Id, code = code }, 
                    protocol: HttpContext.Request.Scheme);
                    */
                await _emailSender.SendResetPasswordEmail(user.Email, user.FirstName, callbackUrl);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                //return View("ForgotPasswordConfirmation");

                return Ok();
            }

            // If we got this far, something failed, redisplay form
            return Ok(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet("forgotpasswordconfirmation")]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return Ok();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? Ok("Error") : Ok("");
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost("resetpassword")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                //Update Password colums in user table
                user.PlanTextPassword = model.Password;
                await _userManager.UpdateAsync(user);
                return Ok();
                //return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return BadRequest(result);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet("resetpasswordconfirmation")]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return Ok();
        }

        //
        // GET: /Account/SendCode
        [HttpGet("sendcode")]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return Ok("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return Ok(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost("sendcode")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return Ok("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return Ok("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                //await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet("verifycode")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return Ok("Error");
            }
            return Ok(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost("verifycode")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                return Ok("Lockout");
            }
            else
            {
                ModelState.AddModelError("", "Invalid code.");
                return Ok(model);
            }
        }

        #region Helpers

        // The following code creates the database and schema if they don't exist.
        // This is a temporary workaround since deploying database through EF migrations is
        // not yet supported in this release.
        // Please see this http://go.microsoft.com/fwlink/?LinkID=615859 for more information on how to do deploy the database
        // when publishing your application.
        //private static void EnsureDatabaseCreated(ApplicationDbContext context)
        //{
        //    if (!_databaseChecked)
        //    {
        //        _databaseChecked = true;
        //        context.Database.EnsureCreated();
        //    }
        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private async Task<ApplicationIdentityUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion

        #region custom helper method
        //Insert role 
        // await _roleManager.CreateAsync(new PMCMIdentityRole { Name = "Guest", ConcurrencyStamp = "Guest rights role" });


        //insert spa

        private async Task AddOpenIdConnectOptions(IServiceScope services, CancellationToken cancellationToken)
        {
            var manager = services.ServiceProvider.GetService<OpenIddictApplicationManager<OpenIddictApplication>>();

            if (await manager.FindByClientIdAsync("PMCMwebaspnetcorespa", cancellationToken) == null)
            {
                //aspnetcorespa
                var host = Startup.Configuration["AppSettings:BaseUrls:Web"].ToString();
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "My2Homespaappcorecore",
                    DisplayName = "My2Home",
                    PostLogoutRedirectUris = { new Uri($"{host}/signout-oidc") },
                    RedirectUris = { new Uri(host) }
                    // RedirectUris = { new Uri($"{host}/signin-oidc") }
                };
                await manager.CreateAsync(descriptor, cancellationToken);
            }
        }
        private async Task EnsureRoleAsync(string roleName)
        {
            if ((await _roleManager.FindByNameAsync(roleName)) == null)
            {
                IdentityRole applicationRole = new IdentityRole();
                var result = await _roleManager.CreateAsync(applicationRole);
                if (result == null)
                    throw new Exception($"Seeding \"{roleName}\" role failed. Errors: {string.Join(Environment.NewLine, "")}");
            }
        }

        private async Task<string> CreateRoles()
        {
            try
            {
                await EnsureRoleAsync("superadmin");
                await EnsureRoleAsync("hosteladmin");
                await EnsureRoleAsync("tenant");
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}