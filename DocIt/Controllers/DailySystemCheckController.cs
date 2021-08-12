
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class DailySystemCheckController : Controller
    {

        public ActionResult Index(int? page, int? page_size, int? location_id, int? parameter_id, int? user_id, string is_starred, string sort, string search, string date_occurred_start, string date_occurred_end)
        {

            Note.ExpireOpenNotes();

            string joinClause = "";
            string orderByClause = "";
            string whereClause = "is_deleted=0 AND dsc_tag IS NOT NULL";

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
            if (!date_occurred_start.IsBlank())
            {
                date_occurred_start = date_occurred_start.Replace("'", "");
                whereClause += " AND date_occurred >= '" + date_occurred_start + "'";
            }

            if (!date_occurred_end.IsBlank())
            {
                date_occurred_end = date_occurred_end.Replace("'", "");
                whereClause += " AND date_occurred <= '" + date_occurred_end + " 23:59'";
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
                orderByClause = "Notes.date_occurred DESC";
            }
            else
            {
                orderByClause = orderByClause.Trim(',');
            }

            //Populate filter drop downs.
            ViewData["location_id"] = Location.FetchSelectListWithLogger(location_id);
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

            var urlParameters = new { action = "Index", controller = "DailySystemCheck", location_id = location_id, parameter_id = parameter_id, is_starred = is_starred, sort = sort, search = search, date_occurred_start = date_occurred_start, date_occurred_end = date_occurred_end };
            Paginator paginator = this.AddDefaultPaginator<Note>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Note> notes = BaseModel.FetchPage<Note>(paginator, new { join = joinClause, where = whereClause, order = orderByClause });
            //paginator.UrlParameters.Add("location", location.ToString());

            // add notice if there is no records
            if (notes.Count == 0)
            {
                this.AddViewNotice("No Daily System Check notes were found.");
            }

            return View(notes);
        }

        //TODO: CENTRALIZE THIS AND REMOVE THIS METHOD!!!!!
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

        public ActionResult SelectDSCLocation()
        {
            var lastDSCs = Note.GetLatestDailySystemChecks();
            var locations = Location.FetchWithLogger();

            // choose location without a DSC for today; skip locations that already have a DSC for today
            foreach (var location in locations)
            {
                if (lastDSCs[location.id] > DateTime.Today)
                {
                    continue;
                }

                return RedirectToRoute(new { action = "Create", location_id = location.id });
            }

            // default to first one
            return RedirectToRoute(new { action = "Create", location_id = locations[0].id }); // NOTE: assume locations is not an empty list
        }

        // GET: /DailySystemCheck/Create
        public ActionResult Create(string location_id)
        {
            var location = Location.Load(location_id);
            var parameters = location.Parameters;
            ViewData["location"] = location;
            ViewData["parameters"] = parameters;
            ViewData["lastDSCs"] = Note.GetLatestDailySystemChecks();
            ViewData["locations"] = Location.FetchWithLogger();

            return View("Form");
        }

        // POST: /DailySystemCheck/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string location_id, FormCollection collection)
        {
            var location = Location.Load(location_id);
            var parameters = location.Parameters;

            // create Notes for saving
            var parameter_notes = new Dictionary<string, Note>();
            var general_parameter_list = new List<string>();
            String currentTime = DateTime.Now.ToISODateTime();
            foreach (var parameter in parameters)
            {
                var note = new Note();
                note.dsc_tag = "DSC " + DateTime.Today.ToISODate();
                note.date_occurred = currentTime;
                note.created_by = this.GetUser().id;
                note.date_created = currentTime;
                note.parameter_list = parameter.id;
                note.location_id = location.id;
                parameter_notes.Add(parameter.id, note);
                ////general_parameter_list.Add(parameter.id); // generate parameter_list for the General note
            }

            var generalNote = new Note();
            generalNote.dsc_tag = "DSC " + DateTime.Today.ToISODate();
            generalNote.date_occurred = currentTime;
            generalNote.created_by = this.GetUser().id;
            generalNote.date_created = currentTime;
            generalNote.location_id = location.id;
            //// generalNote.parameter_list = String.Join(",", general_parameter_list.ToArray()); // General note contains all parameters  // NOTE: General note should NOT contain any parameter
            parameter_notes.Add("0", generalNote);

            // Fill out notes from form
            FormUpdateModel(parameter_notes, collection);

            /*   (removed validate as per 2010-06-11 FogBugz 505)
            // Validation : check if any note is missing body
            ModelException allErrors = FormValidateBody(parameter_notes);
            if (allErrors.hasErrors) {
                allErrors.AddError(new ValidationError("body_0", "General text must be entered if other fields are to be left blank."));
                this.PopulateViewWithErrorMessages(allErrors);
                ViewData["location"] = location;
                ViewData["parameters"] = parameters;
                ViewData["lastDSCs"] = Note.GetLatestDailySystemChecks();
                ViewData["locations"] = Location.FetchWithLogger();
                return View("Form");  // Exit if any fails
            }
             */

            // Combine notes with the exact same body (the parameter list will contain all param ids with common body)
            var param_notes_tmp = new Dictionary<string, string>();
            var duplicate_param_ids = new List<string>();
            foreach (var note in parameter_notes)
            {
                string tmp_key = note.Value.body;
                string parameter_id = note.Key;

                if (parameter_id == "0") { continue; } // don't group up the General comment; plus, things'll break if 0 gets added to a parameter list.

                if (!param_notes_tmp.ContainsKey(tmp_key))
                {
                    param_notes_tmp.Add(tmp_key, parameter_id);
                }
                else
                {
                    string existing_param_id = param_notes_tmp[tmp_key];
                    parameter_notes[existing_param_id].parameter_list += "," + parameter_id;
                    duplicate_param_ids.Add(parameter_id);
                }
            }

            foreach (string parameter_id in duplicate_param_ids)
            {
                parameter_notes.Remove(parameter_id);
            }

            // Save all notes related to parameters
            FormSaveNotes(parameter_notes);
            this.AddTempNotice("Successfully performed Daily System Check at " + DateTime.Today.ToISODate() + " for <b>" + location.name + "</b>");

            // figure out next Location for DSC
            if (collection["submit"] == "Save")
            {
                return RedirectToAction("Edit", new { location_id = location_id });
            }
            else
            {
                return NextDSCLocation(location);
            }
        }

        // GET: /DailySystemCheck/Edit
        public ActionResult Edit(string location_id)
        {

            var location = Location.Load(location_id);
            if (location == null)
            {
                this.AddTempNotice("Unknown location " + location_id);
                return RedirectToAction("Index");
            }

            var notes = BaseModel.FetchAll<Note>("is_deleted = 0 AND date_occurred = (SELECT MAX(date_occurred) FROM Notes WHERE dsc_tag IS NOT NULL and location_id = " + location_id + ")");
            //notes = BaseModel.FetchAll<Note>("is_deleted = 0 AND dsc_tag = '" + notes[0].dsc_tag + "'");
            // TODO: what are we gonna do about historical DSCs?
            var parameters = notes[0].Location.Parameters;

            ViewData["location"] = location;
            ViewData["parameters"] = parameters;
            ViewData["lastDSCs"] = Note.GetLatestDailySystemChecks();
            ViewData["locations"] = Location.FetchWithLogger();

            var parameter_notes = new Dictionary<string, Note>();
            foreach (var note in notes)
            {
                if (note.parameters.Length == 0)
                {
                    parameter_notes.Add("0", note);
                }
                else
                {
                    foreach (var parameter in note.parameters)
                    {
                        parameter_notes.Add(parameter.id, note);
                    }
                }
            }

            ViewData["notes"] = parameter_notes;

            return View("Form");
        }

        // POST: /DailySystemCheck/Edit
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string location_id, FormCollection collection)
        {
            var location = Location.Load(location_id);
            var parameters = location.Parameters;

            // get the current DSC TAG
            string dsc_tag = null;
            String currentTime = DateTime.Now.ToISODateTime();
            foreach (var key in collection.AllKeys)
            {
                if (key.Contains("id"))
                {
                    string id = collection[key];
                    if (id == "" || id == "0") { continue; }
                    Note note = Note.Load(id);
                    dsc_tag = note.dsc_tag;
                    currentTime = note.date_occurred;
                    break;
                }
            }

            // create/load Notes for saving
            var parameter_notes = new Dictionary<string, Note>();
            var general_parameter_list = new List<string>();
            foreach (var key in collection.AllKeys)
            {
                if (key.Contains("id"))
                {
                    string param_id = key.Replace("id_", "");
                    string note_id = collection[key];
                    Note note;
                    if (note_id == "" || note_id == "0")
                    {
                        note = new Note();
                        note.dsc_tag = dsc_tag;
                        note.date_occurred = currentTime;
                        note.created_by = this.GetUser().id;
                        note.date_created = DateTime.Now.ToISODateTime();
                        note.location_id = location.id;
                    }
                    else
                    {
                        note = Note.Load(note_id);
                        note.modified_by = this.GetUser().id;
                        note.date_modified = DateTime.Now.ToISODateTime();
                    }

                    parameter_notes.Add(param_id, note);
                }
            }

            // Update notes from form
            FormUpdateModel(parameter_notes, collection);

            /*   (removed validate as per 2010-06-11 FogBugz 505)
            // Validation : check if any note is missing body
            ModelException allErrors = FormValidateBody(parameter_notes);
            if (allErrors.hasErrors) {
                allErrors.AddError(new ValidationError("body_0", "General text must be entered if other fields are to be left blank."));
                this.PopulateViewWithErrorMessages(allErrors);
                ViewData["location"] = location;
                ViewData["parameters"] = parameters;
                ViewData["lastDSCs"] = Note.GetLatestDailySystemChecks();
                ViewData["locations"] = Location.FetchWithLogger();
                ViewData["notes"] = parameter_notes;
                return View("Form");  // Exit if any fails
            }
             */

            // Look for deleted notes (empty body), (the actual Note delete is just before save)
            var deleted_parameters = new List<string>();
            var deleted_note_id = new List<string>();
            foreach (var kvp in parameter_notes)
            {
                Note note = kvp.Value;
                if (!note.id.IsBlank() && note.body.IsBlank())
                {
                    deleted_parameters.Add(kvp.Key);
                    deleted_note_id.Add(note.id);
                }
            }

            foreach (string parameter_id in deleted_parameters)
            {
                parameter_notes.Remove(parameter_id);
            }

            // Parse existing notes first, combine notes with same body
            var param_notes_tmp = new Dictionary<string, string>(); // hash of note body to parameter_id
            var duplicate_param_ids = new List<string>();
            foreach (var kvp in parameter_notes)
            {
                string parameter_id = kvp.Key;
                if (parameter_id == "0") { continue; }
                Note note = kvp.Value;
                if (note.id.IsBlank()) { continue; }

                string key = note.body + note.is_starred;
                if (!param_notes_tmp.ContainsKey(key))
                {
                    param_notes_tmp.Add(key, parameter_id);
                    note.parameter_list = parameter_id; // reset parameter list
                }
                else
                {
                    string existing_param_id = param_notes_tmp[key];
                    parameter_notes[existing_param_id].parameter_list += "," + parameter_id;
                    duplicate_param_ids.Add(parameter_id);
                    if (note.id != parameter_notes[existing_param_id].id)
                    { // check if this note is merger of two notes
                        var oldNote = Note.Load(note.id);
                        oldNote.is_deleted = "True"; // if this note is being merged into another, delete this note
                        oldNote.Save();
                    }
                }
            }


            // Parse new notes, combine new notes 
            var existing_notes_tmp = new Dictionary<string, string>();
            foreach (var kvp in parameter_notes)
            {
                string parameter_id = kvp.Key;
                if (parameter_id == "0") { continue; }
                Note note = kvp.Value;
                if (!note.id.IsBlank()) { continue; }

                string key = note.body + note.is_deleted;
                if (!param_notes_tmp.ContainsKey(key))
                {
                    param_notes_tmp.Add(key, parameter_id);
                    note.parameter_list = parameter_id;
                }
                else
                {
                    string existing_param_id = param_notes_tmp[key];
                    parameter_notes[existing_param_id].parameter_list += "," + parameter_id;
                    duplicate_param_ids.Add(parameter_id);
                }
            }

            foreach (string parameter_id in duplicate_param_ids)
            {
                parameter_notes.Remove(parameter_id);
            }

            // Split notes (if existing note becomes changed)
            var existingNoteIds = new List<string>();
            var newNotes = new Dictionary<string, Note>();
            foreach (var kvp in parameter_notes)
            {
                string parameter_id = kvp.Key;
                Note note = kvp.Value;
                if (!existingNoteIds.Contains(note.id))
                {
                    existingNoteIds.Add(note.id);
                }
                else
                {
                    Note newNote = new Note
                    {
                        date_occurred = note.date_occurred,
                        body = note.body,
                        is_starred = note.is_starred,
                        dsc_tag = note.dsc_tag,
                        location_id = note.location_id,
                        parameter_list = note.parameter_list,
                        created_by = this.GetUser().id,
                        date_created = currentTime
                    };
                    newNotes.Add(parameter_id, newNote);
                }
            }
            foreach (var kvp in newNotes)
            {
                string parameter_id = kvp.Key;
                Note newNote = kvp.Value;
                parameter_notes[parameter_id] = newNote;
            }

            // Delete notes (existing notes that have no body)
            foreach (var note_id in deleted_note_id)
            {
                if (existingNoteIds.Contains(note_id)) { continue; } // don't delete if the parameter_notes contains one reference to note
                Note deletedNote = Note.Load(note_id);
                deletedNote.is_deleted = "True";
                deletedNote.Save();
            }

            // Save all notes related to parameters
            FormSaveNotes(parameter_notes);
            this.AddTempNotice("Updated Daily System Check at " + DateTime.Today.ToISODate() + " for <b>" + location.name + "</b>");

            // figure out next Location for DSC
            if (collection["submit"] == "Save")
            {
                return RedirectToAction("Edit", new { location_id = location_id });
            }
            else
            {
                return NextDSCLocation(location);
            }
        }

        private void FormUpdateModel(Dictionary<string, Note> parameter_notes, FormCollection collection)
        {
            // Parse form collection & generate ViewData in case validation fails and you need to retain form collection
            foreach (var key in collection.AllKeys)
            {
                if (key.Contains("star"))
                {
                    parameter_notes[key.Replace("star_", "")].starred = collection[key].Contains("true");
                    ViewData[key] = collection[key].Contains("true");
                }
                else if (key.Contains("body"))
                {
                    parameter_notes[key.Replace("body_", "")].body = collection[key].Trim();
                    ViewData[key] = collection[key];
                }
                else if (key.Contains("ok"))
                {
                    ViewData[key] = collection[key].Contains("true");
                }
            }
        }

        private ModelException FormValidateBody(Dictionary<string, Note> parameter_notes)
        {
            // Validation : check if any note is missing body
            ModelException allErrors = new ModelException();
            if (parameter_notes["0"].body.IsBlank())
            {
                foreach (var param_note in parameter_notes)
                {
                    string parameter_id = param_note.Key;
                    if (parameter_id == "0") { continue; }

                    var note = param_note.Value;
                    if (note.body.IsBlank())
                    {
                        string name = Parameter.Load(parameter_id).name;
                        allErrors.AddError(new ValidationError("body_" + parameter_id, "Missing text for " + name));
                    }
                }
            }

            return allErrors;
        }

        private void FormSaveNotes(Dictionary<string, Note> parameter_notes)
        {
            foreach (var note in parameter_notes)
            {
                if (!note.Value.body.IsBlank())
                {
                    note.Value.Save();
                }
            }
        }

        private RedirectToRouteResult NextDSCLocation(Location location)
        {
            // figure out next Location for DSC
            var locations = Location.FetchWithLogger();
            var currentLocation = locations.IndexOf(location);
            if (currentLocation + 1 < locations.Count)
            {
                var newLocation = locations[currentLocation + 1];
                return RedirectToRoute(new { action = "Create", location_id = newLocation.id });
            }
            else
            {
                return RedirectToRoute(new { action = "Index" });
            }
        }
    }
}