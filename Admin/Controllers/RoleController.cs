
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        public ActionResult Index(int? page)
        {
            Paginator paginator = this.AddDefaultPaginator<Role>(page);
            List<Role> roles = BaseModel.FetchPage<Role>(paginator);

            // add notice if there is no records
            if (roles.Count == 0)
            {
                this.AddViewNotice("No Roles were found.");
            }

            return View(roles);
        }

        public ActionResult Details(string id)
        {
            // test to see if the model was able to load.
            Role selectedRole = Role.Load(id);

            if (selectedRole == null)
            {
                this.AddTempNotice("The Role ID " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(selectedRole);
        }

        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Role newRole = new Role();
                UpdateModel(newRole);

                //setup a new policy row.
                Policy newPolicy = new Policy();

                newPolicy.Save();
                newRole.policy_id = newPolicy.id;

                newRole.Save();

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

            return View();
        }

        public ActionResult Edit(string id)
        {
            // test to see if the edit was able to load the Model
            Role selectedRole = Role.Load(id);

            if (selectedRole == null)
            {
                this.AddTempNotice("The Role ID " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            // display edit field.
            return View(selectedRole);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            Role editedRole = Role.Load(id);

            try
            {
                if (editedRole != null)
                {
                    UpdateModel(editedRole);
                    editedRole.Save();
                }

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

            return View(editedRole);
        }
    }
}