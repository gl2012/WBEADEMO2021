/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public partial class Sample : BaseModel
    {
        SampleType _sampleType;
        List<string> _chainOfcustodyIDs = new List<string>();

        #region Properties
        public Note[] Notes
        {
            get
            {
                var note_list = LoadRelatedJoin<Note>("Notes_samples");
                Array.Sort(note_list);
                return note_list;
            }
        }
        public string sample_id
        {
            get
            {
                return _data["sample_id"];
            }

            //  private set
            set
            {
                _data["sample_id"] = value;
            }
        }

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

        public string wbea_id
        {
            get
            {
                return _data["wbea_id"];
            }

            set
            {
                _data["wbea_id"] = value;
            }
        }

        public SampleResult SampleResult
        {
            get
            {
                if (_relatedObjects["sample_result"] == null)
                {
                    _relatedObjects["sample_result"] = FetchSampleResult();
                }
                return (SampleResult)_relatedObjects["sample_result"];
            }
        }

        public string media_serial_number
        {
            get
            {
                return _data["media_serial_number"];
            }

            set
            {
                _data["media_serial_number"] = value;
            }
        }

        public string lab_sample_id
        {
            get
            {
                return _data["lab_sample_id"];
            }

            set
            {
                _data["lab_sample_id"] = value;
            }
        }

        public string DateReceivedFromLab
        {
            get
            {
                return _data["date_recieved_from_lab"].ToDateFormat();
            }
        }

        public string date_received_from_lab
        {
            get
            {
                return _data["date_recieved_from_lab"];
            }

            set
            {
                _data["date_recieved_from_lab"] = value;
            }
        }

        public string prepared_by
        {
            get
            {
                return _data["prepared_by"];
            }

            set
            {
                _data["prepared_by"] = value;
            }
        }

        public User PreparedBy
        {
            get
            {
                return LoadRelated<User>("prepared_by");
            }
        }

        public List<string> ChainOfCustodyIDs
        {
            get
            {
                if (_chainOfcustodyIDs.Count == 0)
                {
                    _chainOfcustodyIDs = FetchChainOfCustodyIDs();
                }

                //Note would copy this but its gonna go out of scope after page load anyways.
                return _chainOfcustodyIDs;
            }
        }

        public string average_storage_temperature
        {
            get
            {
                return _data["average_storage_temperature"];
            }

            set
            {
                _data["average_storage_temperature"] = value;
            }
        }

        public string average_storage_temperature_unit
        {
            get
            {
                return _data["average_storage_temperature_unit"];
            }
            set
            {
                _data["average_storage_temperature_unit"] = value;
            }
        }

        public Unit AverageStorageTemperatureUnit
        {
            get
            {
                return LoadRelated<Unit>("average_storage_temperature_unit");
            }
        }

        public string is_travel_blank
        {
            get
            {
                // nosal 12/7/2015 - Default is_travel_blank (if it's null) to false
                string retval = _data["is_travel_blank"];
                if (retval == null) retval = "false";
                return retval;
            }

            set
            {
                _data["is_travel_blank"] = value;
            }
        }

        public string is_orphaned_sample
        {
            get
            {
                // nosal 12/7/2015 - Default is_travel_blank (if it's null) to false
                return _data["is_orphaned_sample"];

            }

            set
            {
                _data["is_orphaned_sample"] = value;
            }
        }

        public string date_created
        {
            get
            {
                return _data["date_created"];
            }

            set
            {
                _data["date_created"] = value;
            }
        }


        #endregion

        public Sample() : base() { }

        public override void Validate()
        {
            using (ModelException e = new ModelException())
            {
                Sample duplicate = FetchByWBEAId(this.wbea_id);
                if (duplicate != null && duplicate.id != this.id) { e.AddError("wbea_id", "WBEA Sample ID of " + this.wbea_id + " is already in use by another sample."); }

                if (!e.AddError(wbea_id.CheckRequired("wbea_id", "WBEA Sample ID is a required field.")) &&
                    !e.AddError(wbea_id.CheckExactLength(9, "wbea_id", "WBEA Sample ID must be in the format of YYMMXXXXX.")))
                {
                    e.AddError(wbea_id.Substring(0, 2).CheckIfIntAndPositive("wbea_id", "WBEA Sample ID must contain the last two digits of the year in YYMMXXXXX."));
                    e.AddError(wbea_id.Substring(2, 2).CheckIfIntInRange(1, 12, "wbea_id", "WBEA Sample ID", "must contain a valid month in YYMMXXXXX."));
                    e.AddError(wbea_id.Substring(4).CheckIfIntInRange(0, 99999, "wbea_id", "WBEA Sample ID", "must valid digits in YYMMXXXXX."));
                }
                if (date_received_from_lab == null) date_received_from_lab = "InvalidDate";
                if (SampleType != SampleType.PAH && SampleType != SampleType.PASS)
                {
                    date_received_from_lab.CheckIfDateTime(e, "date_received_from_lab", "Received From Lab");
                }
                if (average_storage_temperature == null) average_storage_temperature = "NotANumber";
                average_storage_temperature.CheckIfDecimal(e, "average_storage_temperature");

                if (!average_storage_temperature.IsBlank())
                {
                    if (!e.AddError(average_storage_temperature_unit.CheckRequired("average_storage_temperature_unit")))
                    {
                        e.AddError(average_storage_temperature_unit.CheckIfInt("average_storage_temperature_unit"));
                    }
                }

                if (e.hasErrors)
                {
                    throw e;
                }
            }
        }

        private List<string> FetchChainOfCustodyIDs()
        {
            if (String.IsNullOrEmpty(sample_id))
            {
                return null;
            }

            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand dbfindChainOfCustodys = new SqlCommand(@"SELECT chain_of_custody_id FROM ChainOfCustodys_Samples WHERE sample_id = @sample_id ", connection))
                {
                    dbfindChainOfCustodys.Parameters.AddWithValue("sample_id", sample_id);
                    using (SqlDataReader reader = dbfindChainOfCustodys.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _chainOfcustodyIDs.Add(reader[0].ToString());
                        }
                    }
                }
            }

            return _chainOfcustodyIDs;
        }


        private SampleResult FetchSampleResult()
        {
            if (wbea_id.IsBlank())
            {
                return null;
            }

            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand findSampleResultId = new SqlCommand(@"SELECT sample_result_id 
                        FROM SampleResults 
                        WHERE wbea_id =  @wbea_id
                        ORDER by sample_result_id", connection))
                {

                    findSampleResultId.Parameters.AddWithValue("wbea_id", wbea_id);

                    int? sampleResultId = (int?)findSampleResultId.ExecuteScalar();
                    if (sampleResultId.HasValue)
                    {
                        return Load<SampleResult>(sampleResultId.ToString());
                    }
                }
            }

            return null;
        }
    }
}