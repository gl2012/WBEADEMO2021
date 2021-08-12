

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public class UserSampleType : BaseModel
    {


        public UserSampleType() : base("User_SampleTypes", "id", new string[] { "id", "user_id", "sample_type_id", "Date_modified" }) { }
        public UserSampleType(string roleID) : this()
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

        public string Sample_type_id
        {
            get
            {
                return _data["sample_type_id"];
            }
            set { _data["sample_type_id"] = value; }
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

        public static void DeleteUserSampleType(string UserId)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(@"DELETE from  user_Sampletypes where user_id=" + UserId, connection);
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
            }
        }
        public static Dictionary<string, string> FetchuserSampletypelist(int intUserid)
        {
            Dictionary<string, string> userSampleTypeList = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))

            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"SELECT * FROM user_Sampletypes where user_id=" + intUserid, connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            userSampleTypeList.Add((string)dataReader["sample_Type_id"], (string)dataReader["name"]);

                        }
                    }
                }

                return userSampleTypeList;
            }

        }
        public static List<string> FetchCustom(string userid)
        {
            List<string> clone = new List<string>();

            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"select * from User_SampleTypes join sampletypes on User_SampleTypes.sample_type_id=SampleTypes.sample_type_id  where User_SampleTypes.user_id=" + userid, connection))
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
