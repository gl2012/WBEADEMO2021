using System.Web.Mvc;
using WBEADMS.Models;


namespace WBEADMS.Controllers
{
    public class PrevilegeController : Controller
    {
        // GET: Previlege
        public ActionResult Index()
        {
            return View();
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
        private void SetViewDataForm(User item)
        {
            ViewData["role_id"] = Role.FetchSelectList(item.role_id);
        }
    }
}