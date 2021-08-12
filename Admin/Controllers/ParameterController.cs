
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class ParameterController : Controller
    {
        const string ITEM_NAME = "parameter";

        // GET: /Parameter/
        public ActionResult Index(int? page, int? page_size)
        {
            string orderBy = "name ASC";

            Paginator paginator = this.AddDefaultPaginator<Parameter>(page, new { order = orderBy });

            this.SetPageSize(page_size, paginator);

            List<Parameter> parameters = BaseModel.FetchPage<Parameter>(paginator, new { order = orderBy });

            // add notice if there is no records
            if (parameters.Count == 0)
            {
                this.AddViewNotice("No Parameters were found.");
            }

            return View(parameters);
        }

        // GET: /Parameter/Details/5
        public ActionResult Details(string id)
        {
            var item = Parameter.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The " + ITEM_NAME + " " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: /Parameter/Create
        public ActionResult Create()
        {
            var item = new Parameter();
            return View(item);
        }

        // POST: /Parameter/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            var item = new Parameter();
            try
            {
                UpdateModel(item);
                item.Save();
                this.AddTempNotice("Successfully created " + ITEM_NAME + ": " + item.name);
                return RedirectToRoute(new { action = "Details", id = item.id });
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                return View(item);
            }
        }

        // GET: /Parameter/Edit/5
        public ActionResult Edit(string id)
        {
            var item = Parameter.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The " + ITEM_NAME + " " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // POST: /Parameter/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var item = Parameter.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The " + ITEM_NAME + " " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            try
            {
                UpdateModel(item);
                item.Save();
                this.AddTempNotice("Successfully updated " + ITEM_NAME + ": " + item.name);
                return RedirectToRoute(new { action = "Details", id = item.id });
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                return View(item);
            }
        }
    }
}