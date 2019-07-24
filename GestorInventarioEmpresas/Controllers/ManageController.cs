using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GestorInventarioEmpresas.Models;
using GestorInventarioEmpresas.BackEnd.DAL;
using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using GestorInventarioEmpresas.BackEnd.Domain;
using GestorInventarioEmpresas.BackEnd.Services;

namespace GestorInventarioEmpresas.Controllers
{

    public class ManageController : BaseController
    {
        private IUserService _userService;
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IUserService userService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _userService = userService;
        }
        public ManageController(IUserService userService)
        {


            _userService = userService;
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Users()
        {
            {
                var context = new GestorInventarioEmpresasContext();
                var usersWithRoles = (from user in context.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Username = user.ApplicationUser.UserName,
                                          Email = user.ApplicationUser.Email,
                                          RoleNames = (from userRole in user.ApplicationUser.Roles
                                                       join role in context.Roles on userRole.RoleId
                                                       equals role.Id
                                                       select role.Name).ToList()
                                      }).ToList().Select(p => new Users_in_Role_ViewModel()

                                      {
                                          Id = p.UserId,
                                          Username = p.Username,
                                          Email = p.Email,
                                          Role = string.Join(",", p.RoleNames)
                                      });


                return View(usersWithRoles);
            }

        }
        //
        // GET: /Account/Register
        [Authorize(Roles = "Administrador")]
        public ActionResult CreateUser()
        {

            var model = new RegisterViewModel();
            var context = new GestorInventarioEmpresasContext();
            model.UserRolesItems = new SelectList(context.Roles.ToList(), "Name", "Name");
            return View(model);
        }
        [HttpGet]
        
             public JsonResult DeleteUser(long id)
        {
           
            var user = _userService.GetbyId(id).ApplicationUser;
            ApplicationUser op = new ApplicationUser();
            op.Id = user.Id;
            UserManager.DeleteAsync(user);
            _userService.DeleteById(id);
            UserManager.DeleteAsync(op);
            Success("El usuario fue eliminado con exito", "Exito");
            return Json(new { success = false, responseText = " ." }, JsonRequestBehavior.AllowGet);

        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                //crear perfil  
                var userProfile = new UserProfile
                {
                    Name = model.Name,
                    ProfileImage = string.Empty,
                    Created = DateTime.Now,
                    ApplicationUser = user,
                   TypeEmployer=model.TypeEmployer,
                     Location = model.Location
            };
                user.UserProfile = userProfile;

                var result = await UserManager.CreateAsync(user, model.UserName);
                if (result.Succeeded)
                {
                   // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
    
                     var callbackUrl = Url.Action("ChangePassword", "Manage", new { }, protocol: Request.Url.Scheme);
                     await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                    await UserManager.SendEmailAsync(user.Id, "Acceso Time Entry", "Bienvenido a Time Entry <br> Sus credenciales para ingresar son: User: <strong>" + model.UserName + "</strong> y Contraseña: <strong>" + model.UserName + "</strong>, puede ingresar haciendo clic   <a href=\"" + callbackUrl + "\">Aqui</a>");
                    Success( "se ha creado correctamente, al mismo se lo ha notificado via mail", user.UserName);
                    return RedirectToAction("Users", "Manage");
                }
                AddErrors(result);
            }
            var context = new GestorInventarioEmpresasContext();
            model.UserRolesItems = new SelectList(context.Roles.ToList(), "Name", "Name");
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize(Roles = "Administrador")]
        public ActionResult EditUser(long id)
        {
            RegisterViewModel model = new RegisterViewModel();
            if (ModelState.IsValid)
            {
                var context = new GestorInventarioEmpresasContext();
                var user = _userService.GetbyId(id);
                model.id = id;
                model.UserName = user.ApplicationUser.UserName;
                model.Name = user.Name;
                model.UserRoles = (from userRole in user.ApplicationUser.Roles
                                   join role in context.Roles on userRole.RoleId
                                   equals role.Id
                                   select role.Name).FirstOrDefault();
                model.Email = user.ApplicationUser.Email;
                model.UserRolesItems = new SelectList(context.Roles.ToList(), "Name", "Name");
                model.TypeEmployer = user.TypeEmployer;
                model.Location = user.Location;
            }

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(RegisterViewModel model)
        {
            var context = new GestorInventarioEmpresasContext();
            try
            {
              
                if (ModelState.IsValid)
                {
                    var user = _userService.GetbyId(model.id);
                    //crear perfil  
                    user.Name = model.Name;
                    user.ApplicationUser.UserName = model.UserName;
                    user.ApplicationUser.Email = model.Email;
                    user.TypeEmployer = model.TypeEmployer;
                    user.Location = model.Location;

        var lista = _userService.GetAll();
                    if (lista.Where(x => x.ApplicationUser.Email.Equals(model.Email) && x.Id!=model.id).ToArray().Length!=0) {
                        throw new Exception(" Ese email ya se encuentra en uso ");
                    }
                    if (lista.Where(x => x.ApplicationUser.UserName.Equals(model.UserName) && x.Id != model.id).ToArray().Length != 0)
                    {
                        throw new Exception(" Ese UserName ya se encuentra en uso ");
                    }
                    _userService.Update(user);
                    await UserManager.RemoveFromRolesAsync(user.ApplicationUser.Id, context.Roles.Select(x=>x.Name).ToArray());
                    var roles = await UserManager.GetRolesAsync(user.ApplicationUser.Id);
                    await UserManager.RemoveFromRolesAsync(user.ApplicationUser.Id, roles.ToArray());
                    await this.UserManager.AddToRoleAsync(user.ApplicationUser.Id, model.UserRoles);
                    Success("se ha actualizado correctamente", user.ApplicationUser.UserName);
                    return RedirectToAction("Users", "Manage");
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message, "Ups..");
                
            }
          
           
            model.UserRolesItems = new SelectList(context.Roles.ToList(), "Name", "Name");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

       
        public async Task<JsonResult> ResetPasswordUserAsync(long id)
        {
          
            if (ModelState.IsValid)
            {
                var context = new GestorInventarioEmpresasContext();
                var user = _userService.GetbyId(id);
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code =  await UserManager.GeneratePasswordResetTokenAsync(user.ApplicationUser.Id);
                var result = await UserManager.ResetPasswordAsync(user.ApplicationUser.Id, code, user.ApplicationUser.UserName);
            }
            Success("El usuario fue eliminado con exito", "Exito");
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";
            if (ViewBag.StatusMessage == "Your password has been changed.")
            {
                Success("Tú contraseña ha sido actualizada correctamente", "Felicitaciones");
            }
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return RedirectToAction("Index","Home");
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        [Authorize]
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}