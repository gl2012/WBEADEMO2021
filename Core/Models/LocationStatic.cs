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
    public partial class Location
    {
        public string getsuerid;
        public static Location Load(string locationID)
        {
            return BaseModel.Load<Location>(locationID);
        }

        public static List<Location> FetchPage(int pageNumber, int pageSize)
        {
            if (pageNumber < 0 || pageSize < 0)
            {
                return null;
            }

            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                List<Location> locationList = new List<Location>();
                using (SqlCommand loadLocationCommand = new SqlCommand(@"
                    SELECT location_id 
                    FROM (SELECT row_number() OVER (order by location_id) row, * 
                          FROM Locations) rowIndexs
                    WHERE row > @rowSetStart  
                    AND row <= @rowSetend", connection))
                {
                    loadLocationCommand.Parameters.AddWithValue("rowSetStart", ((pageNumber - 1) * pageSize));
                    loadLocationCommand.Parameters.AddWithValue("rowSetend", pageNumber * pageSize);

                    using (SqlDataReader dataReader = loadLocationCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            locationList.Add(Location.Load(dataReader["location_id"].ToString()));
                        }
                    }
                }

                return locationList;
            }
        }

        public static Location LoadByName(string whereName)
        {
            var locations = FetchAll<Location>("name = '" + whereName.Replace("'", "''") + "'");

            if (locations.Count == 0) return null;

            return locations[0];
        }

        public static List<Location> FetchAll()
        {
            return FetchAll<Location>(new { order = "name ASC" });
        }

        public static List<Location> FetchWithLogger()
        {
            return FetchAll<Location>(new
            {
                where = @"location_id IN 
                            (SELECT location_id FROM Items i
                            INNER JOIN Items_Parameters ip ON i.item_id = ip.item_id
                            INNER JOIN Parameters p ON ip.parameter_id = p.parameter_id
                            WHERE p.name = 'Data Logger') AND active = 1",
                order = "name ASC"
            });
        }

        public static List<Location> FetchAllActive()
        {
            return FetchAll<Location>(new { where = "Active = 1", order = "name ASC" });
        }

        public static List<Location> FetchAllActive(string whereClause)
        {
            return FetchAll<Location>(new { where = "Active = 1 AND " + whereClause, order = "name ASC" });
        }

        public static SelectList FetchSelectList(object selectedValue)
        {
            return new SelectList(FetchAll(), "location_id", "name", selectedValue);
        }

        public static SelectList FetchSelectListActive()
        {
            return FetchSelectListActive(null);
        }

        public static SelectList FetchSelectListActive(object selectedValue)
        {
            return BaseModelExtensions.SelectList("location_id", "name", FetchAllActive(), selectedValue);
        }

        public static SelectList FetchSelectListWithLogger(object selectedValue)
        {
            return BaseModelExtensions.SelectList("location_id", "name", FetchWithLogger(), selectedValue);
        }

        public static SelectList FetchSelectListCustom(object selectedValue)
        {
            return BaseModelExtensions.SelectList("location_id", "name", FetchAllCustom(), selectedValue);
        }
        public static List<Location> FetchAllCustom()
        {
            return FetchAll<Location>(new { where = "Active = 1 and location_id in (select location_id from User_Locations where user_id=118)", order = "name ASC" });
        }
        public static List<Location> FetchAllCustom(string whereClause)
        {
            return FetchAll<Location>(new { where = "Active = 1  AND " + whereClause, order = "name ASC" });
        }


    }
}