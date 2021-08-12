
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class BatchController : Controller
    {
        public ActionResult Index(int? page, int? page_size, string sort)
        {
            List<Batch> batches;

            var urlParameters = new { action = "Index", controller = "Batch", sort = sort };
            Paginator paginator = this.AddDefaultPaginator<Batch>(urlParameters, page);
            this.SetPageSize(page_size, paginator);
            batches = BaseModel.FetchPage<Batch>(paginator);

            return View(batches);
        }

        public ActionResult Details(string id)
        {
            if (id.IsBlank())
            {
                this.AddTempNotice("You must specify a Batch id");
                RedirectToAction("Index");
            }

            Batch batch = Batch.Load(id);

            if (batch == null)
            {
                this.AddTempNotice("Batch for " + id + " could not be found.");
                RedirectToAction("Index");
            }

            return View(batch);
        }
    }
}