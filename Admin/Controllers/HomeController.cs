
using System.Web.Mvc;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    [HandleError]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}