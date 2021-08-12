
using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class SampleController : Controller
    {

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMediaSNList(string stateId)
        {
            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", stateId);

            //  if (!string.IsNullOrEmpty(stateId))
            // {


            return Json(BaseModel.FetchList("SELECT * FROM samples where media_serial_number like '%" + stateId + "%'", "media_serial_number"), JsonRequestBehavior.AllowGet);

            // }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetLabSampleList(string stateId)
        {
            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", stateId);

            //  if (!string.IsNullOrEmpty(stateId))
            // {


            return Json(BaseModel.FetchList("SELECT lab_sample_id FROM samples where lab_sample_id like '%" + stateId + "%' group by lab_sample_id", "lab_sample_id"), JsonRequestBehavior.AllowGet);

            // }
        }

        public ActionResult OrphanedSamples(int? page, int? page_size, int? sample_type_id, string is_orphaned_sample, string date_from, string date_to, string sort)
        {
            string joinClause = "";
            string orderByClause = "";
            string whereClause = "sample_id not in (select sample_id from ChainOfCustodys_Samples) ";

            int pageNo = this.GetPageNo(page);


            // System.IO.File.WriteAllText(@"C:\temp\PageNo1.txt", pageNo.ToString());
            ViewData["is_orphaned_sample"] = new SelectList(new string[] { "", "Yes", "No" }, is_orphaned_sample ?? "");
            // is_orphaned_sample=
            //Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                if (sort.Contains("sample-type"))
                {
                    joinClause += "INNER JOIN SampleTypes ON Samples.sample_type_id = SampleTypes.sample_type_id ";
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
                        case "wbea-id":
                            orderByClause += "wbea_id,";
                            break;
                        case "desc-wbea-id":
                            orderByClause += "wbea_id DESC,";
                            break;
                        case "media-serial":
                            orderByClause += "media_serial_number,";
                            break;
                        case "desc-media-serial":
                            orderByClause += "media_serial_number DESC,";
                            break;
                        case "lab-sample-id":
                            orderByClause += "lab_sample_id,";
                            break;
                        case "desc-lab-sample-id":
                            orderByClause += "lab_sample_id DESC,";
                            break;
                        case "received-from-lab":
                            orderByClause += "date_received_from_lab,";
                            break;
                        case "desc-received-from-lab":
                            orderByClause += "date_received_from_lab DESC,";
                            break;
                        case "prepared-by":
                            orderByClause += "Users.last_name, Users.first_name,";
                            break;
                        case "desc-prepared-by":
                            orderByClause += "Users.last_name DESC, Users.first_name,";
                            break;
                    }
                }
            }


            ////orderByClause = orderByClause.Trim(',');
            orderByClause += "Samples.sample_id DESC"; // default sort is newly created samples first

            //Populate filter drop downs.
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample_type_id);
            ViewData["date_from"] = date_from;
            ViewData["date_to"] = date_to;

            //filter
            if (sample_type_id.HasValue)
            {
                whereClause += " AND Samples.sample_type_id = " + sample_type_id.Value;
                ViewData["TypeId"] = sample_type_id.Value;
            }
            else
            {
                ViewData["TypeId"] = 0;
            }

            if (is_orphaned_sample == "Yes")
                whereClause += " AND isnull(is_orphaned_sample,0)=1";
            if (is_orphaned_sample == "No")
                whereClause += " AND isnull(is_orphaned_sample,0)=0";
            if (!date_from.IsBlank() && !date_to.IsBlank())

            {
                whereClause += " AND Samples.date_created  between '" + date_from + "' and '" + date_to + "'";
                ViewData["From"] = date_from;
                ViewData["To"] = date_to;
            }
            else
            {
                ViewData["From"] = "";
                ViewData["To"] = "";
            }


            var urlParameters = new { action = "OrphanedSamples", controller = "Sample", sample_type_id = sample_type_id, is_orphaned_sample, date_from = date_from, date_to = date_to, sort = sort, };
            Paginator paginator = this.AddDefaultPaginator<Sample>(urlParameters, page, new { join = joinClause, where = whereClause });

            this.SetPageSize(page_size, paginator);
            List<Sample> samples = BaseModel.FetchPage<Sample>(paginator, new { join = joinClause, where = whereClause, order = orderByClause });
            // List<Sample> samples = BaseModel.FetchAll<Sample>(new {  where = whereClause, order = orderByClause });
            // add notice if there is no records
            if (samples.Count == 0)
            {
                this.AddViewNotice("No Samples were found.");
                ViewData["HasSamples"] = "No";
            }
            else
            { ViewData["HasSamples"] = "Yes"; }


            return View(samples);
        }
        [Authorize]

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OrphanedSamples(int? page, int? page_size, int? sample_type_id, string is_orphaned_sample, string date_from, string date_to, string sort, FormCollection collection)
        {
            ViewData["is_orphaned_sample"] = new SelectList(new string[] { "", "Yes", "No" }, is_orphaned_sample ?? "");
            string strname = "";

            List<string> CheckedsampleIdList = new List<string>();
            List<string> PagesampleIdList = new List<string>();

            int pageNo = this.GetPageNo(page);


            //   System.IO.File.WriteAllText(@"C:\temp\PageNo.txt", pageNo.ToString());

            //   if (collection["parameter_list"]!=null)
            //     System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", collection["sample_Id"].ToString());


            string joinClause = "";
            string orderByClause = "";
            string whereClause = "sample_id not in (select sample_id from ChainOfCustodys_Samples) ";

            //Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                if (sort.Contains("sample-type"))
                {
                    joinClause += "INNER JOIN SampleTypes ON Samples.sample_type_id = SampleTypes.sample_type_id ";
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
                        case "wbea-id":
                            orderByClause += "wbea_id,";
                            break;
                        case "desc-wbea-id":
                            orderByClause += "wbea_id DESC,";
                            break;
                        case "media-serial":
                            orderByClause += "media_serial_number,";
                            break;
                        case "desc-media-serial":
                            orderByClause += "media_serial_number DESC,";
                            break;
                        case "lab-sample-id":
                            orderByClause += "lab_sample_id,";
                            break;
                        case "desc-lab-sample-id":
                            orderByClause += "lab_sample_id DESC,";
                            break;
                        case "received-from-lab":
                            orderByClause += "date_received_from_lab,";
                            break;
                        case "desc-received-from-lab":
                            orderByClause += "date_received_from_lab DESC,";
                            break;
                        case "prepared-by":
                            orderByClause += "Users.last_name, Users.first_name,";
                            break;
                        case "desc-prepared-by":
                            orderByClause += "Users.last_name DESC, Users.first_name,";
                            break;
                    }
                }
            }

            ////orderByClause = orderByClause.Trim(',');
            orderByClause += "Samples.sample_id DESC"; // default sort is newly created samples first

            //Populate filter drop downs.
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample_type_id);
            ViewData["date_from"] = date_from;
            ViewData["date_to"] = date_to;
            //filter
            if (sample_type_id.HasValue)
            {
                whereClause += " AND Samples.sample_type_id = " + sample_type_id.Value;
                ViewData["TypeId"] = sample_type_id.Value;
            }
            else
            {
                ViewData["TypeId"] = 0;
            }
            if (!date_from.IsBlank() && !date_to.IsBlank())

            {
                whereClause += " AND Samples.date_created  between '" + date_from + "' and '" + date_to + "'";
                ViewData["From"] = date_from;
                ViewData["To"] = date_to;
            }
            else
            {
                ViewData["From"] = "";
                ViewData["To"] = "";
            }

            if (is_orphaned_sample == "Yes")
                whereClause += " AND isnull(is_orphaned_sample,0)=1";
            if (is_orphaned_sample == "No")
                whereClause += " AND isnull(is_orphaned_sample,0)=0";


            if (collection["form_action"].ToString() == "Submit Selected")
            {


                var urlParameters1 = new { action = "OrphanedSamples", controller = "Sample", sample_type_id = sample_type_id, is_orphaned_sample, date_from = date_from, date_to = date_to, sort = sort, };
                Paginator paginator1 = this.AddDefaultPaginator<Sample>(urlParameters1, pageNo, new { join = joinClause, where = whereClause });
                this.SetPageSize(page_size, paginator1);
                List<Sample> samples1 = BaseModel.FetchPage<Sample>(paginator1, new { join = joinClause, where = whereClause, order = orderByClause });
                string strSampleId = "";

                if (samples1.Count > 0)
                {
                    foreach (var i in samples1)
                    { strSampleId = strSampleId + i.sample_id + ","; }

                    //   System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", strSampleId);
                    PagesampleIdList = strSampleId.Split(',').ToList();
                }



                if (collection["parameter_list"] != null)
                {
                    CheckedsampleIdList = collection["parameter_list"].Split(',').ToList();
                    foreach (var s in CheckedsampleIdList)
                    {
                        Sample sample = this.GetRequestedModel<Sample>(s.ToString());


                        try
                        {
                            //  UpdateModel(sample);
                            sample.is_orphaned_sample = "1";


                            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", strname);
                            sample.Save();


                        }
                        catch (ModelException e)
                        {

                            this.PopulateViewWithErrorMessages(e);
                            return View(sample);
                        }
                        PagesampleIdList.Remove(s.ToString());

                    }



                    foreach (var i in PagesampleIdList)
                    {

                        if (i.Length > 0)
                        {
                            Sample sample1 = this.GetRequestedModel<Sample>(i.ToString());


                            try
                            {
                                //  UpdateModel(sample);
                                sample1.is_orphaned_sample = "0";
                                //   sample.sample_type_id = sample.sample_type_id;

                                strname = strname + sample1.sample_type_id;
                                sample1.Save();


                            }
                            catch (ModelException e)
                            {

                                this.PopulateViewWithErrorMessages(e);
                                return View(sample1);

                            }
                        }
                    }

                }
                else
                {
                    foreach (var i in PagesampleIdList)
                    {

                        if (i.Length > 0)
                        {
                            Sample sample1 = this.GetRequestedModel<Sample>(i.ToString());


                            try
                            {
                                //  UpdateModel(sample);
                                sample1.is_orphaned_sample = "0";
                                //   sample.sample_type_id = sample.sample_type_id;

                                strname = strname + sample1.sample_type_id;
                                sample1.Save();


                            }
                            catch (ModelException e)
                            {

                                this.PopulateViewWithErrorMessages(e);
                                return View(sample1);

                            }
                        }
                    }


                }

            }
            var urlParameters = new { action = "OrphanedSamples", controller = "Sample", sample_type_id = sample_type_id, is_orphaned_sample, date_from = date_from, date_to = date_to, sort = sort, };
            Paginator paginator = this.AddDefaultPaginator<Sample>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Sample> samples = BaseModel.FetchPage<Sample>(paginator, new { join = joinClause, where = whereClause, order = orderByClause });

            // add notice if there is no records
            if (samples.Count == 0)
            {
                this.AddViewNotice("No Samples were found.");
                ViewData["HasSamples"] = "No";
            }
            else
            { ViewData["HasSamples"] = "Yes"; }
            return View(samples);


        }
        public ActionResult ExportOrphanedSamples(FormCollection collection)
        {


            string strfrom = Session["SelectedFrom"].ToString().IsBlank() ? "" : Session["SelectedFrom"].ToString();
            string strto = Session["SelectedTo"].ToString().IsBlank() ? "" : Session["SelectedTo"].ToString();
            int typeId = (int)Session["SelectedType"];





            string strCurrent = DateTime.Now.ToString("MMM_yyyy");

            string strFileName = "WBEA_Orphaned_Samples_Report.xlsx";

            DataTable dta = new DataTable("Samples");
            dta.Columns.AddRange(new DataColumn[8] { new DataColumn("sample_id"),
                                            new DataColumn("PreparedBy"),
                                            new DataColumn("SampleType.name"),
                                            new DataColumn("wbea_id"),
                                            new DataColumn("media_serial_number"),
                                            new DataColumn("lab_sample_id"),
                                            new DataColumn("DateReceivedFromLab"),
                                            new DataColumn("is_travel_blank")
             });

            string whereClause = " docit.samples.sample_id not in (select cs.sample_id from docit.chainofcustodys_samples cs) ";

            if (typeId > 0) { whereClause = whereClause + " AND docit.samples.sample_type_id =" + typeId; }
            if (strfrom != "" && strto != "") { whereClause = whereClause + " AND docit.samples.date_created  between '" + strfrom + "' and '" + strto + "'"; }
            //   System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", whereClause);
            /* List <Sample> pmdata = Sample.FetchAll(whereClause);


            foreach (var i in pmdata)
             {
                 dta.Rows.Add(i.sample_id,i.PreparedBy, i.SampleType.name, i.wbea_id, i.media_serial_number, i.lab_sample_id, i.DateReceivedFromLab, i.is_travel_blank);

             }
             */
            string strsql = "select docit.samples.sample_id, concat(docit.users.first_name, ' ',docit.users.last_name) as PreparedBy,docit.sampletypes.name,docit.samples.wbea_id,docit.samples.media_serial_number,docit.samples.lab_sample_id,docit.samples.date_recieved_from_lab,docit.samples.is_travel_blank from docit.samples left join docit.users on  docit.samples.prepared_by=docit.users.user_id left join docit.sampletypes on docit.samples.sample_type_id=docit.sampletypes.sample_type_id ";
            strsql = strsql + " where " + whereClause;
            dta = getdatatable(strsql);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dta);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFileName);
                }
            }

        }

        public DataTable getdatatable(string sql)
        {

            using (MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchMysqlConnectionString()))

            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                DataTable dt = new DataTable("Samples");
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                    da.Dispose();
                }
                connection.Close();
                return dt;
            }
        }

        public ActionResult Index(int? page, int? page_size, int? sample_type_id, int? user_id, string assigned, string sort, string SelectedMediaNo, string SelectedLabSampleId)
        {
            string joinClause = "";
            string orderByClause = "";
            string whereClause = "1 = 1";


            //Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                if (sort.Contains("sample-type"))
                {
                    joinClause += "INNER JOIN SampleTypes ON Samples.sample_type_id = SampleTypes.sample_type_id ";
                }

                if (sort.Contains("prepared-by"))
                {
                    joinClause += "LEFT OUTER JOIN Users ON Samples.prepared_by = Users.user_id ";
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
                        case "wbea-id":
                            orderByClause += "wbea_id,";
                            break;
                        case "desc-wbea-id":
                            orderByClause += "wbea_id DESC,";
                            break;
                        case "media-serial":
                            orderByClause += "media_serial_number,";
                            break;
                        case "desc-media-serial":
                            orderByClause += "media_serial_number DESC,";
                            break;
                        case "lab-sample-id":
                            orderByClause += "lab_sample_id,";
                            break;
                        case "desc-lab-sample-id":
                            orderByClause += "lab_sample_id DESC,";
                            break;
                        case "received-from-lab":
                            orderByClause += "date_received_from_lab,";
                            break;
                        case "desc-received-from-lab":
                            orderByClause += "date_received_from_lab DESC,";
                            break;
                        case "prepared-by":
                            orderByClause += "Users.last_name, Users.first_name,";
                            break;
                        case "desc-prepared-by":
                            orderByClause += "Users.last_name DESC, Users.first_name,";
                            break;
                    }
                }
            }

            ////orderByClause = orderByClause.Trim(',');
            orderByClause += "Samples.sample_id DESC"; // default sort is newly created samples first

            //Populate filter drop downs.
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample_type_id);
            ViewData["user_id"] = WBEADMS.Models.User.FetchSelectListActive(user_id);
            ViewData["assigned"] = WBEADMS.ControllersCommon.GetYesNoSelectList(assigned);

            //filter
            if (sample_type_id.HasValue)
            {
                whereClause += " AND Samples.sample_type_id = " + sample_type_id.Value;
            }

            if (user_id.HasValue)
            {
                whereClause += " AND Samples.prepared_by = " + user_id.Value;
            }

            if (!assigned.IsBlank())
            {
                if (assigned.ToLower().Trim() == "yes")
                {
                    // Note: this is to prevent duplicate rows from appearing.
                    joinClause += "INNER JOIN (Select distinct sample_id from ChainOfCustodys_Samples) as CoCSampleIds ON Samples.sample_id = CoCSampleIds.sample_id";
                }
                else if (assigned.ToLower().Trim() == "no")
                {
                    joinClause += "LEFT OUTER JOIN ChainOfCustodys_Samples ON Samples.sample_id = ChainOfCustodys_Samples.sample_id ";
                    whereClause += " AND ChainOfCustodys_Samples.chain_of_custody_id is null";
                }
            }
            if (!string.IsNullOrEmpty(SelectedMediaNo))
            {
                // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", SelectedMediaNo.Substring(1));
                whereClause += " AND samples.media_serial_number in (" + SelectedMediaNo.Substring(1) + ")";
            }

            if (!string.IsNullOrEmpty(SelectedLabSampleId))
            {
                //  System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", SelectedLabSampleId.Substring(1));
                whereClause += " AND samples.lab_sample_id in (" + SelectedLabSampleId.Substring(1) + ")";
            }
            var urlParameters = new { action = "Index", controller = "Sample", sample_type_id = sample_type_id, user_id = user_id, assigned = assigned, sort = sort, };
            Paginator paginator = this.AddDefaultPaginator<Sample>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Sample> samples = BaseModel.FetchPage<Sample>(paginator, new { join = joinClause, where = whereClause, order = orderByClause });

            // add notice if there is no records
            if (samples.Count == 0)
            {
                this.AddViewNotice("No Samples were found.");
            }

            return View(samples);
        }

        public ActionResult Details(string id)
        {
            Sample sample = this.GetRequestedModel<Sample>(id);

            if (sample == null)
            {
                return RedirectToAction("Index");
            }

            return View(sample);
        }

        public ActionResult Create()
        {
            ViewData["sample_type_id"] = SampleType.FetchSelectListName();
            return View(new Sample());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string id, FormCollection collection)
        {
            Sample newSample = new Sample();
            UpdateModel(newSample);

            try
            {
                if (newSample.wbea_id.IsBlank())
                {
                    newSample.wbea_id = WBEAIdGenerator.ReserveNew();
                }

                newSample.date_created = System.DateTime.Now.ToISODate();

                newSample.prepared_by = this.GetUser().user_id;
                newSample.Save();

                if (!id.IsBlank())
                {
                    ChainOfCustody coc = ChainOfCustody.Load(id);

                    // add the id to the CoC's list
                    coc.AddSample(newSample.sample_id);

                    // Save the new id(s)
                    coc.SaveRelatedSamples();

                    return RedirectToAction("Open", new { controller = "ChainOfCustody", id = coc.id });
                }
                else
                {
                    this.AddTempNotice("Sample successfully created with WBEA Sample ID of " + newSample.wbea_id + ".");
                    return RedirectToAction("Details", new { id = newSample.id });
                }
            }
            catch (ModelException e)
            {
                ViewData["sample_type_id"] = SampleType.FetchSelectListName();
                this.PopulateViewWithErrorMessages(e);
                if (!id.IsBlank()) { ViewData["coc_id"] = id; }
            }

            return View(newSample);
        }

        public ActionResult Edit(string id)
        {
            Sample sample = this.GetRequestedModel<Sample>(id);
            if (sample == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample.sample_type_id);

            return View(sample);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            Sample sample = this.GetRequestedModel<Sample>(id);
            if (sample == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                UpdateModel(sample);
                sample.prepared_by = this.GetUser().user_id;
                sample.Save();

                this.AddTempNotice("Saved sample " + sample.wbea_id);
                return RedirectToAction("Details", new { id = sample.sample_id });
            }
            catch (ModelException e)
            {
                ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample.sample_type_id);
                this.PopulateViewWithErrorMessages(e);
                return View(sample);
            }
        }

        public class samplemsql
        {
            public string sampleid { get; set; }
            public string sampletype { get; set; }
            public string wbea_id { get; set; }
            public string media_serial_number { get; set; }
            public string lab_sample_id { get; set; }
            public string date_recieved_from_lab { get; set; }
            public string username { get; set; }
            public bool tranvel_blank { get; set; }
        }

        public ActionResult indexmysql(int? page)
        {
            var samples = new List<samplemsql>();
            DataTable dataTable = new DataTable();
            try
            {
                MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchMysqlConnectionString());


                MySqlCommand cmd = new MySqlCommand("select s.sample_id,st.name,s.wbea_id,s.media_serial_number,s.lab_sample_id,date_format(date_recieved_from_lab,'%M%d%Y'),concat(u.first_name,' ',u.last_name) as name, s.is_travel_blank from docit.samples s  join docit.sampletypes st on s.sample_type_id=st.sample_type_id  left join docit.users u on s.prepared_by=u.user_id  ", connection);

                connection.Open();
                ViewData["test"] = "Server Version: " + connection.ServerVersion
                  + "\nDatabase: " + connection.Database + " " + connection.Site;

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    samplemsql s = new samplemsql();

                    s.sampleid = (dataReader.IsDBNull(0) ? " " : dataReader.GetInt32(0).ToString());
                    s.sampletype = (dataReader.IsDBNull(1) ? " " : dataReader.GetString(1));
                    s.wbea_id = ((dataReader.IsDBNull(2) == true) ? "" : dataReader.GetString(2));
                    s.media_serial_number = (dataReader.IsDBNull(3) ? " " : dataReader.GetString(3));
                    s.lab_sample_id = (dataReader.IsDBNull(4) ? " " : dataReader.GetString(4));
                    s.date_recieved_from_lab = (dataReader.IsDBNull(5) ? "" : dataReader.GetString(5));
                    s.username = (dataReader.IsDBNull(6) ? " " : dataReader.GetString(6));
                    s.tranvel_blank = dataReader.GetBoolean(7) ? true : false;







                    samples.Add(s);
                }

                dataReader.Close();
                //    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                //  {
                //     da.Fill(dataTable);
                //  }
                // var p = dataTable;


                connection.Close();
                var pagenumber = page ?? 1;
                var onepageSample = samples.ToPagedList(pagenumber, 40);
                ViewBag.onepageSample = onepageSample;
            }
            catch (Exception ex)
            {
                ViewData["strerror"] = ex.Message.ToString();
            }

            return View();

        }
    }
}