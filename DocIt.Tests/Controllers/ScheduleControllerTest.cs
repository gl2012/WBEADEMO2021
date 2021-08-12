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
using WMD.DocIt.Controllers;

namespace WMD.Tests.Controllers {
    [TestClass]
    public class ScheduleControllerTest {

        [TestMethod]
        public void CalendarGet() {
            // Arrange
            ScheduleController controller = GetScheduleController();

            // Act
            ViewResult result = (ViewResult)controller.Calendar(null /*month*/, null /*year*/, null /*location*/, null /*sample_type*/);

            // Assert
            Assert.IsNotNull(result);
        }


        #region MVC Controller Test initialization
        private static ScheduleController GetScheduleController() {
            ScheduleController controller = new ScheduleController();
            ControllerContext controllerContext = new ControllerContext(new MockHttpContext(), new RouteData(), controller);
            controller.ControllerContext = controllerContext;
            return controller;
        }

        public class MockFormsAuthenticationService : IFormsAuthentication {
            public void SignIn(string userName, bool createPersistentCookie) {
            }

            public void SignOut() {
            }
        }

        public class MockIdentity : IIdentity {
            public string AuthenticationType {
                get {
                    return "MockAuthentication";
                }
            }

            public bool IsAuthenticated {
                get {
                    return true;
                }
            }

            public string Name {
                get {
                    return "someUser";
                }
            }
        }

        public class MockPrincipal : IPrincipal {
            IIdentity _identity;

            public IIdentity Identity {
                get {
                    if (_identity == null) {
                        _identity = new MockIdentity();
                    }
                    return _identity;
                }
            }

            public bool IsInRole(string role) {
                return false;
            }
        }

        public class MockHttpContext : HttpContextBase {
            private IPrincipal _user;

            public override IPrincipal User {
                get {
                    if (_user == null) {
                        _user = new MockPrincipal();
                    }
                    return _user;
                }
                set {
                    _user = value;
                }
            }
        }
        #endregion

    }
}