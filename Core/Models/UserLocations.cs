using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public class UserLocations : BaseModel
    {


        public UserLocations() : base("User_locations", "id", new string[] { "id", "user_id", "location_id", "Date_modified" }) { }
        public UserLocations(string roleID) : this()
        {
            Id = roleID;
            LoadData();
        }


        #region Properties
        public string Id
        {
            get
            {
                return _data["id"];
            }
            set { _data["id"] = value; }
        }
        public string User_id
        {
            get
            {
                return _data["user_id"];
            }
            set { _data["user_id"] = value; }
        }

        public string location_id
        {
            get
            {
                return _data["location_id"];
            }
            set { _data["location_id"] = value; }
        }





        public string Date_modified
        {
            get
            {
                return _data["date_modified"];
            }
            set
            {
                _data["date_modified"] = value;
            }
        }
        public override void Save()
        {
            if (String.IsNullOrEmpty(Id))
            {
                // create new location
                // Validate();
                SaveNew();
            }
            else
            {
                // Save Edits.
                // Validate();
                SaveEdits();
            }
        }

        #endregion

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public static void DeleteUserLocations(string UserId)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(@"DELETE from  user_locations where user_id=" + UserId, connection);
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
            }
        }
        public static SelectList FetchuserUserLocationlist(string Userid, object selectedValue)
        {
            List<SelectListItem> newList = new List<SelectListItem>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))

            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"select * from User_locations join locations on User_locations.location_id = locations.location_id  where User_locations.user_id = " + Userid, connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            SelectListItem selListItem = new SelectListItem() { Value = dataReader["location_id"].ToString(), Text = (string)dataReader["name"] };
                            newList.Add(selListItem);
                        }
                    }
                }

                connection.Close();
            }
            return new SelectList(newList, "Value", "Text", selectedValue);
            // return BaseModelExtensions.SelectList("location_id", "name", userSampleTypeList, selectedValue);
        }
        public static List<string> FetchUserLocationCustom(string userid)
        {
            List<string> clone = new List<string>();

            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"select * from User_lcations join locations on User_locations.location_id=locations.location_id  where User_locations.user_id=" + userid, connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            clone.Add((string)dataReader["name"]);

                        }
                    }
                }
                connection.Close();
            }



            return clone;
        }




    }
}
