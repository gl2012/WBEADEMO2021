
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public partial class User
    {
        public static int UsersCountByRole(string roleID)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand dbUserCount = new SqlCommand(@"SELECT Count(*) FROM Users Where role_id = @role_id", connection))
                {
                    dbUserCount.Parameters.AddWithValue("role_id", roleID);
                    return (int)dbUserCount.ExecuteScalar();
                }
            }
        }

        public static List<User> FetchPage(int pageNumber, int pageSize)
        {
            if (pageNumber < 0 || pageSize < 0)
            {
                return null;
            }

            List<User> scheduleList = new List<User>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand loadSchedules = new SqlCommand(@"
                    SELECT user_id 
                    FROM (SELECT row_number() OVER (order by date_created) row, * 
                          FROM Users) rowIndexs
                    WHERE row > @rowSetStart  
                    AND row <= @rowSetend", connection))
                {
                    loadSchedules.Parameters.AddWithValue("rowSetStart", ((pageNumber - 1) * pageSize));
                    loadSchedules.Parameters.AddWithValue("rowSetend", pageNumber * pageSize);

                    using (SqlDataReader dataReader = loadSchedules.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            scheduleList.Add(User.Load(dataReader["user_id"].ToString()));
                        }
                    }
                }
            }

            return scheduleList;
        }

        public static User FetchByName(string user_name)
        {
            return Fetch<User>("user_name", user_name);
        }

        public static List<User> FetchByRole(string role_id)
        {
            if (role_id.IsInt())
            {
                return FetchAll<User>("role_id = " + role_id);
            }
            else
            {
                return new List<User>();
            }
        }

        public static SelectList FetchSelectListActive()
        {
            return FetchSelectListActive(null);
        }

        public static SelectList FetchSelectListActive(object selectedValue)
        {
            return BaseModelExtensions.SelectList<User>("user_id", "display_name", FetchAllActive(), selectedValue);
        }
        public static SelectList FetchSelectListUser()
        {
            return new SelectList(FetchAll<User>(), "user_id", "display_name");
        }
        public static SelectList FetchSelectList(object selectedValue)
        {
            return BaseModelExtensions.SelectList<User>("user_id", "display_name", FetchAll<User>(new { order = "first_name ASC, last_name ASC" }), selectedValue);
        }

        public static List<User> FetchAllActive()
        {
            return FetchAll<User>(new { where = "Active = 1", order = "first_name ASC, last_name ASC" });
        }


        public static User Load(string id)
        {
            try
            {
                return new User(id);
            }
            catch (ArgumentException)
            {
                // return null if there was a problem loading the location.
                return null;
            }
        }
    }
}