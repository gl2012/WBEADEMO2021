/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Collections.Generic;

namespace WBEADMS.Models
{
    public class Blackout : BaseModel
    {

        public Blackout() : base(_table_fields) { }

        #region static properties and private members

        /* the following is not used
        private const string _table_name = "BlackOuts";
        private const string _id_field = "blackout_id";
         */

        private static readonly string[] _table_fields = new string[] {
            "blackout_id",
            "station_id",
            "analyzer",
            "date_start",
            "date_end",
            "comment"
        };

        #endregion

        #region public properties

        public string blackout_id
        {
            get
            {
                return _data["blackout_id"];
            }

            set
            {
                _data["blackout_id"] = value;
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

        public string date_start
        {
            get
            {
                return _data["date_start"];
            }

            set
            {
                _data["date_start"] = value;
            }
        }

        public string date_end
        {
            get
            {
                return _data["date_end"];
            }

            set
            {
                _data["date_end"] = value;
            }
        }

        public string comment
        {
            get
            {
                return _data["comment"];
            }

            set
            {
                _data["comment"] = value;
            }
        }

        #endregion

        #region public static methods

        public static List<Blackout> FetchAll()
        {
            return FetchAll<Blackout>();
        }

        public static List<Blackout> FetchPage(int pageNumber, int pageSize)
        {
            return FetchPage<Blackout>(pageNumber, pageSize);
        }

        public static int TotalCount()
        {
            return TotalCount();
        }

        public static Blackout Load(string id)
        {
            var item = Load<Blackout>(id);

            return item;
        }

        // TODO: refactor the following method into a common method; possibly in BaseModelExtensions
        public static IEnumerable<string> FetchStations()
        {
            var list = new List<string>();
            using (var connection = new System.Data.SqlClient.SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                string sql = "SELECT DISTINCT station_id FROM BlackOuts";

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

                string sql = "SELECT DISTINCT analyzer FROM BlackOuts";

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
            using (ModelException e = new ModelException())
            {
                station_id.CheckRequired(e, "station_id", "Station");
                station_id.CheckMaxLength(e, 20, "station_id", "Station");

                analyzer.CheckRequired(e, "analyzer");
                analyzer.CheckMaxLength(e, 20, "analyzer");

                bool hasStartDateTime = !date_start.CheckRequired(e, "date_start", "Start Date") &&
                                        !date_start.CheckIfDateTime(e, "date_start", "Start Date");

                //Note: there is a bug in porc that if there is no end date, its likely a null exception would be thrown.
                //This 
                bool hasEndDateTime = !date_end.CheckRequired(e, "date_end", "End Date") &&
                                      !date_end.CheckIfDateTime(e, "date_end", "End Date");

                if (hasStartDateTime && hasEndDateTime)
                {
                    if (date_start.ToDateTime() >= date_end.ToDateTime())
                    {
                        e.AddError("date_end", "End Date must be greater then Start Date");
                    }
                }

                // TODO: add business logic validation
            }
        }
    }
}