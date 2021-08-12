using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WBEADMS;
using WBEADMS.Models;

namespace DocIt.Controllers
{
    public class NoteSettingController : Controller
    {
        // GET: NoteSettingContoller

        public ActionResult Index(int? page, int? page_size)
        {
            string joinClause = "";
            string whereClause = "Create_by=" + this.GetUser().user_id + "or Create_by=118";
            string orderByClause = "";

            // System.IO.File.WriteAllText(@"C:\temp\logonpost.txt", "logon");

            //paginate
            var urlParameters = new { action = "Index", controller = "NoteSetting" };
            Paginator paginator = this.AddDefaultPaginator<NoteSetting>(urlParameters, page, new { join = joinClause, where = whereClause });
            this.SetPageSize(page_size, paginator);

            List<NoteSetting> notesetting = BaseModel.FetchPage<NoteSetting>(paginator, new { join = joinClause, order = orderByClause, where = whereClause });

            // add notice if there is no records
            if (notesetting.Count == 0)
            {
                this.AddViewNotice("No Notes Anvanced Search Setting were found.");
            }

            return View(notesetting);
        }

        public ActionResult Create()

        {
            ViewData["authorList"] = WBEADMS.Models.User.FetchSelectListActive();
            ViewData["parameterList"] = WBEADMS.Models.Parameter.FetchSelectListActive();
            ViewData["sampletypeList"] = WBEADMS.Models.SampleType.FetchDictionaryByName();
            return View();
            // return RedirectToAction("Index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)

        {
            //  System.IO.File.WriteAllText(@"C:\temp\logonpost.txt", collection["parameter_list"].ToString()+collection["sampletype_list"].ToString());
            List<string> newsampletypeList = new List<string>();
            List<string> newparameterList = new List<string>();
            List<string> newuserList = new List<string>();
            if (collection["sampletype_list"] != null)
            { newsampletypeList = collection["sampletype_list"].Split(',').ToList(); }

            if (collection["parameter_list"] != null)
            { newparameterList = collection["parameter_list"].Split(',').ToList(); }

            if (collection["author_list"] != null)
            { newuserList = collection["author_list"].Split(',').ToList(); }

            NoteSetting newsetting = new NoteSetting();

            ViewData["authorList"] = WBEADMS.Models.User.FetchSelectListActive();
            ViewData["parameterList"] = WBEADMS.Models.Parameter.FetchSelectListActive();
            ViewData["sampletypeList"] = WBEADMS.Models.SampleType.FetchDictionaryByName();

            UpdateModel(newsetting);
            newsetting.create_by = this.GetUser().user_id;
            try
            {
                newsetting.Save();

                //  System.IO.File.WriteAllText(@"C:\temp\newid.txt", newsetting.note_setting_id);
                if (!newsetting.note_setting_id.IsBlank())
                {
                    var newsetting1 = NoteSetting.Load(newsetting.note_setting_id);
                    foreach (var i in newsampletypeList)
                    {
                        newsetting1.AddSampleType(i);

                    }
                    newsetting1.SaveRelatedSampleType();
                    foreach (var i in newparameterList)
                    {
                        newsetting1.AddParameter(i);

                    }
                    newsetting1.SaveRelatedParameter();

                    foreach (var i in newuserList)
                    {
                        newsetting1.AddUser(i);

                    }
                    newsetting1.SaveRelatedUser();

                }
            }
            catch (ModelException e)
            {

                this.PopulateViewWithErrorMessages(e);
                return View();

            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            ViewBag.message = "";
            if (id.IsBlank())
            {
                this.AddTempNotice("Invalid Note Advanced Search Setting Id.");
                return RedirectToAction("Index");
            }

            NoteSetting selectedsetting = NoteSetting.Load(id);
            ViewData["authorList"] = WBEADMS.Models.User.FetchSelectListActive();
            ViewData["parameterList"] = WBEADMS.Models.Parameter.FetchSelectListActive();
            ViewData["sampletypeList"] = WBEADMS.Models.SampleType.FetchDictionaryByName();
            ViewData["SelectedSampletypeIdList"] = selectedsetting.FetchNoteSettingSampleTypeIdlist();
            ViewData["SelectedParameterIdList"] = selectedsetting.FetchNoteSettingParameterIdlist();
            ViewData["SelectedUserIdList"] = selectedsetting.FetchNoteSettingUserIdlist();

            if (selectedsetting == null)
            {
                this.AddTempNotice("Note Advanced Search Setting  " + id + " could not be found.");
                return RedirectToAction("Index");
            }

            return View(selectedsetting);
        }

        public ActionResult Edit(string id)
        {
            if (id.IsBlank())
            {
                this.AddTempNotice("Invalid Note Advanced Search Setting Id.");
                return RedirectToAction("Index");
            }
            NoteSetting selectedSetting = NoteSetting.Load(id);
            ViewData["authorList"] = WBEADMS.Models.User.FetchSelectListActive();
            ViewData["parameterList"] = WBEADMS.Models.Parameter.FetchSelectListActive();
            ViewData["sampletypeList"] = WBEADMS.Models.SampleType.FetchDictionaryByName();




            ViewData["SelectedUserIdList"] = selectedSetting.FetchNoteSettingUserIdlist();
            ViewData["SelectedSampletypeIdList"] = selectedSetting.FetchNoteSettingSampleTypeIdlist();
            ViewData["SelectedParameterIdList"] = selectedSetting.FetchNoteSettingParameterIdlist();



            return View(selectedSetting);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            NoteSetting modifiedCoC = NoteSetting.Load(id);
            ViewData["authorList"] = WBEADMS.Models.User.FetchSelectListActive();
            ViewData["parameterList"] = WBEADMS.Models.Parameter.FetchSelectListActive();
            ViewData["sampletypeList"] = WBEADMS.Models.SampleType.FetchDictionaryByName();

            ViewData["SelectedUserIdList"] = modifiedCoC.FetchNoteSettingUserIdlist();
            ViewData["SelectedSampletypeIdList"] = modifiedCoC.FetchNoteSettingSampleTypeIdlist();
            ViewData["SelectedParameterIdList"] = modifiedCoC.FetchNoteSettingParameterIdlist();
            List<string> newsampletypeList = new List<string>();
            List<string> newparameterList = new List<string>();
            List<string> newuserList = new List<string>();
            if (collection["sampletype_list"] != null)
            { newsampletypeList = collection["sampletype_list"].Split(',').ToList(); }

            if (collection["parameter_list"] != null)
            { newparameterList = collection["parameter_list"].Split(',').ToList(); }

            if (collection["author_list"] != null)
            { newuserList = collection["author_list"].Split(',').ToList(); }
            try
            {
                UpdateModel(modifiedCoC);
                foreach (var i in modifiedCoC.FetchNoteSettingSampleTypeIdlist())
                { modifiedCoC.RemoveSampleType(i); }

                foreach (var i in modifiedCoC.FetchNoteSettingParameterIdlist())
                { modifiedCoC.RemoveParameter(i); }

                foreach (var i in modifiedCoC.FetchNoteSettingUserIdlist())
                { modifiedCoC.RemoveUser(i); }

                modifiedCoC.Save();

                foreach (var i in newsampletypeList)
                {
                    modifiedCoC.AddSampleType(i);


                }
                modifiedCoC.SaveRelatedSampleType();

                foreach (var i in newparameterList)
                {
                    modifiedCoC.AddParameter(i);

                }
                modifiedCoC.SaveRelatedParameter();

                foreach (var i in newuserList)
                {
                    modifiedCoC.AddUser(i);

                }
                modifiedCoC.SaveRelatedUser();
                this.AddTempNotice("Successfully updated your Note Advanced Search Setting " + modifiedCoC.setting_name + "'s details.");
                return RedirectToAction("Index");
            }
            catch (ModelException me)
            {
                this.PopulateViewWithErrorMessages(me);
            }
            catch (Exception e)
            {
                ModelException error = new ModelException(e);
                this.PopulateViewWithErrorMessages(error);
            }


            return View(modifiedCoC);
        }



    }
}