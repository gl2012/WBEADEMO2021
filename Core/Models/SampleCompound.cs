/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */

namespace WBEADMS.Models
{
    public partial class SampleCompound : BaseModel
    {

        #region Properties
        public string sample_compound_id
        {
            get
            {
                return _data["sample_compound_id"];
            }
        }

        public string sample_result_id
        {
            get
            {
                return _data["sample_result_id"];
            }

            set
            { // NOTE: from James: why was this accessor private?!?
                _data["sample_result_id"] = value;
            }
        }

        public string sample_sub_type
        {
            get
            {
                return _data["sample_sub_type"];
            }

            private set
            {
                _data["sample_sub_type"] = value;
            }
        }

        public string compound_name
        {
            get
            {
                return _data["compound_name"];
            }

            private set
            {
                _data["compound_name"] = value;
            }
        }

        public string compound_number
        {
            get
            {
                return _data["compound_number"];
            }

            private set
            {
                _data["compound_number"] = value;
            }
        }

        public string cas_number
        {
            get
            {
                return _data["cas_number"];
            }

            private set
            {
                _data["cas_number"] = value;
            }
        }

        public string molecular_formula
        {
            get
            {
                return _data["molecular_formula"];
            }

            private set
            {
                _data["molecular_formula"] = value;
            }
        }

        public string molecular_weight
        {
            get
            {
                return _data["molecular_weight"];
            }

            private set
            {
                _data["molecular_weight"] = value;
            }
        }

        public string compound_group_name
        {
            get
            {
                return _data["compound_group_name"];
            }

            private set
            {
                _data["compound_group_name"] = value;
            }
        }

        public string retention_time
        {
            get
            {
                return _data["retention_time"];
            }

            private set
            {
                _data["retention_time"] = value;
            }
        }

        public string match_quality
        {
            get
            {
                return _data["match_quality"];
            }

            private set
            {
                _data["match_quality"] = value;
            }
        }

        public string method_detection_limit
        {
            get
            {
                return _data["method_detection_limit"];
            }

            private set
            {
                _data["method_detection_limit"] = value;
            }
        }

        public string method_detection_unit
        {
            get
            {
                return _data["method_detection_unit"];
            }

            private set
            {
                _data["method_detection_unit"] = value;
            }
        }

        public string reporting_detection_limit
        {
            get
            {
                return _data["reporting_detection_limit"];
            }

            private set
            {
                _data["reporting_detection_limit"] = value;
            }
        }

        public string reporting_detection_unit
        {
            get
            {
                return _data["reporting_detection_unit"];
            }

            private set
            {
                _data["reporting_detection_unit"] = value;
            }
        }

        public string raw_measurement
        {
            get
            {
                return _data["raw_measurement"];
            }

            private set
            {
                _data["raw_measurement"] = value;
            }
        }

        public string raw_measurement_unit
        {
            get
            {
                return _data["raw_measurement_unit"];
            }

            private set
            {
                _data["raw_measurement_unit"] = value;
            }
        }

        public string final_concentration
        {
            get
            {
                return _data["final_concentration"];
            }

            private set
            {
                _data["final_concentration"] = value;
            }
        }

        public string final_concentration_unit
        {
            get
            {
                return _data["final_concentration_unit"];
            }

            private set
            {
                _data["final_concentration_unit"] = value;
            }
        }

        public string qaqc_flag
        {
            get
            {
                return _data["qaqc_flag"];
            }

            private set
            {
                _data["qaqc_flag"] = value;
            }
        }
        #endregion

        public SampleCompound() : base() { }

        public SampleCompound(string labSampleID) : this()
        {
            sample_result_id = labSampleID;
            Load();
        }

        private void Load()
        {
            LoadData();
        }

        /// <summary>Initializes a new instance of the ResultData class. Used by Web Service to create a ResultData.</summary>
        public SampleCompound(System.Collections.Generic.Dictionary<string, string> _data)
            : this()
        {
            foreach (var keyvaluepair in _data)
            {
                this._data[keyvaluepair.Key] = keyvaluepair.Value;
            }  // TODO: perhaps make this into a DictionaryExtension method?  call it ToNameValueCollection() ?
        }

        public override void Validate()
        {
            ModelException errors = new ModelException();

            if (errors.hasErrors)
            {
                throw errors;
            }
        }

        public override void Save()
        {
            base.Save();
        }

        /// <summary>Hack to return _data for Webservice</summary>
        public System.Collections.Specialized.NameValueCollection GetData()
        {
            return _data;
        }

        /// <summary>Hack to set _data for Webservice</summary>
        public void SetData(System.Collections.Specialized.NameValueCollection newData)
        {
            _data = newData;
        }

        public static SampleCompound Load(string id)
        {
            return Load<SampleCompound>(id);
        }
    }
}