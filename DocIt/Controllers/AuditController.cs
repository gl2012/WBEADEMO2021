
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    [Authorize]
    public class AuditController : Controller
    {
        public ActionResult Log(int? page, int? page_size, string auditType, int? auditId, int? user_id, string field, string sort)
        {
            List<Audit> log;

            string sql = @"SELECT * 
                            FROM Audits
                            INNER JOIN Users ON Users.user_id = Audits.user_id
                            WHERE 1 = 1";
            using (SqlCommand command = new SqlCommand(sql))
            {
                if (!auditType.IsBlank())
                {
                    command.CommandText += " AND Audits.type = @type";
                    command.Parameters.AddWithValue("type", auditType);

                    if (auditId.HasValue)
                    {
                        command.CommandText += " AND Audits.id = @id";
                        command.Parameters.AddWithValue("id", auditId);
                    }
                }

                if (!field.IsBlank())
                {
                    command.CommandText += " AND Audits.field = @field";
                    command.Parameters.AddWithValue("field", field);
                }

                if (user_id.HasValue)
                {
                    command.CommandText += " AND Audits.user_id = @user_id";
                    command.Parameters.AddWithValue("user_id", user_id);
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
                            case "type":
                                orderByClause += "Audits.type,";
                                break;
                            case "desc-type":
                                orderByClause += "Audits.type DESC,";
                                break;
                            case "date-modified":
                                orderByClause += "Audits.date_modified,";
                                break;
                            case "desc-date-modified":
                                orderByClause += "Audits.date_modified DESC,";
                                break;
                            case "field":
                                orderByClause += "Audits.field,";
                                break;
                            case "desc-field":
                                orderByClause += "Audits.field DESC,";
                                break;
                        }
                    }
                }

                if (orderByClause.IsBlank())
                {
                    command.CommandText += " ORDER BY Audits.date_modified DESC";
                }
                else
                {
                    command.CommandText += " ORDER BY " + orderByClause.Trim(',');
                }

                var urlParameters = new { action = "Log", controller = "Audit", auditType = auditType, auditId = auditId, user_id = user_id, field = field, sort = sort };
                Paginator paginator = this.AddDefaultPaginator<ItemHistory>(urlParameters, page, command);
                this.SetPageSize(page_size, paginator);
                log = BaseModel.FetchPage<Audit>(paginator, command);
            }

            //Populate filter drop downs.
            ViewData["user_id"] = WBEADMS.Models.User.FetchSelectList(user_id);
            ViewData["auditType"] = Audit.FetchTypeSelectList(auditType);
            ViewData["field"] = Audit.FetchFieldSelectList(field);

            return View(log);
        }
    }
}