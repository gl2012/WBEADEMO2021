
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class SlopeOffsetController : Controller
    {
        public ActionResult Index(int? page, int? page_size, string station_id, string analyzer, string sort)
        {
            string orderByClause = String.Empty;
            string whereClause = "1 = 1";
            string joinClause = "";

            // Sort
            if (!string.IsNullOrEmpty(sort))
            {
                ViewData["sort"] = sort;
                sort = sort.ToLower();

                string[] sortOrder = sort.Split('_');
                foreach (string column in sortOrder)
                {
                    switch (column)
                    {
                        case "date-created":
                            orderByClause += "date_created,";
                            break;
                        case "desc-date-created":
                            orderByClause += "date_created DESC,";
                            break;
                        case "date-active":
                            orderByClause += "date_active,";
                            break;
                        case "desc-date-active":
                            orderByClause += "date_active DESC,";
                            break;
                        case "station":
                            orderByClause += "station_id,";
                            break;
                        case "desc-station":
                            orderByClause += "station_id DESC,";
                            break;
                        case "analyzer":
                            orderByClause += "analyzer,";
                            break;
                        case "desc-analyzer":
                            orderByClause += "analyzer DESC,";
                            break;
                    }
                }
            }

            orderByClause = orderByClause.Trim(',');

            // Populate filter drop downs.
            ViewData["station_id"] = new SelectList(SlopeOffset.FetchStations());
            ////ViewData["analyzer"] = new SelectList(SlopeOffset.FetchAnalyzers());

            // filter
            if (!station_id.IsBlank())
            {
                station_id = station_id.Replace("'", "''");
                whereClause += " AND station_id = '" + station_id + "'";
            }

            if (!analyzer.IsBlank())
            {
                analyzer = analyzer.Replace("'", "''");
                whereClause += " AND analyzer = '" + analyzer + "'";
            }

            // Paginate
            var urlParameters = new { action = "Index", controller = "SlopeOffset", sort = sort, station_id = station_id, analyzer = analyzer };
            Paginator paginator = this.AddDefaultPaginator<SlopeOffset>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<SlopeOffset> items = BaseModel.FetchPage<SlopeOffset>(paginator, new { join = joinClause, where = whereClause, order = orderByClause });

            // add notice if there is no records
            if (items.Count == 0)
            {
                this.AddViewNotice("No slope/offsets were found.");
            }

            return View(items);
        }

        public ActionResult Details(string id)
        {
            var item = SlopeOffset.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The slope/offset " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        public ActionResult Create()
        {
            var item = new SlopeOffset();
            return View(item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            var item = new SlopeOffset();
            try
            {
                UpdateModel(item);
                item.date_created = DateTime.Now.ToString();
                item.created_by = this.GetUser().user_id;
                item.Save();
                this.AddTempNotice("Successfully created slope/offset for " + item.station_id + " " + item.analyzer);
                return RedirectToAction("Index");
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                return View(item);
            }
        }

        public ActionResult Edit(string id)
        {
            var item = SlopeOffset.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The slope/offset " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var item = SlopeOffset.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The item " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            try
            {
                UpdateModel(item);
                item.Save();

                this.AddTempNotice("Successfully updated slope/offset for " + item.station_id + " " + item.analyzer);
                return RedirectToAction("Index");
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                return View(item);
            }
        }
    }
}