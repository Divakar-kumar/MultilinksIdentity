using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Multilinks.Identity.Models.AccountViewModels;
using Multilinks.Identity.Services;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Events;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Multilinks.Identity.Entities;
using Multilinks.Identity.Models;
using Microsoft.Extensions.Options;

namespace Multilinks.Identity.Controllers
{
   [Authorize]
   [Route("[controller]/[action]")]
   public class AccountController : Controller
   {
      private readonly UserManager<UserEntity> _userManager;
      private readonly SignInManager<UserEntity> _signInManager;

      private readonly IEmailSender _emailSender;
      private readonly ILogger _logger;

      private readonly IClientStore _clientStore;
      private readonly IIdentityServerInteractionService _interaction;
      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly IAuthenticationSchemeProvider _schemeProvider;
      private readonly IEventService _events;

      private readonly WebConsoleClientOptions _webConsoleClientOptions;

      public AccountController(
          UserManager<UserEntity> userManager,
          SignInManager<UserEntity> signInManager,
          IEmailSender emailSender,
          ILogger<AccountController> logger,
          IEventService events,
          IIdentityServerInteractionService interaction,
          IClientStore clientStore,
          IHttpContextAccessor httpContextAccessor,
          IAuthenticationSchemeProvider schemeProvider,
          IOptions<WebConsoleClientOptions> webConsoleClientOptions
      )
      {
         _userManager = userManager;
         _signInManager = signInManager;

         _emailSender = emailSender;
         _logger = logger;

         _interaction = interaction;
         _httpContextAccessor = httpContextAccessor;
         _schemeProvider = schemeProvider;
         _clientStore = clientStore;
         _events = events;

         _webConsoleClientOptions = webConsoleClientOptions.Value;
      }

      [HttpGet]
      [AllowAnonymous]
      public async Task<IActionResult> Login(string returnUrl = null)
      {
         // Clear the existing external cookie to ensure a clean login process
         await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

         ViewData["ReturnUrl"] = returnUrl;
         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
      {
         if(ModelState.IsValid)
         {
            /* Require the user to have completed registration before they can log on. */
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user != null)
            {
               if(!await _userManager.IsEmailConfirmedAsync(user))
               {
                  ModelState.AddModelError(string.Empty, "You must have a confirmed email to log in.");

                  return View(model);
               }
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: true);

            if(result.Succeeded)
            {
               if(_interaction.IsValidReturnUrl(returnUrl))
               {
                  var principal = await _signInManager.CreateUserPrincipalAsync(user);

                  var email = principal.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email).Value;
                  var name = principal.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name).Value;

                  await _events.RaiseAsync(new UserLoginSuccessEvent(email, user.Id, name));

                  return Redirect(returnUrl);
               }

               /* TODO: Should always be a valid return url? */
               return Redirect("~/");
            }

            if(result.IsLockedOut)
            {
               await _events.RaiseAsync(new UserLoginFailureEvent(model.Email, "locked out"));

               return RedirectToAction(nameof(Lockout));
            }
            else
            {
               await _events.RaiseAsync(new UserLoginFailureEvent(model.Email, "invalid credentials"));
               ModelState.AddModelError(string.Empty, "Invalid login attempt.");

               return View(model);
            }
         }

         await _events.RaiseAsync(new UserLoginFailureEvent(model.Email, "something went wrong"));
         return View(model);
      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult Lockout()
      {
         return View();
      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult Register(string returnUrl = null)
      {
         ViewData["ReturnUrl"] = returnUrl;
         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
      {
         ViewData["ReturnUrl"] = returnUrl;
         if(ModelState.IsValid)
         {
            var user = new UserEntity
            {
               UserName = model.Email,
               Email = model.Email
            };

            var result = await _userManager.CreateAsync(user);

            if(result.Succeeded)
            {
               _logger.LogInformation("User created a new account.");

               result = _userManager.AddClaimsAsync(user, new Claim[]{
                  new Claim(JwtClaimTypes.Email, model.Email),
                  new Claim(JwtClaimTypes.Role, "Application User"),
                  new Claim("RegisteredDateTimeOffsetUtc", DateTimeOffset.UtcNow.ToString())
               }).Result;

               if(!result.Succeeded)
               {
                  throw new Exception(result.Errors.First().Description);
               }

               var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
               var callbackUrl = Url.RegisterConfirmationLink(user.Id, code, Request.Scheme);
               await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

               return RedirectToAction(nameof(AccountController.RegisterConfirmationPending), "Account");
            }

            AddErrors(result);
         }

         // If we got this far, something failed, redisplay form
         return View(model);
      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult RegisterConfirmationPending(string returnUrl = null)
      {
         ViewData["ReturnUrl"] = returnUrl;
         return View();
      }

      [HttpGet]
      [AllowAnonymous]
      public async Task<IActionResult> Logout(string logoutId)
      {
         var viewModel = await BuildLoggedOutViewModelAsync(logoutId);

         if (User?.Identity.IsAuthenticated == true)
         {
              await _signInManager.SignOutAsync();
         }
         _logger.LogInformation("User logged out.");

         if (viewModel.PostLogoutRedirectUri == null)
         {
             return View("Error");
         }

         return Redirect(viewModel.PostLogoutRedirectUri);
      }
      
      private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
      {
         // get context information (client name, post logout redirect URI and iframe for federated signout)
         var logout = await _interaction.GetLogoutContextAsync(logoutId);

         var vm = new LoggedOutViewModel
         {
             PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
         };

         return vm;
      }

      [HttpGet]
      [AllowAnonymous]
      public async Task<IActionResult> RegisterConfirmation(string userId, string code)
      {
         if(userId == null || code == null)
         {
            throw new ApplicationException("User Id and code shouldn't be null.");
         }

         var user = await _userManager.FindByIdAsync(userId);

         if(user == null)
         {
            throw new ApplicationException($"Unable to load user with ID '{userId}'.");
         }

         ViewData["UserId"] = userId;
         ViewData["Code"] = code;

         return View("RegisterConfirmation");
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> RegisterConfirmation(RegisterConfirmationViewModel model, string returnUrl = null)
      {
         if(model.UserId == null || model.Code == null)
         {
            throw new ApplicationException("User Id and code shouldn't be null.");
         }

         var user = await _userManager.FindByIdAsync(model.UserId);

         if(user == null)
         {
            throw new ApplicationException($"Unable to load user with ID '{model.UserId}'.");
         }

         if(ModelState.IsValid)
         {
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded)
            {
               result = _userManager.AddClaimsAsync(user, new Claim[]{
                  new Claim(JwtClaimTypes.GivenName, model.FirstName),
                  new Claim(JwtClaimTypes.FamilyName, model.LastName),
                  new Claim(JwtClaimTypes.Name, model.FirstName + " " + model.LastName),
                  new Claim("RegisterConfirmationDateTimeOffsetUtc", DateTimeOffset.UtcNow.ToString())
               }).Result;

               if(!result.Succeeded)
               {
                  throw new Exception(result.Errors.First().Description);
               }

               _logger.LogInformation("User details updated.");

               var confirmationResult = await _userManager.ConfirmEmailAsync(user, model.Code);

               if(confirmationResult.Succeeded)
               {
                  return Redirect(_webConsoleClientOptions.RegistrationConfirmedRedirectUri);
               }

               /* Failed to confirm email code. */
               return View("Error");
            }

            /* Failed to update user details. */
            return View("Error");
         }

         ViewData["UserId"] = model.UserId;
         ViewData["Code"] = model.Code;
         ViewData["ReturnUrl"] = returnUrl;

         /* If we got this far, something failed, redisplay form. */
         return View(model);
      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult ForgotPassword(string code = null)
      {
         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
      {
         if(ModelState.IsValid)
         {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
               /* Don't reveal that the user does not exist or is not confirmed */
               return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
            await _emailSender.SendEmailForgotPasswordAsync(model.Email, callbackUrl);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
         }

         /* If we got this far, something failed, redisplay form */
         return View(model);
      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult ForgotPasswordConfirmation()
      {
         return View();
      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult ResetPassword(string code = null, string userId = null)
      {
         if(code == null || userId == null)
         {
            throw new ApplicationException("A code and user Id must be supplied for password reset.");
         }

         var model = new ResetPasswordViewModel { Code = code, UserId = userId };

         return View(model);
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
      {
         if(!ModelState.IsValid)
         {
            return View(model);
         }

         var user = await _userManager.FindByIdAsync(model.UserId);

         if(user == null)
         {
            // Don't reveal that the user does not exist
            return Redirect(_webConsoleClientOptions.ResetPasswordSuccessfulRedirectUri);
         }

         var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

         if(result.Succeeded)
         {
            return Redirect(_webConsoleClientOptions.ResetPasswordSuccessfulRedirectUri);
         }

         AddErrors(result);

         return View();
      }

      #region Helpers

      private void AddErrors(IdentityResult result)
      {
         foreach(var error in result.Errors)
         {
            ModelState.AddModelError(string.Empty, error.Description);
         }
      }

      private IActionResult RedirectToLocal(string returnUrl)
      {
         if(Url.IsLocalUrl(returnUrl))
         {
            return Redirect(returnUrl);
         }
         else
         {
            return RedirectToAction(nameof(HomeController.Index), "Home");
         }
      }

      #endregion
   }
}
