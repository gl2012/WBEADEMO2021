/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;

namespace WBEADMS.Models
{
    public partial class SampleResult : BaseModel
    {

        #region Properties
        public string sample_result_id
        {
            get
            {
                return _data["sample_result_id"];
            }

            private set
            {
                _data["sample_result_id"] = value;
            }
        }

        public string lab_name
        {
            get
            {
                return _data["lab_name"];
            }

            private set
            {
                _data["lab_name"] = value;
            }
        }

        public string client_name
        {
            get
            {
                return _data["client_name"];
            }

            private set
            {
                _data["client_name"] = value;
            }
        }

        public string wbea_id
        {
            get
            {
                return _data["wbea_id"];
            }

            set
            {  // NOTE: from James: why was this accessor private?!?
                _data["wbea_id"] = value;
            }
        }

        public string lab_sample_id
        {
            get
            {
                return _data["lab_sample_id"];
            }

            private set
            {
                _data["lab_sample_id"] = value;
            }
        }

        public string sample_media_serial_number
        {
            get
            {
                return _data["sample_media_serial_number"];
            }

            private set
            {
                _data["sample_media_serial_number"] = value;
            }
        }

        public string date_sample_received
        {
            get
            {
                return _data["date_sample_recieved"];
            }

            private set
            {
                _data["date_sample_recieved"] = value;
            }
        }

        public string sample_damaged
        {
            get
            {
                return _data["sample_damaged"];
            }

            private set
            {
                _data["sample_damaged"] = value;
            }
        }

        public string location_name
        {
            get
            {
                return _data["location_name"];
            }

            set
            {  // NOTE: why would this be private?!?
                _data["location_name"] = value;
            }
        }

        // TODO: there should be just location_name; unless we give the client location_ids for them to send to the webservice or make some kind of location name string parser and give really good error message when it fails... i.e. "Error: Location with name of 'AMS1' not found. This could possibly be a mistype of 'AMS01'."  and do we give conflict resolution?
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

        public string date_sample_start
        {
            get
            {
                return _data["date_sample_start"];
            }

            private set
            {
                _data["date_sample_start"] = value;
            }
        }

        public string date_sample_end
        {
            get
            {
                return _data["date_sample_end"];
            }

            private set
            {
                _data["date_sample_end"] = value;
            }
        }

        public string lab_technician_initials
        {
            get
            {
                return _data["lab_technician_initials"];
            }

            private set
            {
                _data["lab_technician_initials"] = value;
            }
        }

        public string voc_received_cannister_pressure
        {
            get
            {
                return _data["voc_recieved_cannister_pressure"];
            }

            private set
            {
                _data["voc_recieved_cannister_pressure"] = value;
            }
        }

        public string sample_type
        {
            get
            {
                return _data["sample_type"];
            }

            set
            { // NOTE: why is this private?!?
                _data["sample_type"] = value;
            }
        }

        public string file_name
        {
            get
            {
                return _data["file_name"];
            }

            private set
            {
                _data["file_name"] = value;
            }
        }

        public string batch_id
        {
            get
            {
                return _data["batch_id"];
            }

            private set
            {
                _data["batch_id"] = value;
            }
        }

        public string analysis_id
        {
            get
            {
                return _data["analysis_id"];
            }

            private set
            {
                _data["analysis_id"] = value;
            }
        }

        public string date_results_reported
        {
            get
            {
                return _data["date_results_reported"];
            }

            private set
            {
                _data["date_results_reported"] = value;
            }
        }

        public string comments
        {
            get
            {
                return _data["comments"];
            }

            private set
            {
                _data["comments"] = value;
            }
        }

        public string wind_speed
        {
            get
            {
                return _data["wind_speed"];
            }

            private set
            {
                _data["wind_speed"] = value;
            }
        }

        public string relative_humidity
        {
            get
            {
                return _data["relative_humidity"];
            }

            private set
            {
                _data["relative_humidity"] = value;
            }
        }

        public string temperature
        {
            get
            {
                return _data["temperature"];
            }

            private set
            {
                _data["temperature"] = value;
            }
        }

        public Sample Sample
        {
            get
            {
                if (_relatedObjects["wbea_id"] == null)
                {
                    _relatedObjects["wbea_id"] = Sample.FetchByWBEAId(wbea_id);
                }
                return (Sample)_relatedObjects["wbea_id"];
            }
        }

        public SampleCompound[] SampleCompounds
        {
            get
            {
                return LoadByForeignKey<SampleCompound>();
            }
        }

        #endregion

        public SampleResult() : base() { }

        public SampleResult(string labSampleID) : this()
        {
            sample_result_id = labSampleID;
            Load();
        }

        private void Load()
        {
            LoadData();
        }

        /// <summary>Initializes a new instance of the SampleResult class. Used by Web Service to create a SampleResult.</summary>
        public SampleResult(System.Collections.Generic.Dictionary<string, string> _data)
            : this()
        {
            foreach (var keyvaluepair in _data)
            {
                this._data[keyvaluepair.Key] = keyvaluepair.Value;
            }  // TODO: perhaps make this into a DictionaryExtension method?  call it ToNameValueCollection() ?
        }

        /// <summary>Initializes a new instance of the SampleResult class. Used by Web Service to create a SampleResult.</summary>
        public SampleResult(System.Collections.Specialized.NameValueCollection _data) :
            this()
        {
            this._data = _data;
        }

        public override void Validate()
        {
            using (var errors = new ModelException())
            {
                errors.AddError(batch_id.CheckRequired("batch_id"));
            }
        }

        public override void Save()
        {
            base.Save();
        }

        public static SampleResult Load(string sample_result_id)
        {
            return Load<SampleResult>(sample_result_id);
        }


        public static SampleResult FetchByWBEAId(string WBEAId)
        {
            ////throw new NotImplementedException();
            return FetchByWBEAId(WBEAId, "Primary");
        }

        public static SampleResult FetchByWBEAId(string WBEAId, string sample_media_type)
        {
            /* TODO:
             *  sample_media_type is like replicate or primary, since WBEAId cannot be used in that situation
             */
            throw new NotImplementedException();
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

        public override string ToString()
        {
            return lab_sample_id;
        }
    }
}