/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Collections.Generic;

namespace WBEADMS.Models
{
    public class SlopeOffset : BaseModel
    {

        public SlopeOffset() : base() { }

        #region public properties

        public string slope_offset_id
        {
            get
            {
                return _data["slope_offset_id"];
            }

            set
            {
                _data["slope_offset_id"] = value;
            }
        }

        public string station_id
        {
            get
            {
                return _data["station_id"];
            }

            set
            {
                _data["station_id"] = value;
            }
        }

        public string analyzer
        {
            get
            {
                return _data["analyzer"];
            }

            set
            {
                _data["analyzer"] = value;
            }
        }

        public string date_active
        {
            get
            {
                return _data["date_active"];
            }

            set
            {
                _data["date_active"] = value;
            }
        }

        public string slope
        {
            get
            {
                return _data["slope"];
            }

            set
            {
                _data["slope"] = value.ToString();
            }
        }

        public string offset
        {
            get
            {
                return _data["offset"];
            }

            set
            {
                _data["offset"] = value.ToString();
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

        public User CreatedBy
        {
            get
            {
                return LoadRelated<User>("created_by");
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

        #endregion

        #region public static methods

        public static List<SlopeOffset> FetchAll()
        {
            return FetchAll<SlopeOffset>();
        }

        public static List<SlopeOffset> FetchPage(int pageNumber, int pageSize)
        {
            return FetchPage<SlopeOffset>(pageNumber, pageSize);
        }

        public static int TotalCount()
        {
            return TotalCount();
        }

        public static SlopeOffset Load(string id)
        {
            var item = Load<SlopeOffset>(id);

            return item;
        }

        // TODO: refactor the following method into a common method; possibly in BaseModelExtensions
        public static IEnumerable<string> FetchStations()
        {
            var list = new List<string>();
            using (var connection = new System.Data.SqlClient.SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                string sql = "SELECT DISTINCT station_id FROM SlopeOffsets";

                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, connection))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(reader[0].ToString());
                    }
                }
            }

            return list;
        }

        // TODO: refactor the following method into a common method; possibly in BaseModelExtensions
        public static IEnumerable<string> FetchAnalyzers()
        {
            var list = new List<string>();
            using (var connection = new System.Data.SqlClient.SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                string sql = "SELECT DISTINCT analyzer FROM SlopeOffsets";

                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, connection))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(reader[0].ToString());
                    }
                }
            }

            return list;
        }
        #endregion

        public override void Validate()
        {
            using (var e = new ModelException())
            {
                e.AddError(_data["station_id"].CheckRequired("station_id"));
                e.AddError(_data["station_id"].CheckMaxLength(20, "station_id"));

                e.AddError(_data["analyzer"].CheckRequired("analyzer"));
                e.AddError(_data["analyzer"].CheckMaxLength(20, "analyzer"));

                e.AddError(_data["date_active"].CheckRequired("date_active", "Active Date"));
                e.AddError(_data["date_active"].CheckIfDateTime("date_active", "Active Date"));

                e.AddError(_data["slope"].CheckRequired("slope"));
                e.AddError(_data["slope"].CheckIfDecimal("slope"));

                e.AddError(_data["offset"].CheckRequired("offset"));
                e.AddError(_data["offset"].CheckIfDecimal("offset"));

                // TODO: add business logic validation (i.e. date_active/slope/offset validation)
            }
        }
    }
}