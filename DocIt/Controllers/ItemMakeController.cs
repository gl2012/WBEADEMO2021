
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class ItemMakeController : Controller
    {
        const string ITEM_NAME = "make";

        // GET: /ItemMake/
        public ActionResult Index(int? page, int? page_size)
        {
            Paginator paginator = this.AddDefaultPaginator<ItemMake>(page);
            this.SetPageSize(page_size, paginator);
            List<ItemMake> itemMakes = BaseModel.FetchPage<ItemMake>(paginator, ItemMake._table_name, ItemMake._id_field, null /*clauses*/);

            // add notice if there is no records
            if (itemMakes.Count == 0)
            {
                this.AddViewNotice("No Item Makes were found.");
            }

            return View(itemMakes);
        }

        // GET: /ItemMake/Details/5
        public ActionResult Details(string id)
        {
            var item = ItemMake.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The " + ITEM_NAME + " " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: /ItemMake/Create
        public ActionResult Create()
        {
            var item = new ItemMake();
            return View(item);
        }

        // POST: /ItemMake/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            var item = new ItemMake();
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

        // GET: /ItemMake/Edit/5
        public ActionResult Edit(string id)
        {
            var item = ItemMake.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The " + ITEM_NAME + " " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // POST: /ItemMake/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var item = ItemMake.Load(id);
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