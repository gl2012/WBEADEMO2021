/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;

namespace WBEADMS.Models
{
    public partial class Item : BaseModel
    {
        #region properties and private members
        public string item_id { get { return _data["item_id"]; } private set { _data["item_id"] = value; } }
        public string serial_number { get { return _data["serial_number"]; } set { _data["serial_number"] = value; } }
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
        public string model_id { get { return _data["model_id"]; } set { _data["model_id"] = value; } }
        public string range { get { return _data["range"]; } set { _data["range"] = value; } }
        public string asset_number { get { return _data["asset_number"]; } set { _data["asset_number"] = value; } }
        public string comment { get { return _data["comment"]; } set { _data["comment"] = value; } }
        public string date_created { get { return _data["date_created"].ToDateTimeFormat(); } set { _data["date_created"] = value.ToDateTimeFormat(); } }
        public string location_id { get { return _data["location_id"]; } set { _data["location_id"] = value; } }
        public string is_integrated { get { return _data["is_integrated"]; } set { _data["is_integrated"] = value; } }
        public string sample_type_id { get { return _data["sample_type_id"]; } set { _data["sample_type_id"] = value; } }
        public bool IsUnassigned { get { if (Location == null) return true; else return Location.name == "Unassigned"; } }
        public bool IsIntegrated { get { return is_integrated.ToBool(); } }

        public string parameter_list { get { return String.Join(",", GetRelatedList<Parameter>().ToArray()); } set { SetRelatedList<Parameter>(value); } }

        public ItemModel model { get { return LoadRelated<ItemModel>("model_id"); } }
        public ItemMake make { get { return model.make; } }
        public Parameter[] parameters { get { return LoadRelatedJoin<Parameter>(); } }

        public void MyToString(Parameter s) { return; }

        public string parameterNames
        {
            get
            {
                //parameters.map{ |x| x.name }.join(',') //Ruby
                var list = new System.Collections.Generic.List<string>();
                foreach (var parameter in parameters)
                {
                    list.Add(parameter.name);
                }
                return String.Join(" ", list.ToArray());
            }
        }

        public Note[] notes
        {
            get
            {
                var note_list = LoadRelatedJoin<Note>("Notes_Items");
                Array.Sort(note_list);
                return note_list;
            }
        }
        public SampleType SampleType
        {
            get
            {
                if (sample_type_id.IsBlank())
                {
                    return null;
                }
                else
                {
                    return SampleType.Load(sample_type_id);
                }
            }
        }

        private ItemHistory RecentItemHistory { get { return LoadItemHistory(); } }
        public string DateInstalled { get { return RecentItemHistory.DateInstalled; } }
        public string DateRemoved { get { return RecentItemHistory.DateRemoved; } }
        public Location Location { get { return LoadRelated<Location>(); } }

        public string DisplayName
        {
            get
            {
                if (_data["name"].IsBlank())
                {
                    return GenerateName();
                }
                else
                {
                    return _data["name"];
                }
            }
        }
        #endregion

        public Item() : base(_table_name, _id_field, _table_fields) { }

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                serial_number.CheckRequired(errors, "serial_number");

                model_id.CheckRequired(errors, "model_id");

                if (IsIntegrated)
                {
                    sample_type_id.CheckRequired(errors, "sample_type_id", "Sample Type", "is required for Integrated samplers.");
                }
                else
                {
                    parameter_list.CheckRequired(errors, "parameter_list", "Parameter", "is required for Continuous samplers.");
                }
            }
        }

        public override void Save()
        {
            if (_data["name"] == GenerateName())
            {
                _data["name"] = "";
            }

            bool newRecord = false;
            if (id.IsBlank() && location_id.IsBlank())
            {
                newRecord = true;
                var inventoryFetchLocation = Location.LoadByName("Inventory");
                if (inventoryFetchLocation == null)
                {
                    throw new ModelException(new ValidationError("location_id", "Unable to find Inventory to place the new item in. Create a location named Inventory first."));
                }

                location_id = inventoryFetchLocation.id;
            }

            // save model
            base.Save();

            // save parameters (ensure that SampleType is automatically a parameter)
            if (SampleType != null && SampleType.Parameter != null && !GetRelatedList<Parameter>().Contains(SampleType.Parameter.id))
            {
                AddToRelatedList<Parameter>(SampleType.Parameter.id);
            }
            SaveRelatedJoin<Parameter>();

            if (newRecord)
            {
                ItemHistory.RelocateItem(this, "0", this.location_id, DateTime.Today); //hack to insert a new item history for Inventory
            }
        }

        public void Relocate(Location newLocation, DateTime date)
        {
            ItemHistory.RelocateItem(this, this.Location.id, newLocation.id, date);
            //update our 
            this._data["location_id"] = newLocation.id;
            _relatedObjects.Remove("location_id");
            Save();
        }

        public override string ToString()
        {
            return DisplayName;
        }

        private ItemHistory LoadItemHistory()
        {
            if (_relatedObjects["recentHistory"] != null)
            {
                return (ItemHistory)_relatedObjects["recentHistory"];
            }

            var recentHistory = ItemHistory.GetMostRecentHistory(id);
            _relatedObjects["recentHistory"] = recentHistory;
            return recentHistory;
        }

        private string GenerateName()
        {
            if (location_id.IsBlank())
            {
                return serial_number;
            }
            else
            {
                return Location.name + " " + (IsIntegrated ? SampleType.name : parameterNames);
            }
        }
    }
}