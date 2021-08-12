using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Mvc;
using WBEADMS;
using WBEADMS.Models;

namespace DocIt.Controllers
{

    public class ECOCDataController : Controller
    {

        // GET: ExportModel
        public ActionResult Index(int? page, int? page_size, string sort, string date_from, string date_to)
        {
            string whereClause = "";
            string orderByClause = "";
            string joinClause = "";

            if (!date_from.IsBlank() && !date_to.IsBlank())
            {



                DateTime dtfrom = Convert.ToDateTime(date_from);
                DateTime dtto = Convert.ToDateTime(date_to);
                TimeSpan t = dtto - dtfrom;
                if (t.TotalDays < 0)
                {
                    whereClause = " isnull(date_actual,'') < '1990-01-01' and isnull(date_actual,'')<>'' ";
                    ViewData["SearchDate"] = "";
                    this.AddViewNotice(" Search End Date must be greater than Start Date!" + "\n");
                }
                else
                {
                    whereClause = " isnull(date_actual,'')>='" + date_from + "' and isnull(date_actual,'')<='" + date_to + "'";
                    if (date_from.Substring(0, 7) == date_to.Substring(0, 7))
                    {
                        ViewData["SearchDate"] = String.Format("{0:y}", dtfrom);
                        ViewData["From"] = date_from;
                        ViewData["To"] = date_to;
                    }
                    else

                    {
                        ViewData["SearchDate"] = date_from + " to  " + date_to;
                        ViewData["From"] = date_from;
                        ViewData["To"] = date_to;
                    }
                }
            }
            else
            {
                string strNow = DateTime.Now.ToString("yyyy-MM");

                whereClause = " isnull(date_actual,'') < '1990-01-01' and isnull(date_actual,'')<>'' ";
                ViewData["SearchDate"] = "";

                if (date_from.IsBlank() && date_to.IsBlank())
                { //ViewData["err"] = strErr + "Both start date and end date are mandatory search fields!" + "\n"; 
                    this.AddViewNotice("Both start date and end date are mandatory search fields!");
                }
                else if (date_from.IsBlank())
                {
                    this.AddViewNotice("Start Date is issing!");
                }
                else
                { this.AddViewNotice("End Date is missing!"); }
            }

            if (!page_size.HasValue)
            {
                page_size = 40;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();



                string[] sortOrder = sort.Split('_');
                foreach (string column in sortOrder)
                {
                    switch (column)
                    {
                        case "Site":
                            orderByClause += "site,";
                            break;
                        case "DATE":
                            orderByClause += "DATE DESC,";
                            break;


                    }
                }
            }


            orderByClause = "site";
            var urlParameters = new { action = "Index", controller = "ECOCData", sort = sort, date_from, date_to };
            Paginator paginator = this.AddDefaultPaginator<ECOCData_View>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<ECOCData_View> pmdata = BaseModel.FetchPage<ECOCData_View>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });
            return View(pmdata);


            //paginate



        }
        public ActionResult ExportPMData()
        {


            string strfrom = (string)Session["SelectedFrom"];
            string strto = (string)Session["Selectedto"];
            //  System.IO.File.WriteAllText(@"C:\temp\WriteDataID.txt", strdate);


            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition();
            string stryear = DateTime.Now.ToString("MMM_yyyy");

            cd.FileName = "PM Data Export_" + strfrom + " to " + strto + ".csv";
            Response.AppendHeader("Content-Disposition", cd.ToString());

            StringBuilder csvString = new StringBuilder("");
            // Header
            csvString.Append("PM Data Export\n");
            csvString.Append("Export PM Date:," + DateTime.Now.ToString("MM/dd/yyyy") + "\n");
            csvString.Append("SITE,DATE,SIZE,TVOC,TVOU,WEBA_ID,TID,Group,Memo,WBEA_NOTES\n");



            List<PMData_View> pmdata = PMData_View.FetchAll("isnull(Date_actual,'')>='" + strfrom + "' and isnull(date_actual,'')<='" + strto + "'");
            foreach (var i in pmdata)
            {
                csvString.Append(i.SITE);
                csvString.Append(",");
                csvString.Append(i.DATE);
                csvString.Append(",");
                csvString.Append(i.Size);
                csvString.Append(",");
                csvString.Append(i.TVOC);
                csvString.Append(",");
                csvString.Append(i.TVOU);
                csvString.Append(",");
                csvString.Append(i.WEBA_ID);
                csvString.Append(",");
                csvString.Append(i.TID);
                csvString.Append(",");
                csvString.Append(i.Group);
                csvString.Append(",");
                csvString.Append(i.Memo);
                csvString.Append(",");
                csvString.Append(i.WEBA_NOTE);

                csvString.Append(",");

                csvString.Append("\n");
            }


            return File(System.Text.Encoding.Default.GetBytes(csvString.ToString()), "Content-type: text/csv");


        }

        public ActionResult ExportECOCDataToExcel()
        {


            string strfrom = (string)Session["SelectedFrom"];
            string strto = (string)Session["Selectedto"];
            string strFileName = "";
            //  System.IO.File.WriteAllText(@"C:\temp\WriteDataID.txt", strdate);



            string strCurrent = DateTime.Now.ToString("MMM_yyyy");
            if (strto.Substring(0, 7) == strfrom.Substring(0, 7))
                strFileName = "WBEA ECOC " + (string)Session["SelectedMonth"] + " DIR Reapot.xlsx";
            else
                strFileName = "WBEA ECOC " + strfrom + " to " + strto + " DIR Reapot.xlsx";

            DataTable dt = new DataTable("ECOC");
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("SITE"),
                                            new DataColumn("DATE"),
                                            new DataColumn("SIZE"),
                                            new DataColumn("TVOC"),
                                            new DataColumn("TVOU"),
                                            new DataColumn("WEBA_ID"),
                                            new DataColumn("TID"),

                                            new DataColumn("WBEA_NOTES")


            });






            List<ECOCData_View> pmdata3 = ECOCData_View.FetchAll("isnull(Date_actual,'')>='" + strfrom + "' and isnull(date_actual,'')<='" + strto + "'");
            foreach (var i in pmdata3)
            {
                dt.Rows.Add(i.SITE, i.DATE, i.Size, i.TVOC, i.TVOU, i.WEBA_ID, i.TID, i.WEBA_NOTE);

            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFileName);
                }
            }

        }


    }
}