
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class ShippingCompanyController : Controller
    {
        public ActionResult Index(int? page, int? page_size, string active, string sort)
        {
            List<ShippingCompany> shippingCompanies;

            string sql = @"SELECT * 
                            FROM ShippingCompanys
                            WHERE 1 = 1";

            using (SqlCommand command = new SqlCommand(sql))
            {
                if (!active.IsBlank())
                {
                    if (active.ToLower().Trim() == "yes")
                    {
                        command.CommandText += " AND ShippingCompanys.active = @active";
                        command.Parameters.AddWithValue("active", 1);
                    }
                    else if (active.ToLower().Trim() == "no")
                    {
                        command.CommandText += " AND ShippingCompanys.active = @active";
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
                                orderByClause += "ShippingCompanys.name,";
                                break;
                            case "desc-name":
                                orderByClause += "ShippingCompanys.name DESC,";
                                break;
                            case "active":
                                orderByClause += "ShippingCompanys.active,";
                                break;
                            case "desc-active":
                                orderByClause += "ShippingCompanys.active DESC,";
                                break;
                        }
                    }
                }

                if (!orderByClause.IsBlank())
                {
                    command.CommandText += " ORDER BY " + orderByClause.Trim(',');
                }

                var urlParameters = new { action = "Index", controller = "ShippingCompany", active = active, sort = sort };
                Paginator paginator = this.AddDefaultPaginator<ShippingCompany>(urlParameters, page, command);
                this.SetPageSize(page_size, paginator);
                shippingCompanies = BaseModel.FetchPage<ShippingCompany>(paginator, command);
            }

            //Populate filter drop downs.
            ViewData["active"] = WBEADMS.ControllersCommon.GetYesNoSelectList(active);

            return View(shippingCompanies);
        }

        public ActionResult Create()
        {
            ShippingCompany newShippingCompany = new ShippingCompany();
            newShippingCompany.active = "True"; //set the default form to active = true.
            return View(newShippingCompany);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            ShippingCompany newShippingCompany = new ShippingCompany();

            try
            {
                UpdateModel(newShippingCompany, collection.ToValueProvider());
                newShippingCompany.Save();
                this.AddTempNotice("Successfully created shipping company " + newShippingCompany.name);
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

            return View(newShippingCompany);
        }

        public ActionResult Edit(string id)
        {
            if (id.IsBlank())
            {
                this.AddTempNotice("You must specify the shipping company to be edited via ID.");
                return RedirectToAction("Index");
            }

            ShippingCompany editShippingCompany = ShippingCompany.Load(id);

            if (editShippingCompany == null)
            {
                this.AddTempNotice("Cannot find target shipping company (" + id + ").");
                return RedirectToAction("Index");
            }

            return View(editShippingCompany);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            ShippingCompany editShippingCompany = ShippingCompany.Load(id);

            try
            {
                if (editShippingCompany != null)
                {
                    UpdateModel(editShippingCompany, collection.ToValueProvider());
                    editShippingCompany.Save();
                    this.AddTempNotice("Successfully modified shipping company " + editShippingCompany.name);
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

            return View(editShippingCompany);
        }
    }
}