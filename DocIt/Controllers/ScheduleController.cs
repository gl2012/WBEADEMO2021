
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Controllers.LocationExtensions;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        public ActionResult Calendar(int? month, int? year, string location, string sample_type_id)
        {
            if (location == "all")
            {
                location = null;
            }
            else if (!month.HasValue && location.IsBlank() || !location.IsInt())
            {
                location = this.GetLocationId();
            }

            if (!sample_type_id.IsInt() && sample_type_id != "XPASS")
            {
                sample_type_id = null;
            }

            int m = month ?? DateTime.Today.Month;
            int y = year ?? DateTime.Today.Year;

            if (new DateTime(y, m, 1).AddMonths(-3) > new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
            {
                var maxDate = DateTime.Today.AddMonths(3);
                return RedirectToAction("Calendar", new { month = maxDate.Month, year = maxDate.Year, location = location, sample_type = sample_type_id });
            }

            // calendar default on right side bar
            ViewData["this_month"] = m;
            ViewData["this_year"] = y;

            // location filter

            //System.IO.File.WriteAllText(@"C:\temp\locationText.txt", location);
            // ViewData["location_filter"] = Location.FetchSelectListActive(location);
            ViewData["location_filter"] = UserLocations.FetchuserUserLocationlist(this.GetUser().user_id, location);
            // sample type filter (XPASS = Integrated Samples Only)
            var sample_type_list = SampleType.FetchDictionaryByName();
            sample_type_list.Add("XPASS", "NON PASS");
            ViewData["sample_type_filter"] = sample_type_list;

            ViewData["selected_sample_type_id"] = sample_type_id;
            var sample_type_custom_list = UserSampleType.FetchCustom(this.GetUser().user_id);
            ViewData["sample_type_custom"] = sample_type_custom_list;
            var calendar = new ScheduleCalendar(m, y, location, sample_type_id);

            // add temp message
            if (calendar.Count == 0)
            {
                if (!location.IsBlank() || !sample_type_id.IsBlank())
                {
                    this.AddViewNotice("No CoCs found under those filters.");
                }
                else
                {
                    this.AddViewNotice("No Chain of Custodys found for this month.");
                }
            }

            return View(calendar);
        }

        public ActionResult Index(int? page, int? page_size, int? location_id, int? contact_id, int? sample_type_id, int? interval_id, string is_active, string sort)
        {
            string joinClause = "";
            string whereClause = "1 = 1";
            string orderByClause = "";

            //Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                if (sort.Contains("sample-type"))
                {
                    joinClause += "INNER JOIN SampleTypes ON Schedules.sample_type_id = SampleTypes.sample_type_id ";
                }

                if (sort.Contains("contact"))
                {
                    joinClause += "LEFT OUTER JOIN Users ON Schedules.contact_id = Users.user_id ";
                }

                if (sort.Contains("location"))
                {
                    joinClause += "INNER JOIN Locations ON Schedules.location_id = Locations.location_id ";
                }

                string[] sortOrder = sort.Split('_');
                foreach (string column in sortOrder)
                {
                    switch (column)
                    {
                        case "name":
                            orderByClause += "Schedules.name,";
                            break;
                        case "desc-name":
                            orderByClause += "Schedules.name DESC,";
                            break;
                        case "location":
                            orderByClause += "Locations.name,";
                            break;
                        case "desc-location":
                            orderByClause += "Locations.name DESC,";
                            break;
                        case "contact":
                            orderByClause += "Users.last_name, Users.first_name,";
                            break;
                        case "desc-contact":
                            orderByClause += "Users.last_name DESC, Users.first_name,";
                            break;
                        case "sample-type":
                            orderByClause += "SampleTypes.name,";
                            break;
                        case "desc-sample-type":
                            orderByClause += "SampleTypes.name DESC,";
                            break;
                        case "start-date":
                            orderByClause += "Schedules.date_start,";
                            break;
                        case "desc-start-date":
                            orderByClause += "Schedules.date_start DESC,";
                            break;
                        case "end-date":
                            orderByClause += "Schedules.date_end,";
                            break;
                        case "desc-end-date":
                            orderByClause += "Schedules.date_end DESC,";
                            break;
                        case "interval":
                            orderByClause += "Schedules.interval_id, Schedules.frequency_data,";
                            break;
                        case "desc-interval":
                            orderByClause += "Schedules.interval_id DESC, Schedules.frequency_data,";
                            break;
                        case "active":
                            orderByClause += "Schedules.is_active,";
                            break;
                        case "desc-active":
                            orderByClause += "Schedules.is_active DESC,";
                            break;
                    }
                }
            }

            orderByClause = orderByClause.Trim(',');

            //Populate filter drop downs.
            ViewData["location_id"] = Location.FetchSelectListActive(location_id);
            ViewData["contact_id"] = WBEADMS.Models.User.FetchSelectListActive(contact_id);
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(sample_type_id);
            ViewData["is_active"] = WBEADMS.ControllersCommon.GetYesNoSelectList(is_active);
            ViewData["interval_id"] = Interval.FetchSelectList(interval_id);

            //filter
            if (location_id.HasValue)
            {
                whereClause += " AND Schedules.location_id = " + location_id.Value;
            }

            if (contact_id.HasValue)
            {
                whereClause += " AND Schedules.contact_id = " + contact_id.Value;
            }

            if (sample_type_id.HasValue)
            {
                whereClause += " AND Schedules.sample_type_id = " + sample_type_id.Value;
            }

            if (interval_id.HasValue)
            {
                whereClause += " AND Schedules.interval_id = " + interval_id.Value;
            }

            if (!is_active.IsBlank())
            {
                if (is_active.ToLower().Trim() == "yes")
                {
                    whereClause += " AND Schedules.is_active = 1";
                }
                else if (is_active.ToLower().Trim() == "no")
                {
                    whereClause += " AND Schedules.is_active = 0";
                }
            }

            //paginate
            var urlParameters = new { action = "Index", controller = "Schedule", location_id = location_id, contact_id = contact_id, sample_type_id = sample_type_id, interval_id = interval_id, is_active = is_active, sort = sort };
            Paginator paginator = this.AddDefaultPaginator<Schedule>(urlParameters, page, new { where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Schedule> schedules = BaseModel.FetchPage<Schedule>(paginator, new { join = joinClause, where = whereClause, order = orderByClause });

            // add notice if there is no records
            if (schedules.Count == 0)
            {
                this.AddViewNotice("No Schedules were found.");
            }

            return View(schedules);
        }

        public ActionResult Details(string id)
        {
            Schedule schedule = Schedule.Load(id);
            if (schedule != null)
            {
                ViewData["parent_id"] = schedule.id;
                ViewData["parent_type"] = Note.ParentType.Schedule;

                return View(schedule);
            }
            else
            {
                this.AddTempNotice("Unable to find Schedule with ID of [" + id + "]");
                return RedirectToAction("Index");
            }
        }

        public ActionResult Create()
        {
            SetViewDataForm();
            return View(new Schedule());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            var schedule = new Schedule();
            schedule.created_by = this.GetUser().user_id;
            schedule.date_created = DateTime.Now.ToISODate();
            schedule.is_active = "1";
            try
            {
                UpdateModel(schedule);
                schedule.Save();
                this.AddTempNotice("Successfully created schedule: " + schedule.name);
                return RedirectToRoute(new { action = "Details", id = schedule.id });
            }
            catch (ModelException e)
            {
                SetViewDataForm(schedule);
                this.PopulateViewWithErrorMessages(e);
                return View(schedule);
            }
        }

        public ActionResult Edit(string id)
        {
            var schedule = Schedule.Load(id);
            if (schedule == null)
            {
                this.AddTempNotice("Unable to find Schedule with ID of [" + id + "]");
                return RedirectToAction("Index");
            }

            SetViewDataForm(schedule);
            return View(schedule);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var schedule = Schedule.Load(id);
            if (schedule == null)
            {
                this.AddTempNotice("Unable to find Schedule with ID of [" + id + "]");
                return RedirectToAction("Index");
            }

            schedule.modified_by = this.GetUser().user_id;
            schedule.date_modified = DateTime.Now.ToString();

            try
            {
                UpdateModel(schedule);
                schedule.Save();
                this.AddTempNotice("Successfully updated schedule: " + schedule.name);
                return RedirectToRoute(new { action = "Details", id = schedule.id });
            }
            catch (ModelException e)
            {
                SetViewDataForm(schedule);
                this.PopulateViewWithErrorMessages(e);
                return View(schedule);
            }
        }

        private void SetViewDataForm() { SetViewDataForm(new Schedule()); }

        private void SetViewDataForm(Schedule s)
        {
            ViewData["location_id"] = Location.FetchSelectListActive(s.location_id);

            // Name space must be included with User as to not conflict with the ASP User class.
            // TODO:Update to user first and last name....
            ViewData["contact_id"] = WBEADMS.Models.User.FetchSelectListActive(s.contact_id);
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(s.sample_type_id);
            ViewData["interval_id"] = Interval.FetchSelectList(s.interval_id);
        }
    }
}