
using ClosedXML.Excel;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.SectionReportModel;
using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web.Mvc;
using System.Xml;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class ChainOfCustodyController : Controller
    {
        public ActionResult ExportCOCExcel(FormCollection collection)
        {


            string strfrom = Session["SelectedFrom"].ToString().IsBlank() ? "" : Session["SelectedFrom"].ToString();
            string strto = Session["SelectedTo"].ToString().IsBlank() ? "" : Session["SelectedTo"].ToString();
            int typeId = (int)Session["SelectedType"];

            deleteFiles(Directory.GetFiles(Server.MapPath("~/App_Data")));



            string strCurrent = DateTime.Now.ToString("MMM_yyyy");



            DataTable dta = new DataTable("Associated Sample(s)");
            dta.Columns.AddRange(new DataColumn[7] {new DataColumn("Chain of Custody Fields"),
                                            new DataColumn("WBEA Sample ID"),
                                            new DataColumn("PreparedBy:"),
                                            new DataColumn("Received From Lab"),
                                            new DataColumn("Avg.Storage Temperture"),
                                            new DataColumn("Media Serial Number"),
                                            new DataColumn("Lab Sample Id")

             });
            DataTable dtb = new DataTable("Chain of Custody Fields");
            dtb.Columns.AddRange(new DataColumn[5] { new DataColumn("Chain of Custody Fields"),
                                            new DataColumn("Created By:"),
                                            new DataColumn("Sample Type"),
                                            new DataColumn("Scgedule"),
                                            new DataColumn("Sampling Scheduled for")


             });
            DataTable dtc = new DataTable("Deployment Fields");
            dtc.Columns.AddRange(new DataColumn[17] {new DataColumn("Chain of Custody Fields"),
                                            new DataColumn("Date Deployed"),
                                            new DataColumn("Deployed By:"),
                                            new DataColumn("Location Name"),
                                            new DataColumn("Location full name"),
                                            new DataColumn("Sampler Name"),
                                             new DataColumn("Sampler Serial Number:"),
                                            new DataColumn("Sampler Make"),
                                            new DataColumn("Sampler Model"),
                                            new DataColumn("Last Calibrated Date"),
                                             new DataColumn("Date of Last Leak ChecK"),
                                           new DataColumn(" Sampling Head Cleaned On"),
                                           new DataColumn(" Sampler Flowrate"),
                                           new DataColumn(" Sampler Setpoint"),
                                           new DataColumn(" Programmed Sample Start Date:"),
                                           new DataColumn(" Programmed Sample End Date"),
                                           new DataColumn(" Travel Blank Present")
             });
            DataTable dtd = new DataTable("Retrieval Fields");
            dtd.Columns.AddRange(new DataColumn[12] { new DataColumn("Chain of Custody Fields"),new DataColumn("Elapsed Sampling Duration"),
                                            new DataColumn("Sample Volume"),
                                            new DataColumn("Actual Sample Start Date"),
                                            new DataColumn("Actual Sample End Date"),
                                            new DataColumn("Average Station Temperature"),
                                             new DataColumn("Ambient Temperature"),
                                            new DataColumn("Atmospheric Pressure"),
                                            new DataColumn("Ambient Relative Humidity"),
                                            new DataColumn("Retrieved By:"),
                                             new DataColumn("Date Retrieved"),
                                             new DataColumn("Field User Flag")

             });

            DataTable dte = new DataTable("Shipping Fields");
            dte.Columns.AddRange(new DataColumn[8] {new DataColumn("Chain of Custody Fields"),
                                            new DataColumn("Date Shipped To Lab"),
                                            new DataColumn("Shipped To"),
                                            new DataColumn("Shipping Company"),
                                            new DataColumn("Waybill Number"),
                                             new DataColumn("VOC CannisterPressure Before Shipping"),

                                              new DataColumn("Shipped By"),
                                            new DataColumn("Exported")


             });
            DataTable dtf = new DataTable("Travel Blank(s)");
            dtf.Columns.AddRange(new DataColumn[7] {new DataColumn("Chain of Custody Fields"),
                                            new DataColumn("WBEA Sample ID"),
                                            new DataColumn("Prepared By"),
                                            new DataColumn("Received From Lab"),
                                            new DataColumn("Avg. Storage Temperature"),
                                             new DataColumn("Media Serial Number"),

                                              new DataColumn("Lab Sample ID")



             });

            DataTable dtg = new DataTable("Note(s)");
            dtg.Columns.AddRange(new DataColumn[2] {new DataColumn("Chain of Custody Fields"),
                                            new DataColumn("Notes")




             });
            //   string whereClause = " docit.samples.sample_id not in (select cs.sample_id from docit.chainofcustodys_samples cs) ";
            string whereClause = "1=1";
            if (typeId > 0) { whereClause = whereClause + " AND chainofcustodys.sample_type_id =" + typeId; }
            if (strfrom != "" && strto != "") { whereClause = whereClause + " AND chainofcustodys.date_sampling_scheduled  between '" + strfrom + "' and '" + strto + "'"; }
            //   System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", whereClause);

            List<ChainOfCustody> cocs = BaseModel.FetchAll<ChainOfCustody>(whereClause);
            string strsampler_name = "", strlocation_name = "", strlocation_fullname = "";



            foreach (var c in cocs)
            {
                try
                {
                    if (c.Samples.Count > 0)
                    {
                        foreach (var i in c.Samples)
                        {
                            dta.Rows.Add(c.chain_of_custody_id, i.wbea_id, i.PreparedBy, i.DateReceivedFromLab, (i.average_storage_temperature + i.AverageStorageTemperatureUnit), i.media_serial_number, i.lab_sample_id);
                        }
                    }
                    else
                        dta.Rows.Add(c.chain_of_custody_id, "", "", "", "", "", "");

                    if (c.Deployment.Location != null)
                    {
                        strlocation_name = c.Deployment.Location.name;
                        strlocation_fullname = c.Deployment.Location.full_name;
                    }
                    else
                    {
                        strlocation_name = "";
                        strlocation_fullname = "";
                    }
                    if (c.Deployment.Sampler != null)
                    {
                        strsampler_name = c.Deployment.Sampler.name;
                        dtc.Rows.Add(c.chain_of_custody_id, c.Deployment.date_deployed, c.Deployment.DeployedBy, strlocation_name, strlocation_fullname, strsampler_name, c.Deployment.Sampler.serial_number, c.Deployment.Sampler.make, c.Deployment.Sampler.model, c.Deployment.date_sampler_last_calibrated, c.Deployment.date_sampler_last_leak_checked, c.Deployment.date_sampling_head_cleaned_on, (c.Deployment.sampler_flowrate + c.Deployment.SamplerFlowrateUnit), c.Deployment.sampler_setpoint, c.Deployment.date_sample_start, c.Deployment.date_sample_end, c.Deployment.travel_blank_present);

                    }
                    else
                    {
                        strsampler_name = "N/A (no samplers at this location)";
                        dtc.Rows.Add(c.chain_of_custody_id, c.Deployment.date_deployed, c.Deployment.DeployedBy, strlocation_name, strlocation_fullname, strsampler_name, "", "", "", "", "", "", "", "", c.Deployment.date_sample_start, c.Deployment.date_sample_end, c.Deployment.travel_blank_present);
                    }

                    if (c.Schedule != null)
                        dtb.Rows.Add(c.chain_of_custody_id, c.UserCreated, c.SampleType.name, c.Schedule.name, c.date_sampling_scheduled);
                    else
                        dtb.Rows.Add(c.chain_of_custody_id, c.UserCreated, c.SampleType.name, "None", c.date_sampling_scheduled);

                    dtd.Rows.Add(c.chain_of_custody_id, c.Retrieval.elapsed_sampling_duration + "Seconds", (c.Retrieval.sample_volume + c.Retrieval.SampleVolumeUnit), c.Retrieval.date_actual_sample_start, c.Retrieval.date_actual_sample_end, (c.Retrieval.average_station_temperature + c.Retrieval.AverageStationTemperatureUnit), (c.Retrieval.average_ambient_temperature + c.Retrieval.AverageAmbientTemperatureUnit), (c.Retrieval.average_barometric_pressure + c.Retrieval.AverageBarometricPressureUnit), c.Retrieval.average_relative_humidity, c.Retrieval.RetrievedBy, c.Retrieval.date_sample_retrieved, c.Retrieval.field_user_flag);
                    dte.Rows.Add(c.chain_of_custody_id, c.Shipping.date_shipped_to_lab, c.Shipping.ShippedTo, c.Shipping.ShippingCompany, c.Shipping.waybill_number, c.Shipping.voc_cannister_pressure_before_shipping + c.Shipping.VOCCannisterPressureBeforeShippingUnit, c.Shipping.ShippedBy, c.Shipping.printed);

                    if (c.TravelBlanks.Count > 0)
                    {
                        foreach (var i in c.TravelBlanks)
                        {

                            dtf.Rows.Add(c.chain_of_custody_id, i.wbea_id, i.PreparedBy, i.DateReceivedFromLab, (i.average_storage_temperature + i.AverageStorageTemperatureUnit), i.media_serial_number, i.lab_sample_id);

                        }
                    }
                    else
                        dtf.Rows.Add(c.chain_of_custody_id, "", "", "", "", "", "");
                    if (c.Note.Count > 0)
                    {
                        foreach (var i in c.Note)
                        {

                            dtg.Rows.Add(c.chain_of_custody_id, i.body);

                        }
                    }
                    else
                        dtg.Rows.Add(c.chain_of_custody_id, " ");
                }
                catch (Exception ex)
                { //System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", ex.Message.ToString()); 
                    this.AddViewNotice(ex.Message.ToString());
                }
                ViewData["Loading"] = "No";




            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dta);

                wb.Worksheets.Add(dtb);
                wb.Worksheets.Add(dtc);
                wb.Worksheets.Add(dtd);
                wb.Worksheets.Add(dte);
                wb.Worksheets.Add(dtf);
                wb.Worksheets.Add(dtg);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ChainOfCustody_" + DateTime.Now.ToISODate() + ".xlsx");
                }


            }

        }
        public ActionResult ExportCOC(FormCollection collection)
        {


            string strfrom = Session["SelectedFrom"].ToString().IsBlank() ? "" : Session["SelectedFrom"].ToString();
            string strto = Session["SelectedTo"].ToString().IsBlank() ? "" : Session["SelectedTo"].ToString();
            int typeId = (int)Session["SelectedType"];

            deleteFiles(Directory.GetFiles(Server.MapPath("~/App_Data")));



            string strCurrent = DateTime.Now.ToString("MMM_yyyy");

            string strFileName = "";

            DataTable dta = new DataTable("Associated Sample(s)");
            dta.Columns.AddRange(new DataColumn[6] { new DataColumn("WBEA Sample ID"),
                                            new DataColumn("PreparedBy:"),
                                            new DataColumn("Received From Lab"),
                                            new DataColumn("Avg.Storage Temperture"),
                                            new DataColumn("Media Serial Number"),
                                            new DataColumn("Lab Sample Id")

             });
            DataTable dtb = new DataTable("Chain of Custody Fields");
            dtb.Columns.AddRange(new DataColumn[5] { new DataColumn("Chain of Custody Fields"),
                                            new DataColumn("Created By:"),
                                            new DataColumn("Sample Type"),
                                            new DataColumn("Scgedule"),
                                            new DataColumn("Sampling Scheduled for")


             });
            DataTable dtc = new DataTable("Deployment Fields");
            dtc.Columns.AddRange(new DataColumn[16] { new DataColumn("Date Deployed"),
                                            new DataColumn("Deployed By:"),
                                            new DataColumn("Location Name"),
                                            new DataColumn("Location full name"),
                                            new DataColumn("Sampler Name"),
                                             new DataColumn("Sampler Serial Number:"),
                                            new DataColumn("Sampler Make"),
                                            new DataColumn("Sampler Model"),
                                            new DataColumn("Last Calibrated Date"),
                                             new DataColumn("Date of Last Leak ChecK"),
                                           new DataColumn(" Sampling Head Cleaned On"),
                                           new DataColumn(" Sampler Flowrate"),
                                           new DataColumn(" Sampler Setpoint"),
                                           new DataColumn(" Programmed Sample Start Date:"),
                                           new DataColumn(" Programmed Sample End Date"),
                                           new DataColumn(" Travel Blank Present")
             });
            DataTable dtd = new DataTable("Retrieval Fields");
            dtd.Columns.AddRange(new DataColumn[11] { new DataColumn("Elapsed Sampling Duration"),
                                            new DataColumn("Sample Volume"),
                                            new DataColumn("Actual Sample Start Date"),
                                            new DataColumn("Actual Sample End Date"),
                                            new DataColumn("Average Station Temperature"),
                                             new DataColumn("Ambient Temperature"),
                                            new DataColumn("Atmospheric Pressure"),
                                            new DataColumn("Ambient Relative Humidity"),
                                            new DataColumn("Retrieved By:"),
                                             new DataColumn("Date Retrieved"),
                                             new DataColumn("Field User Flag")

             });

            DataTable dte = new DataTable("Shipping Fields");
            dte.Columns.AddRange(new DataColumn[7] {
                                            new DataColumn("Date Shipped To Lab"),
                                            new DataColumn("Shipped To"),
                                            new DataColumn("Shipping Company"),
                                            new DataColumn("Waybill Number"),
                                             new DataColumn("VOC CannisterPressure Before Shipping"),

                                              new DataColumn("Shipped By"),
                                            new DataColumn("Exported")


             });
            DataTable dtf = new DataTable("Travel Blank(s)");
            dtf.Columns.AddRange(new DataColumn[6] {
                                            new DataColumn("WBEA Sample ID"),
                                            new DataColumn("Prepared By"),
                                            new DataColumn("Received From Lab"),
                                            new DataColumn("Avg. Storage Temperature"),
                                             new DataColumn("Media Serial Number"),

                                              new DataColumn("Lab Sample ID")



             });

            DataTable dtg = new DataTable("Note(s)");
            dtg.Columns.AddRange(new DataColumn[1] {
                                            new DataColumn("Notes")




             });
            //   string whereClause = " docit.samples.sample_id not in (select cs.sample_id from docit.chainofcustodys_samples cs) ";
            string whereClause = "1=1";
            if (typeId > 0) { whereClause = whereClause + " AND chainofcustodys.sample_type_id =" + typeId; }
            if (strfrom != "" && strto != "") { whereClause = whereClause + " AND chainofcustodys.date_sampling_scheduled  between '" + strfrom + "' and '" + strto + "'"; }
            //   System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", whereClause);

            List<ChainOfCustody> cocs = BaseModel.FetchAll<ChainOfCustody>(whereClause);
            string strsampler_name = "", strlocation_name = "", strlocation_fullname = "";

            using (var memoryStream = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    foreach (var c in cocs)
                    {
                        try
                        {
                            foreach (var i in c.Samples)
                            {
                                dta.Rows.Add(i.wbea_id, i.PreparedBy, i.DateReceivedFromLab, (i.average_storage_temperature + i.AverageStorageTemperatureUnit), i.media_serial_number, i.lab_sample_id);
                            }


                            if (c.Deployment.Location != null)
                            {
                                strlocation_name = c.Deployment.Location.name;
                                strlocation_fullname = c.Deployment.Location.full_name;
                            }
                            else
                            {
                                strlocation_name = "";
                                strlocation_fullname = "";
                            }
                            if (c.Deployment.Sampler != null)
                            {
                                strsampler_name = c.Deployment.Sampler.name;
                                dtc.Rows.Add(c.Deployment.date_deployed, c.Deployment.DeployedBy, strlocation_name, strlocation_fullname, strsampler_name, c.Deployment.Sampler.serial_number, c.Deployment.Sampler.make, c.Deployment.Sampler.model, c.Deployment.date_sampler_last_calibrated, c.Deployment.date_sampler_last_leak_checked, c.Deployment.date_sampling_head_cleaned_on, (c.Deployment.sampler_flowrate + c.Deployment.SamplerFlowrateUnit), c.Deployment.sampler_setpoint, c.Deployment.date_sample_start, c.Deployment.date_sample_end, c.Deployment.travel_blank_present);

                            }
                            else
                            {
                                strsampler_name = "N/A (no samplers at this location)";
                                dtc.Rows.Add(c.Deployment.date_deployed, c.Deployment.DeployedBy, strlocation_name, strlocation_fullname, strsampler_name, "", "", "", "", "", "", "", "", c.Deployment.date_sample_start, c.Deployment.date_sample_end, c.Deployment.travel_blank_present);
                            }

                            if (c.Schedule != null)
                                dtb.Rows.Add(c.chain_of_custody_id, c.UserCreated, c.SampleType.name, c.Schedule.name, c.date_sampling_scheduled);
                            else
                                dtb.Rows.Add(c.chain_of_custody_id, c.UserCreated, c.SampleType.name, "None", c.date_sampling_scheduled);

                            dtd.Rows.Add(c.Retrieval.elapsed_sampling_duration + "Seconds", (c.Retrieval.sample_volume + c.Retrieval.SampleVolumeUnit), c.Retrieval.date_actual_sample_start, c.Retrieval.date_actual_sample_end, (c.Retrieval.average_station_temperature + c.Retrieval.AverageStationTemperatureUnit), (c.Retrieval.average_ambient_temperature + c.Retrieval.AverageAmbientTemperatureUnit), (c.Retrieval.average_barometric_pressure + c.Retrieval.AverageBarometricPressureUnit), c.Retrieval.average_relative_humidity, c.Retrieval.RetrievedBy, c.Retrieval.date_sample_retrieved, c.Retrieval.field_user_flag);
                            dte.Rows.Add(c.Shipping.date_shipped_to_lab, c.Shipping.ShippedTo, c.Shipping.ShippingCompany, c.Shipping.waybill_number, c.Shipping.voc_cannister_pressure_before_shipping + c.Shipping.VOCCannisterPressureBeforeShippingUnit, c.Shipping.ShippedBy, c.Shipping.printed);
                            foreach (var i in c.TravelBlanks)
                            {
                                dtf.Rows.Add(i.wbea_id, i.PreparedBy, i.DateReceivedFromLab, (i.average_storage_temperature + i.AverageStorageTemperatureUnit), i.media_serial_number, i.lab_sample_id);
                            }

                            foreach (var i in c.Notes)
                            {
                                dtg.Rows.Add(i.body);
                            }
                            strFileName = Server.MapPath("~/App_Data/COC_" + c.chain_of_custody_id + ".xlsx");
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dta);

                                wb.Worksheets.Add(dtb);
                                wb.Worksheets.Add(dtc);
                                wb.Worksheets.Add(dtd);
                                wb.Worksheets.Add(dte);
                                wb.Worksheets.Add(dtf);
                                wb.Worksheets.Add(dtg);
                                wb.SaveAs(strFileName);


                            }

                            zip.AddFile(strFileName, "");

                            dta.Rows.Clear();
                            dtb.Rows.Clear();
                            dtc.Rows.Clear();
                            dtd.Rows.Clear();
                            dte.Rows.Clear();
                            dtf.Rows.Clear();
                            //strdownloadMessage = strdownloadMessage + "Passive_COC_" + cocId + ".pdf" + " ";
                        }
                        catch (Exception ex)
                        { //System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", ex.Message.ToString()); 
                            this.AddViewNotice(ex.Message.ToString());
                        }
                    }
                    zip.Save(memoryStream);
                    ViewData["Loading"] = "No";
                    // ViewBag.Message = strdownloadMessage + "have been downloaded to " + Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Downloads");
                }

                deleteCOCFiles(Directory.GetFiles(Server.MapPath("~/App_Data")));
                return File(memoryStream.ToArray(), "application/zip", "ChainOfCustody_" + DateTime.Now.ToISODate() + ".zip");


            }
        }

        public ActionResult BatchCOCExport(int? page, int? page_size, int? sample_type_id, string file_type_id)
        {
            string orderByClause = "";
            string joinClause = "";
            string whereClause = "1 < 1";
            //Populate filter drop downs.
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample_type_id);
            ViewData["file_type_id"] = new SelectList(new string[] { "Excel", "Zip" }, file_type_id ?? "Excel");
            ViewData["Loading"] = "No";
            ViewData["HasCOC"] = "No";
            var urlParameters = new { action = "BatchCOCsExport", controller = "ChainOfCustody", sample_type_id = sample_type_id };
            Paginator paginator = this.AddDefaultPaginator<ChainOfCustody>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);

            List<ChainOfCustody> chainOfCustodys = BaseModel.FetchPage<ChainOfCustody>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

            return View(chainOfCustodys);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BatchCOCExport(int? page, int? page_size, int? sample_type_id, string file_type_id, string date_from, string date_to)
        {
            string joinClause = "";
            string whereClause = "1 = 1";
            string orderByClause = "";

            //Populate filter drop downs.
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample_type_id);
            ViewData["file_type_id"] = new SelectList(new string[] { "Excel", "Zip" }, file_type_id ?? "Excel");

            //Sort




            orderByClause += "chain_of_custody_id DESC";




            if (sample_type_id.HasValue)
            {
                whereClause += " AND ChainOfCustodys.sample_type_id = " + sample_type_id.Value;
                ViewData["TypeId"] = sample_type_id.Value;
            }
            else
            {
                ViewData["TypeId"] = 0;
            }

            if (!date_from.IsBlank() && !date_to.IsBlank())

            {
                whereClause += " AND  ChainOfCustodys.date_sampling_scheduled  between '" + date_from + "' and '" + date_to + "'";
                ViewData["From"] = date_from;
                ViewData["To"] = date_to;
            }
            else
            {
                ViewData["From"] = "";
                ViewData["To"] = "";
            }
            if (file_type_id == "Excel")
                ViewData["Type"] = "Excel";
            else
                ViewData["Type"] = "Zip";

            ViewData["Loading"] = "No";
            //paginate
            if (!page_size.HasValue)
            {
                page_size = 40;
            }

            int cocCount = BaseModel.TotalCount("ChainOfCustodys", whereClause);
            page_size = cocCount;
            var urlParameters = new { action = "BatchCOCExport", controller = "ChainOfCustody", sample_type_id = sample_type_id, date_from, date_to };
            Paginator paginator = this.AddDefaultPaginator<ChainOfCustody>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);

            List<ChainOfCustody> chainOfCustodys = BaseModel.FetchPage<ChainOfCustody>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

            //List<ChainOfCustody> cocs = BaseModel.FetchAll<ChainOfCustody>(whereClause);

            //System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", cocCount.ToString());

            // add notice if there is no records
            if (cocCount == 0)
            {
                this.AddViewNotice("No Chain of Custody forms were found.");
                ViewData["HasCOC"] = "No";

            }
            else
            {
                if (file_type_id == "Excel")
                {
                    if (cocCount <= 5000)
                    { ViewData["HasCOC"] = "Yes"; }
                    else
                    {
                        ViewData["HasCOC"] = "No";
                        this.AddViewNotice("Search reacords is over limited(5000 Records) .");
                    }


                }
                else
                {
                    if (cocCount <= 150)
                    {
                        ViewData["HasCOC"] = "Yes";
                    }
                    else
                    {
                        this.AddViewNotice("Search reacords is over limited(150 Records) .");
                        ViewData["HasCOC"] = "No";
                    }
                }
            }

            return View(chainOfCustodys);

        }


        public ActionResult Index(int? page, int? page_size, int? sample_type_id, int? location_id, int? schedule_id, int? status_id, string progress_state, string sort)
        {
            string joinClause = "";
            string whereClause = "1 = 1";
            string orderByClause = "";

            //Populate filter drop downs.
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample_type_id);
            ViewData["location_id"] = Location.FetchSelectListActive(location_id);
            ViewData["schedule_id"] = Schedule.FetchSelectListActive(schedule_id);
            ViewData["status_id"] = Status.FetchSelectList(status_id);
            ViewData["progress_state"] = new SelectList(new string[] { "", "Incomplete", "Completed" }, progress_state ?? "Incompleted");

            if ((!sort.IsBlank() && sort.Contains("status")) || !progress_state.IsBlank())
            {
                joinClause += "LEFT OUTER JOIN Statuses ON ChainOfCustodys.status_id = Statuses.status_id ";
            }

            //Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                if (sort.Contains("sample-type"))
                {
                    joinClause += "INNER JOIN SampleTypes ON ChainOfCustodys.sample_type_id = SampleTypes.sample_type_id ";
                }

                if (sort.Contains("location"))
                {
                    joinClause += "INNER JOIN Locations ON ChainOfCustodys.location_id = Locations.location_id ";
                }

                if (sort.Contains("schedule"))
                {
                    joinClause += "LEFT OUTER JOIN Schedules ON ChainOfCustodys.schedule_id = Schedules.schedule_id ";
                }

                if (sort.Contains("wbea-id") || sort.Contains("media-serial"))
                {
                    joinClause += "LEFT OUTER JOIN ChainOfCustodys_Samples ON ChainOfCustodys.chain_of_custody_id = ChainOfCustodys_Samples.chain_of_custody_id " +
                        "LEFT OUTER JOIN Samples ON ChainOfCustodys_Samples.sample_id = Samples.sample_id ";
                }

                string[] sortOrder = sort.Split('_');
                foreach (string column in sortOrder)
                {
                    switch (column)
                    {
                        case "sample-type":
                            orderByClause += "SampleTypes.name,";
                            break;
                        case "desc-sample-type":
                            orderByClause += "SampleTypes.name DESC,";
                            break;
                        case "location":
                            orderByClause += "Locations.name,";
                            break;
                        case "desc-location":
                            orderByClause += "Locations.name DESC,";
                            break;
                        case "wbea-id":
                            orderByClause += "Samples.wbea_id,";
                            break;
                        case "desc-wbea-id":
                            orderByClause += "Samples.wbea_id DESC,";
                            break;
                        case "media-serial":
                            orderByClause += "Samples.media_serial_number,";
                            break;
                        case "desc-media-serial":
                            orderByClause += "Samples.media_serial_number DESC,";
                            break;
                        case "schedule":
                            orderByClause += "Schedules.name,";
                            break;
                        case "desc-schedule":
                            orderByClause += "Schedules.name DESC,";
                            break;
                        case "scheduled-date":
                            orderByClause += "ChainOfCustodys.date_sampling_scheduled,";
                            break;
                        case "desc-scheduled-date":
                            orderByClause += "ChainOfCustodys.date_sampling_scheduled DESC,";
                            break;
                        case "status":
                            orderByClause += "Statuses.sort,";
                            break;
                        case "desc-status":
                            orderByClause += "Statuses.sort DESC,";
                            break;
                    }
                }
            }
            else
            {
                orderByClause += "chain_of_custody_id DESC";
            }

            orderByClause = orderByClause.Trim(',');

            //filter
            if (sample_type_id.HasValue)
            {
                whereClause += " AND ChainOfCustodys.sample_type_id = " + sample_type_id.Value;
            }

            if (location_id.HasValue)
            {
                whereClause += " AND ChainOfCustodys.location_id = " + location_id.Value;
            }

            if (schedule_id.HasValue)
            {
                whereClause += " AND ChainOfCustodys.schedule_id = " + schedule_id.Value;
            }

            if (status_id.HasValue)
            {
                whereClause += " AND ChainOfCustodys.status_id = " + status_id.Value;
            }

            if (!progress_state.IsBlank())
            {
                switch (progress_state.ToLower().Trim())
                {
                    case "completed":
                        whereClause += " AND Statuses.is_completed = 1";
                        break;
                    case "incomplete":
                        whereClause += " AND Statuses.is_completed = 0";
                        break;
                    default:
                        //do nothing
                        break;
                }
            }

            //paginate
            var urlParameters = new { action = "Index", controller = "ChainOfCustody", sample_type_id = sample_type_id, location_id = location_id, schedule_id = schedule_id, status_id = status_id, progress_state = progress_state, sort = sort };
            Paginator paginator = this.AddDefaultPaginator<ChainOfCustody>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);

            List<ChainOfCustody> chainOfCustodys = BaseModel.FetchPage<ChainOfCustody>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

            // add notice if there is no records
            if (chainOfCustodys.Count == 0)
            {
                this.AddViewNotice("No Chain of Custody forms were found.");
            }

            return View(chainOfCustodys);
        }

        public ActionResult Details(string id)
        {
            ViewBag.message = "";
            if (id.IsBlank())
            {
                this.AddTempNotice("Invalid Chain of Custody Id.");
                return RedirectToAction("Index");
            }

            ChainOfCustody selectedCoC = ChainOfCustody.Load(id);

            ViewData["parent_id"] = selectedCoC.id;
            ViewData["parent_type"] = Note.ParentType.ChainOfCustody;

            if (selectedCoC == null)
            {
                this.AddTempNotice("Chain of custody " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(selectedCoC);
        }

        public ActionResult Edit(string id, string editSection)
        {
            if (id.IsBlank())
            {
                this.AddTempNotice("Invalid Chain of Custody Id.");
                return RedirectToAction("Index");
            }

            if (editSection.IsBlank())
            {
                this.AddTempNotice("You must provide the section to be edited.");
                return RedirectToAction("Details", new { id = id });
            }

            ChainOfCustody selectedCoC = ChainOfCustody.Load(id);

            if (selectedCoC == null)
            {
                this.AddTempNotice("Chain of Custody " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            if (selectedCoC.Status.ToString() == "Historical")
            {
                this.AddTempNotice("Chain of Custody " + id + " is historical and can not be edited.");
                return RedirectToAction("Details", new { id = id });
            }
            PopulateDropDowns(selectedCoC);
            return View(selectedCoC);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, string editSection, FormCollection collection)
        {
            ChainOfCustody modifiedCoC = new ChainOfCustody(id);
            FilterForNACheckBoxes(collection, true);
            try
            {
                BaseSection modifiedSection = UpdateSection(collection["section"], modifiedCoC, collection);
                UpdateModel(modifiedSection, collection.ToValueProvider());
                modifiedCoC.CommitEdits(modifiedSection, this.GetUser());

                return RedirectToAction("Details", new { id = modifiedCoC.id });
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                ModelException error = new ModelException(e);
                this.PopulateViewWithErrorMessages(error);
            }

            PopulateDropDowns(modifiedCoC);
            return View(modifiedCoC);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Getschedule(string stateId)
        {
            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", stateId);

            //  if (!string.IsNullOrEmpty(stateId))
            // {


            return Json(Schedule.Fetchschedulelist(stateId), JsonRequestBehavior.AllowGet);

            // }
        }
        public ActionResult Create()
        {
            ViewData["sample_type_id"] = SampleType.FetchSelectListName();
            ViewData["schedule_id"] = Schedule.Fetchschedulelist("1");
            return View();
        }

        public ActionResult AddCOCExport(string id)
        {
            return View();
        }
        public ActionResult AddBatchCOCExport(string id)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {


            /*   if (!string.IsNullOrEmpty(collection["scheduleId"].ToString()))
               System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", collection["scheduleId"].ToString());
              else
                   System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", "test");*/

            try
            {

                ChainOfCustody newChainOfCustody = new ChainOfCustody();
                UpdateModel(newChainOfCustody);
                newChainOfCustody.created_by = this.GetUser().user_id;
                newChainOfCustody.date_opened = DateTime.Now.ToISODateTime();
                if (!string.IsNullOrEmpty(collection["scheduleId"].ToString()))
                {
                    newChainOfCustody.schedule_id = collection["scheduleId"].ToString();
                    var schedule = Schedule.Load(collection["scheduleId"].ToString());
                    newChainOfCustody.location_id = schedule.location_id;
                }

                newChainOfCustody.Create();

                return RedirectToAction("Open", new { id = newChainOfCustody.chain_of_custody_id });
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(new ModelException(e));
                ViewData["sample_type_id"] = SampleType.FetchSelectListName();
                return View(collection);
            }
        }

        public ActionResult CreateWithSchedule(string id, string date_sample_start)
        {
            ChainOfCustody coctemplate = new ChainOfCustody();
            coctemplate.schedule_id = id;
            coctemplate.date_sampling_scheduled = date_sample_start;
            coctemplate.created_by = this.GetUser().user_id;
            return View(coctemplate);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateWithSchedule(FormCollection collection)
        {
            ChainOfCustody newChainOfCustody = new ChainOfCustody();
            try
            {
                UpdateModel(newChainOfCustody, collection.ToValueProvider()); //NOTE: paramet
                newChainOfCustody.location_id = newChainOfCustody.Schedule.location_id;
                newChainOfCustody.sample_type_id = newChainOfCustody.Schedule.sample_type_id;
                newChainOfCustody.date_sampling_scheduled = collection["date_sampling_scheduled"];
                newChainOfCustody.created_by = this.GetUser().user_id;
                newChainOfCustody.date_opened = DateTime.Now.ToISODateTime();
                newChainOfCustody.Create();

                return RedirectToAction("open", new { id = newChainOfCustody.chain_of_custody_id });
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(new ModelException(e));
                return View(newChainOfCustody);
            }
        }

        public ActionResult Open(string id)
        {

            if (id.IsBlank())
            {
                this.AddTempNotice("Please enter the Chain of Custody Id to be opened.");
                return RedirectToAction("Index");
            }

            if (ChainOfCustody.Load(id) == null)
            {
                this.AddTempNotice("Chain of Custody " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            ChainOfCustody selectedCoC = new ChainOfCustody(id);

            if (selectedCoC.Shipping.printed.IsBlank())
            {
                selectedCoC.Shipping.printed = "False";
            }

            PopulateDropDowns(selectedCoC);

            return View(selectedCoC);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OpenWithId(FormCollection collection)
        {

            return RedirectToAction("Open", new { id = collection["id"] });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OpenWithWbeaId(FormCollection collection)
        {
            if (collection["wbeaId"].IsBlank())
            {
                this.AddTempNotice("Please enter a vaid WBEA Sample ID");
                return RedirectToAction("Index");
            }

            string id = ChainOfCustody.FindCoCIdWithWbeaId(collection["wbeaId"]);
            if (id.IsBlank())
            {
                this.AddTempNotice("No Chain of Custody found for WBEA Sample ID " + collection["wbeaId"] + ".");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Open", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Open(string id, FormCollection collection)
        {
            ChainOfCustody openedChainOfCustody = ChainOfCustody.Load(id);


            if (openedChainOfCustody == null)
            {
                this.AddTempNotice("Error: Unable to open target Chain of Custody");
                return RedirectToAction("Index");
            }

            try
            {
                FilterForNACheckBoxes(collection, collection["form_action"].ToLower() == "commit");
                BaseSection section = UpdateSection(collection["section"], openedChainOfCustody, collection);
                UpdateModel(section, collection.ToValueProvider());
                switch (collection["form_action"].ToLower())
                {
                    case "save":
                        openedChainOfCustody.Save(section, this.GetUser());
                        break;
                    case "commit":
                        User user = this.GetUser();
                        section.CommittedBy(user.user_id);
                        openedChainOfCustody.Commit(section, user);
                        break;
                    default:
                        throw new Exception("Error: Invalid form action " + collection["form_action"].ToLower() + ". Chain of custody not saved.");
                }

                if (!collection["auto_save"].IsBlank())
                {
                    this.AddTempNotice("Auto-saved form at " + DateTime.Now.ToISODateTime());
                }

                return RedirectToAction("Open", new { id = openedChainOfCustody.id });
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                ModelException error = new ModelException(e);
                this.PopulateViewWithErrorMessages(error);
            }

            if (!collection["auto_save"].IsBlank())
            {
                this.AddTempNotice("Auto-save was attempted at " + DateTime.Now.ToISODateTime() + ", but not all fields were saved due to errors.");
            }

            PopulateDropDowns(openedChainOfCustody);
            return View(openedChainOfCustody);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetLocationDetails(string location_id)
        {
            if (String.IsNullOrEmpty(location_id))
            {
                return Json(new { full_name = "unspecified" }, JsonRequestBehavior.AllowGet);
            }

            Location selectedLocation = Location.Load(location_id);

            string fullName = selectedLocation.full_name;
            if (fullName.IsBlank())
            {
                fullName = "none";
            }

            return Json(new { full_name = fullName }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSamplerDetails(string sampler_item_id)
        {
            if (!String.IsNullOrEmpty(sampler_item_id) && sampler_item_id.IsInt())
            {

                Item selectedItem = Item.Load(sampler_item_id);
                if (selectedItem != null)
                {
                    string serialNumber = selectedItem.serial_number;
                    if (serialNumber.IsBlank())
                    {
                        serialNumber = "none";
                    }

                    string samplerMake = selectedItem.make.name;
                    if (samplerMake.IsBlank())
                    {
                        samplerMake = "none";
                    }

                    string samplerModel = selectedItem.model.name;
                    if (samplerModel.IsBlank())
                    {
                        samplerModel = "none";
                    }

                    return Json(new
                    {
                        serial_number = serialNumber,
                        make = samplerMake,
                        model = samplerModel
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                serial_number = "unspecified",
                make = "unspecified",
                model = "unspecified"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSamplerDropdown(string sample_type_id, string location_id, string sampler_item_id)
        {
            ViewData["sampler_item_id_selectlist"] = Item.FetchSelectList(sample_type_id, location_id, sampler_item_id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult PreviewSample(string sample_id)
        {
            Sample sample = Sample.Load(sample_id);
            if (sample == null) return Content(string.Empty);
            ViewData["Preview"] = true;
            return View("AssociatedSample", sample);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult NewPreviewSamplen(string sample_id)
        {
            Sample sample = Sample.Load(sample_id);
            if (sample == null) return Content(string.Empty);
            ViewData["Preview"] = true;
            return View("NewAssociatedSample", sample);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddSample(string id, FormCollection collection)
        {
            ChainOfCustody coc = new ChainOfCustody(id);

            // add the id to the CoC's list
            if (collection["sample_id"].IsBlank())
            {
                return Content("<div>Error adding sample.</div>");
            }

            coc.AddSample(collection["sample_id"]);

            // Save the new id(s)
            coc.SaveRelatedSamples();

            return SampleResult(coc, collection["sample_id"]);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveSample(string id, FormCollection collection)
        {
            try
            {
                ChainOfCustody coc = new ChainOfCustody(id);

                // remove the id from the CoC's list
                coc.RemoveSample(collection["sample_id"]);

                // Save the new id(s)
                coc.SaveRelatedSamples();

                return SampleResult(coc, collection["sample_id"]);
            }
            catch
            {
                return Content("<div>Error removing sample.</div>");
            }
        }

        private ActionResult SampleResult(ChainOfCustody coc, string sample_id)
        {
            ViewData["coc"] = coc;
            var sample = Sample.Load(sample_id);
            if (!sample.is_travel_blank.ToBool())
            {
                // ViewData["sample_id"] = Sample.FetchSelectListAllUnassigned(coc.sample_type_id);
                ViewData["sample_id"] = Sample.FetchSelectListAllUnassignedWithoutOrphaned(coc.sample_type_id);
                return View("AssociatedSamplesSection", coc);
            }
            else
            {
                ViewData["travel_sample_id"] = Sample.FetchAvailableTravelBlanksSelectList(coc);
                return View("TravelBlanksSection", coc);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NoteCreate(string chain_of_custody_id, string comment)
        {
            var coc = ChainOfCustody.Load(chain_of_custody_id);
            if (coc == null)
            {
                return null;
            }

            string coc_type = ((int)Note.ParentType.ChainOfCustody).ToString();
            string now = DateTime.Now.ToISODateTime();

            var item = new Note()
            {
                body = comment,
                parent_type_id = coc_type,
                parent_id = chain_of_custody_id,
                location_id = coc.Deployment.location_id,
                created_by = this.GetUser().id,
                modified_by = this.GetUser().id,
                date_occurred = DateTime.Now.ToISODate(),
                date_created = DateTime.Now.ToISODateTime(),
                date_modified = DateTime.Now.ToISODateTime(),
                starred = false,
                deleted = false,
                committed = false,
            };

            if (item.location_id.IsBlank())
            {
                var unassigned = Location.LoadByName("Unassigned");
                if (unassigned != null)
                {
                    item.location_id = unassigned.id;
                }
            }

            try
            {
                if (!item.body.IsBlank())
                {
                    item.Save();
                }
            }
            catch
            {
                // do nothing?
            }
            return View("QuickNoteCommentBlock", coc.Notes);
        }

        private BaseSection UpdateSection(string section, ChainOfCustody target, FormCollection collection)
        {
            switch (section)
            {
                case "Prepare":
                    UpdateModel(target.Preparation, collection.ToValueProvider());
                    return target.Preparation;
                case "Deploy":
                    UpdateModel(target.Deployment, collection.ToValueProvider());
                    return target.Deployment;
                case "Retrieve":
                    UpdateModel(target.Retrieval, collection.ToValueProvider());
                    return target.Retrieval;
                case "Ship":
                    UpdateModel(target.Shipping, collection.ToValueProvider());
                    return target.Shipping;
                default:
                    throw new Exception("Error: Cannot save unknown chain of custody section " + section);
            }
        }

        private void PopulateDropDowns(ChainOfCustody coc)
        {
            //  ViewData["sample_id"] = Sample.FetchSelectListAllUnassigned(coc.sample_type_id);
            ViewData["sample_id"] = Sample.FetchSelectListAllUnassignedWithoutOrphaned(coc.sample_type_id);
            ViewData["travel_sample_id"] = Sample.FetchAvailableTravelBlanksSelectList(coc);

            //Only need to populate the dropdownlist if there is no associated schedule.
            if (String.IsNullOrEmpty(coc.schedule_id))
            {
                ViewData["location_id_selectlist"] = Location.FetchSelectListActive(coc.Deployment.location_id);
                ViewData["location_id"] = coc.Deployment.location_id;
            }

            ViewData["sampler_item_id_selectlist"] = Item.FetchSelectList(coc.sample_type_id, coc.Deployment.location_id, coc.Deployment.sampler_item_id);
            ViewData["sampler_item_id"] = coc.Deployment.sampler_item_id;

            ViewData["collecting_duplicate"] = ControllersCommon.GetTrueFalseSelectList(coc.Deployment.collecting_duplicate);
            ViewData["travel_blank_present"] = ControllersCommon.GetTrueFalseSelectList(coc.Deployment.travel_blank_present);
            ViewData["voc_valve_open"] = ControllersCommon.GetTrueFalseSelectList(coc.Deployment.voc_valve_open);
            ViewData["voc_valve_closed"] = ControllersCommon.GetTrueFalseSelectList(coc.Retrieval.voc_valve_closed);
            ViewData["shipped_to"] = Lab.FetchAllActiveSelectList(coc.Shipping.shipped_to);
            ViewData["shipping_company"] = ShippingCompany.FetchAllActiveSelectList(coc.Shipping.shipping_company);

            ViewData["printed"] = new SelectList(
                new Dictionary<string, string>() {
                { "False", "no" },
                { "True", "yes" },
            }, "Key", "Value", coc.Shipping.printed);
        }

        /*
        private void CombineDatesWithTimes(FormCollection collection) {
            List<string> keys = new List<string>(collection.AllKeys);
            foreach (string key in keys) {
                string dateSuffix = "_date";
                if(key.EndsWith(dateSuffix)){
                    string field = key.Substring(0, key.Length - dateSuffix.Length);
                    
                    string timeSuffix = "_time";
                    string timeField = field + timeSuffix;
                    if (keys.Contains(field + timeSuffix)) {
                        collection[field] = (collection[key] + " " + collection[timeField]).Trim();
                    }
                }
            }
        }*/

        /// <summary>Find all CheckBoxNA fields </summary>
        private FormCollection FilterForNACheckBoxes(FormCollection collection, bool isCommit)
        {
            List<string> cbKeys = new List<string>();

            foreach (string key in collection.Keys)
            {
                if (key.StartsWith("cb-"))
                {
                    cbKeys.Add(key);
                }
            }

            foreach (string key in cbKeys)
            {
                if (ChainOfCustody.jQueryCheckBoxEnabled() == !collection[key].Contains("true"))
                {
                    string field = key.Substring(3);
                    if (isCommit)
                        collection[field] = "N/A";
                    else
                        collection[field] = string.Empty;
                }
                collection.Remove(key);
            }

            return collection;
        }
        public ActionResult BatchShipping(int? page, int? page_size, int? sample_type_id, string date_from, string date_to)
        {
            string joinClause = "";
            string whereClause = "";
            string orderByClause = "";

            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample_type_id);

            if (date_from.IsBlank() || date_to.IsBlank())
            {
                if (sample_type_id.HasValue)
                {
                    whereClause = " ChainOfCustodys.status_id in (7,8) AND ChainOfCustodys.sample_type_id = " + sample_type_id.Value;
                }
                else
                {
                    whereClause = " ChainOfCustodys.status_id =0";
                }
            }
            else
            {
                if (sample_type_id.HasValue)
                { whereClause = " ChainOfCustodys.status_id in (7,8) AND ChainOfCustodys.sample_type_id = " + sample_type_id.Value + " and isnull(date_sampling_scheduled,'2010-01-13 10:11:00.000') >='" + date_from + "' and isnull(date_sampling_scheduled,'2010-01-13 10:11:00.000')<='" + date_to + "'"; }

                else
                {
                    whereClause = " ChainOfCustodys.status_id in (7,8)  " + " and isnull(date_sampling_scheduled,'2010-01-13 10:11:00.000') >='" + date_from + "' and isnull(date_sampling_scheduled,'2010-01-13 10:11:00.000')<='" + date_to + "'";
                }
            }

            var urlParameters = new { action = "BatchShipping", controller = "ChainOfCustody", sample_type_id = sample_type_id, date_from = date_from, date_to = date_to };
            Paginator paginator = this.AddDefaultPaginator<ChainOfCustody>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);

            List<ChainOfCustody> chainOfCustodys = BaseModel.FetchPage<ChainOfCustody>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

            // add notice if there is no records
            if ((date_from.IsBlank() && date_to.IsBlank()) || !sample_type_id.HasValue)

            {
                if (chainOfCustodys.Count == 0)
                {
                    this.AddViewNotice("Enter date range or Type.");
                }
            }
            else
            {
                if (chainOfCustodys.Count == 0)
                {
                    this.AddViewNotice("No Chain of Custody forms were found.");
                }
            }

            return View(chainOfCustodys);


        }

        // [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateBatchShipping(int? page, int? page_size, string id)
        {
            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", collection["parameter_list"].ToString());
            string joinClause = "";
            string whereClause = "";
            string orderByClause = "";
            if (id.Contains("#")) { id = id.Replace("#", ","); }
            string strcocid = id.Substring(0, id.Length - 1);
            ViewData["cocstrId"] = "'" + strcocid + "'";
            string[] firstid = strcocid.Split(',');

            strcocid = "(" + strcocid + ")";

            ChainOfCustody selectedCoC = new ChainOfCustody(firstid[0]);
            PopulateDropDowns(selectedCoC);
            ViewData["cocModel"] = selectedCoC;
            if (!page_size.HasValue)
            {
                page_size = 50;
            }
            whereClause = " chain_of_custody_id in " + strcocid;
            var urlParameters = new { action = "CreateBatchShipping", controller = "ChainOfCustody", sample_type_id = id };
            Paginator paginator = this.AddDefaultPaginator<ChainOfCustody>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);

            List<ChainOfCustody> chainOfCustodys = BaseModel.FetchPage<ChainOfCustody>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

            return View(chainOfCustodys);


        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateBatchShipping(string id, FormCollection collection)
        {
            int index = 0;


            string[] idlist = null;
            // string[] Pressurebeforeshipping = null;
            string[] pressureunitlist = null;

            List<ChainOfCustody> coclist = new List<ChainOfCustody>();
            //   if (id != null)
            //  {
            string strcocid = collection["passId"].ToString();
            ViewData["cocstrId"] = "'" + strcocid + "'";
            idlist = strcocid.Split(',');
            //  }
            //  else
            // { System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", collection["pressureunit2"].ToString()); }

            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", collection["voc_cannister_pressure_before_shipping3"].ToString());

            //  if (collection["voc_cannister_pressure_before_shipping"] != null) Pressurebeforeshipping = collection["voc_cannister_pressure_before_shipping"].ToString().Split(',');
            if (collection["voc_cannister_pressure_before_shipping_unit"] != null) pressureunitlist = collection["voc_cannister_pressure_before_shipping_unit"].ToString().Split(',');

            foreach (var strid in idlist)
            {
                ChainOfCustody openedChainOfCustody = ChainOfCustody.Load(strid);

                if (openedChainOfCustody == null)
                {
                    this.AddTempNotice("Error: Unable to open target Chain of Custody");
                    return RedirectToAction("CreateBatchShipping");
                }

                try
                {
                    FilterForNACheckBoxes(collection, collection["form_action"].ToLower() == "commit");
                    BaseSection section = UpdateSection(collection["section"], openedChainOfCustody, collection);
                    UpdateModel(section, collection.ToValueProvider());

                    switch (collection["form_action"].ToLower())
                    {
                        case "save":
                            openedChainOfCustody.shipped_by = this.GetUser().user_id;
                            // openedChainOfCustody.Shipping.voc_cannister_pressure_before_shipping = Pressurebeforeshipping[index];
                            openedChainOfCustody.Shipping.voc_cannister_pressure_before_shipping = collection["voc_cannister_pressure_before_shipping" + (index + 1).ToString()].ToString();
                            if (collection["voc_cannister_pressure_before_shipping" + (index + 1).ToString()] != "")
                            { openedChainOfCustody.Shipping.voc_cannister_pressure_before_shipping_unit = pressureunitlist[index]; }
                            else
                            {
                                openedChainOfCustody.Shipping.voc_cannister_pressure_before_shipping_unit = null;

                            }

                            openedChainOfCustody.Save(section, this.GetUser());
                            break;
                        case "commit":
                            User user = this.GetUser();
                            openedChainOfCustody.shipped_by = this.GetUser().user_id;
                            openedChainOfCustody.Shipping.voc_cannister_pressure_before_shipping = collection["voc_cannister_pressure_before_shipping" + (index + 1).ToString()].ToString();
                            if (collection["voc_cannister_pressure_before_shipping" + (index + 1).ToString()] != "")
                            { openedChainOfCustody.Shipping.voc_cannister_pressure_before_shipping_unit = pressureunitlist[index]; }
                            else
                            {
                                openedChainOfCustody.Shipping.voc_cannister_pressure_before_shipping_unit = null;

                            }
                            section.CommittedBy(user.user_id);
                            openedChainOfCustody.Commit(section, user);
                            break;
                        default:
                            throw new Exception("Error: Invalid form action " + collection["form_action"].ToLower() + ". Chain of custody not saved.");
                    }

                    if (!collection["auto_save"].IsBlank())
                    {
                        this.AddTempNotice("Auto-saved form at " + DateTime.Now.ToISODateTime());
                    }

                    // return RedirectToAction("Open", new { id = openedChainOfCustody.id });
                }
                catch (ModelException me)
                {
                    this.PopulateViewWithErrorMessages(me);
                }
                catch (Exception e)
                {
                    ModelException error = new ModelException(e);
                    this.PopulateViewWithErrorMessages(error);
                }

                if (!collection["auto_save"].IsBlank())
                {
                    this.AddTempNotice("Auto-save was attempted at " + DateTime.Now.ToISODateTime() + ", but not all fields were saved due to errors.");
                }

                //   PopulateDropDowns(openedChainOfCustody);
                coclist.Add(openedChainOfCustody);
                index = index + 1;
            }
            ChainOfCustody selectedCoC = coclist[0];
            PopulateDropDowns(selectedCoC);
            ViewData["cocModel"] = selectedCoC;

            return View(coclist);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BatchNoteCreate(string chain_of_custody_id, string comment, FormCollection collection)
        {
            string[] idlist = null;
            string strcocid = "";
            if (collection["NoteId"] != null)
            {
                strcocid = collection["NoteId"].ToString();

                ViewData["cocstrId"] = "'" + strcocid + "'";
                idlist = strcocid.Split(',');

                //  System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", collection["NoteId"].ToString());
                foreach (var cocid in idlist)
                {
                    var coc = ChainOfCustody.Load(cocid);
                    if (coc == null)
                    {
                        return null;
                    }

                    string coc_type = ((int)Note.ParentType.ChainOfCustody).ToString();
                    string now = DateTime.Now.ToISODateTime();

                    var item = new Note()
                    {
                        body = comment,
                        parent_type_id = coc_type,
                        parent_id = cocid,
                        location_id = coc.Deployment.location_id,
                        created_by = this.GetUser().id,
                        modified_by = this.GetUser().id,
                        date_occurred = DateTime.Now.ToISODate(),
                        date_created = DateTime.Now.ToISODateTime(),
                        date_modified = DateTime.Now.ToISODateTime(),
                        starred = false,
                        deleted = false,
                        committed = false,
                    };

                    if (item.location_id.IsBlank())
                    {
                        var unassigned = Location.LoadByName("Unassigned");
                        if (unassigned != null)
                        {
                            item.location_id = unassigned.id;
                        }
                    }

                    try
                    {
                        if (!item.body.IsBlank())
                        {
                            item.Save();
                        }
                    }
                    catch
                    {
                        // do nothing?
                    }

                }
            }

            var coc1st = ChainOfCustody.Load(idlist[0]);
            return View("QuickNoteCommentBlock", coc1st.Notes);

        }


        public ActionResult BatchImportCOC()
        {
            ViewData["DuplicateData"] = "";
            ViewData["Status"] = "Upload";
            ViewData["Completed"] = "Yes";
            DataTable dt = new DataTable();
            string strPath = Path.Combine(Server.MapPath("~/App_Data/"), this.GetUser().user_id + "CompletedPasssiveAirSample.xlsx");
            //string strPath = @"c:\temp\CompletedPasssiveAirSample.xlsx";
            if (System.IO.File.Exists(strPath))
            {
                dt = ConvertExcelToDataTable(strPath);
                ViewData["ExportPDF"] = "Yes";
            }
            else
            { ViewData["ExportPDF"] = ""; }

            return View(dt);

        }

        private void GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
        }

        [HttpPost]
        public ActionResult BatchImportCOC(FormCollection collection)
        {
            DataTable dt = new DataTable();
            ViewData["DuplicateData"] = "";
            ViewData["ExportPDF"] = "";
            string strSampleFilePath = "~/App_Data/" + this.GetUser().user_id;
            switch (collection["form_action"])
            {
                case "Upload File":
                    ViewData["Status"] = "Verify";
                    if (ModelState.IsValid)
                    {
                        var postedfile = Request.Files["fileUpload"];
                        //Checking file content length and Extension must be .xlsx  
                        try
                        {
                            if (postedfile != null && postedfile.ContentLength > 0 && System.IO.Path.GetExtension(postedfile.FileName).ToLower() == ".xlsx")
                            {
                                // string filePath = Server.MapPath("~/bin/temp/") + Path.GetFileName(postedfile.FileName);
                                //     if (!Directory.Exists(Server.MapPath("~/UploadFile")))
                                //          Directory.CreateDirectory(Server.MapPath("~/UploadFile"));


                                //    GrantAccess(Server.MapPath("~/UploadFile"));




                                if (System.IO.File.Exists(Server.MapPath(strSampleFilePath + "DuplicatePasssiveAirSample.xlsx")))
                                    System.IO.File.Delete(Server.MapPath(strSampleFilePath + "DuplicatePasssiveAirSample.xlsx"));
                                if (System.IO.File.Exists(Server.MapPath(strSampleFilePath + "PasssiveAirSample.xlsx")))
                                    System.IO.File.Delete(Server.MapPath(strSampleFilePath + "PasssiveAirSample.xlsx"));
                                if (System.IO.File.Exists(Server.MapPath(strSampleFilePath + "newPasssiveAirSample.xlsx")))
                                    System.IO.File.Delete(Server.MapPath(strSampleFilePath + "newPasssiveAirSample.xlsx"));

                                if (System.IO.File.Exists(Server.MapPath(strSampleFilePath + "CompletedPasssiveAirSample.xlsx")))
                                    System.IO.File.Delete(Server.MapPath(strSampleFilePath + "CompletedPasssiveAirSample.xlsx"));

                                //  string filePath = Server.MapPath("~/UploadFile/"+ this.GetUser().user_id +"PasssiveAirSample.xlsx");
                                string filePath = Server.MapPath(strSampleFilePath + "PasssiveAirSample.xlsx");
                                postedfile.SaveAs(filePath);

                                //   string path = @"c:\temp\PasssiveAirSample.xlsx";

                                // Saving the file  

                                //   file.SaveAs(path);
                                // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", Path.GetFileName(file.FileName));
                                //Started reading the Excel file.  
                                using (XLWorkbook workbook = new XLWorkbook(filePath))
                                {
                                    IXLWorksheet worksheet = workbook.Worksheet(1);
                                    bool FirstRow = true;
                                    //Range for reading the cells based on the last cell used.  
                                    string readRange = "1:1";
                                    foreach (IXLRow row in worksheet.RowsUsed())
                                    {
                                        //If Reading the First Row (used) then add them as column name  
                                        if (FirstRow)
                                        {
                                            //Checking the Last cellused for column generation in datatable  
                                            readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                                            foreach (IXLCell cell in row.Cells(readRange))
                                            {
                                                dt.Columns.Add(cell.Value.ToString());
                                            }
                                            FirstRow = false;
                                        }
                                        else
                                        {
                                            //Adding a Row in datatable  
                                            dt.Rows.Add();
                                            int cellIndex = 0;
                                            //Updating the values of datatable  
                                            foreach (IXLCell cell in row.Cells(readRange))
                                            {
                                                dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                                cellIndex++;
                                            }
                                        }
                                    }
                                    //If no data in Excel file  
                                    if (FirstRow)
                                    {
                                        // ViewBag.Message = "Empty Excel File!";
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        { ViewBag.Message = ex.Message.ToString(); }
                    }

                    else
                    {
                        //If file extension of the uploaded file is different then .xlsx  
                        //  ViewBag.Message = "Please select file with .xlsx extension!";
                    }


                    break;

                case "Verify":
                    ViewData["Status"] = "Save";
                    ViewData["ExportPDF"] = "";
                    //  string strPath = Server.MapPath("~/UploadFile/" + this.GetUser().user_id + "PasssiveAirSample.xlsx");
                    string strPath = Server.MapPath(strSampleFilePath + "PasssiveAirSample.xlsx");
                    //  System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", strPath);
                    if (strPath != null)
                    {
                        using (XLWorkbook workbook = new XLWorkbook(strPath))
                        {
                            IXLWorksheet worksheet = workbook.Worksheet(1);
                            bool FirstRow = true;
                            //Range for reading the cells based on the last cell used.  
                            string readRange = "1:1";
                            foreach (IXLRow row in worksheet.RowsUsed())
                            {
                                //If Reading the First Row (used) then add them as column name  
                                if (FirstRow)
                                {
                                    //Checking the Last cellused for column generation in datatable  
                                    readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Columns.Add(cell.Value.ToString());
                                    }
                                    FirstRow = false;
                                }
                                else
                                {
                                    //Adding a Row in datatable  
                                    dt.Rows.Add();
                                    int cellIndex = 0;
                                    //Updating the values of datatable  
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                        cellIndex++;
                                    }
                                }
                            }


                        }
                        dt = verifyDuplicate(dt);
                        GenerateWBEAId(dt);
                        SaveasNewSpreadsheet(dt);
                        // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", COC_Counter(dt).ToString());
                    }
                    break;

                case "Save":
                    string strPath1 = Server.MapPath(strSampleFilePath + "newPasssiveAirSample.xlsx");
                    ViewData["Status"] = "Upload";
                    ViewData["ExportPDF"] = "";
                    if (strPath1 != null)
                    {
                        using (XLWorkbook workbook = new XLWorkbook(strPath1))
                        {
                            IXLWorksheet worksheet = workbook.Worksheet(1);
                            bool FirstRow = true;
                            //Range for reading the cells based on the last cell used.  
                            string readRange = "1:1";
                            foreach (IXLRow row in worksheet.RowsUsed())
                            {
                                //If Reading the First Row (used) then add them as column name  
                                if (FirstRow)
                                {
                                    //Checking the Last cellused for column generation in datatable  
                                    readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Columns.Add(cell.Value.ToString());
                                    }
                                    dt.Columns.Add("Export PassiveAirSample");
                                    FirstRow = false;
                                }
                                else
                                {
                                    //Adding a Row in datatable  
                                    dt.Rows.Add();
                                    int cellIndex = 0;
                                    //Updating the values of datatable  
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                        cellIndex++;
                                    }
                                }
                            }


                        }
                        if (dt.Rows.Count > 0)
                        {
                            GenerateWBEAIdandSave(dt);
                            SaveCocandSamples(dt);
                        }
                    }
                    break;




                //  System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", strDownloadPath);

                default:
                    throw new Exception("Error: Invalid form action " + collection["form_action"].ToLower());
            }

            return View(dt);
        }
        private List<string> COCIdList(DataTable dt)
        {
            List<string> IdsList = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (!IdsList.Contains(dt.Rows[i][1].ToString()) && dt.Rows[i][1].ToString() != "")
                {

                    IdsList.Add(dt.Rows[i][1].ToString());

                }


            }

            return IdsList;
        }

        private DataTable ConvertExcelToDataTable(string path)
        {
            DataTable dt = new DataTable();
            using (XLWorkbook workbook = new XLWorkbook(path))
            {
                IXLWorksheet worksheet = workbook.Worksheet(1);
                bool FirstRow = true;
                //Range for reading the cells based on the last cell used.  
                string readRange = "1:1";
                foreach (IXLRow row in worksheet.RowsUsed())
                {
                    //If Reading the First Row (used) then add them as column name  
                    if (FirstRow)
                    {
                        //Checking the Last cellused for column generation in datatable  
                        readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        FirstRow = false;

                    }
                    else
                    {
                        //Adding a Row in datatable  
                        dt.Rows.Add();
                        int cellIndex = 0;
                        //Updating the values of datatable  
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                            cellIndex++;
                        }
                    }
                }
                //If no data in Excel file  
                if (FirstRow)
                {
                    // ViewBag.Message = "Empty Excel File!";
                }

            }
            return dt;
        }
        private void SaveasCompletedSpreadsheet(DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "passive Air Sample");

                wb.SaveAs(Server.MapPath("~/App_Data/" + this.GetUser().user_id + "CompletedPasssiveAirSample.xlsx"));
                ViewData["ExportPDF"] = "Yes";
            }
        }
        private void SaveasNewSpreadsheet(DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "passive Air Sample");

                wb.SaveAs(Server.MapPath("~/App_Data/" + this.GetUser().user_id + "newPasssiveAirSample.xlsx"));

            }
        }
        private void SaveAsDuplicateSpreadsheet(DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Duplicate Passive Air Sample");

                wb.SaveAs(Server.MapPath("~/App_Data/" + this.GetUser().user_id + "DuplicatePasssiveAirSample.xlsx"));

            }
        }
        private void OverwriteCurrentSpreadsheet(DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "passive Air Sample");

                wb.SaveAs(Server.MapPath("~/App_Data/" + this.GetUser().user_id + "PasssiveAirSample.xlsx"));

            }
        }

        private DataTable verifyDuplicate(DataTable dtold)
        {
            DataTable dtnew, dtDuplicate;
            dtnew = dtold.Clone();
            dtDuplicate = dtold.Clone();
            foreach (DataRow dr in dtold.Rows)
            {
                if (checkDuplicateRecord(dr["media_serial_number"].ToString(), dr["date_received_from_lab"].ToString()))
                    dtDuplicate.ImportRow(dr);

                else
                    dtnew.ImportRow(dr);
            }

            if (dtDuplicate.Rows.Count > 0)
            {
                SaveAsDuplicateSpreadsheet(dtDuplicate);
                ViewData["DuplicateData"] = "Y";

            }
            else
                ViewData["DuplicateData"] = "";

            OverwriteCurrentSpreadsheet(dtnew);

            return dtnew;
        }

        private Boolean checkDuplicateRecord(string mediano, string datereceive)
        {
            Boolean flgexist = true;
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"SELECT count(*) as counter FROM SAMPLES  WHERE media_serial_number='" + mediano + "' AND date_recieved_from_lab='" + datereceive + "'", connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            if (Convert.ToInt32(dataReader["counter"].ToString()) > 0)
                                flgexist = true;
                            else
                                flgexist = false;

                        }
                    }
                }

                connection.Close();
            }
            return flgexist;

        }
        private string CreatePassiveCoc(string strScheduleId, string strsiteId, string strYear, string strMonth)
        {

            ChainOfCustody newChainOfCustody = new ChainOfCustody();
            UpdateModel(newChainOfCustody);
            newChainOfCustody.created_by = this.GetUser().user_id;
            newChainOfCustody.date_opened = DateTime.Now.ToISODateTime();
            newChainOfCustody.sample_type_id = "9";
            newChainOfCustody.schedule_id = strScheduleId;
            newChainOfCustody.location_id = getLocationId(strsiteId);
            newChainOfCustody.date_sampling_scheduled = getScheduleDate(strYear, strMonth);
            newChainOfCustody.status_id = "3";
            newChainOfCustody.CreatePassiveSampleCoc();

            return newChainOfCustody.chain_of_custody_id;
        }

        private void CreatePassiveSample(string cocId, string webaId, string date_from_lab, string mediano)

        {
            Sample newSample = new Sample();
            UpdateModel(newSample);



            newSample.wbea_id = webaId;
            newSample.date_received_from_lab = date_from_lab.ToDateTimeFormat();
            newSample.media_serial_number = mediano;
            newSample.date_created = System.DateTime.Now.ToISODate();
            newSample.average_storage_temperature = "4.0000000";
            newSample.average_storage_temperature_unit = "1";
            newSample.prepared_by = this.GetUser().user_id;
            newSample.sample_type_id = "9";
            newSample.is_travel_blank = "0";
            newSample.is_orphaned_sample = "0";
            newSample.Save();

            if (!cocId.IsBlank())
            {
                ChainOfCustody coc = ChainOfCustody.Load(cocId);

                // add the id to the CoC's list
                coc.AddSample(newSample.sample_id);

                // Save the new id(s)
                coc.SaveRelatedSamples();


            }



        }

        private string getLocationId(string siteId)
        {
            string strSiteId = "";
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"select location_id from Locations where charindex('" + siteId + "',name)>0", connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            if (string.IsNullOrWhiteSpace(dataReader["location_id"].ToString()))
                                strSiteId = "";
                            else
                                strSiteId = dataReader["location_id"].ToString();

                        }
                    }
                }

                connection.Close();
            }
            return strSiteId;
        }
        private string getScheduleDate(string strYear, string strMonth)
        {

            return Convert.ToDateTime("01-" + strMonth + "-20" + strYear).ToISODateTime();

        }
        private int COC_Counter(DataTable dt)
        {
            string strlastsite = "last";
            int intcounter = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][3].ToString() != strlastsite && strlastsite != "")
                {
                    intcounter = intcounter + 1;

                }
                strlastsite = dt.Rows[i][3].ToString();
            }
            return intcounter;
        }
        private DataTable SaveCocandSamples(DataTable dt)
        {
            string strlastSchedule = "last";
            string strnewCOCId = "";
            List<string> scheduleId = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][8].ToString() != strlastSchedule && strlastSchedule != "")
                {
                    dt.Rows[i][10] = "Download";
                    if (!scheduleId.Contains(dt.Rows[i][8].ToString()))
                    {
                        strnewCOCId = CreatePassiveCoc(dt.Rows[i][8].ToString(), dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), dt.Rows[i][5].ToString());
                        scheduleId.Add(dt.Rows[i][8].ToString());
                        dt.Rows[i][1] = strnewCOCId;
                        CreatePassiveSample(strnewCOCId, dt.Rows[i][0].ToString(), dt.Rows[i][9].ToString(), dt.Rows[i][2].ToString());
                    }

                }
                else
                {
                    dt.Rows[i][10] = "";

                    CreatePassiveSample(strnewCOCId, dt.Rows[i][0].ToString(), dt.Rows[i][9].ToString(), dt.Rows[i][2].ToString());
                    dt.Rows[i][1] = strnewCOCId;
                }
                strlastSchedule = dt.Rows[i][8].ToString();
                dt.AcceptChanges();
            }
            SaveasCompletedSpreadsheet(dt);
            return dt;
        }
        private DataTable GenerateWBEAId(DataTable dt)
        {
            int lastGeneratedDigits;
            int lastSetSize;
            DateTime dateLastGenerated;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            WBEAIdGenerator.FetchDetails(DateTime.Today.Year, out lastGeneratedDigits, out lastSetSize, out dateLastGenerated);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = WBEAIdGenerator.GenerateWBEAId(year, month, (lastGeneratedDigits + 1 + i));

                dt.AcceptChanges();
            }
            return dt;
        }
        private DataTable GenerateWBEAIdandSave(DataTable dt)
        {
            int lastGeneratedDigits;
            int lastSetSize;
            DateTime dateLastGenerated;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            WBEAIdGenerator.FetchDetails(DateTime.Today.Year, out lastGeneratedDigits, out lastSetSize, out dateLastGenerated);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = WBEAIdGenerator.GenerateWBEAId(year, month, (lastGeneratedDigits + 1 + i));

                dt.AcceptChanges();
            }

            WBEAIdGenerator.ReserveNewSet(dt.Rows.Count);
            return dt;
        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }
        public ActionResult ViewReport(string id, string reporttype)
        {
            return View("Viewer", new ReportDescriptor { Id = id, Type = reporttype });

        }
        public ActionResult ExportDuplicatePassive(FormCollection collection)
        {
            string strPath = Path.Combine(Server.MapPath("~/App_Data/ "), this.GetUser().user_id + "DuplicatePasssiveAirSample.xlsx");
            using (XLWorkbook wb = new XLWorkbook(strPath))
            {

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DuplicatePasssiveAirSample_" + DateTime.Now.ToISODate() + ".xlsx");
                }
            }



        }

        public ActionResult ExportPDF(FormCollection collection)
        {
            DataTable dt = new DataTable();
            ViewData["Status"] = "Upload";
            string strdownloadMessage = "";
            string strPath2 = Path.Combine(Server.MapPath("~/App_Data/"), this.GetUser().user_id + "CompletedPasssiveAirSample.xlsx");

            dt = ConvertExcelToDataTable(strPath2);

            List<string> IdsList = new List<string>();
            IdsList = COCIdList(dt);

            using (var memoryStream = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    //   zip.AddDirectoryByName(Server.MapPath("~/UploadFile"));
                    //  zip.AddDirectoryByName(Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Downloads"));
                    foreach (string cocId in IdsList)
                    {
                        var sectionReport1 = new SectionReport();
                        sectionReport1.LoadLayout(XmlReader.Create(Server.MapPath("~/Reports/COC_Pass.rpx")));
                        // sectionReport.DataSource = Repository.GetCOC(data[1]);
                        sectionReport1.DataSource = Repository.GetData(cocId);
                        sectionReport1.PageSettings.Margins.Top = 0.3f;
                        sectionReport1.PageSettings.Margins.Bottom = 0.75f;

                        ((TextBox)sectionReport1.Sections["PageHeader"].Controls["txtRefCOC"]).Text = cocId;

                        using (DataTable dta = Repository.GetData(cocId))
                        {
                            ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtReceivedDate"]).Text = dta.Rows[0].Field<string>(3);
                            ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtDeployment"]).Text = dta.Rows[0].Field<string>(4);
                            ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtRetrieval"]).Text = dta.Rows[0].Field<string>(5);
                            ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtShippedToLab"]).Text = dta.Rows[0].Field<string>(2);
                            ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtCreateBy"]).Text = dta.Rows[0].Field<string>(6);
                            ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtDeployedby"]).Text = dta.Rows[0].Field<string>(7);
                            ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtRetrievedby"]).Text = dta.Rows[0].Field<string>(8);
                            ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtShippedby"]).Text = dta.Rows[0].Field<string>(9);
                            ((TextBox)sectionReport1.Sections["PageHeader"].Controls["txtSiteID"]).Text = dta.Rows[0].Field<string>(10);
                        }
                        sectionReport1.Run();
                        var pdfExp = new GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport();
                        //  string strDownloadPath = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Downloads") + "\\Passive_COC_" + cocId + ".pdf";
                        string strDownloadPath = Server.MapPath("~/App_Data/Passive_COC_" + cocId + ".pdf");
                        pdfExp.Export(sectionReport1.Document, strDownloadPath);

                        zip.AddFile(strDownloadPath, "");
                        //strdownloadMessage = strdownloadMessage + "Passive_COC_" + cocId + ".pdf" + " ";

                    }
                    zip.Save(memoryStream);

                    // ViewBag.Message = strdownloadMessage + "have been downloaded to " + Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Downloads");
                }

                deleteFiles(Directory.GetFiles(Server.MapPath("~/App_Data")));
                return File(memoryStream.ToArray(), "application/zip", "PassiveAirSample_" + DateTime.Now.ToISODate() + ".zip");
            }




        }
        public ActionResult DownloadActiveReport(string cocId)
        {

            ViewBag.message = "";




            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    var sectionReport1 = new SectionReport();
                    sectionReport1.LoadLayout(XmlReader.Create(Server.MapPath("~/Reports/COC_Pass.rpx")));

                    sectionReport1.DataSource = Repository.GetData(cocId);
                    sectionReport1.PageSettings.Margins.Top = 0.3f;
                    sectionReport1.PageSettings.Margins.Bottom = 0.75f;

                    ((TextBox)sectionReport1.Sections["PageHeader"].Controls["txtRefCOC"]).Text = cocId;

                    using (DataTable dta = Repository.GetData(cocId))
                    {
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtReceivedDate"]).Text = dta.Rows[0].Field<string>(3);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtDeployment"]).Text = dta.Rows[0].Field<string>(4);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtRetrieval"]).Text = dta.Rows[0].Field<string>(5);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtShippedToLab"]).Text = dta.Rows[0].Field<string>(2);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtCreateBy"]).Text = dta.Rows[0].Field<string>(6);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtDeployedby"]).Text = dta.Rows[0].Field<string>(7);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtRetrievedby"]).Text = dta.Rows[0].Field<string>(8);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtShippedby"]).Text = dta.Rows[0].Field<string>(9);
                        ((TextBox)sectionReport1.Sections["PageHeader"].Controls["txtSiteID"]).Text = dta.Rows[0].Field<string>(10);
                    }
                    sectionReport1.Run();
                    GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport pdfExp = new GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport();



                    pdfExp.Export(sectionReport1.Document, memoryStream);

                }
                catch (Exception ex)
                {
                    ViewBag.message = ex.Message.ToString();

                    // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", ex.Message.ToString());
                }




                return File(memoryStream.ToArray(), "application/PDF", "PassiveAirSample_" + cocId + "_" + DateTime.Now.ToISODate() + ".pdf");
            }




        }

        public void deleteFiles(string[] files)
        {
            try
            {
                foreach (string file in files)
                {
                    if (file.Contains(".pdf"))
                        System.IO.File.Delete(file);

                }
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message.ToString(); }
        }

        public void deleteCOCFiles(string[] files)
        {
            try
            {
                foreach (string file in files)
                {
                    if (file.Contains("COC_"))
                        System.IO.File.Delete(file);

                }
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message.ToString(); }
        }
    }
}
