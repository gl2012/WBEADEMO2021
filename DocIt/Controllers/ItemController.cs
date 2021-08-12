
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WBEADMS.Models;
using WBEADMS.Views.ItemHelpers;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {

        public ActionResult Index(int? page, int? page_size, int? model_id, int? location_id, int? parameter_id, string is_integrated, string sort)
        {
            string orderByClause = String.Empty;
            string whereClause = "1 = 1";
            string joinClause = "";

            // Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                if (sort.Contains("location"))
                {
                    joinClause += "LEFT OUTER JOIN Locations ON Items.location_id = Locations.location_id ";
                }

                string[] sortOrder = sort.Split('_');
                foreach (string column in sortOrder)
                {
                    switch (column)
                    {
                        case "serial-number":
                            orderByClause += "Items.serial_number,";
                            break;
                        case "desc-serial-number":
                            orderByClause += "Items.serial_number DESC,";
                            break;
                        case "name":
                            orderByClause += "Items.name,";
                            break;
                        case "desc-name":
                            orderByClause += "Items.name DESC,";
                            break;
                        case "range":
                            orderByClause += "Items.range,";
                            break;
                        case "desc-range":
                            orderByClause += "Items.range DESC,";
                            break;
                        case "is-integrated":
                            orderByClause += "Items.is_integrated,";
                            break;
                        case "desc-is-integrated":
                            orderByClause += "Items.is_integrated DESC,";
                            break;
                        case "location":
                            orderByClause += "Locations.name,";
                            break;
                        case "desc-location":
                            orderByClause += "Locations.name DESC,";
                            break;
                    }
                }
            }

            orderByClause = orderByClause.Trim(',');

            // Populate filter drop downs.
            ViewData["model_id"] = ItemModel.FetchSelectList(model_id);
            ViewData["location_id"] = Location.FetchSelectListActive(location_id);
            ViewData["parameter_id"] = Parameter.FetchSelectListActive(parameter_id);
            Dictionary<string, string> samplerType = new Dictionary<string, string>();
            samplerType.Add("integrated", "Integrated");
            samplerType.Add("continuous", "Continuous");

            ViewData["is_integrated"] = new SelectList(samplerType, "key", "value", is_integrated);

            // filter
            if (model_id.HasValue)
            {
                whereClause += " AND Items.model_id = " + model_id.Value;
            }

            if (location_id.HasValue)
            {
                whereClause += " AND Items.location_id = " + location_id.Value;
            }

            if (parameter_id.HasValue)
            {
                joinClause += "INNER JOIN Items_Parameters ON Items.item_id = Items_Parameters.item_id";
                whereClause += " AND Items_Parameters.parameter_id = " + parameter_id.Value;
            }

            if (!is_integrated.IsBlank())
            {
                if (is_integrated.ToLower().Trim() == "integrated")
                {
                    whereClause += " AND Items.is_integrated = 1";
                }
                else if (is_integrated.ToLower().Trim() == "continuous")
                {
                    whereClause += " AND Items.is_integrated = 0";
                }
            }

            // Paginate
            var urlParameters = new { action = "Index", controller = "Item", sort = sort, model_id = model_id, location_id = location_id, parameter_id = parameter_id, is_integrated = is_integrated };
            Paginator paginator = this.AddDefaultPaginator<Item>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Item> items = BaseModel.FetchPage<Item>(paginator, new { join = joinClause, where = whereClause, order = orderByClause });

            // add notice if there is no records
            if (items.Count == 0)
            {
                this.AddViewNotice("No items were found.");
            }

            return View(items);
        }

        public ActionResult Details(string id)
        {
            var item = Item.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The item " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            ViewData["parent_id"] = item.id;
            ViewData["parent_type"] = Note.ParentType.Item;

            return View(item);
        }

        public ActionResult Create()
        {
            var item = new Item();
            item.is_integrated = "0"; // default to continuous sampler
            SetViewDataForm(item);
            return View(item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            var item = new Item();
            try
            {
                UpdateModel(item);
                item.parameter_list = collection["parameter_list"];
                item.date_created = DateTime.Now.ToISODateTime();
                item.Save();
                this.AddTempNotice("Successfully created item: " + (item.name.IsBlank() ? item.serial_number : item.name));
                return RedirectToRoute(new { action = "Details", id = item.id });
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                SetViewDataForm(item);
                return View(item);
            }
        }

        public ActionResult Edit(string id)
        {
            var item = Item.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The item " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            SetViewDataForm(item);
            return View(item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var item = Item.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The item " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            try
            {
                UpdateModel(item);
                item.parameter_list = collection["parameter_list"]; // TODO: figure out how to make UpdateModel set this param; it seems to be ignoring checkboxes without hidden, or it will only take the value of the first checkbox
                item.Save();
                this.AddTempNotice("Successfully updated item: " + (item.name.IsBlank() ? item.serial_number : item.name));
                return RedirectToRoute(new { action = "Details", id = item.id });
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                SetViewDataForm(item);
                return View(item);
            }
        }

        public ActionResult Relocate(string location_id, string item_id)
        {
            Location location = new Location();
            if (!item_id.IsBlank())
            {
                location = Item.Load(item_id).Location;
            }
            else if (!location_id.IsBlank())
            {
                location = Location.Load(location_id);
            }

            ViewData["date_moved"] = DateTime.Today;

            ViewData["left_location"] = Location.FetchSelectListActive(location.location_id);
            ViewData["left_items"] = location.Items;

            ViewData["right_location"] = Location.FetchSelectListActive(); // NOTE: Unassigned must be not active.
            ViewData["right_items"] = new Item[] { };
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Relocate(FormCollection collection)
        {
            // get items
            string[] leftIds = collection["left_ids"].Trim().Split(' ');
            string[] rightIds = collection["right_ids"].Trim().Split(' ');

            // get date moved
            DateTime date_moved;
            if (!DateTime.TryParse(collection["date_moved"], out date_moved))
            {
                this.AddTempNotice("Invalid date specified on Date Moved");
                return RedirectToAction("Index");
            }

            // get left location
            Location leftLocation = Location.Load(collection["left_location"]);
            if (leftLocation.name == "Unassigned")
            {
                this.AddTempNotice("You must dispose of items on the right.");
                return RedirectToAction("Index");
            }

            // get right location (or Unassigned)
            Location rightLocation;
            if (!collection["right_location"].IsBlank())
            {
                rightLocation = Location.Load(collection["right_location"]);
            }
            else
            {
                var unassigned = Location.LoadByName("Unassigned");  // TODO: maybe make a FetchFirstByName method to just get the first one?
                if (unassigned != null)
                {
                    rightLocation = unassigned;
                }
                else
                {
                    this.AddTempNotice("Unable to find Unassigned location to dispose of items.  Create a location with name Unassigned.");
                    return RedirectToAction("Index", "Location");
                }
            }

            // move items
            var movedItems = MoveItems(leftLocation, rightLocation, leftIds, rightIds, date_moved);

            // build message
            string message;
            if (rightLocation.name == "Unassigned")
            {
                message = "The following items were disposed:";
            }
            else
            {
                message = "The following items were moved:";
            }

            message += "<ul>";
            foreach (var item_location in movedItems)
            {
                message += "<li>" + item_location.Key + " moved to " + item_location.Value + "</li>\n";
            }

            message += "</ul>";

            this.AddTempNotice(message);
            return RedirectToAction("Index");
        }

        // returns JSON list of item display names i.e. [ { id : xxx, name : xxx }, { id : yyy, name : yyy }, ... ]
        public ActionResult GetItems(string location_id)
        {
            if (location_id.IsBlank()) { return Json(new List<object>(), JsonRequestBehavior.AllowGet); }

            var location = Location.Load(location_id);
            var list = new List<object>();
            foreach (var item in location.Items)
            {
                list.Add(new { id = item.id, name = item.DisplayName, listname = item.ItemListedName(), date = item.DateInstalled });
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // returns JSON list of dateInstalled & dateRemoved i.e. [ { location : xxx, install_date : xxx, remove_date : xxx }, { location : yyy, install_date : yyy, remove_date : yyy }, ... ]
        public ActionResult GetItemHistory(string item_id)
        {
            var hists = WBEADMS.Models.ItemHistory.GetItemHistorys(item_id);
            var list = new List<object>();
            foreach (var hist in hists)
            {
                list.Add(new { location = hist.Location.name, install_date = hist.DateInstalled, remove_date = hist.DateRemoved });
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemsHistory(int? page, int? page_size, int? location_id, string start_install_date, string end_install_date, string sort)
        {
            DateTime startDate, endDate;
            List<ItemHistory> historyList;

            string sql = @"SELECT item_history_id 
                            FROM ItemHistorys 
                            INNER JOIN Locations ON ItemHistorys.location_id = Locations.location_id
                            INNER JOIN Items ON ItemHistorys.item_id = Items.item_id
                            WHERE 1 = 1";

            using (SqlCommand command = new SqlCommand(sql))
            {
                if (location_id.HasValue)
                {
                    command.CommandText += " AND ItemHistorys.location_id = @location_id";
                    command.Parameters.AddWithValue("location_id", location_id);
                }

                if (DateTime.TryParse(start_install_date, out startDate))
                {
                    command.CommandText += " AND ItemHistorys.date_installed >= @start_install_date";
                    command.Parameters.AddWithValue("start_install_date", startDate);
                }
                else if (!start_install_date.IsBlank())
                {
                    this.AddViewNotice("Install Date From [" + start_install_date + "] is not a valid date.<br/>");
                }

                if (DateTime.TryParse(end_install_date, out endDate))
                {
                    command.CommandText += " AND ItemHistorys.date_installed <= @end_install_date";
                    command.Parameters.AddWithValue("end_install_date", endDate);
                }
                else if (!end_install_date.IsBlank())
                {
                    this.AddViewNotice("Install Date To [" + end_install_date + "] is not a valid date.<br/>");
                }

                // default sorting.
                string orderByClause = "";
                if (!sort.IsBlank())
                {
                    ViewData["sort"] = sort;
                    sort = sort.ToLower();

                    string[] sortOrder = sort.Split('_');
                    foreach (string column in sortOrder)
                    {
                        switch (column)
                        {
                            case "install-date":
                                orderByClause += "ItemHistorys.date_installed,";
                                break;
                            case "desc-install-date":
                                orderByClause += "ItemHistorys.date_installed DESC,";
                                break;
                            case "removal-date":
                                orderByClause += "ItemHistorys.date_removed,";
                                break;
                            case "desc-removal-date":
                                orderByClause += "ItemHistorys.date_removed DESC,";
                                break;
                            case "location":
                                orderByClause += "Locations.name,";
                                break;
                            case "desc-location":
                                orderByClause += "Locations.name DESC,";
                                break;
                            case "item-name":
                                orderByClause += "Items.name,";
                                break;
                            case "desc-item-name":
                                orderByClause += "Items.name DESC,";
                                break;
                        }
                    }
                }

                if (orderByClause.IsBlank())
                {
                    command.CommandText += " ORDER BY ItemHistorys.item_id, ItemHistorys.date_installed";
                }
                else
                {
                    command.CommandText += " ORDER BY " + orderByClause.Trim(',');
                }

                // Populate filter drop downs.
                ViewData["location_id"] = Location.FetchSelectList(location_id);

                var urlParameters = new { action = "ItemsHistory", controller = "Item", location_id = location_id, start_install_date = start_install_date, end_install_date = end_install_date, sort = sort };
                Paginator paginator = this.AddDefaultPaginator<ItemHistory>(urlParameters, page, command);
                this.SetPageSize(page_size, paginator);
                historyList = BaseModel.FetchPage<ItemHistory>(paginator, command);
            }

            return View(historyList);
        }

        private void SetViewDataForm(Item item)
        {
            ViewData["sample_type_id"] = SampleType.FetchSelectListName(item.sample_type_id);
            ViewData["model_id"] = ItemModel.FetchSelectList(item.model_id);

            List<string> parameterIds = new List<string>();
            foreach (var parameter in item.parameters)
            {
                parameterIds.Add(parameter.id);
            }

            ViewData["parameter_ids"] = parameterIds;
        }

        private Dictionary<string, string> MoveItems(Location leftLocation, Location rightLocation, string[] leftItemIds, string[] rightItemIds, DateTime dateMoved)
        {
            var itemsToLocations = new Dictionary<Item, Location>();
            var itemsToLocationsNames = new Dictionary<string, string>();

            var rightIds = new List<string>(rightItemIds);
            var leftIds = new List<string>(leftItemIds);

            // get list of items moved from left to right
            if (rightIds.Count > 0)
            {
                foreach (var item in leftLocation.Items)
                { // TODO: this is inefficient, fetching all Items just to use the ids and names? Maybe make an method that returns a Dictionary<ItemId, ItemName>
                    if (rightIds.Contains(item.id))
                    {
                        itemsToLocations.Add(item, rightLocation);
                    }
                }
            }

            // get list of items moved from right to left
            if (leftIds.Count > 0 && rightLocation.name != "Unassigned")
            {
                foreach (var item in rightLocation.Items)
                {
                    if (leftIds.Contains(item.id))
                    {
                        itemsToLocations.Add(item, leftLocation);
                    }
                }
            }

            // move items
            foreach (var item_location in itemsToLocations)
            {
                var item = item_location.Key;
                var location = item_location.Value;

                item.Relocate(location, dateMoved);
                itemsToLocationsNames.Add((item.name.IsBlank() ? item.serial_number : item.name), location.name);
            }

            return itemsToLocationsNames;
        }
    }
}