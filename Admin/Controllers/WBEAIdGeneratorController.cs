
using System;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class WBEAIdController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Generate");
        }

        public ActionResult Generate()
        {
            int lastGeneratedDigits;
            int lastSetSize;
            DateTime dateLastGenerated;
            WBEAIdGenerator.FetchDetails(DateTime.Today.Year, out lastGeneratedDigits, out lastSetSize, out dateLastGenerated);
            if (lastGeneratedDigits >= 0)
            {
                this.AddViewNotice("Last generated a set of " + lastSetSize + " at " + dateLastGenerated.ToISODateTime() + " ending at [" + WBEAIdGenerator.GenerateWBEAId(dateLastGenerated.Year, dateLastGenerated.Month, lastGeneratedDigits) + "].");
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Generate(FormCollection collection)
        {
            try
            {
                string[] wbeaIds = WBEAIdGenerator.ReserveNewSet(collection["set_size"]);

                byte[] csvBytes = ArrayToLabelFile.ByteArray(wbeaIds, 4);

                string filename = "WBEASID-" + DateTime.Today.Year.ToString().Substring(2) + DateTime.Today.Month.ToString().PadLeft(2, '0') + "x" + collection["set_size"].PadLeft(5, '0') + ".CSV";

                ////this.AddTempNotice("Generated WBEA Sample ID set in file: " + filename);
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