
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public ActionResult Index(int? page, int? page_size, int? role_id, string is_active, string sort)
        {
            string sql = @"SELECT * 
                            FROM Users
                            INNER JOIN Roles ON Users.role_id = Roles.role_id
                            WHERE 1 = 1";

            List<User> users;
            using (SqlCommand command = new SqlCommand(sql))
            {
                if (role_id.HasValue)
                {
                    command.CommandText += " AND Roles.role_id = @role_id";
                    command.Parameters.AddWithValue("role_id", role_id.Value);
                }

                if (!is_active.IsBlank())
                {
                    if (is_active.ToLower().Trim() == "yes")
                    {
                        command.CommandText += " AND Users.active = 1";
                    }
                    else if (is_active.ToLower().Trim() == "no")
                    {
                        command.CommandText += " AND Users.active = 0";
                    }
                }

                string orderByClause = "";
                if (!sort.IsBlank())
                {
                    ViewData["sort"] = sort;
                    sort = sort.ToLower();

                    string[] sortOrder = sort.Split('_');
                    foreach (string column in sortOrder)
                    {
                        switch (column)
                        {
                            case "role":
                                orderByClause += "Roles.name,";
                                break;
                            case "desc-role":
                                orderByClause += "Roles.name DESC,";
                                break;
                            case "first-name":
                                orderByClause += "Users.first_name,";
                                break;
                            case "desc-first-name":
                                orderByClause += "Users.first_name DESC,";
                                break;
                            case "last-name":
                                orderByClause += "Users.last_name,";
                                break;
                            case "desc-last-name":
                                orderByClause += "Users.last_name DESC,";
                                break;
                            case "email":
                                orderByClause += "Users.email,";
                                break;
                            case "desc-email":
                                orderByClause += "Users.email DESC,";
                                break;
                            case "active":
                                orderByClause += "Users.active,";
                                break;
                            case "desc-active":
                                orderByClause += "Users.active DESC,";
                                break;

                        }
                    }
                }

                if (!orderByClause.IsBlank())
                {
                    command.CommandText += " ORDER BY " + orderByClause.Trim(',');
                }

                var urlParameters = new { action = "Index", controller = "Users", role_id = role_id, is_active = is_active, sort = sort };
                Paginator paginator = this.AddDefaultPaginator<User>(urlParameters, page, command);
                this.SetPageSize(page_size, paginator);
                users = BaseModel.FetchPage<User>(paginator, command);
            }

            //Populate filter drop downs.
            ViewData["role_id"] = Role.FetchSelectList(role_id);
            ViewData["is_active"] = WBEADMS.ControllersCommon.GetYesNoSelectList(is_active);

            // add notice if there is no records
            if (users.Count == 0)
            {
                this.AddViewNotice("No Users were found.");
            }

            return View(users);
        }

        public ActionResult Details(string id)
        {
            // test to see if the model was able to load.
            WBEADMS.Models.User selectedUser = WBEADMS.Models.User.Load(id);

            if (selectedUser == null)
            {
                this.AddTempNotice("The User ID " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(selectedUser);
        }
        public ActionResult Previlege(string id)
        {
            WBEADMS.Models.User selectedUser = WBEADMS.Models.User.Load(id);

            if (selectedUser == null)
            {
                this.AddTempNotice("The User ID " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            // display edit field.
            SetViewDataForm(selectedUser);
            return View(selectedUser);
        }
        public ActionResult Create()
        {
            var user = new User();
            SetViewDataForm(user);
            user.active = "True";
            return View(user);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            WBEADMS.Models.User newUser = new WBEADMS.Models.User();

            try
            {
                UpdateModel(newUser);
                newUser.Save();

                this.AddTempNotice("Successfully created " + newUser.user_name + ", and sent an email to " + newUser.email);
                return RedirectToAction("Index");
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(new ModelException(e));
            }

            SetViewDataForm(newUser);
            return View(newUser);
        }

        public ActionResult Edit(string id)
        {
            // test to see if the edit was able to load the Model
            WBEADMS.Models.User selectedUser = WBEADMS.Models.User.Load(id);

            if (selectedUser == null)
            {
                this.AddTempNotice("The User ID " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            // display edit field.
            SetViewDataForm(selectedUser);
            return View(selectedUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            WBEADMS.Models.User editedUser = WBEADMS.Models.User.Load(id);

            try
            {
                if (editedUser != null)
                {
                    UpdateModel(editedUser);
                    editedUser.Save();
                }

                this.AddTempNotice("Successfully modified " + editedUser.user_name);
                return RedirectToAction("Index");
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(new ModelException(e));
            }

            SetViewDataForm(editedUser);
            return View(editedUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PasswordReset(string id)
        {
            ResetPassword(id);
            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IndexPasswordReset(string id)
        {
            ResetPassword(id);
            return RedirectToAction("Index", new { id = id });
        }

        private void ResetPassword(string userId)
        {
            var user = WBEADMS.Models.User.Load(userId);
            if (user != null)
            {
                Authentication.ResetPassword(user);
                this.AddTempNotice("Password for " + user.user_name + " has been reset and an email sent to " + user.email);
            }
        }

        private void SetViewDataForm(User item)
        {
            ViewData["role_id"] = Role.FetchSelectList(item.role_id);
        }
    }
}