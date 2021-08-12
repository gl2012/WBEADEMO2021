
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        public ActionResult Index(int? page, int? page_size, string is_active, string sort)
        {
            string whereClause = "1 = 1";
            string orderByClause = "";

            //Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                string[] sortOrder = sort.Split('_');
                foreach (string column in sortOrder)
                {
                    switch (column)
                    {
                        case "name":
                            orderByClause += "Locations.name,";
                            break;
                        case "desc-name":
                            orderByClause += "Locations.name DESC,";
                            break;
                        case "full-name":
                            orderByClause += "Locations.full_name,";
                            break;
                        case "desc-full-name":
                            orderByClause += "Locations.full_name DESC,";
                            break;
                        case "latitude":
                            orderByClause += "Locations.latitude,";
                            break;
                        case "desc-latitude":
                            orderByClause += "Locations.latitude DESC,";
                            break;
                        case "longitude":
                            orderByClause += "Locations.longitude,";
                            break;
                        case "desc-location":
                            orderByClause += "Locations.longitude DESC,";
                            break;
                        case "active":
                            orderByClause += "Locations.active,";
                            break;
                        case "desc-active":
                            orderByClause += "Locations.active DESC,";
                            break;
                    }
                }
            }

            orderByClause = orderByClause.Trim(',');

            //Populate filter drop downs.
            ViewData["is_active"] = WBEADMS.ControllersCommon.GetYesNoSelectList(is_active);

            if (!is_active.IsBlank())
            {
                if (is_active.ToLower().Trim() == "yes")
                {
                    whereClause += " AND Locations.active = 1";
                }
                else if (is_active.ToLower().Trim() == "no")
                {
                    whereClause += " AND Locations.active = 0";
                }
            }

            var urlParameters = new { action = "Index", controller = "Location", is_active = is_active, sort = sort };
            Paginator paginator = this.AddDefaultPaginator<Location>(urlParameters, page, new { where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Location> locationList = BaseModel.FetchPage<Location>(paginator, new { where = whereClause, order = orderByClause });

            // add notice if there is no records
            if (locationList.Count == 0)
            {
                this.AddViewNotice("No locations were found.");
            }

            return View(locationList);
        }

        public ActionResult Details(string id)
        {
            Location location = this.GetRequestedModel<Location>(id);
            if (location == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["parent_id"] = location.id;
            ViewData["parent_type"] = Note.ParentType.Location;

            return View(location);
        }

        public ActionResult Create()
        {
            return View(new Location());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            Location newLocation = new Location();

            try
            {
                UpdateModel(newLocation);

                newLocation.created_by = ((WBEADMS.Models.User)Session["user"]).user_id;
                newLocation.date_created = DateTime.Now.ToString();
                newLocation.modified_by = newLocation.created_by;
                newLocation.date_modified = newLocation.date_created;

                newLocation.Save();

                return RedirectToAction("Details", new { id = newLocation.id });
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(new ModelException(new ValidationError("unknown", e.Message)));
            }

            return View(newLocation);
        }

        public ActionResult Edit(string id)
        {
            Location location = this.GetRequestedModel<Location>(id);
            if (location == null)
            {
                return RedirectToAction("Index");
            }

            // display edit field.
            return View(location);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            Location location = this.GetRequestedModel<Location>(id);
            if (location == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                UpdateModel(location, collection.ToValueProvider());
                if (location.IsModified())
                {
                    location.modified_by = this.GetUser().user_id;
                    location.date_modified = DateTime.Now.ToString();
                    location.Save();
                }

                return RedirectToAction("Details", new { id = location.id });
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(new ModelException(new ValidationError("unknown", e.Message)));
            }

            return View(location);
        }
    }
}