
 */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.Controllers.LocationExtensions
{
    public static class ControllersExtensions
    {
        public static string GetLocationId(this Controller controller)
        {
            if (GetLocation(controller) != null)
            {
                return GetLocation(controller).id;
            }

            return null;
        }

        public static Location GetLocation(this Controller controller)
        {
            Location location = (Location)controller.Session["location"];
            if (location == null)
            {
                var locationCooke = controller.Request.Cookies["LocationSetting"];
                if (locationCooke != null)
                {
                    location = Location.Load(locationCooke["location_id"]);
                    controller.Session["location"] = location;
                }
            }

            return location;
        }
    }
}

namespace WBEADMS
{
    public static class ControllersCommon
    {
        public static void PopulateViewWithErrorMessages(this Controller controller, ModelException errors)
        {
            foreach (ValidationError ve in errors.Errors)
            {
                controller.ModelState.AddModelError(ve.Field, ve.Message);
                controller.ModelState.SetModelValue(ve.Field, controller.ValueProvider.GetValue(ve.Field));
            }
        }

        public static void PopulateViewWithErrorMessages(this Controller controller, Exception e)
        {
            PopulateViewWithErrorMessages(controller, new ModelException(new ValidationError("unknown", e.Message)));
        }

        public static User GetUser(this Controller controller)
        {
            User user;
            user = (User)controller.Session["user"]; // gets user from session
            if (user == null)
            {
                user = User.FetchByName(controller.User.Identity.Name); // gets user by auth cookie
            }

            return user;
        }

        public static T GetRequestedModel<T>(this Controller c, string id) where T : BaseModel, new()
        {

            string modelName = typeof(T).Name;
            if (id.IsBlank())
            {
                c.AddTempNotice("No " + modelName + " found.");
                return null;
            }

            T model = BaseModel.Load<T>(id);
            if (model == null)
            {
                c.AddTempNotice(modelName + " " + id + " could not be found.");
                return null;
            }

            return model;
        }

        #region Pagination functions
        public static Paginator AddDefaultPaginator<TModel>(this Controller controller, int? currentPage) where TModel : BaseModel, new()
        {
            return AddDefaultPaginator<TModel>(controller, null /* url */, currentPage, (object)null /* filterClauses */);
        }

        public static Paginator AddDefaultPaginator<TModel>(this Controller controller, object url, int? currentPage) where TModel : BaseModel, new()
        {
            return AddDefaultPaginator<TModel>(controller, url, currentPage, (object)null /* filterClauses */);
        }


        public static Paginator AddDefaultPaginator<TModel>(this Controller controller, object url, int? currentPage, SqlCommand command) where TModel : BaseModel, new()
        {
            string stringUrl = CreateUrlString(controller, url, BaseModel.TypeName(typeof(TModel)));

            Paginator paginator = new Paginator(stringUrl, currentPage ?? 1, BaseModel.TotalCount<TModel>(command));
            controller.ViewData["pagination"] = paginator;
            return paginator;
        }

        public static Paginator AddDefaultPaginator<TModel>(this Controller controller, int? currentPage, object filterClauses) where TModel : BaseModel, new()
        {
            return AddDefaultPaginator<TModel>(controller, null /* url */, currentPage, filterClauses);
        }

        public static Paginator AddDefaultPaginator<TModel>(this Controller controller, object url, int? currentPage, object filterClauses) where TModel : BaseModel, new()
        {
            string stringUrl = CreateUrlString(controller, url, BaseModel.TypeName(typeof(TModel)));

            Paginator paginator = new Paginator(stringUrl, currentPage ?? 1, BaseModel.TotalCount<TModel>(filterClauses));
            controller.ViewData["pagination"] = paginator;
            return paginator;
        }



        private static string CreateUrlString(Controller controller, object url, string targetController)
        {
            if (url == null)
            {
                return controller.Url.RouteUrl(new { action = "Index", controller = targetController });
            }

            return controller.Url.RouteUrl(url);
        }

        public static void SetPageSize(this Controller controller, int? page_size, Paginator paginator)
        {
            if (page_size.HasValue)
            {
                controller.Session["page_size"] = page_size.Value;
            }

            if (controller.Session["page_size"] != null)
            {
                paginator.PageSize = (int)controller.Session["page_size"];
            }
        }
        public static int GetPageNo(this Controller controller, int? page)
        {
            int pageNo = 0;
            if (page.HasValue)
            {
                controller.Session["page"] = page.Value;
            }

            if (controller.Session["page"] != null)
            {
                pageNo = (int)controller.Session["page"];
            }
            return pageNo;
        }

        #endregion

        #region notice
        public static void AddTempNotice(this Controller controller, string msg)
        {
            if (controller.TempData["notice"] == null)
            {
                controller.TempData["notice"] = msg;
            }
            else
            {
                controller.TempData["notice"] = (string)controller.TempData["notice"] + "\n" + msg; // TODO: perhaps use a List<string> instead of a \n delimited string
            }
        }

        public static void AddViewNotice(this Controller controller, string msg)
        {
            if (controller.ViewData["notice"] == null)
            {
                controller.ViewData["notice"] = msg;
            }
            else
            {
                controller.ViewData["notice"] = (string)controller.ViewData["notice"] + "\n" + msg; // TODO: perhaps use a List<string> instead of a \n delimited string
            }
        }

        public static SelectList GetYesNoSelectList(object selectedValue)
        {
            return new SelectList(new List<string>(new string[] { "Yes", "No" }), selectedValue);
        }

        public static SelectList GetTrueFalseSelectList(object selectedValue)
        {
            Dictionary<string, string> yesNoOptions = new Dictionary<string, string>() {
                { "", "" },
                { "True", "yes" },
                { "False", "no" }
            };

            return new SelectList(yesNoOptions, "key", "value", selectedValue);
        }
        #endregion

        #region Pagination functions(MySQL)
        public static Paginator AddDefaultPaginatorMySQL<TModel>(this Controller controller, int? currentPage) where TModel : BaseModelMySQL, new()
        {
            return AddDefaultPaginatorMySQL<TModel>(controller, null /* url */, currentPage, (object)null /* filterClauses */);
        }

        public static Paginator AddDefaultPaginatorMySQL<TModel>(this Controller controller, object url, int? currentPage) where TModel : BaseModelMySQL, new()
        {
            return AddDefaultPaginatorMySQL<TModel>(controller, url, currentPage, (object)null /* filterClauses */);
        }


        public static Paginator AddDefaultPaginatorMySQL<TModel>(this Controller controller, object url, int? currentPage, MySqlCommand command) where TModel : BaseModelMySQL, new()
        {
            string stringUrl = CreateUrlString(controller, url, BaseModelMySQL.TypeName(typeof(TModel)));

            Paginator paginator = new Paginator(stringUrl, currentPage ?? 1, BaseModelMySQL.TotalCount<TModel>(command));
            controller.ViewData["pagination"] = paginator;
            return paginator;
        }

        public static Paginator AddDefaultPaginatorMySQL<TModel>(this Controller controller, int? currentPage, object filterClauses) where TModel : BaseModelMySQL, new()
        {
            return AddDefaultPaginatorMySQL<TModel>(controller, null /* url */, currentPage, filterClauses);
        }

        public static Paginator AddDefaultPaginatorMySQL<TModel>(this Controller controller, object url, int? currentPage, object filterClauses) where TModel : BaseModelMySQL, new()
        {
            string stringUrl = CreateUrlString(controller, url, BaseModelMySQL.TypeName(typeof(TModel)));

            Paginator paginator = new Paginator(stringUrl, currentPage ?? 1, BaseModelMySQL.TotalCount<TModel>(filterClauses));
            controller.ViewData["pagination"] = paginator;
            return paginator;
        }

        #endregion
    }
}