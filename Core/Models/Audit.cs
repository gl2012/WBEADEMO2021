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
    public class Audit : BaseModel
    {

        public Audit()
            : base(new string[] {
            "audit_id",
            "type",
            "id",
            "user_id",
            "date_modified",
            "field",
            "original_value",
            "new_value"})
        { }

        #region Properties
        public string type
        {
            get
            {
                return _data["type"];
            }
        }

        public string id
        {
            get
            {
                return _data["id"];
            }
        }

        public string user_id
        {
            get
            {
                return _data["user_id"];
            }
        }

        public User User
        {
            get
            {
                return User.Load(user_id);
            }
        }

        public DateTime DateModified
        {
            get
            {
                return DateTime.Parse(date_modified);
            }
        }

        public string date_modified
        {
            get
            {
                return _data["date_modified"];
            }
        }

        public string field
        {
            get
            {
                return _data["field"];
            }
        }

        public string original_value
        {
            get
            {
                return _data["original_value"];
            }
        }

        public string new_value
        {
            get
            {
                return _data["new_value"];
            }
        }
        #endregion

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public static SelectList FetchTypeSelectList(object selectedValue)
        {
            Dictionary<string, string> types = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT type FROM Audits", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string typeName = reader[0].ToString();
                            types.Add(typeName, typeName.ToTitleCase());
                        }
                    }
                }
            }
            return new SelectList(types, "key", "value", selectedValue);
        }

        public static SelectList FetchFieldSelectList(object selectedValue)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT field FROM Audits", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fieldName = reader[0].ToString();
                            fields.Add(fieldName, fieldName.ToTitleCase());
                        }
                    }
                }
            }
            return new SelectList(fields, "key", "value", selectedValue);
        }
    }
}