using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS;
using WBEADMS.Models;

namespace DocIt.Controllers
{
    public class AdvancedNoteSearchController : Controller
    {
        // GET: AdvancedNoteSearch
        public ActionResult Index1(int? note_setting_id)
        {

            ViewData["note_setting_id"] = WBEADMS.Models.NoteSetting.FetchSettingSelectList(note_setting_id, this.GetUser().user_id);

            ViewData["setting_description"] = "";


            return View();

        }




        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index1(int? page, int? page_size, int? note_setting_id, string setting_description, string sort, string date_from, string date_to)
        {
            string whereClause = "(is_deleted=0";
            string whereClause1 = "";
            string whereClause2 = "";
            string whereClause3 = "";
            string orderByClause = "";
            string joinClause = "";
            ViewData["note_setting_id"] = WBEADMS.Models.NoteSetting.FetchSettingSelectList(note_setting_id, this.GetUser().user_id);
            ViewData["setting_description"] = "";


            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", setting_description);

            if (note_setting_id.HasValue)
            {
                NoteSetting selectedsetting = NoteSetting.Load(note_setting_id.ToString());
                ViewData["setting_description"] = selectedsetting.setting_description;
                if (!date_from.IsBlank())
                {
                    date_from = date_from.Replace("'", "");
                    whereClause += " AND date_occurred >= '" + date_from + "'";
                }

                if (!date_to.IsBlank())
                {
                    date_to = date_to.Replace("'", "");
                    whereClause += " AND date_occurred <= '" + date_to + " 23:59'";
                }
                if (!string.IsNullOrEmpty(sort))
                {
                    ViewData["sort"] = sort;
                    sort = sort.ToLower();



                    string[] sortOrder = sort.Split('_');
                    foreach (string column in sortOrder)
                    {
                        switch (column)
                        {
                            case "location":
                                orderByClause += "Locations.name,";
                                break;
                            case "desc-location":
                                orderByClause += "Locations.name DESC,";
                                break;
                            case "date-created":
                                orderByClause += "DATEPART(YEAR, date_created), DATEPART(DAYOFYEAR, date_created),";
                                break;
                            case "desc-date-created":
                                orderByClause += "DATEPART(YEAR, date_created) DESC, DATEPART(DAYOFYEAR, date_created) DESC,";
                                break;
                            case "date-occurred":
                                orderByClause += "DATEPART(YEAR, date_occurred), DATEPART(DAYOFYEAR, date_occurred),";
                                break;
                            case "desc-date-occurred":
                                orderByClause += "DATEPART(YEAR, date_occurred) DESC, DATEPART(DAYOFYEAR, date_occurred) DESC,";
                                break;
                            case "author":
                                orderByClause += "Users.last_name, Users.first_name,";
                                break;
                            case "desc-author":
                                orderByClause += "Users.last_name DESC, Users.first_name,";
                                break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(orderByClause))
                {
                    orderByClause = "advancednotesearch_views.date_created DESC";
                }
                else
                {
                    orderByClause = orderByClause.Trim(',');
                }


                NoteSetting selectedSetting = NoteSetting.Load(note_setting_id.ToString());
                string strSampleId = "";
                if (selectedSetting.FetchNoteSettingSampleTypeIdlist().Count > 0)
                {
                    foreach (var i in selectedSetting.FetchNoteSettingSampleTypeIdlist())
                    {
                        strSampleId = strSampleId + "," + i.ToString();

                    }
                }

                if (strSampleId.StartsWith(","))
                    strSampleId = strSampleId.Substring(1);


                string strParameterId = "";
                if (selectedSetting.FetchNoteSettingParameterIdlist().Count > 0)
                {
                    foreach (var i in selectedSetting.FetchNoteSettingParameterIdlist())
                    {
                        strParameterId = strParameterId + "," + i.ToString();
                    }

                }
                if (strParameterId.StartsWith(","))
                    strParameterId = strParameterId.Substring(1);


                string strUserId = "";
                if (selectedSetting.FetchNoteSettingUserIdlist().Count > 0)
                {
                    foreach (var i in selectedSetting.FetchNoteSettingUserIdlist())
                    {
                        strUserId = strUserId + "," + i.ToString();
                    }

                }
                if (strUserId.StartsWith(","))
                    strUserId = strUserId.Substring(1);


                if (strParameterId != "")
                {
                    if (strUserId != "")
                        whereClause1 = whereClause + " and sample_type_id is null and  created_by in (" + strUserId + ") and advancednotesearch_view_id in (select note_id from notes_parameters  where  parameter_id in (" + strParameterId + ")))";
                    else
                        whereClause1 = whereClause + " and sample_type_id is null and advancednotesearch_view_id  in (select note_id from notes_parameters where  parameter_id in (" + strParameterId + ") ))";
                }

                if (strSampleId != "")
                {
                    if (strUserId != "")
                        whereClause2 = whereClause + " and sample_type_id is not null and  created_by in (" + strUserId + ")  and sample_type_id in (" + strSampleId + "))";
                    else
                        whereClause2 = whereClause + " and sample_type_id  is not null  and sample_type_id in (" + strSampleId + "))";
                }

                if (strParameterId == "" && strSampleId == "")
                {
                    if (strUserId != "")
                    {
                        whereClause3 = whereClause + " and created_by in (" + strUserId + "))";

                        whereClause = whereClause3;
                    }
                    else
                        whereClause = "1<1";
                }

                if (!page_size.HasValue)
                {
                    page_size = 40;
                }
                if (strSampleId == "" && strParameterId != "")
                    whereClause = whereClause1;
                if (strSampleId != "" && strParameterId == "")
                    whereClause = whereClause2;
                if (strSampleId != "" && strParameterId != "")
                    whereClause = whereClause1 + " or " + whereClause2;


                var urlParameters = new { action = "Index1", controller = "AdvancedNoteSearch", note_setting_id = note_setting_id, sort = sort, date_from, date_to };
                Paginator paginator = this.AddDefaultPaginator<AdvancedNoteSearch_View>(urlParameters, page, new { join = joinClause, where = whereClause });
                this.SetPageSize(page_size, paginator);
                List<AdvancedNoteSearch_View> notes = BaseModel.FetchPage<AdvancedNoteSearch_View>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

                // add notice if there is no records
                if (notes.Count == 0)
                {
                    this.AddViewNotice("No Notes were found.");
                }

                return View(notes);
            }
            else

                return View();


        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDescription(string stateId)
        {
            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", stateId);

            //  if (!string.IsNullOrEmpty(stateId))
            // {
            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", NoteSetting.FetchDescriptionSelectList(stateId));

            return Json(NoteSetting.FetchDescriptionSelectList(stateId), JsonRequestBehavior.AllowGet);

            // }
        }
    }
}