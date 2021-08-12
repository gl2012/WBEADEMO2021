/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public partial class Note : BaseModel
    {
        SampleType _sampleType;
        #region private members and properties
        public string note_id { get { return _data["note_id"]; } set { _data["note_id"] = value; } }
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
        public string dsc_tag { get { return _data["dsc_tag"]; } set { _data["dsc_tag"] = value; } }

        public bool committed { get { return _data["is_committed"].ToBool(); } set { _data["is_committed"] = value.ToString(); } }
        public bool starred { get { return _data["is_starred"].ToBool(); } set { _data["is_starred"] = value.ToString(); } }
        public bool deleted { get { return _data["is_deleted"].ToBool(); } set { _data["is_deleted"] = value.ToString(); } }

        public string parameter_list { get { return String.Join(",", GetRelatedList<Parameter>().ToArray()); } set { SetRelatedList<Parameter>(value); } }

        public BaseModel parent { get { return LoadParent(); } set { SetParent(value); } }
        public bool HasParent { get { return parent_type != ParentType.None && LoadParent() != null; } }
        public ParentType parent_type { get { return parent_type_id.IsBlank() ? ParentType.None : (ParentType)int.Parse(parent_type_id); } }
        public string parent_id { get { return (HasParent) ? parent.id : null; } set { _relatedObjects["parent_id"] = value; } }

        public string DateOccurred { get { return _data["date_occurred"].ToDateFormat(); } } // NOTE: this is no longer used as per conversation with Sanjay -James 2010-06-03
        public string DateCreated { get { return _data["date_created"].ToDateTimeFormat(); } }

        public Location Location { get { return LoadRelated<Location>(); } }
        public User created_user { get { return LoadRelated<User>("created_by"); } }
        public User modified_user { get { return LoadRelated<User>("modified_by"); } }
        public string sample_type_id { get { return _data["sample_type_id"]; } set { _data["sample_type_id"] = value; } }
        public Note[] notes
        {
            get
            {
                var note_list = LoadRelatedJoin<Note>("child_note_id", "parent_note_id");
                Array.Sort(note_list);
                return note_list;
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
        public string GetAttachmentLinks()
        {
            NoteAttachment att = NoteAttachment.GetAttachmentByNoteID(note_id);
            if (att != null && att.filename != null)
            {
                return "<a href=\"/Note.aspx/DownloadAttachment/" + att.note_attachment_id + "\">" + att.filename + "</a>";
            }
            else
            {
                return "None";
            }
        }

        public Parameter[] parameters { get { return LoadRelatedJoin<Parameter>(); } }

        #endregion

        public Note() : base(_table_fields)
        {
            committed = false;
            deleted = false;
            starred = false;
        }

        // instance methods
        public override void Validate()
        {
            var v = new Validator(_data);
            v.RequiredFields(Note._required_fields);
            v.TryInt("location_id");

            DateTime dateOccurred;
            if (v.TryDateTime("date_occurred", out dateOccurred))
            {
                if (dateOccurred > DateTime.Today.AddDays(1))
                {
                    v.AddError("date_occurred", "Date Occurred cannot be in the future.");
                }
            }

            v.MinLength("body", 3);

            v.Validate();
        }

        public override void Save()
        {
            base.Save();
            SaveRelatedJoin<Parameter>();
            if (HasParent)
            {
                switch (parent_type)
                {
                    case ParentType.Note:
                        SetRelatedList<Note>(parent_id);
                        SaveRelatedJoin<Note>("parent_note_id", "child_note_id");
                        break;
                    case ParentType.Item:
                        SetRelatedList<Item>(parent_id);
                        SaveRelatedJoin<Item>();
                        break;
                    case ParentType.Schedule:
                        SetRelatedList<Schedule>(parent_id);
                        SaveRelatedJoin<Schedule>();
                        break;
                    case ParentType.ChainOfCustody:
                        SetRelatedList<ChainOfCustody>(parent_id);
                        SaveRelatedJoin<ChainOfCustody>();
                        break;
                    case ParentType.Sample:
                        SetRelatedList<Sample>(parent_id);
                        SaveRelatedJoin<Sample>();
                        break;

                    default:
                        throw new InvalidOperationException("Unable to save note; [" + parent_type.ToString() + "] is an invalid Note.ParentType");
                }
            }
        }

        #region parent handling methods
        private BaseModel LoadParent()
        {
            if (parent_type == ParentType.None) { return null; }

            if (_relatedObjects["parent"] != null) { return (BaseModel)_relatedObjects["parent"]; }

            BaseModel parent;
            var relObjParentId = (string)_relatedObjects["parent_id"];
            if (!relObjParentId.IsBlank())
            {
                parent = GetParentById(relObjParentId);
            }
            else
            {
                parent = GetParentByJoin();
                if (parent != null)
                {
                    _relatedObjects["parent_id"] = parent.id;
                }
            }

            _relatedObjects["parent"] = parent;
            return parent;
        }

        private BaseModel GetParentById(string relObjParentId)
        {
            switch (parent_type)
            {
                case ParentType.Note:
                    return Note.Load(relObjParentId);
                case ParentType.Item:
                    return Item.Load(relObjParentId);
                case ParentType.Schedule:
                    return Schedule.Load(relObjParentId);
                case ParentType.ChainOfCustody:
                    return ChainOfCustody.Load(relObjParentId);
                case ParentType.Sample:
                    return Sample.Load(relObjParentId);

                case ParentType.Location:
                ////    return Location.Load(relObjParentId);  //use the Location property to load location
                default:
                    throw new InvalidOperationException("Unable to get note parent; [" + parent_type.ToString() + "] is an invalid Note.ParentType");
            }
        }

        private BaseModel GetParentByJoin()
        {
            BaseModel[] parents;
            switch (parent_type)
            {
                case ParentType.Note:
                    parents = LoadRelatedJoin<Note>("parent_note_id", "child_note_id");
                    break;
                case ParentType.Item:
                    parents = LoadRelatedJoin<Item>();
                    break;
                case ParentType.Schedule:
                    parents = LoadRelatedJoin<Schedule>();
                    break;
                case ParentType.ChainOfCustody:
                    parents = LoadRelatedJoin<ChainOfCustody>();
                    break;
                case ParentType.Sample:
                    parents = LoadRelatedJoin<Sample>();
                    break;

                case ParentType.Location:
                ////    return LoadRelated<Location>();  //use the Location property to load location
                default:
                    throw new InvalidOperationException("Unable to get related parent; [" + parent_type.ToString() + "] is an invalid Note.ParentType");
            }

            if (parents.Length > 0)
            {
                return parents[0];
            }
            else
            {
                return null;
            }
        }

        public void SetParent(BaseModel parent)
        {
            _relatedObjects["parent"] = parent;
            parent_id = parent.id;
            string parent_model = parent.GetModelName();
            if (Enum.IsDefined(typeof(ParentType), parent_model))
            {
                parent_type_id = ((int)Enum.Parse(typeof(ParentType), parent_model)).ToString();
            }
            else
            {
                throw new ArgumentException("Unable to set parent; [" + parent_id + "] is not a valid note parent", "parent");
            }
        }

        public string GetParentLocationId()
        {
            if (!HasParent) { throw new InvalidOperationException("This note does not have a parent."); }

            switch (parent_type)
            {
                case ParentType.Note:
                    return ((Note)parent).location_id;
                case ParentType.Item:
                    return ((Item)parent).location_id;
                case ParentType.Schedule:
                    return ((Schedule)parent).location_id;
                case ParentType.ChainOfCustody:
                    var coc = ((ChainOfCustody)parent);
                    if (coc.Preparation.Schedule != null)
                    {
                        return coc.Preparation.Schedule.location_id;
                    }
                    else if (coc.Deployment.Location != null)
                    {
                        return coc.Deployment.location_id;
                    }
                    else
                    {
                        return null;
                    }
                case ParentType.Sample:
                    return null;

                case ParentType.Location:
                    return this.location_id;
                default:
                    throw new InvalidOperationException("Unable to get related parent; [" + parent_type.ToString() + "] is an invalid Note.ParentType");
            }
        }
        #endregion

        public override string ToString()
        {
            string user;
            if (created_by.IsBlank())
            {
                var regex = new System.Text.RegularExpressions.Regex("^(.*?) \\(GuestBook\\)");
                var match = regex.Match(body);
                if (match.Success)
                {
                    user = match.Groups[1].ToString();
                }
                else
                {
                    user = "Unknown";
                }
            }
            else
            {
                user = created_user.display_name;
            }
            string date = date_occurred.ToDateFormat();

            return String.Format("Note from {1} by {0}", user, date);
        }

        #region IComparable Members

        public override int CompareTo(object obj)
        {
            Note target = obj as Note;
            if (target != null)
            {
                int dateCompare = -DateTime.Parse(date_occurred).CompareTo(DateTime.Parse(target.date_occurred));
                if (dateCompare == 0)
                {
                    return -id.CompareTo(target.id);
                }
                else
                {
                    return dateCompare;
                }
            }

            throw new ArgumentException("object is not a Note", "obj");
        }

        #endregion

        /// <summary>
        /// sets all notes that are 1 day old to committed status
        /// </summary>
        public static void ExpireOpenNotes()
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                string sql = "UPDATE Notes SET is_committed = 1 WHERE date_created < getdate() - 1 AND is_committed = 0";
                using (SqlCommand updateCommand = new SqlCommand(sql, connection))
                {
                    updateCommand.ExecuteNonQuery();
                }
            }
        }
    }
}