/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;

namespace WBEADMS.Models
{
    public partial class Location : BaseModel
    {

        User _userCreated;
        User _userModified;

        #region Properties
        public string location_id
        {
            get
            {
                return _data["location_id"];
            }

            private set
            {
                _data["location_id"] = value;
            }
        }

        public string name
        {
            get
            {
                return _data["name"];
            }

            set
            {
                _data["name"] = value;
            }
        }

        public string full_name
        {
            get
            {
                return _data["full_name"];
            }

            set
            {
                _data["full_name"] = value;
            }
        }

        public string latitude
        {
            get
            {
                return _data["latitude"];
            }

            set
            {
                _data["latitude"] = value;
            }
        }

        public string longitude
        {
            get
            {
                return _data["longitude"];
            }

            set
            {
                _data["longitude"] = value;
            }
        }

        public string address
        {
            get
            {
                return _data["address"];
            }

            set
            {
                _data["address"] = value;
            }
        }

        public string phone_number
        {
            get
            {
                return _data["phone_number"];
            }

            set
            {
                _data["phone_number"] = value;
            }
        }

        public string comments
        {
            get
            {
                return _data["comments"];
            }

            set
            {
                _data["comments"] = value;
            }
        }

        public string terrain_description
        {
            get
            {
                return _data["terrain_description"];
            }

            set
            {
                _data["terrain_description"] = value;
            }
        }

        public string tree_canopy_description
        {
            get
            {
                return _data["tree_canopy_description"];
            }

            set
            {
                _data["tree_canopy_description"] = value;
            }
        }

        public string nearby_sources
        {
            get
            {
                return _data["nearby_sources"];
            }

            set
            {
                _data["nearby_sources"] = value;
            }
        }

        public string wet_weather_access
        {
            get
            {
                return _data["wet_weather_access"];
            }

            set
            {
                _data["wet_weather_access"] = value;
            }
        }

        public string winter_access
        {
            get
            {
                return _data["winter_access"];
            }

            set
            {
                _data["winter_access"] = value;
            }
        }

        public string modified_by
        {
            get
            {
                return _data["modified_by"];
            }

            set
            {
                _data["modified_by"] = value;
            }
        }

        public string date_modified
        {
            get
            {
                return _data["date_modified"].ToDateTimeFormat();
            }

            set
            {
                _data["date_modified"] = value.ToDateTimeFormat();
            }
        }

        public string created_by
        {
            get
            {
                return _data["created_by"];
            }

            set
            {
                _data["created_by"] = value;
            }
        }

        public string date_created
        {
            get
            {
                return _data["date_created"].ToDateTimeFormat();
            }

            set
            {
                _data["date_created"] = value.ToDateTimeFormat();
            }
        }

        public string qa_signoff
        {
            get
            {
                return _data["qa_signoff"];
            }

            set
            {
                _data["qa_signoff"] = value;
            }
        }

        public string date_qa_signoff
        {
            get
            {
                return _data["date_qa_signoff"];
            }

            set
            {
                _data["date_qa_signoff"] = value;
            }
        }

        public string active
        {
            get
            {
                return _data["active"];
            }

            set
            {
                _data["active"] = value;
            }
        }

        public string DateQASignoff
        {
            get
            {
                return _data["date_qa_signoff"].ToDateTimeFormat();
            }
        }

        public User UserCreated
        {
            get
            {
                if (_userCreated == null && !String.IsNullOrEmpty(created_by))
                {
                    _userCreated = new User(created_by);
                }

                return _userCreated;
            }
        }

        public User UserModified
        {
            get
            {

                if (_userModified == null && !String.IsNullOrEmpty(modified_by))
                {
                    _userModified = new User(modified_by);
                }

                return _userModified;
            }
        }

        /// <summary>Gets notes that are associated with this location.</summary>
        public Note[] Notes
        {
            get
            {
                var note_list = LoadByForeignKey<Note>(1, 99); // TODO: what if we wanted to show last 99 instead? should we add a base model method that takes in a order by parameter?
                Array.Sort(note_list);
                return note_list;
            }
        }

        /// <summary>Gets items that are currently at this location.</summary>
        public Item[] Items
        {
            get
            {
                if (_relatedObjects["item_id"] == null)
                {
                    //_relatedObjects["item_id"] = ItemHistory.GetItemsByLocation(id).ToArray();
                    _relatedObjects["item_id"] = Item.FetchAll("location_id", location_id).ToArray();
                }
                return (Item[])_relatedObjects["item_id"];
            }
        }

        /// <summary>Gets parameters associated with items currently at this location.</summary>
        public Parameter[] Parameters
        {
            get
            {
                var relatedParameters = new System.Collections.Generic.List<Parameter>();
                foreach (var item in Items)
                {
                    relatedParameters.AddRange(item.parameters);
                }

                return BaseModel.RemoveDuplicates(relatedParameters.ToArray());
            }
        }
        #endregion

        public Location() : base() { }

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                name.CheckRequired(errors, "name");
                name.CheckMaxLength(errors, 50, "name");

                errors.AddError(full_name.CheckMaxLength(50, "full_name"));

                if (!errors.AddError(latitude.CheckIfDecimal("latitude")))
                {
                    errors.AddError(latitude.CheckIfDecimalInRange(-90, 90, "latitude"));
                }

                if (!longitude.CheckIfDecimal(errors, "longitude"))
                {
                    longitude.CheckIfDecimalInRange(errors, -180, 180, "longitude");
                }

                errors.AddError(phone_number.CheckMaxLength(50, "phone_number"));

                errors.AddError(modified_by.CheckRequired("modified_by", "Last Modified By"));

                if (!errors.AddError(date_modified.CheckRequired("date_modified", "Date Last Modified")))
                {
                    errors.AddError(date_modified.CheckIfDateTime("date_modified", "Date Last Modified"));
                }

                errors.AddError(date_qa_signoff.CheckIfDateTime("date_qa_signoff", "Date of QA Signoff"));

                if (!errors.AddError(active.CheckRequired("active")))
                {
                    errors.AddError(active.CheckIfBool("active"));
                }
            }
        }

        public override void Save()
        {
            //check to see if this is a new location or an location edit.
            if (IsNewRecord)
            {
                base.Save();
            }
            else
            {
                base.Save();
                UpdateAuditLog();
            }
        }

        private void UpdateAuditLog()
        {
            List<string> ignore = new List<string>();
            ignore.Add("modified_by");
            ignore.Add("date_modified");

            base.UpdateAuditLog(ignore, UserModified.user_id);
        }
    }
}