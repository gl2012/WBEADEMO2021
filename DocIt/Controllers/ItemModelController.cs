
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class ItemModelController : Controller
    {
        const string ITEM_NAME = "model";

        // GET: /ItemModel/
        public ActionResult Index(int? page, int? page_size)
        {
            Paginator paginator = this.AddDefaultPaginator<ItemModel>(page);
            this.SetPageSize(page_size, paginator);
            List<ItemModel> itemModels = BaseModel.FetchPage<ItemModel>(paginator, ItemModel._table_name, ItemModel._id_field, null /*clauses*/);

            // add notice if there is no records
            if (itemModels.Count == 0)
            {
                this.AddViewNotice("No Item Models were found.");
            }

            return View(itemModels);
        }

        // GET: /ItemModel/Details/5
        public ActionResult Details(string id)
        {
            var item = ItemModel.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The " + ITEM_NAME + " " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            SetViewDataForm(item);
            return View(item);
        }

        // GET: /ItemModel/Create
        public ActionResult Create()
        {
            var item = new ItemModel();
            SetViewDataForm(item);
            return View(item);
        }

        // POST: /ItemModel/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            var item = new ItemModel();
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
                SetViewDataForm(item);
                return View(item);
            }
        }

        // GET: /ItemModel/Edit/5
        public ActionResult Edit(string id)
        {
            var item = ItemModel.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The " + ITEM_NAME + " " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            SetViewDataForm(item);
            return View(item);
        }

        // POST: /ItemModel/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var item = ItemModel.Load(id);
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
                SetViewDataForm(item);
                return View(item);
            }
        }

        private void SetViewDataForm(ItemModel itemmodel)
        {
            ViewData["make_id"] = ItemMake.FetchSelectList(itemmodel.make_id);
        }
    }
}