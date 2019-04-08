using ExpenseTracker.BAL;
using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ExpenseTracker.Controllers
{
    public class AuthController : BaseController
    {
        private AuthService _authService = new AuthService();


        // GET: Auth
        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
            else
                return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!_authService.CheckUserExists(model.Email))
                ModelState.AddModelError("Email", "User does not exist.");
            if (!_authService.CheckCorrectPassword(model))
                ModelState.AddModelError("Password", "Incorrect Password.");

            if (ModelState.IsValid)
            {
                //Login successful lets put him to requested page
                var data = Authenticate(model.Email, model.RememberMe);
                if (data)
                {
                    string returnUrl = Request.QueryString["ReturnUrl"] as string;

                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {

                        //no return URL specified redirect to dashboard
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            return View();
        }

        public bool Authenticate(string email, bool RememberMe)
        {
            var authentiate = false;
            try
            {
                var user = _authService.GetUserByEmail(email);
                string userDataString = String.Concat(user.Id, "|", user.Name, "|", user.Email, "|", user.RoleId, "|", user);

                // Create the cookie that contains the forms authentication ticket
                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(user.Email, RememberMe);

                // Get the FormsAuthenticationTicket out of the encrypted cookie
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                DateTime Expirationdate = (RememberMe) ? DateTime.Now.AddMonths(6) : ticket.Expiration;

                // Create a new FormsAuthenticationTicket that includes our custom User Data
                FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(ticket.Version,
                    ticket.Name,
                    ticket.IssueDate,
                    Expirationdate,
                    ticket.IsPersistent,
                    userDataString,
                    FormsAuthentication.FormsCookiePath);

                // Update the authCookie's Value to use the encrypted version of newTicket
                authCookie.Value = FormsAuthentication.Encrypt(newTicket);
                HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, authCookie.Value);

                // Manually add the authCookie to the Cookies collection
                ck.Expires = Expirationdate;
                Response.Cookies.Add(ck);
                authentiate = true;
            }
            catch (Exception ex)
            {
                authentiate = false;
            }

            return authentiate;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// What this does?
        /// Register users and registeration is successful Authenticate user and redirects to dashboard.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!model.Terms)
                ModelState.AddModelError("Terms", "You need to agree the terms before proceding.");
            if (ModelState.IsValid)
            {
                var data = _authService.RegisterUser(model);

                if (data.StatusCode == 200)
                {
                    Authenticate(model.Email, false);
                    ShowStatus(data.StatusCode, data.Status);
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;

            if (ModelState.IsValid)
            {
                if (!_authService.CheckOldPasswordIsCorrect(ident.UserId, model.OldPassword))
                {
                    ModelState.AddModelError("OldPassword", "Old Password is incorrect.");
                }
                else
                {
                    var data = _authService.ChangePassword(model, ident.UserId);
                    ShowStatus(data.StatusCode, data.Status);
                    if (data.StatusCode == 200)
                        return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }
    }
}