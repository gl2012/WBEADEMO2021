
using MySql.Data.MySqlClient;
using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Controllers.LocationExtensions;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        // TODO: run Note.CommitNotes() somewhere here or in the Home controller.
        public ActionResult AdvancedNoteSearch(int? note_setting_id)
        {

            ViewData["note_setting_id"] = WBEADMS.Models.NoteSetting.FetchSettingSelectList(note_setting_id, this.GetUser().user_id);



            return View();

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDescription(string stateId)
        {


            return Json(NoteSetting.FetchDescriptionSelectList(stateId), JsonRequestBehavior.AllowGet);


        }


        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult AdvancedNoteSearch(int? page, int? page_size, int? note_setting_id, string sort, string date_from, string date_to, string setting_Description)
        {
            string whereClause = "";
            string orderByClause = "";
            string joinClause = "";
            ViewData["note_setting_id"] = WBEADMS.Models.NoteSetting.FetchSettingSelectList(note_setting_id, this.GetUser().user_id);
            ViewData["setting_Description"] = setting_Description;

            if (!date_from.IsBlank())
            {
                date_from = date_from.Replace("'", "");
                whereClause += " AND date_created >= '" + date_from + "'";
            }

            if (!date_to.IsBlank())
            {
                date_to = date_to.Replace("'", "");
                whereClause += " AND date_created <= '" + date_to + " 23:59'";
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
                        case "location":
                            orderByClause += "Locations.name,";
                            break;
                        case "desc-location":
                            orderByClause += "Locations.name DESC,";
                            break;
                        case "date-created":
                            orderByClause += "DATEPART(YEAR, date_created), DATEPART(DAYOFYEAR, date_created),";
                            break;
                        case "desc-date-created":
                            orderByClause += "DATEPART(YEAR, date_created) DESC, DATEPART(DAYOFYEAR, date_created) DESC,";
                            break;
                        case "date-occurred":
                            orderByClause += "DATEPART(YEAR, date_occurred), DATEPART(DAYOFYEAR, date_occurred),";
                            break;
                        case "desc-date-occurred":
                            orderByClause += "DATEPART(YEAR, date_occurred) DESC, DATEPART(DAYOFYEAR, date_occurred) DESC,";
                            break;
                        case "author":
                            orderByClause += "Users.last_name, Users.first_name,";
                            break;
                        case "desc-author":
                            orderByClause += "Users.last_name DESC, Users.first_name,";
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(orderByClause))
            {
                orderByClause = "Notes.date_created DESC";
            }
            else
            {
                orderByClause = orderByClause.Trim(',');
            }


            NoteSetting selectedSetting = NoteSetting.Load(note_setting_id.ToString());
            string strSampleId = "";
            if (selectedSetting.FetchNoteSettingSampleTypeIdlist().Count > 0)
            {
                foreach (var i in selectedSetting.FetchNoteSettingSampleTypeIdlist())
                {
                    strSampleId = strSampleId + "," + i.ToString();

                }
            }

            if (strSampleId.StartsWith(","))
                strSampleId = strSampleId.Substring(1);


            string strParameterId = "";
            if (selectedSetting.FetchNoteSettingParameterIdlist().Count > 0)
            {
                foreach (var i in selectedSetting.FetchNoteSettingParameterIdlist())
                {
                    strParameterId = strParameterId + "," + i.ToString();
                }

            }
            if (strParameterId.StartsWith(","))
                strParameterId = strParameterId.Substring(1);


            string strUserId = "";
            if (selectedSetting.FetchNoteSettingUserIdlist().Count > 0)
            {
                foreach (var i in selectedSetting.FetchNoteSettingUserIdlist())
                {
                    strUserId = strUserId + "," + i.ToString();
                }

            }
            if (strUserId.StartsWith(","))
                strUserId = strUserId.Substring(1);





            System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", strUserId);
            if (strParameterId != "")
            {
                if (strUserId != "")
                    whereClause += "is_deleted=0 and  note_id in (select note_id from notes_parameters  where  note_id  not in (select note_id from  notes_chainofcustodys)) and  created_by in (" + strUserId + ") and note_id in (select note_id from notes_parameters  where  parameter_id in (" + strParameterId + "))";
                else
                    whereClause += "is_deleted=0 and note_id in (select note_id from notes_parameters where  note_id not in (select note_id from  notes_chainofcustodys)) and note_id in (select note_id from notes_parameters where  parameter_id in (" + strParameterId + ") )";
            }

            if (strSampleId != "")
            {





            }
            if (!page_size.HasValue)
            {
                page_size = 40;
            }
            var urlParameters = new { action = "AdvancedNoteSearch", controller = "Note", note_setting_id = note_setting_id, sort = sort, date_from, date_to, setting_Description };
            Paginator paginator = this.AddDefaultPaginator<Note>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Note> notes = BaseModel.FetchPage<Note>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

            // add notice if there is no records
            if (notes.Count == 0)
            {
                this.AddViewNotice("No Notes were found.");
            }

            return View(notes);




        }

        public ActionResult Index(int? page, int? page_size, int? location_id, int? parameter_id, int? user_id, string is_starred, string sort, string search, string date_created_start, string date_created_end)
        {

            Note.ExpireOpenNotes();

            string whereClause = "is_deleted=0";
            string orderByClause = "";
            string joinClause = "";

            //Search 
            if (search != null)
            {
                ViewData["search"] = search;
                search = search.Trim();
                if (search != String.Empty)
                {
                    whereClause += CreateSearchClause(search);
                }
            }

            //Date Filter
            if (!date_created_start.IsBlank())
            {
                date_created_start = date_created_start.Replace("'", "");
                whereClause += " AND date_created >= '" + date_created_start + "'";
            }

            if (!date_created_end.IsBlank())
            {
                date_created_end = date_created_end.Replace("'", "");
                whereClause += " AND date_created <= '" + date_created_end + " 23:59'";
            }


            //Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                if (sort.Contains("location"))
                {
                    joinClause += "INNER JOIN Locations ON Notes.location_id = Locations.location_id ";
                }

                if (sort.Contains("author"))
                {
                    joinClause += "INNER JOIN Users ON notes.created_by = Users.user_id ";
                }

                string[] sortOrder = sort.Split('_');
                foreach (string column in sortOrder)
                {
                    switch (column)
                    {
                        case "location":
                            orderByClause += "Locations.name,";
                            break;
                        case "desc-location":
                            orderByClause += "Locations.name DESC,";
                            break;
                        case "date-created":
                            orderByClause += "DATEPART(YEAR, date_created), DATEPART(DAYOFYEAR, date_created),";
                            break;
                        case "desc-date-created":
                            orderByClause += "DATEPART(YEAR, date_created) DESC, DATEPART(DAYOFYEAR, date_created) DESC,";
                            break;
                        case "date-occurred":
                            orderByClause += "DATEPART(YEAR, date_occurred), DATEPART(DAYOFYEAR, date_occurred),";
                            break;
                        case "desc-date-occurred":
                            orderByClause += "DATEPART(YEAR, date_occurred) DESC, DATEPART(DAYOFYEAR, date_occurred) DESC,";
                            break;
                        case "author":
                            orderByClause += "Users.last_name, Users.first_name,";
                            break;
                        case "desc-author":
                            orderByClause += "Users.last_name DESC, Users.first_name,";
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(orderByClause))
            {
                orderByClause = "Notes.date_created DESC";
            }
            else
            {
                orderByClause = orderByClause.Trim(',');
            }

            //Populate filter drop downs.
            ViewData["location_id"] = Location.FetchSelectListActive(location_id);
            ViewData["user_id"] = WBEADMS.Models.User.FetchSelectListActive(user_id);
            ViewData["parameter_id"] = WBEADMS.Models.Parameter.FetchSelectListActive(parameter_id);
            ViewData["is_starred"] = WBEADMS.ControllersCommon.GetYesNoSelectList(is_starred);

            //filter
            if (location_id.HasValue)
            {
                whereClause += " AND Notes.location_id = " + location_id.Value;
            }

            if (user_id.HasValue)
            {
                whereClause += " AND Notes.created_by = " + user_id.Value;
            }

            if (parameter_id.HasValue)
            {
                joinClause += "INNER JOIN Notes_Parameters ON Notes.note_id = Notes_Parameters.note_id";
                whereClause += " AND Notes_Parameters.parameter_id = " + parameter_id.Value;
            }

            if (is_starred == "Yes")
            {
                whereClause += " AND Notes.is_starred = 1";
            }
            else if (is_starred == "No")
            {
                whereClause += " AND Notes.is_starred = 0";
            }

            //page size
            if (!page_size.HasValue)
            {
                page_size = 100;
            }

            //paginate
            var urlParameters = new { action = "Index", controller = "Note", location_id = location_id, parameter_id = parameter_id, user_id = user_id, is_starred = is_starred, sort = sort, search = search, date_created_start = date_created_start, date_created_end = date_created_end };
            Paginator paginator = this.AddDefaultPaginator<Note>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Note> notes = BaseModel.FetchPage<Note>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

            // add notice if there is no records
            if (notes.Count == 0)
            {
                this.AddViewNotice("No Notes were found.");
            }

            return View(notes);
        }

        private string CreateSearchClause(string search)
        {
            string doubleQuote = "\"";
            string searchClause = String.Empty;
            search = search.Replace("'", "''");
            // string searchClause = " TRUE "; //use this version if you don't know if you alreayd have a where clause.
            bool hasDoubleQuotes = search.Contains(doubleQuote);
            bool isWholeTerm = search.StartsWith(doubleQuote);
            search = search.Trim('"');

            foreach (string term in search.Split('"'))
            {
                if (hasDoubleQuotes && isWholeTerm)
                {
                    string wholeTerm = term;
                    if (string.IsNullOrEmpty(wholeTerm))
                    {
                        continue;
                    }

                    searchClause += " AND Notes.body like '%" + wholeTerm + "%' ";
                    isWholeTerm = false;
                }
                else
                {
                    foreach (string word in term.Split(' '))
                    {
                        string searchWord = word.Trim();
                        if (string.IsNullOrEmpty(searchWord))
                        {
                            continue;
                        }

                        if (searchWord.Length <= 2)
                        {
                            searchClause += " AND (Notes.body like '" + searchWord + "%' OR Notes.body like '% " + searchWord + "%') ";
                        }
                        else
                        {
                            searchClause += " AND Notes.body like '%" + searchWord + "%' ";
                        }
                    }

                    isWholeTerm = true;
                }
            }

            return searchClause;
        }

        // GET: /Note/Details/5
        public ActionResult Details(string id)
        {
            var item = Note.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The note " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            ViewData["parent_type"] = Note.ParentType.Note;
            ViewData["parent_id"] = item.id;

            return View(item);
        }

        public ActionResult Star(string parent_id)
        {
            // TODO: check roles for permission to star
            var parent = Note.Load(parent_id);
            var item = new Note();
            item.parent = parent;
            item.location_id = parent.location_id;
            item.parameter_list = parent.parameter_list;
            item.date_occurred = DateTime.Now.ToISODateTime();
            item.body = (parent.starred) ? "Removed star from note. " : "Added star to note. ";
            ViewData["noteHeading"] = (parent.starred) ? "Reason for Unstarring:" : "Reason for Starring:";

            SetViewDataForm(item);
            return View("Create", item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Star(string parent_id, FormCollection collection)
        {
            // TODO: check roles for permission to star
            var parent = Note.Load(parent_id);
            var item = new Note();
            item.parent = parent;

            item.created_by = this.GetUser().id;
            item.date_created = DateTime.Now.ToString();
            try
            {
                UpdateModel(item);
                item.parameter_list = collection["parameter_list"]; // TODO: figure out how to make UpdateModel set this param; it seems to be ignoring checkboxes without hidden, or it will only take the value of the first checkbox
                item.Save();

                // TODO: revert item if parent fails to save
                parent.starred = !parent.starred;
                parent.committed = true;
                parent.Save();
                this.AddTempNotice("Successfully " + (parent.starred ? "unstarred" : "starred") + " note for " + parent.ToString());
                return RedirectToRoute(new { action = "Details", id = parent.id });
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                ViewData["noteHeading"] = (parent.starred) ? "Reason for Unstarring:" : "Reason for Starring:";
                item.body = (parent.starred) ? "Action completed. " : "Note marked for action. ";
                SetViewDataForm(item);
                return View("Create", item);
            }
        }

        ////[AcceptVerbs(HttpVerbs.Post)]
        ////public ActionResult Star(string id, string returnAction) {
        ////    var item = Note.Load(id);
        ////    if (item == null) {
        ////        this.AddTempNotice("The note " + id + " could not be found.");
        ////        return RedirectToAction("Index");
        ////    }
        ////    if (returnAction.IsBlank()) { returnAction = "Index"; }
        ////    try {
        ////        item.starred = !item.starred;
        ////        item.modified_by = this.GetUser().id;
        ////        item.date_modified = DateTime.Now.ToString();
        ////        item.Save();
        ////        return Json(new { id = item.id, starred = item.starred.ToString().ToLower() });
        ////    }
        ////    catch (ModelException e) {
        ////        this.AddTempNotice(e.Message);
        ////        return Json(false);
        ////    }
        ////}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string id, string returnAction)
        {
            var item = Note.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The note " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            if (returnAction.IsBlank()) { returnAction = "Index"; }
            try
            {
                item.deleted = true;
                item.modified_by = this.GetUser().id;
                item.date_modified = DateTime.Now.ToString();
                item.Save();
                return RedirectToAction(returnAction, new { id = id });
            }
            catch (ModelException e)
            {
                this.AddTempNotice(e.Message);
                return RedirectToAction(returnAction, new { id = id });
            }
        }

        // GET: /Note/Create
        // TODO: mabye a overload will clean this up if mvc will allow it.
        public ActionResult Create(int? parent_id, int? parent_type)
        {
            var item = new Note();

            // TODO: parent type, parent id should be selectable (same as old DocIt)
            //  parent can be latest scheduled datasheet or 
            if (parent_type == (int)Note.ParentType.Location && parent_id != null)
            {
                item.parent_type_id = null;
                item.parent_id = null;
                item.location_id = parent_id.ToString();
            }
            else
            {
                item.parent_type_id = parent_type.ToString();
                item.parent_id = parent_id.ToString();
                if (item.HasParent)
                {
                    item.location_id = item.GetParentLocationId();
                }
                else
                {
                    item.location_id = this.GetLocationId();
                }
            }

            if (item.parent_type == Note.ParentType.Item)
            {
                var parentItem = Item.Load(item.parent_id);
                item.parameter_list = parentItem.parameter_list;
            }


            // System.IO.File.WriteAllText(@"C:\temp\strparentid.txt", Request["coc_id"].Params);
            item.date_occurred = DateTime.Now.ToISODateTime();
            SetViewDataForm(item);
            return View(item);
        }
        public ActionResult Create1(int? parent_id, int? parent_type, string cocid)
        {
            var item = new Note();

            // TODO: parent type, parent id should be selectable (same as old DocIt)
            //  parent can be latest scheduled datasheet or 
            if (parent_type == (int)Note.ParentType.Location && parent_id != null)
            {
                item.parent_type_id = null;
                item.parent_id = null;
                item.location_id = parent_id.ToString();
            }
            else
            {
                item.parent_type_id = parent_type.ToString();
                item.parent_id = parent_id.ToString();
                if (item.HasParent)
                {
                    var coc = ChainOfCustody.Load(cocid);
                    if (coc.Preparation.Schedule != null)
                    {
                        item.location_id = coc.Preparation.Schedule.location_id;
                    }
                    else if (coc.Deployment.Location != null)
                    {
                        item.location_id = coc.Deployment.location_id;
                    }
                    else
                    {
                        item.location_id = null;
                    }
                }
                else
                {
                    item.location_id = this.GetLocationId();
                }
            }

            if (item.parent_type == Note.ParentType.Item)
            {
                var parentItem = Item.Load(item.parent_id);
                item.parameter_list = parentItem.parameter_list;
            }
            ViewData["parent_type"] = parent_type.ToString();
            ViewData["parent_id"] = parent_id.ToString();
            ViewData["cocid"] = cocid;
            //  System.IO.File.WriteAllText(@"C:\temp\strparentid.txt", cocid);
            item.date_occurred = DateTime.Now.ToISODateTime();
            SetViewDataForm(item);
            return View(item);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create1(int? parent_id, int? parent_type, FormCollection collection)
        {
            var item = new Note();
            string cocid = collection["cocid"].ToString();
            // if they create a note and specify a parent, it must be an Item  NOTE: currently Note Parent is limited to only items
            //System.IO.File.WriteAllText(@"C:\temp\strparentidsave.txt", collection["cocid"].ToString()+parent_id.ToString());

            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time");

            item.parent_type_id = parent_type.ToString();
            item.parent_id = parent_id.ToString();




            item.created_by = this.GetUser().id;
            item.date_created = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo).ToString();

            try
            {
                UpdateModel(item);
                item.parameter_list = collection["parameter_list"]; // TODO: figure out how to make UpdateModel set this param; it seems to be ignoring checkboxes without hidden, or it will only take the value of the first checkbox
                item.Save();

                // Note was created! Now save the attachment if there is one
                System.Web.HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
                if (file != null)
                {
                    // File was attached!
                    string fname = System.IO.Path.GetFileName(file.FileName);
                    NoteAttachment att = new NoteAttachment();
                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                    {
                        att.attachment = binaryReader.ReadBytes(file.ContentLength);
                        att.mime_type = file.ContentType;
                    }
                    att.note_id = item.id;
                    att.filename = file.FileName;
                    att.date_modified = TimeZoneInfo.ConvertTime(System.DateTime.Now, timeZoneInfo).ToString();
                    att.date_uploaded = TimeZoneInfo.ConvertTime(System.DateTime.Now, timeZoneInfo).ToString();
                    att.uploaded_by = item.created_by;
                    att.modified_by = item.created_by;
                    //   att.Save();
                }

                this.AddTempNotice("Successfully created note for " + item.date_created);

                return RedirectToAction("Open", new { id = cocid, controller = "ChainOfCustody" });

            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                ViewData["noteHeading"] = "Create Note";
                SetViewDataForm(item);
                return View(item);
            }
        }

        // POST: /Note/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int? parent_id, int? parent_type, FormCollection collection)
        {
            var item = new Note();


            if (!collection["parent_id"].IsBlank() && collection["parent_type"].IsBlank())
            {
                collection["parent_type"] = ((int)Note.ParentType.Item).ToString();
            }


            // TODO: parent type, parent id should be selectable (same as old DocIt)
            if (parent_type == (int)Note.ParentType.Location && parent_id != null)
            {
                item.parent_type_id = null;
                item.parent_id = null;
            }
            else
            {
                item.parent_type_id = parent_type.ToString();
                item.parent_id = parent_id.ToString();
            }



            item.created_by = this.GetUser().id;
            item.date_created = DateTime.Now.ToString();

            try
            {
                UpdateModel(item);
                item.parameter_list = collection["parameter_list"]; // TODO: figure out how to make UpdateModel set this param; it seems to be ignoring checkboxes without hidden, or it will only take the value of the first checkbox
                item.Save();

                // Note was created! Now save the attachment if there is one
                System.Web.HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
                if (file != null)
                {
                    // File was attached!
                    string fname = System.IO.Path.GetFileName(file.FileName);
                    NoteAttachment att = new NoteAttachment();
                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                    {
                        att.attachment = binaryReader.ReadBytes(file.ContentLength);
                        att.mime_type = file.ContentType;
                    }
                    att.note_id = item.id;
                    att.filename = file.FileName;
                    att.date_modified = System.DateTime.Now.ToString();
                    att.date_uploaded = System.DateTime.Now.ToString();
                    att.uploaded_by = item.created_by;
                    att.modified_by = item.created_by;
                    att.Save();
                }

                this.AddTempNotice("Successfully created note for " + item.date_created);
                switch (item.parent_type)
                {
                    case Note.ParentType.ChainOfCustody:
                        return RedirectToAction("Details", new { id = item.parent_id, controller = "ChainOfCustody" });
                    case Note.ParentType.Item:
                        return RedirectToAction("Details", new { id = item.parent_id, controller = "Item" });
                    case Note.ParentType.Location:
                        return RedirectToAction("Details", new { id = item.parent_id, controller = "Location" });
                    case Note.ParentType.Schedule:
                        return RedirectToAction("Details", new { id = item.parent_id, controller = "Schedule" });
                    case Note.ParentType.Sample:

                        return RedirectToAction("Details", new { id = item.parent_id, controller = "Sample" });

                    default:
                        return RedirectToAction("Details", new { id = item.id });
                }
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                ViewData["noteHeading"] = "Create Note";
                SetViewDataForm(item);
                return View(item);
            }
        }

        // GET: /Note/Edit/5
        public ActionResult Edit(string id)
        {
            // TODO: check user == item.created_by; only author allowed to edit their own notes
            var item = Note.Load(id);

            // TODO: parent type, parent id should be selectable (same as old DocIt)
            if (item == null)
            {
                this.AddTempNotice("The note " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            SetViewDataForm(item);
            return View(item);
        }


        public ActionResult DownloadAttachment(string id)
        {
            NoteAttachment att = NoteAttachment.Load(id);
            if (att != null)
            {
                byte[] attachment = att.GetAttachment();
                if (att != null)
                {
                    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition();
                    cd.FileName = att.filename;

                    Response.AppendHeader("Content-Disposition", cd.ToString());
                    return File(att.GetAttachment(), att.mime_type);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // POST: /Note/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            // TODO: check user == item.created_by; only author allowed to edit their own notes
            var item = Note.Load(id);

            // TODO: parent type, parent id should be selectable (same as old DocIt)
            item.modified_by = this.GetUser().id;
            item.date_modified = DateTime.Now.ToString();
            if (item == null)
            {
                this.AddTempNotice("The note " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            try
            {
                UpdateModel(item);
                item.parameter_list = collection["parameter_list"]; // TODO: figure out how to make UpdateModel set this param; it seems to be ignoring checkboxes without hidden, or it will only take the value of the first checkbox
                item.Save();

                // Is there a file we're going to try changing?
                System.Web.HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
                if (file != null)
                {
                    // File was attached!
                    string fname = System.IO.Path.GetFileName(file.FileName);
                    NoteAttachment att = NoteAttachment.GetAttachmentByNoteID(item.note_id);
                    if (att == null) att = new NoteAttachment();

                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                    {
                        att.attachment = binaryReader.ReadBytes(file.ContentLength);
                        att.mime_type = file.ContentType;
                    }
                    att.note_id = item.id;
                    att.filename = file.FileName;
                    att.date_modified = System.DateTime.Now.ToString();
                    att.date_uploaded = System.DateTime.Now.ToString();
                    att.uploaded_by = item.created_by;
                    att.modified_by = item.created_by;
                    att.Save();
                }

                this.AddTempNotice("Successfully updated note for " + item.date_created);
                return RedirectToRoute(new { action = "Details", id = item.id });
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                SetViewDataForm(item);
                return View(item);
            }
        }

        public ActionResult GetLocationParameters(string id)
        {
            var list = new List<object>();
            var location = Location.Load(id);

            if (location == null)
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            foreach (var parameter in location.Parameters)
            {
                list.Add(new { name = parameter.name, id = parameter.id });
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ParentDropdownForm(string id)
        {
            if (id.IsBlank() || !id.IsInt() || Item.TotalCount("Items", "location_id = " + id) == 0)
            {
                return Content("<span id='parent_id_span'>None</span>");
            }

            ViewData["parent_id"] = Item.FetchSelectList(null, id, null); // NOTE: currently Note Parent is limited to only items

            return View("ParentDropdownForm");
        }

        public ActionResult GetItemParameter(string id)
        {
            var item = Item.Load(id);

            if (item == null)
            {
                return Json(new string[] { "0" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(item.parameter_list.Split(','), JsonRequestBehavior.AllowGet);
            }
        }

        private void SetViewDataForm(Note item)
        {
            ViewData["location_id"] = Location.FetchSelectListActive(item.location_id);
            ViewData["parent_id"] = Item.FetchSelectList(null, item.location_id, item.parent_id); // NOTE: currently Note Parent is limited to only items

            List<string> parameterIds;
            if (item.parameter_list.IsBlank())
            {
                parameterIds = new List<string>();
                foreach (var parameter in item.parameters)
                {
                    parameterIds.Add(parameter.id);
                }
            }
            else
            {
                parameterIds = new List<string>(item.parameter_list.Split(','));
            }

            ViewData["parameter_ids"] = parameterIds;
        }

        // TODO: allow select parent on creation/edit
        private object[] GetParentOptions(string location_id)
        {
            var location = Location.Load(location_id);

            var items = new List<object>();
            foreach (var item in location.Items)
            {
                items.Add(new { text = item.DisplayName, value = item.id });
            }

            var cocs = new List<object>();
            /*
            foreach(var coc in Schedule.Fe
            return new object[] {
                new { optiongroup = "Item Parameters", options = items.ToArray()},
                new { optiongroup = "Chain of Custodys", options = cocs.ToArray()}
            };
             */
            throw new NotImplementedException();
        }

        public class notesql
        {
            public int note_id { get; set; }
            public string location_id { get; set; }
            public string name { get; set; }
            public string date_created { get; set; }
            public string date_occurred { get; set; }
            public string parameter { get; set; }
            public string username { get; set; }
            public string body { get; set; }
        }

        public ActionResult ListNotesMysql(int? page)
        {
            var notemysql = new List<notesql>();
            Dictionary<int, string> paraList = new Dictionary<int, string>();
            try
            {
                MySqlConnection connection1 = new MySqlConnection(ModelsCommon.FetchMysqlConnectionString());
                connection1.Open();
                MySqlCommand cmd1 = new MySqlCommand("select Group_concat(p.name) as Parameter ,np.note_id from docit.parameters p join docit.notes_parameters np on p.parameter_id=np.parameter_id group by np.note_id ", connection1);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();

                while (dataReader1.Read() && dataReader1.HasRows)
                {

                    paraList.Add(dataReader1.GetInt32(1), dataReader1.GetString(0));

                }
                dataReader1.Close();
                connection1.Close();


                MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchMysqlConnectionString());


                MySqlCommand cmd = new MySqlCommand("SELECT n.note_id, l.location_id, l.name, n.date_created , n.date_occurred, concat(u.first_name, ' ', u.last_name) as username, n.body FROM docit.notes n left join docit.locations l on n.location_id = l.location_id  left join docit.users u on n.created_by = u.user_id   ", connection);



                connection.Open();
                ViewData["test"] = "Server Version: " + connection.ServerVersion
                  + "\nDatabase: " + connection.Database + " " + connection.Site;

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    notesql n = new notesql();

                    n.note_id = dataReader.GetInt32(0);
                    n.location_id = (dataReader.IsDBNull(1) ? " " : dataReader.GetInt32(1).ToString());
                    n.name = ((dataReader.IsDBNull(2) == true) ? "" : dataReader.GetString(2));
                    n.date_created = dataReader.IsDBNull(3) ? "" : (Convert.ToDateTime(dataReader["date_created"])).ToString();
                    n.date_occurred = dataReader.IsDBNull(4) ? "" : (Convert.ToDateTime(dataReader["date_occurred"])).ToString();
                    n.username = (dataReader.IsDBNull(5) ? "" : dataReader.GetString(5));
                    n.body = (dataReader.IsDBNull(6) ? " " : dataReader.GetString(6));
                    string result = "";
                    n.parameter = paraList.TryGetValue(n.note_id, out result) ? result : "";








                    ViewData["test"] = ViewData["test"];

                    notemysql.Add(n);
                }

                dataReader.Close();
                dataReader.Close();
                //    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                //  {
                //     da.Fill(dataTable);
                //  }
                // var p = dataTable;


                connection.Close();
                var pagenumber = page ?? 1;
                var onepageNote = notemysql.ToPagedList(pagenumber, 40);

                ViewBag.onepageNote = onepageNote;
            }
            catch (Exception ex)
            {
                ViewData["strerror"] = ex.Message.ToString();
            }

            return View();
        }
        protected T GetValue<T>(object obj)
        {
            if (typeof(DBNull) != obj.GetType())
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            return default(T);
        }

        protected T GetValue<T>(object obj, object defaultValue)
        {
            if (typeof(DBNull) != obj.GetType())
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            return (T)defaultValue;
        }
    }
}