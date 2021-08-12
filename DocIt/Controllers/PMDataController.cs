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

    public class PMDataController : Controller
    {

        // GET: ExportModel
        public ActionResult Index(int? page, int? page_size, string sort, string month_state, string date_from, string date_to)
        {
            string whereClause = "";
            string orderByClause = "";
            string joinClause = "";
            string strErr = "";
            ViewData["month_state"] = new SelectList(new string[] { "", "Yes", "No" }, month_state ?? "No");
            if (!date_from.IsBlank() && !date_to.IsBlank())
            {



                DateTime dtfrom = Convert.ToDateTime(date_from);
                DateTime dtto = Convert.ToDateTime(date_to);
                TimeSpan t = dtto - dtfrom;
                if (t.TotalDays < 0)
                {
                    whereClause = " isnull(date_actual,'') < '1990-01-01' and isnull(date_actual,'')<>'' ";
                    // whereClause = " date_actual < '1990-01-01' ";
                    ViewData["SearchDate"] = "";
                    this.AddViewNotice(" Search End Date must be greater than Start Date!" + "\n");
                }
                else
                {
                    whereClause = " isnull(date_actual,'')>='" + date_from + "' and isnull(date_actual,'')<='" + date_to + "'";
                    // whereClause = " date_actual>='" + date_from + "' and date_actual<='" + date_to + "'";
                    if (month_state == "Yes")
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
                //whereClause = " date_actual < '1990-01-01' ";
                whereClause = " isnull(date_actual,'') < '1990-01-01' and isnull(date_actual,'')<>'' ";
                ViewData["SearchDate"] = "";

                if (date_from.IsBlank() && date_to.IsBlank())
                { //ViewData["err"] = strErr + "Both start date and end date are mandatory search fields!" + "\n"; 
                    this.AddViewNotice("Both start date and end date are mandatory search fields!");
                }
                else if (date_from.IsBlank())
                { this.AddViewNotice("Start Date is issing!"); }
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
            var urlParameters = new { action = "Index", controller = "PMData", sort = sort, month_state, date_from, date_to };
            Paginator paginator = this.AddDefaultPaginator<PMData_View>(urlParameters, page, new { join = joinClause, where = whereClause });
            // Paginator paginator = this.AddDefaultPaginatorMySQL<PMData_View>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<PMData_View> pmdata = BaseModel.FetchPage<PMData_View>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });
            //  List<PMData_View> pmdata = BaseModelMySQL.FetchPage<PMData_View>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });
            return View(pmdata);


            //paginate



        }
        public ActionResult ExportPMData(string month_state)
        {


            string strfrom = (string)Session["SelectedFrom"];
            string strto = (string)Session["Selectedto"];
            // System.IO.File.WriteAllText(@"C:\temp\WriteDataID.txt", month_state);


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

        public ActionResult ExportPMDataToExcel(FormCollection collection)
        {


            string strfrom = (string)Session["SelectedFrom"];
            string strto = (string)Session["Selectedto"];

            //  System.IO.File.WriteAllText(@"C:\temp\WriteDataID.txt", month_state);
            string strFileName = "";

            //  var selectedlist = (string)collection["month_state"];
            //  System.IO.File.WriteAllText(@"C:\temp\WriteDataID.txt", selectedlist);
            string strCurrent = DateTime.Now.ToString("MMM_yyyy");
            if (strfrom.Substring(5, 2) == strto.Substring(5, 2))
                strFileName = "WBEA PM Data " + (string)Session["Selectedmonth"] + " Report.xlsx";
            else
                strFileName = "WBEA PM Data " + strfrom + " to " + strto + " Report.xlsx";

            DataTable dta = new DataTable("Group A");
            dta.Columns.AddRange(new DataColumn[9] { new DataColumn("SITE"),
                                            new DataColumn("DATE"),
                                            new DataColumn("SIZE"),
                                            new DataColumn("TVOC"),
                                            new DataColumn("TVOU"),
                                            new DataColumn("WEBA_ID"),
                                            new DataColumn("TID"),
                                            new DataColumn("Group"),
                                            new DataColumn("WBEA_NOTES")


            });


            List<PMData_View> pmdata = PMData_View.FetchAll("isnull(Date_actual,'')>='" + strfrom + "' and isnull(date_actual,'')<='" + strto + "' and GroupType='A' ");
            foreach (var i in pmdata)
            {
                dta.Rows.Add(i.SITE, i.DATE, i.Size, i.TVOC, i.TVOU, i.WEBA_ID, i.TID, i.Group, i.WEBA_NOTE);

            }


            DataTable dtb = new DataTable("Group B");
            dtb.Columns.AddRange(new DataColumn[9] { new DataColumn("SITE"),
                                            new DataColumn("DATE"),
                                            new DataColumn("SIZE"),
                                            new DataColumn("TVOC"),
                                            new DataColumn("TVOU"),
                                            new DataColumn("WBEA_ID"),
                                            new DataColumn("TID"),
                                            new DataColumn("Group"),
                                            new DataColumn("WBEA_NOTES")


            });


            List<PMData_View> pmdata2 = PMData_View.FetchAll("isnull(Date_actual,'')>='" + strfrom + "' and isnull(date_actual,'')<='" + strto + "' and GroupType='B' ");
            foreach (var i in pmdata2)
            {
                dtb.Rows.Add(i.SITE, i.DATE, i.Size, i.TVOC, i.TVOU, i.WEBA_ID, i.TID, i.Group, i.WEBA_NOTE);

            }

            DataTable dtc = new DataTable("Others");
            dtc.Columns.AddRange(new DataColumn[10] { new DataColumn("SITE"),
                                            new DataColumn("DATE"),
                                            new DataColumn("SIZE"),
                                            new DataColumn("TVOC"),
                                            new DataColumn("TVOU"),
                                            new DataColumn("WBEA_ID"),
                                            new DataColumn("TID"),
                                            new DataColumn("Group"),
                                            new DataColumn("Memo"),
                                            new DataColumn("WBEA_NOTES")


            });



            List<PMData_View> pmdata3 = PMData_View.FetchAll("isnull(Date_actual,'')>='" + strfrom + "' and isnull(date_actual,'')<='" + strto + "' and GroupType='O' ");
            foreach (var i in pmdata3)
            {
                dtc.Rows.Add(i.SITE, i.DATE, i.Size, i.TVOC, i.TVOU, i.WEBA_ID, i.TID, i.Group, i.Memo, i.WEBA_NOTE);

            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dta);
                wb.Worksheets.Add(dtb);
                wb.Worksheets.Add(dtc);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFileName);
                }
            }

        }


    }
}