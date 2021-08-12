using System;
using System.Collections.Generic;

namespace WBEADMS.Models
{
    public partial class AdvancedNoteSearch_View : BaseModel
    {

        SampleType _sampleType;
        #region private members and properties
        public string note_id { get { return _data["advancednotesearch_view_id"]; } set { _data["advancednotesearch_view_id"] = value; } }
        public string location_id { get { return _data["location_id"]; } set { _data["location_id"] = value; } }
        //public string note_attachment_id{ get {return _data["note_attachment_id"]; } set { _data["note_attachment_id"] = value; } }
        public string date_occurred { get { return _data["date_occurred"]; } set { _data["date_occurred"] = value; } }
        public string body { get { return _data["body"]; } set { _data["body"] = value; } }
        public string is_starred { get { return _data["is_starred"]; } set { _data["is_starred"] = value; } }
        public string is_committed { get { return _data["is_committed"]; } set { _data["is_committed"] = value; } }
        public string is_deleted { get { return _data["is_deleted"]; } set { _data["is_deleted"] = value; } }
        public string created_by { get { return _data["created_by"]; } set { _data["created_by"] = value; } }
        public string date_created { get { return _data["date_created"].ToDateTimeFormat(); } set { _data["date_created"] = value.ToDateTimeFormat(); } }
        public string modified_by { get { return _data["modified_by"]; } set { _data["modified_by"] = value; } }
        public string date_modified { get { return _data["date_modified"].ToDateTimeFormat(); } set { _data["date_modified"] = value.ToDateTimeFormat(); } }
        public string parent_type_id { get { return _data["parent_type_id"]; } set { _data["parent_type_id"] = value; } }
        public string DateOccurred { get { return _data["date_occurred"].ToDateFormat(); } } // NOTE: this is no longer used as per conversation with Sanjay -James 2010-06-03
        public string DateCreated { get { return _data["date_created"].ToDateTimeFormat(); } }

        public string parameter_id { get { return _data["parameter_id"]; } set { _data["parameter_id"] = value; } }
        public Location Location { get { return LoadRelated<Location>(); } }

        public User created_user { get { return LoadRelated<User>("created_by"); } }
        // public Parameter pname { get { return LoadRelated<Parameter>("parameter_id"); } }
        public string sample_type_id
        {
            get
            {
                return _data["sample_type_id"];
            }

            set
            {
                _data["sample_type_id"] = value;
            }
        }

        public SampleType SampleType
        {
            get
            {
                if (_sampleType == null && !String.IsNullOrEmpty(sample_type_id))
                {
                    _sampleType = new SampleType(sample_type_id);
                }

                return _sampleType;
            }
        }
        List<string> _parametername = new List<string>();

        public List<string> FetchParameter()
        {
            if (String.IsNullOrEmpty(note_id))

                return null;

            else

                return BaseModel.FetchList("select name from Parameters where parameter_id in (select parameter_id from Notes_Parameters where note_id=" + note_id + ")", "name");


        }

        public List<string> GetCOCId()
        {
            if (String.IsNullOrEmpty(note_id))
                return null;
            else
                return BaseModel.FetchList("select  chain_of_custody_id from notes_chainofcustodys where note_id =" + note_id, "chain_of_custody_id");
        }
        public List<string> GetScheduleDate()
        {
            if (String.IsNullOrEmpty(note_id))
                return null;
            else
                return BaseModel.FetchList("select date_sampling_scheduled from chainofcustodys where chain_of_custody_id in (select  chain_of_custody_id from notes_chainofcustodys where note_id= " + note_id + ")", "date_sampling_scheduled");
        }

        #endregion
        public AdvancedNoteSearch_View() : base() { }
        public override void Validate()
        {
            return;
        }


        public static List<AdvancedNoteSearch_View> FetchAll(string whereClause)
        {
            return FetchAll<AdvancedNoteSearch_View>(new { where = whereClause });
        }


    }

}
