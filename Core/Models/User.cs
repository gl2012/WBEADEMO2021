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

    public partial class User : BaseModel
    {

        private Role userRole = null;

        #region properties
        public string user_id { get { return _data["user_id"]; } set { _data["user_id"] = value; } }
        public string user_name { get { return _data["user_name"]; } set { _data["user_name"] = value; } }
        public string password { get { return _data["password"]; } set { Authentication.SetPassword(this, password); } }
        public string salt { get { return _data["salt"]; } }
        public string display_name { get { return first_name + " " + last_name; } }
        public string first_name { get { return _data["first_name"]; } set { _data["first_name"] = value; } }
        public string last_name { get { return _data["last_name"]; } set { _data["last_name"] = value; } }
        public string email { get { return _data["email"]; } set { _data["email"] = value; } }
        public string phone_number { get { return _data["phone_number"]; } set { _data["phone_number"] = value; } }
        public string role_id { get { return _data["role_id"]; } set { _data["role_id"] = value; } }
        public string comments { get { return _data["comments"]; } set { _data["comments"] = value; } }
        public string technician_code { get { return _data["technician_code"]; } set { _data["technician_code"] = value; } }
        public string active { get { return _data["active"]; } set { _data["active"] = value; } }
        public string date_created { get { return _data["date_created"].ToDateTimeFormat(); } set { _data["date_created"] = value.ToDateTimeFormat(); } }

        public Role Role
        {
            get
            {
                if (userRole == null && !role_id.IsBlank())
                {
                    userRole = new Role(role_id);
                }

                return userRole;
            }
        }
        #endregion

        public User() : base() { }

        public User(string id) : this()
        {
            user_id = id;
            LoadData();
        }

        public override void Validate()
        {

            using (ModelException errors = new ModelException())
            {

                errors.AddError(user_name.CheckRequired("user_name"));
                errors.AddError(user_name.CheckMaxLength(50, "user_name"));

                errors.AddError(password.CheckRequired("password"));
                errors.AddError(password.CheckMaxLength(50, "password"));

                errors.AddError(first_name.CheckRequired("first_name"));
                errors.AddError(first_name.CheckMaxLength(50, "first_name"));
                errors.AddError(last_name.CheckRequired("last_name"));
                errors.AddError(last_name.CheckMaxLength(50, "last_name"));

                errors.AddError(email.CheckRequired("email"));
                errors.AddError(email.CheckMaxLength(50, "email"));
                errors.AddError(email.CheckValidEmail("email"));

                errors.AddError(phone_number.CheckMaxLength(50, "phone_number"));

                errors.AddError(role_id.CheckRequired("role_id"));
                errors.AddError(role_id.CheckIfInt("role_id"));

                errors.AddError(technician_code.CheckMaxLength(50, "technician_code"));

                errors.AddError(active.CheckRequired("active"));
                errors.AddError(active.CheckIfBool("active"));

                errors.AddError(date_created.CheckRequired("date_created"));
                errors.AddError(date_created.CheckIfDateTime("date_created"));
            }
        }

        public override void Save()
        {
            if (String.IsNullOrEmpty(user_id))
            {
                date_created = DateTime.Now.ToString();
                string clearPassword = Authentication.SetNewPassword(this); // sets the salt & password hash for the user
                Validate();
                SaveNew();
                Authentication.SendNewPassword(this, clearPassword);
            }
            else
            {
                // Save Edits.
                Validate();
                SaveEdits();
            }
        }

        /// <summary>Sets the Salt and Password fields with a salt and hash. 
        /// The password property automatically calls this method via Authentication.SetPassword().</summary>
        public void SetSaltAndHash(string salt, string hash)
        {
            _data["salt"] = salt;
            _data["password"] = hash;
        }

        public override string ToString()
        {
            return display_name;
        }
        public static Dictionary<int, string> fetchSampleType()
        {
            var SampleTypeList = new Dictionary<int, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"SELECT * FROM Sampletypes ", connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            SampleTypeList.Add((int)dataReader["sample_Type_id"], (string)dataReader["name"]);

                        }
                    }
                }
                connection.Close();
            }
            return SampleTypeList;

        }
        public static Dictionary<int, string> fetchLocationType()
        {
            var LocationTypeList = new Dictionary<int, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"SELECT * FROM locations order by name", connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            LocationTypeList.Add((int)dataReader["Location_id"], (string)dataReader["name"]);

                        }
                    }
                }
                connection.Close();
            }
            return LocationTypeList;

        }
        public static List<string> FetchuserSampletypelist(string userid)
        {

            return BaseModel.FetchList("SELECT * FROM user_Sampletypes where user_id=" + userid, "sample_Type_id");
        }
        public static List<string> Fetchuserlocationlist(string userid)
        {


            return BaseModel.FetchList("SELECT * FROM user_locations where user_id=" + userid, "location_id");
        }

        public static List<string> FetchuserlocationlistInactive()
        {


            return BaseModel.FetchList(@"SELECT * FROM locations where active=0", "location_id");
        }


        public static Dictionary<string, string> fetchCurrentUserSampleType()
        {
            var SampleTypeList = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"SELECT * FROM Sampletypes ", connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            SampleTypeList.Add((string)dataReader["id"], (string)dataReader["sample_Type_id"]);

                        }
                    }
                }
                connection.Close();
            }
            return SampleTypeList;

        }

    }
}