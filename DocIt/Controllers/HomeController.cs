
using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Mvc;
//using Excel=Microsoft.Office.Interop.Excel;

using WBEADMS.Controllers.LocationExtensions;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["location"] = this.GetLocation();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Location()
        {
            SetViewDataForm();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Location(FormCollection collection)
        {
            string location_id = collection["location_id"];

            HttpCookie locationCookie = new HttpCookie("LocationSetting");
            locationCookie["location_id"] = location_id;
            locationCookie.Expires = new DateTime(2099, 1, 1);
            Response.Cookies.Add(locationCookie);

            if (location_id.IsBlank())
            {
                Session["location"] = null;
                this.AddTempNotice("Warning: Default location has been unset.");
            }
            else
            {
                var location = WBEADMS.Models.Location.Load(location_id); // update session
                Session["location"] = location;
                this.AddTempNotice("Updated location to " + location + ".");
            }

            return RedirectToAction("Index", "Home");
        }

        private StringBuilder AddSampleCSV(Models.ChainOfCustody coc, Models.Sample sample, StringBuilder csvString, bool isTravel)
        {
            WBEADMS.Models.Location loc = WBEADMS.Models.Location.Load(coc.location_id);

            csvString.Append(sample.wbea_id);
            csvString.Append(",");
            csvString.Append(coc.chain_of_custody_id);
            csvString.Append(",");
            csvString.Append(sample.lab_sample_id);
            csvString.Append(",");
            csvString.Append(sample.media_serial_number);
            csvString.Append(",");
            csvString.Append(loc.name);
            csvString.Append(",");
            csvString.Append(loc.full_name);
            csvString.Append(",");
            csvString.Append(coc.Retrieval.date_actual_sample_start);
            csvString.Append(",");
            csvString.Append(coc.Retrieval.date_actual_sample_end);
            csvString.Append(",");
            csvString.Append(coc.Retrieval.elapsed_sampling_duration);
            csvString.Append(",seconds,");
            string unit;
            try
            {
                unit = Models.Unit.Load(coc.Retrieval.sample_volume_unit).symbol;
                csvString.Append(coc.Retrieval.sample_volume + "," + unit);
            }
            catch (Exception e)
            {
                csvString.Append("N/A,N/A");
            }
            csvString.Append(",");
            string fuf = coc.Retrieval.field_user_flag;
            csvString.Append(fuf);
            csvString.Append(",");
            try
            {
                csvString.Append(Models.RetrievalSection.FieldUserFlags[fuf].Replace(',', ';'));
            }
            catch (Exception e)
            {

            }
            csvString.Append(",");
            try
            {
                unit = Models.Unit.Load(coc.Retrieval.average_ambient_temperature_unit).symbol;
                csvString.Append(coc.Retrieval.average_ambient_temperature + "," + unit);
            }
            catch (Exception e)
            {
                csvString.Append("N/A,N/A");
            }
            csvString.Append(",");
            try
            {
                unit = Models.Unit.Load(coc.Retrieval.average_barometric_pressure_unit).symbol;
                csvString.Append(coc.Retrieval.average_barometric_pressure + "," + unit);
            }
            catch (Exception e)
            {
                csvString.Append("N/A,N/A");
            }
            csvString.Append(",");
            csvString.Append(coc.SampleType.name);
            csvString.Append(",");
            csvString.Append(isTravel ? "travel" : "data");
            csvString.Append(",");
            foreach (Models.Note note in coc.Notes)
            {
                csvString.Append(note.body.Replace(",", " "));
                csvString.Append(";");
            }
            csvString.Append("\n");
            return csvString;
        }
        public ActionResult ExportCOC()
        {

            ArrayList cocs = (ArrayList)Session["ExportCOCList"];
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition();
            DateTime now = DateTime.Now;
            cd.FileName = "COC Export - " + now.Month + now.Day + now.Year + now.Hour + now.Minute + ".csv";
            Response.AppendHeader("Content-Disposition", cd.ToString());

            StringBuilder csvString = new StringBuilder("");
            // Header
            csvString.Append("CoC Export\n");
            csvString.Append("Export Date:," + DateTime.Now.ToString() + "\n");
            csvString.Append("WBEA ID,C of C ID,Lab Sample ID,Sample Media ID,Site,Site Name,Start Date,End Date,Elapsed Sampling Duration,Elapsed Sampling Duration Unit,Total Volume,Total Volume Unit,Field Flag,Field Flag Text,Temperature,Temperature Unit,Pressure,Pressure Unit,Sample Type,Data Type,Notes\n");
            try
            {
                if (cocs != null)
                {
                    foreach (string cocID in cocs)
                    {
                        WBEADMS.Models.ChainOfCustody coc = WBEADMS.Models.ChainOfCustody.Load(cocID);
                        if (coc != null)
                        {
                            foreach (WBEADMS.Models.Sample sample in coc.Samples)
                            {
                                csvString = AddSampleCSV(coc, sample, csvString, false);
                            }
                            foreach (WBEADMS.Models.Sample blank in coc.TravelBlanks)
                            {
                                csvString = AddSampleCSV(coc, blank, csvString, true);
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return File(System.Text.Encoding.Default.GetBytes(csvString.ToString()), "Content-type: text/csv");
        }

        public string ClearExportCOC()
        {
            Session["ExportCOCList"] = null;

            return GetExportCOC();
        }

        public string GetExportCOC()
        {
            ArrayList cocs = (ArrayList)Session["ExportCOCList"];
            if (cocs == null) cocs = new ArrayList();

            string exportList = "";
            foreach (string cocid in cocs)
            {
                Models.ChainOfCustody coc = Models.ChainOfCustody.Load(cocid);
                exportList += "<li><a href=\"/ChainOfCustody.aspx/Details/" + cocid + "\">" + cocid + " - [" + coc.SampleType + "]</a> ";
                exportList += "<button id=\"deleteButton\" style=\"cursor: pointer\" onclick=\"deleteCOCExport(" + cocid + "); return false;\">X</button>";
            }
            if (exportList == "") return "No CoC's to export yet.";

            return exportList;
        }

        public string RemoveExportCOC(string id)
        {
            ArrayList cocs = (ArrayList)Session["ExportCOCList"];
            if (cocs == null) cocs = new ArrayList();
            cocs.Remove(id);

            Session["ExportCOCList"] = cocs;

            return GetExportCOC();
        }
        public string RemoveExportBatchCOC(string id)
        {
            ArrayList cocs = (ArrayList)Session["ExportCOCList"];
            if (cocs == null) cocs = new ArrayList();
            cocs.Remove(id);

            Session["ExportCOCList"] = cocs;

            return GetExportCOC();
        }
        public string AddExportCOC(string id)
        {
            ArrayList cocs = (ArrayList)Session["ExportCOCList"];
            if (cocs == null) cocs = new ArrayList();
            if (!cocs.Contains(id)) cocs.Add(id);
            Session["ExportCOCList"] = cocs;

            return GetExportCOC();
        }
        public string AddExportBatchCOC(string id)
        {
            string[] idlist = id.Split(',');
            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", id);

            ArrayList cocs = (ArrayList)Session["ExportCOCList"];
            if (cocs == null) cocs = new ArrayList();
            foreach (var i in idlist)
            {
                if (!cocs.Contains(i)) cocs.Add(i);
            }

            Session["ExportCOCList"] = cocs;

            return GetExportCOC();
        }
        private void SetViewDataForm()
        {
            string location_id = this.GetLocationId();
            ViewData["location"] = this.GetLocation();

            ViewData["location_id"] = WBEADMS.Models.Location.FetchSelectListActive(location_id);
        }
    }
}