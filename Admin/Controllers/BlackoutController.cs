
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class BlackoutController : Controller
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
                        case "start-date":
                            orderByClause += "date_start,";
                            break;
                        case "desc-start-date":
                            orderByClause += "date_start DESC,";
                            break;
                        case "end-date":
                            orderByClause += "date_end,";
                            break;
                        case "desc-end-date":
                            orderByClause += "date_end DESC,";
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
            ViewData["station_id"] = new SelectList(Blackout.FetchStations());
            ////ViewData["analyzer"] = new SelectList(Blackout.FetchAnalyzers());

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
            var urlParameters = new { action = "Index", controller = "Blackout", sort = sort, station_id = station_id, analyzer = analyzer };
            Paginator paginator = this.AddDefaultPaginator<Blackout>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);
            List<Blackout> items = BaseModel.FetchPage<Blackout>(paginator, new { join = joinClause, where = whereClause, order = orderByClause });

            // add notice if there is no records
            if (items.Count == 0)
            {
                this.AddViewNotice("No blackouts were found.");
            }

            return View(items);
        }

        public ActionResult Details(string id)
        {
            var item = Blackout.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The blackout " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        public ActionResult Create()
        {
            var item = new Blackout();
            return View(item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            var item = new Blackout();
            try
            {
                UpdateModel(item);
                item.Save();
                this.AddTempNotice("Successfully created blackout for " + item.station_id + " " + item.analyzer);
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
            var item = Blackout.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The blackout " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var item = Blackout.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The item " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            try
            {
                UpdateModel(item);
                item.Save();

                this.AddTempNotice("Successfully updated blackout for " + item.station_id + " " + item.analyzer);
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