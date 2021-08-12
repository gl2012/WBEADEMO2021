
using System;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class SampleMediaIdController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Generate");
        }

        public ActionResult Generate()
        {
            int lastGeneratedDigits;
            int lastSetSize;
            string lastParameter;
            DateTime dateLastGenerated;
            SampleMediaIdGenerator.FetchDetails(DateTime.Today.Year, out lastParameter, out lastGeneratedDigits, out lastSetSize, out dateLastGenerated);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Generate(FormCollection collection)
        {
            try
            {
                string parameter = collection["parameter"];
                ViewData["parameter"] = parameter;
                ViewData["year"] = collection["year"];
                ViewData["month"] = collection["month"];

                string[] wbeaIds = SampleMediaIdGenerator.ReserveNewSet(collection["parameter"], collection["set_size"], collection["year"], collection["month"]);

                byte[] csvBytes = ArrayToLabelFile.ByteArray(wbeaIds, 4);

                string filename = "SamMedID-" + collection["year"].Substring(2) + collection["month"].PadLeft(2, '0') + parameter + "x" + collection["set_size"].PadLeft(5, '0') + ".CSV";

                this.AddTempNotice("Generated Sample Media ID set in file: " + filename);
                return File(csvBytes, "text/csv", filename);
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                return View();
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(e);
                return View();
            }
        }
    }
}