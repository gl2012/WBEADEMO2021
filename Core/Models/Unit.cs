/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public class Unit : BaseModel
    {
        private static readonly string[] _table_fields = new string[] {
            "unit_id",
            "type",
            "name",
            "symbol"
        };

        public Unit() : base(_table_fields) { }

        #region properties
        public string unit_id
        {
            get
            {
                return _data["unit_id"];
            }
        }

        public string type
        {
            get
            {
                return _data["type"];
            }
            set
            {
                _data["type"] = value;
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

        public string symbol
        {
            get
            {
                return _data["symbol"];
            }
            set
            {
                _data["symbol"] = value;
            }
        }
        #endregion

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                if (!errors.AddError(type.CheckRequired("type")))
                {
                    errors.AddError(type.CheckMaxLength(50, "type"));
                }

                if (!errors.AddError(name.CheckRequired("name")))
                {
                    errors.AddError(name.CheckMaxLength(50, "name"));
                }

                if (!errors.AddError(symbol.CheckRequired("symbol")))
                {
                    errors.AddError(symbol.CheckMaxLength(10, "symbol"));
                }
            }
        }

        public override string ToString()
        {
            return this.symbol;
        }

        #region Static Methods
        public static Unit Load(string unit_id)
        {
            return BaseModel.Load<Unit>(unit_id);
        }

        public static SelectList FetchTypeSelectList()
        {
            return FetchTypeSelectList(null);
        }

        public static SelectList FetchTypeSelectList(object defalut)
        {
            Dictionary<string, string> types = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT type FROM Units", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string typeName = reader[0].ToString();
                            types.Add(typeName, typeName);
                        }
                    }
                }
            }
            return new SelectList(types, "key", "value", defalut);
        }

        public static SelectList FetchSelectList(string type, object defalut)
        {
            Dictionary<string, string> units = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT unit_id, symbol FROM Units where type = @type", connection))
                {
                    command.Parameters.AddWithValue("type", type);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            units.Add(reader["unit_id"].ToString(), reader["symbol"].ToString());
                        }
                    }
                }
            }
            return new SelectList(units, "key", "value", defalut);
        }
        #endregion
    }
}