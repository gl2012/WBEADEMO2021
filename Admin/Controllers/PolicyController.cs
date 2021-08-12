
using System;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class PolicyController : Controller
    {
        public ActionResult Edit(string id)
        {
            // test to see if the edit was able to load the Model
            Role selectedRole = new Role(id);

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
            Role editedRole = new Role(id);
            if (editedRole == null)
            {
                this.AddTempNotice("The Role ID " + id + " could not be found.");
                return RedirectToRoute(new { controller = "Role", action = "Index" });
            }

            try
            {
                UpdateModel(editedRole.Policy);
                editedRole.Policy.Save();

                return RedirectToRoute(new { controller = "Role", action = "Index" });
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