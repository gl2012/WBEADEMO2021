
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class LabController : Controller
    {
        public ActionResult Index(int? page, int? page_size, string active, string sort)
        {
            List<Lab> labs;

            string sql = @"SELECT * 
                            FROM Labs
                            WHERE 1 = 1";

            using (SqlCommand command = new SqlCommand(sql))
            {
                if (!active.IsBlank())
                {
                    if (active.ToLower().Trim() == "yes")
                    {
                        command.CommandText += " AND Labs.active = @active";
                        command.Parameters.AddWithValue("active", 1);
                    }
                    else if (active.ToLower().Trim() == "no")
                    {
                        command.CommandText += " AND Labs.active = @active";
                        command.Parameters.AddWithValue("active", 0);
                    }
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
                                orderByClause += "Labs.name,";
                                break;
                            case "desc-name":
                                orderByClause += "Labs.name DESC,";
                                break;
                            case "active":
                                orderByClause += "Labs.active,";
                                break;
                            case "desc-active":
                                orderByClause += "Labs.active DESC,";
                                break;
                        }
                    }
                }

                if (!orderByClause.IsBlank())
                {
                    command.CommandText += " ORDER BY " + orderByClause.Trim(',');
                }

                var urlParameters = new { action = "Index", controller = "Lab", active = active, sort = sort };
                Paginator paginator = this.AddDefaultPaginator<Lab>(urlParameters, page, command);
                this.SetPageSize(page_size, paginator);
                labs = BaseModel.FetchPage<Lab>(paginator, command);
            }

            //Populate filter drop downs.
            ViewData["active"] = WBEADMS.ControllersCommon.GetYesNoSelectList(active);

            return View(labs);
        }

        public ActionResult Create()
        {
            Lab newLab = new Lab();
            newLab.active = "True"; //set the default form to active = true.
            return View(newLab);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            Lab newLab = new Lab();

            try
            {
                UpdateModel(newLab, collection.ToValueProvider());
                newLab.Save();
                this.AddTempNotice("Successfully created lab " + newLab.name);
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

            return View(newLab);
        }

        public ActionResult Edit(string id)
        {
            if (id.IsBlank())
            {
                this.AddTempNotice("You must specify the lab to be edited via ID.");
                return RedirectToAction("Index");
            }

            Lab editLab = Lab.Load(id);

            if (editLab == null)
            {
                this.AddTempNotice("Cannot find target lab (" + id + ").");
                return RedirectToAction("Index");
            }

            return View(editLab);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            Lab editLab = Lab.Load(id);

            try
            {
                if (editLab != null)
                {
                    UpdateModel(editLab, collection.ToValueProvider());
                    editLab.Save();
                    this.AddTempNotice("Successfully modified lab " + editLab.name);
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

            return View(editLab);
        }
    }
}