
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class SampleResultController : Controller
    {
        public ActionResult Index(int? page, int? page_size, string sort)
        {
            List<SampleResult> sampleResults;

            var urlParameters = new { action = "Index", controller = "SampleResult", sort = sort };
            Paginator paginator = this.AddDefaultPaginator<SampleResult>(urlParameters, page);
            this.SetPageSize(page_size, paginator);
            sampleResults = BaseModel.FetchPage<SampleResult>(paginator);

            return View(sampleResults);
        }

        public ActionResult Details(string id)
        {
            if (id.IsBlank())
            {
                this.AddTempNotice("You must specify a Sample Result id");
                RedirectToAction("Index");
            }

            SampleResult result = SampleResult.Load(id);

            if (result == null)
            {
                this.AddTempNotice("Sample Result for " + id + " could not be found.");
                RedirectToAction("Index");
            }

            return View(result);
        }
    }
}