/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public class SampleType : BaseModel
    {
        private static readonly Dictionary<string, SampleType> _SampleTypeLookUp;
        private static readonly Dictionary<string, string> _SampleTypeLookUpById;
        private static readonly List<SampleType> _sampleTypeList;
        private static readonly List<string> _sampleTypeListCustom;
        private int _sample_type_id;
        private string _name;

        #region Static Properties
        public static SampleType RSC
        {
            get
            {
                return new SampleType(_SampleTypeLookUp["RSC"]);
            }
        }
        public static SampleType PM25
        {
            get
            {
                return new SampleType(_SampleTypeLookUp["PM2.5"]);
            }
        }

        public static SampleType PM10E
        {
            get
            {
                return new SampleType(_SampleTypeLookUp["PM10E"]);
            }
        }

        public static SampleType PRECIP
        {
            get
            {
                return new SampleType(_SampleTypeLookUp["PRECIP"]);
            }
        }

        public static SampleType VOC
        {
            get
            {
                return new SampleType(_SampleTypeLookUp["VOC"]);
            }
        }

        public static SampleType PM10
        {
            get
            {
                return new SampleType(_SampleTypeLookUp["PM10"]);
            }
        }

        public static SampleType ADS
        {
            get
            {
                return new SampleType(_SampleTypeLookUp["ADS"]);
            }
        }

        public static SampleType PAH
        {
            get
            {
                return new SampleType(_SampleTypeLookUp["PAH"]);
            }
        }

        public static SampleType PASS
        {
            get
            {

                return new SampleType(_SampleTypeLookUp["PASS"]);

            }
        }
        #endregion

        #region properties
        public string name
        {
            get
            {
                return _name;
            }
        }

        public int sample_type_id
        {
            get
            {
                return _sample_type_id;
            }
        }

        public Parameter Parameter
        {
            get
            {
                if (_relatedObjects["parameter_id"] == null)
                {
                    _relatedObjects["parameter_id"] = BaseModel.Fetch<Parameter>("name", name);
                }

                return (Parameter)_relatedObjects["parameter_id"];
            }
        }
        #endregion

        #region Constructors
        static SampleType()
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                _sampleTypeList = new List<SampleType>();
                _SampleTypeLookUp = new Dictionary<string, SampleType>();
                _SampleTypeLookUpById = new Dictionary<string, string>();
                _sampleTypeListCustom = new List<string>();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"SELECT * FROM Sampletypes ", connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            SampleType sampleType = new SampleType((int)dataReader["sample_type_id"], (string)dataReader["name"]);
                            _sampleTypeList.Add(sampleType);
                            _SampleTypeLookUp.Add(sampleType.name, sampleType);
                            _SampleTypeLookUpById.Add(sampleType.id, sampleType.name);
                        }
                    }
                }


            }
        }
        //chnaged from priave to public on March 4,2020
        //  public  SampleType() : base() { }
        public SampleType() : base("SampleTypes", "sample_type_id", new string[] { "sample_type_id", "name" })
        {
        }

        //Clone constructor.
        private SampleType(SampleType sampleType) : this(sampleType.sample_type_id, sampleType.name)
        {
        }

        private SampleType(int sample_type_id, string name) : this()
        {
            _sample_type_id = sample_type_id;
            _name = name;
            // NOTE: this is a workaround so that the basemodels data array gets the primary id so that the comparion code works properly.
            _data["sample_type_id"] = _sample_type_id.ToString();
        }

        public SampleType(int sample_type_id) : this()
        {
            _sample_type_id = sample_type_id;
            _name = _SampleTypeLookUpById[_sample_type_id.ToString()];
            // NOTE: this is a workaround so that the basemodels data array gets the primary id so that the comparion code works properly.
            _data["sample_type_id"] = _sample_type_id.ToString();
            _data["name"] = _name;
        }

        public SampleType(string id) : this(Int32.Parse(id)) { }

        #endregion

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        #region Static Methods
        public static SampleType Load(int id)
        {
            return new SampleType(id);
        }

        public static SampleType Load(string id)
        {
            if (id.IsBlank())
            {
                return null;
            }
            else
            {
                return new SampleType(id);
            }
        }

        public static SampleType LoadByName(string sampleTypeName)
        {
            sampleTypeName = sampleTypeName.Trim().ToUpper();
            switch (sampleTypeName)
            {
                case "PM2.5":
                case "PM10E":
                case "PRECIP":
                case "VOC":
                case "PM10":
                case "ADS":
                case "PAH":
                case "PASS":
                    return new SampleType(_SampleTypeLookUp[sampleTypeName]);
                default:
                    return null;
            }
        }

        public static List<SampleType> FetchAll()
        {
            List<SampleType> clone = new List<SampleType>();
            foreach (SampleType sampleType in _sampleTypeList)
            {
                clone.Add(new SampleType(sampleType.sample_type_id, sampleType.name));
            }
            return clone;
        }
        public static List<string> FetchCustom()
        {
            List<string> clone = new List<string>();
            foreach (var sampleType in _sampleTypeListCustom)
            {
                clone.Add(sampleType);
            }
            return clone;
        }
        public static Dictionary<string, string> FetchDictionaryByName()
        {
            return FetchDictionary<SampleType>("name");
        }

        public static SelectList FetchSelectListName()
        {
            return FetchSelectList("name", null);
        }

        public static SelectList FetchSelectListName(object defaultValue)
        {
            return FetchSelectList("name", defaultValue);
        }

        private static SelectList FetchSelectList(string field, object defaultValue)
        {
            return new SelectList(FetchDictionary<SampleType>(field), "key", "value", defaultValue);
        }


        #endregion

        public override string ToString()
        {
            return name;
        }
    }
}