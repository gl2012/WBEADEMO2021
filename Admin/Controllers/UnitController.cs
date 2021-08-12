
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class UnitController : Controller
    {
        public ActionResult Index(int? page, int? page_size, string type, string sort)
        {
            List<Unit> types;

            string sql = @"SELECT * 
                            FROM Units
                            WHERE 1 = 1";

            using (SqlCommand command = new SqlCommand(sql))
            {
                if (!type.IsBlank())
                {
                    command.CommandText += " AND Units.type = @type";
                    command.Parameters.AddWithValue("type", type);
                }

                //default sorting.
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
                            case "name":
                                orderByClause += "Units.name,";
                                break;
                            case "desc-name":
                                orderByClause += "Units.name DESC,";
                                break;
                            case "type":
                                orderByClause += "Units.type,";
                                break;
                            case "desc-type":
                                orderByClause += "Units.type DESC,";
                                break;
                        }
                    }
                }

                if (!orderByClause.IsBlank())
                {
                    command.CommandText += " ORDER BY " + orderByClause.Trim(',');
                }

                var urlParameters = new { action = "Index", controller = "Units", type = type, sort = sort };
                Paginator paginator = this.AddDefaultPaginator<Unit>(urlParameters, page, command);
                this.SetPageSize(page_size, paginator);
                types = BaseModel.FetchPage<Unit>(paginator, command);
            }

            //Populate filter drop downs.
            ViewData["type"] = Unit.FetchTypeSelectList(type);

            return View(types);
        }

        public ActionResult Create()
        {
            //Populate filter drop downs.
            ViewData["type"] = Unit.FetchTypeSelectList();
            return View(new Unit());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            Unit newUnit = new Unit();

            try
            {
                UpdateModel(newUnit, collection.ToValueProvider());
                newUnit.Save();
                this.AddTempNotice("Successfully created Unit " + newUnit.name);
                return RedirectToAction("Index");
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(e);
            }

            //Populate filter drop downs.
            ViewData["type"] = Unit.FetchTypeSelectList(newUnit.type);

            return View(newUnit);
        }

        public ActionResult Edit(string id)
        {
            if (id.IsBlank())
            {
                this.AddTempNotice("You must specify the Unit to be edited.");
                return RedirectToAction("Index");
            }

            Unit editUnit = Unit.Load(id);

            if (editUnit == null)
            {
                this.AddTempNotice("Cannot find target Unit (" + id + ").");
                return RedirectToAction("Index");
            }

            //Populate filter drop downs.
            ViewData["type"] = Unit.FetchTypeSelectList(editUnit.type);

            return View(editUnit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            Unit editUnit = Unit.Load(id);

            try
            {
                if (editUnit != null)
                {
                    UpdateModel(editUnit, collection.ToValueProvider());
                    editUnit.Save();
                    this.AddTempNotice("Successfully modified Unit " + editUnit.name);
                    return RedirectToAction("Index");
                }
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                this.PopulateViewWithErrorMessages(e);
            }

            return View(editUnit);
        }
    }
}