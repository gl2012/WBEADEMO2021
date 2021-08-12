
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using WBEADMS.Controllers.LocationExtensions;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        /* This constructor is used by the MVC framework to instantiate the controller using
         * the default forms authentication and membership providers.
         */

        public AccountController()
            : this(null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GuestBook(string name, string body)
        {
            // note validation
            var errors = new WBEADMS.Models.ModelException();
            name.CheckRequired(errors, "name");
            name.CheckMinLength(errors, 2, "name");
            body.CheckRequired(errors, "body");
            body.CheckMinLength(errors, 5, "body");

            // creating a new note
            var note = new WBEADMS.Models.Note();
            note.date_occurred = DateTime.Now.ToISODateTime();
            note.body = name + " (GuestBook) : " + body;
            note.committed = true;

            // setting location
            note.location_id = this.GetLocationId();
            if (note.location_id == null)
            {
                var unassigned = WBEADMS.Models.Location.LoadByName("Unassigned");
                if (unassigned != null)
                {
                    errors.AddError(new Exception("Unable to add note due to missing Default Location. Please contact a technician/administrator to set a Default Location."));
                }
                else
                {
                    note.location_id = unassigned.id;
                }
            }

            // save note
            if (errors.hasErrors)
            {
                this.PopulateViewWithErrorMessages(errors);
                return View("LogOn");
            }
            else
            {
                note.Save();

                this.AddTempNotice("Successfully added note from " + name + ".");

                if (note.Location.name == "Unassigned")
                {
                    this.AddTempNotice("<br/>Warning: Default Location has not been set. Please contact a technician/administrator to set a Default Location.");
                }

                return RedirectToAction("LogOn");
            }
        }

        public ActionResult LogOn(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            // System.IO.File.WriteAllText(@"C:\temp\logon.txt", "logon");
            ViewData["name"] = FetchUsername();
            return View();
        }
        public ActionResult test(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;

            // System.IO.File.WriteAllText(@"C:\temp\logon.txt", "logon");
            return View();
        }
        public static string FetchUsername()
        {
            string name = null;
            try
            {

                BaseModel.FetchList("select user_name from users where user_id=3", "user_name")[0].ToString();


            }
            catch (Exception ex)
            {
                name = ex.Message.ToString();
            }


            return name;
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {
            // System.IO.File.WriteAllText(@"C:\temp\logonpost.txt", "logon");
            if (!ValidateLogOn(userName, password))
            {
                return View();

            }

            var user = WBEADMS.Models.User.FetchByName(userName);
            if (!user.active.ToBool())
            {
                ModelState.AddModelError("_FORM", "The user name is invalid. Please contact your administrator.");
                return View();
            }

            Session["user"] = user;
            FormsAuth.SignIn(userName, rememberMe);

            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            /* removed as per change request User Comments to Airshed (pg1para1)
            if (this.GetLocation() == null) {
                return RedirectToAction("Location", "Home"); // set default location if not set
            }
             */
            // return RedirectToAction("BatchImportcoc", "ChainofCustody");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {

            FormsAuth.SignOut();
            TempData["notice"] = TempData["notice"]; // NOTE: this is done to preserve the temp notice upon logout

            return RedirectToAction("LogOn");
        }

        public ActionResult Register()
        {

            ViewData["PasswordLength"] = Authentication.MinPasswordLength;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(string userName, string email, string password, string confirmPassword)
        {

            ViewData["PasswordLength"] = Authentication.MinPasswordLength;

            if (ValidateRegistration(userName, email, password, confirmPassword))
            {
                // Attempt to register the user
                string errorMessage = Authentication.CreateUser(userName, password, email);

                if (errorMessage == null)
                {
                    FormsAuth.SignIn(userName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("_FORM", errorMessage);
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {

            ViewData["PasswordLength"] = Authentication.MinPasswordLength;

            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {

            ViewData["PasswordLength"] = Authentication.MinPasswordLength;

            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            if (Authentication.ChangePassword(User.Identity.Name, currentPassword, newPassword))
            {
                return RedirectToAction("ChangePasswordSuccess");
            }
            else
            {
                ModelState.AddModelError("_FORM", "The current password is incorrect or the new password is invalid.");
                return View();
            }
        }

        public ActionResult ChangePasswordSuccess()
        {

            return View();
        }

        public ActionResult ForgottenPassword()
        {

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgottenPassword(string userName)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "User name is required.");
                return View();
            }

            TempData["forgotten_username"] = userName;
            if (Authentication.ForgottenPassword(userName))
            {
                return RedirectToAction("ForgottenPasswordSuccess");
            }
            else
            {
                ModelState.AddModelError("username", "This user does not exist or does not have a registered email.");
                return View();
            }
        }

        public ActionResult ForgottenPasswordSuccess()
        {
            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        [Authorize]
        public ActionResult MyAccount()
        {
            var user = this.GetUser();
            //  ViewData["sampletypeList"] = WBEADMS.Models.User.fetchSampleType();
            ViewData["sampletypeList"] = WBEADMS.Models.User.fetchSampleType();
            ViewData["locationtypeList"] = WBEADMS.Models.User.fetchLocationType();
            ViewData["usersampletypeList"] = WBEADMS.Models.User.FetchuserSampletypelist(this.GetUser().user_id);

            ViewData["userlocationtypeList"] = WBEADMS.Models.User.Fetchuserlocationlist(this.GetUser().user_id);
            ViewData["locationtypeListInactive"] = WBEADMS.Models.User.FetchuserlocationlistInactive();

            return View(user);
        }

        [Authorize]
        public ActionResult EditAccount()
        {
            //  ViewData["sampletypeList"] = WBEADMS.Models.User.fetchSampleType();
            ViewData["sampletypeList"] = WBEADMS.Models.SampleType.FetchDictionaryByName();
            ViewData["locationtypeList"] = WBEADMS.Models.User.fetchLocationType();
            ViewData["usersampletypeList"] = WBEADMS.Models.User.FetchuserSampletypelist(this.GetUser().user_id);
            ViewData["userlocationtypeList"] = WBEADMS.Models.User.Fetchuserlocationlist(this.GetUser().user_id);
            ViewData["locationtypeListInactive"] = WBEADMS.Models.User.FetchuserlocationlistInactive();
            return View(this.GetUser());
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditAccount(FormCollection collection)
        {
            var editedUser = WBEADMS.Models.User.Load(this.GetUser().user_id);
            WBEADMS.Models.UserSampleType EdituserSampleType = new WBEADMS.Models.UserSampleType();
            WBEADMS.Models.UserLocations EdituserlocationType = new WBEADMS.Models.UserLocations();
            List<string> newsampleList = new List<string>();
            List<string> newlocationList = new List<string>();

            if (collection["parameter_list"] != null)
            { newsampleList = collection["parameter_list"].Split(',').ToList(); }

            if (collection["location_list"] != null)
            { newlocationList = collection["location_list"].Split(',').ToList(); }
            string strtxt = "";

            try
            {
                if (editedUser != null)
                {
                    UpdateModel(editedUser);
                    editedUser.Save();
                    Session["user"] = editedUser;
                    WBEADMS.Models.UserSampleType.DeleteUserSampleType(this.GetUser().user_id);
                    WBEADMS.Models.UserLocations.DeleteUserLocations(this.GetUser().user_id);
                    foreach (var a in newsampleList)
                    {

                        EdituserSampleType.User_id = this.GetUser().user_id;
                        EdituserSampleType.Sample_type_id = a.ToString();
                        EdituserSampleType.Date_modified = DateTime.Now.ToISODateTime();
                        EdituserSampleType.Save();
                        // strtxt = strtxt + " " + a.ToString();


                    }

                    foreach (var a in newlocationList)
                    {

                        EdituserlocationType.User_id = this.GetUser().user_id;
                        EdituserlocationType.location_id = a.ToString();
                        EdituserlocationType.Date_modified = DateTime.Now.ToISODateTime();
                        EdituserlocationType.Save();
                        strtxt = strtxt + " " + a.ToString();


                    }
                }

                //     System.IO.File.WriteAllText(@"C:\temp\WriteText.txt", strtxt);
                this.AddTempNotice("Successfully updated your account details.");
                return RedirectToAction("MyAccount");
            }
            catch (WBEADMS.Models.ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            /*
            catch (Exception e) {
                this.PopulateViewWithErrorMessages(new WBEADMS.Models.ModelException(e));
            }
            */
            return View(editedUser);
        }

        public ActionResult GetDateTime()
        {
            string currentDateTime = DateTime.Now.ToString(WBEADMS.ViewsCommon.FetchDateTimeFormat() + ":ss");
            return Json(new { updateddatetime = currentDateTime }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Returns DateTime in IETF standard (RFC 1123 Section 5.2.14) for use in javascript Date object</summary>
        public ActionResult GetDateTimeRFC1123()
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
            string currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo).ToString("MMM dd, yyyy HH:mm:ss");
            // string currentDateTime = DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss");
            return Json(new { updateddatetime = currentDateTime }, JsonRequestBehavior.AllowGet);
        }

        #region Validation Methods

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", "You must specify a current password.");
            }
            if (newPassword == null || newPassword.Length < Authentication.MinPasswordLength)
            {
                ModelState.AddModelError("newPassword",
                    String.Format(CultureInfo.CurrentCulture,
                         "You must specify a new password of {0} or more characters.",
                         Authentication.MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }
            if (!Authentication.ValidateUser(userName, password))
            {
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateRegistration(string userName, string email, string password, string confirmPassword)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "You must specify an email address.");
            }
            if (password == null || password.Length < Authentication.MinPasswordLength)
            {
                ModelState.AddModelError("password",
                    String.Format(CultureInfo.CurrentCulture,
                         "You must specify a password of {0} or more characters.",
                         Authentication.MinPasswordLength));
            }
            if (!String.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
            }
            return ModelState.IsValid;
        }

        #endregion
    }

    /* The FormsAuthentication type is sealed and contains static members, so it is difficult to
     * unit test code that calls its members. The interface and helper class below demonstrate
     * how to create an abstract wrapper around such a type in order to make the AccountController
     * code unit testable.
     */

    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);

        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}