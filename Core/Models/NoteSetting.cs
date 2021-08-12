using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WBEADMS.Models
{

    public partial class NoteSetting : BaseModel
    {
        List<SampleType> _sampletype;
        List<Parameter> _parameter;
        List<User> _user;
        #region properties
        public string note_setting_id { get { return _data["note_setting_id"]; } set { _data["note_setting_id"] = value; } }
        public string setting_name { get { return _data["setting_name"]; } set { _data["setting_name"] = value; } }
        public string setting_description { get { return _data["setting_description"]; } set { _data["setting_description"] = value; } }
        public string author_id { get { return _data["author_id"]; } set { _data["author_id"] = value; } }
        public string create_by { get { return _data["create_by"]; } set { _data["create_by"] = value; } }
        public string create_date { get { return _data["create_date"].ToDateTimeFormat(); } set { _data["create_date"] = value.ToDateTimeFormat(); } }
        public string edit_date { get { return _data["edit_date"].ToDateTimeFormat(); } set { _data["edit_date"] = value.ToDateTimeFormat(); } }

        public User Authorby
        {
            get
            {
                return LoadRelated<User>("author_id");
            }
        }

        #endregion
        public List<SampleType> SampleType
        {
            get
            {

                if (_sampletype == null)
                {
                    LoadRelatedSampleType();
                }

                return _sampletype;
            }
        }
        private void LoadRelatedSampleType()
        {
            // This abuses the fact that we are check on null to populate this list.

            _sampletype = new List<SampleType>();

            foreach (SampleType n in LoadRelatedJoin<SampleType>("NoteSettings_SampleTypes"))
            {
                SampleType s = new SampleType(n.id);

                _sampletype.Add(s);
            }
        }
        public List<Parameter> parameter
        {
            get
            {

                if (_parameter == null)
                {
                    LoadRelatedParameter();
                }

                return _parameter;
            }
        }
        public void LoadRelatedParameter()
        {
            // This abuses the fact that we are check on null to populate this list.

            _parameter = new List<Parameter>();

            foreach (Parameter n in LoadRelatedJoin<Parameter>("NoteSettings_Parameters"))
            {
                _parameter.Add(n);
            }
        }
        public List<User> user
        {
            get
            {

                if (_user == null)
                {
                    LoadRelateduser();
                }

                return _user;
            }
        }
        private void LoadRelateduser()
        {
            // This abuses the fact that we are check on null to populate this list.

            _user = new List<User>();

            foreach (User n in LoadRelatedJoin<User>("NoteSettings_Users"))
            {
                _user.Add(n);
            }
        }




        public List<string> FetchNoteSettingSampleTypeIdlist()
        {
            List<string> userSampleTypeList = new List<string>();
            foreach (SampleType n in LoadRelatedJoin<SampleType>("NoteSettings_SampleTypes"))
            {
                userSampleTypeList.Add(n.id);
            }


            return userSampleTypeList;

        }
        public List<string> FetchNoteSettingParameterIdlist()
        {
            List<string> parameterList = new List<string>();
            foreach (Parameter n in LoadRelatedJoin<Parameter>("NoteSettings_Parameters"))
            {
                parameterList.Add(n.parameter_id);
            }


            return parameterList;

        }
        public List<string> FetchNoteSettingUserIdlist()
        {
            List<string> userList = new List<string>();
            foreach (User n in LoadRelatedJoin<User>("NoteSettings_Users"))
            {
                userList.Add(n.user_id);
            }


            return userList;

        }

        public NoteSetting() : base() { }

        public static NoteSetting Load(string id)
        {
            return Load<NoteSetting>(id);
        }
        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {

                errors.AddError(setting_name.CheckRequired("setting_name"));
                errors.AddError(setting_name.CheckMaxLength(50, "setting_name"));

                errors.AddError(setting_description.CheckRequired("setting_description"));
                errors.AddError(setting_description.CheckMaxLength(500, "setting_description"));




            }

        }
        public void AddSampleType(string id)
        {
            AddToRelatedList<SampleType>(id);
        }

        public void RemoveSampleType(string id)
        {
            RemoveFromRelatedList<SampleType>(id);
        }
        public void SetSampleType(string id)
        {
            SetRelatedList<SampleType>(id);
        }
        /// <summary>Saves a related Samples.  Set the IDs with AddRelatedSample().</summary>
        public void SaveRelatedSampleType()
        {
            SaveRelatedJoin<SampleType>();
        }

        public void AddParameter(string id)
        {
            AddToRelatedList<Parameter>(id);
        }
        public void SetParameter(string id)
        {
            SetRelatedList<Parameter>(id);
        }
        public void RemoveParameter(string id)
        {
            RemoveFromRelatedList<Parameter>(id);
        }

        /// <summary>Saves a related Samples.  Set the IDs with AddRelatedSample().</summary>
        public void SaveRelatedParameter()
        {
            SaveRelatedJoin<Parameter>();
        }
        public void SaveRelatedUser()
        {
            SaveRelatedJoin<User>();
        }
        public void RemoveUser(string id)
        {
            RemoveFromRelatedList<User>(id);
        }
        public void AddUser(string id)
        {
            AddToRelatedList<User>(id);
        }

        public override void Save()
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");

            if (String.IsNullOrEmpty(note_setting_id))
            {
                create_date = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo).ToISODateTime();



                base.Save();

            }
            else
            {
                // Save Edits.
                edit_date = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo).ToISODateTime();

                base.Save();
            }
        }

        public static SelectList FetchSettingSelectList(object selectedValue, string userid)
        {
            return new SelectList(FetchAll<NoteSetting>(new { where = "create_by=" + userid + " or Create_by=118" }), "note_setting_id", "setting_name", selectedValue);
        }
        public static SelectList FetchDescriptionSelectList(object selectedValue)
        {

            return new SelectList(FetchAll<NoteSetting>(), "note_setting_id", "setting_description", selectedValue);


        }

    }
}
