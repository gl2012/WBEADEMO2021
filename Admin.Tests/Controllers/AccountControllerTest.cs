/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMD;
using WMD.AdminPanel.Controllers;

namespace WMD.AdminPanel.Tests.Controllers
{

    [TestClass]
    public class AccountControllerTest
    {

        [TestMethod]
        public void ChangePasswordGetReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword();

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
        }

        [TestMethod]
        public void ChangePasswordPostRedirectsOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.ChangePassword("oldPass", "newPass", "newPass");

            // Assert
            Assert.AreEqual("ChangePasswordSuccess", result.RouteValues["action"]);
        }

        [TestMethod]
        public void ChangePasswordPostReturnsViewIfCurrentPasswordNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("", "newPassword", "newPassword");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("You must specify a current password.", result.ViewData.ModelState["currentPassword"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void ChangePasswordPostReturnsViewIfNewPasswordDoesNotMatchConfirmPassword()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("currentPassword", "newPassword", "otherPassword");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("The new password and confirmation password do not match.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void ChangePasswordPostReturnsViewIfNewPasswordIsNull()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("currentPassword", null, null);

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("You must specify a new password of 6 or more characters.", result.ViewData.ModelState["newPassword"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void ChangePasswordPostReturnsViewIfNewPasswordIsTooShort()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("currentPassword", "12345", "12345");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("You must specify a new password of 6 or more characters.", result.ViewData.ModelState["newPassword"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void ChangePasswordPostReturnsViewIfProviderRejectsPassword()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("oldPass", "badPass", "badPass");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("The current password is incorrect or the new password is invalid.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void ChangePasswordSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePasswordSuccess();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConstructorSetsProperties()
        {
            // Arrange
            IFormsAuthentication formsAuth = new MockFormsAuthenticationService();

            // Act
            AccountController controller = new AccountController(formsAuth);

            // Assert
            Assert.AreEqual(formsAuth, controller.FormsAuth, "FormsAuth property did not match.");
        }

        [TestMethod]
        public void ConstructorSetsPropertiesToDefaultValues()
        {
            // Act
            AccountController controller = new AccountController();

            // Assert
            Assert.IsNotNull(controller.FormsAuth, "FormsAuth property is null.");
        }

        [TestMethod]
        public void LoginGet()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoginPostRedirectsHomeIfLoginSuccessfulButNoReturnUrlGiven()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.LogOn("someUser", "goodPass", true, null);

            // Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void LoginPostRedirectsToReturnUrlIfLoginSuccessfulAndReturnUrlGiven()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectResult result = (RedirectResult)controller.LogOn("someUser", "goodPass", false, "someUrl");

            // Assert
            Assert.AreEqual("someUrl", result.Url);
        }

        [TestMethod]
        public void LoginPostReturnsViewIfPasswordNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn("username", "", true, null);

            // Assert
            Assert.AreEqual("You must specify a password.", result.ViewData.ModelState["password"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void LoginPostReturnsViewIfUsernameNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn("", "somePass", false, null);

            // Assert
            Assert.AreEqual("You must specify a username.", result.ViewData.ModelState["username"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void LoginPostReturnsViewIfUsernameOrPasswordIsIncorrect()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn("someUser", "badPass", true, null);

            // Assert
            Assert.AreEqual("The username or password provided is incorrect.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void LogOff()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.LogOff();

            // Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void RegisterGet()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register();

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
        }

        [TestMethod]
        public void RegisterPostRedirectsHomeIfRegistrationSuccessful()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Register("someUser", "email", "goodPass", "goodPass");

            // Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void RegisterPostReturnsViewIfEmailNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "", "password", "password");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("You must specify an email address.", result.ViewData.ModelState["email"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void RegisterPostReturnsViewIfNewPasswordDoesNotMatchConfirmPassword()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "email", "password", "password2");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("The new password and confirmation password do not match.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void RegisterPostReturnsViewIfPasswordIsNull()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "email", null, null);

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("You must specify a password of 6 or more characters.", result.ViewData.ModelState["password"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void RegisterPostReturnsViewIfPasswordIsTooShort()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "email", "12345", "12345");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("You must specify a password of 6 or more characters.", result.ViewData.ModelState["password"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void RegisterPostReturnsViewIfRegistrationFails()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("someUser", "DuplicateUserName" /* error */, "badPass", "badPass");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("Username already exists. Please enter a different user name.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void RegisterPostReturnsViewIfUsernameNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("", "email", "password", "password");

            // Assert
            Assert.AreEqual(6, result.ViewData["PasswordLength"]);
            Assert.AreEqual("You must specify a username.", result.ViewData.ModelState["username"].Errors[0].ErrorMessage);
        }

        private static AccountController GetAccountController()
        {
            IFormsAuthentication formsAuth = new MockFormsAuthenticationService();
            AccountController controller = new AccountController(formsAuth);
            ControllerContext controllerContext = new ControllerContext(new MockHttpContext(), new RouteData(), controller);
            controller.ControllerContext = controllerContext;
            return controller;
        }

        public class MockFormsAuthenticationService : IFormsAuthentication
        {
            public void SignIn(string userName, bool createPersistentCookie)
            {
            }

            public void SignOut()
            {
            }
        }

        public class MockIdentity : IIdentity
        {
            public string AuthenticationType
            {
                get
                {
                    return "MockAuthentication";
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return true;
                }
            }

            public string Name
            {
                get
                {
                    return "someUser";
                }
            }
        }

        public class MockPrincipal : IPrincipal
        {
            IIdentity _identity;

            public IIdentity Identity
            {
                get
                {
                    if (_identity == null)
                    {
                        _identity = new MockIdentity();
                    }
                    return _identity;
                }
            }

            public bool IsInRole(string role)
            {
                return false;
            }
        }

        public class MockMembershipUser : MembershipUser
        {
            public override bool ChangePassword(string oldPassword, string newPassword)
            {
                return newPassword.Equals("newPass");
            }
        }

        public class MockHttpContext : HttpContextBase
        {
            private IPrincipal _user;

            public override IPrincipal User
            {
                get
                {
                    if (_user == null)
                    {
                        _user = new MockPrincipal();
                    }
                    return _user;
                }
                set
                {
                    _user = value;
                }
            }
        }

    }
}