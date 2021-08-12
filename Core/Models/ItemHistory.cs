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
    public class ItemHistory : BaseModel
    {
        public ItemHistory() : base(new string[] {
            "item_history_id",
            "item_id",
            "location_id",
            "date_installed",
            "date_removed"})
        { }

        #region Properties
        public Item Item
        {
            get
            {
                if (String.IsNullOrEmpty(_data["item_id"]))
                {
                    return null;
                }
                else
                {
                    return WBEADMS.Models.Item.Load(_data["item_id"]);
                }
            }
        }

        public Location Location
        {
            get
            {
                if (String.IsNullOrEmpty(_data["location_id"]))
                {
                    return null;
                }
                else
                {
                    return WBEADMS.Models.Location.Load(_data["location_id"]);
                }
            }
        }

        public string DateInstalled
        {
            get
            {
                return _data["date_installed"].ToDateFormat();
            }
        }

        public string DateRemoved
        {
            get
            {
                return _data["date_removed"].ToDateFormat();
            }
        }
        #endregion

        public static List<Item> GetItemsByLocation(string location_id)
        {
            var location = Location.Load(location_id);
            if (location == null) { throw new ArgumentException("Location with " + location_id + " does not exist!"); }

            var hists = new List<ItemHistory>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                string sql = "SELECT item_history_id FROM ItemHistorys WHERE location_id = @location_id AND date_removed IS NULL ORDER BY date_installed DESC";
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    selectCommand.Parameters.Add(new SqlParameter("location_id", location_id));

                    using (SqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            hists.Add(BaseModel.Load<ItemHistory>(dataReader["item_history_id"].ToString()));
                        }
                    }
                }
            }

            var list = new List<Item>();
            foreach (var hist in hists)
            {
                list.Add(hist.Item);
            }

            return list;
        }

        public static ItemHistory GetMostRecentHistory(string item_id)
        {
            if (item_id.IsBlank()) { throw new ArgumentException("item_id is blank"); }

            var ih = new ItemHistory();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                string sql = "SELECT TOP 1 item_history_id FROM ItemHistorys WHERE item_id = @item_id AND date_removed IS NULL ORDER BY date_installed DESC";
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    selectCommand.Parameters.Add(new SqlParameter("item_id", item_id));

                    using (SqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            ih = BaseModel.Load<ItemHistory>(dataReader["item_history_id"].ToString());
                        }
                    }
                }
            }

            return ih;
        }

        public static List<ItemHistory> GetItemHistorys(string item_id)
        { // NOTE: this is kept in ItemHistory in case you don't care to load Item
            if (item_id.IsBlank()) { throw new ArgumentException("item_id is blank"); }

            var list = new List<ItemHistory>();
            var item = Item.Load(item_id);

            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                string sql = "SELECT item_history_id FROM ItemHistorys WHERE item_id = @item_id ORDER BY date_installed ASC";
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    selectCommand.Parameters.Add(new SqlParameter("item_id", item_id));

                    using (SqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            list.Add(BaseModel.Load<ItemHistory>(dataReader["item_history_id"].ToString()));
                        }
                    }
                }
            }

            return list;
        }

        public static void RelocateItem(Item item, string currentLocationID, string newLocationID, DateTime date_moved)
        { // TODO: this is only called from Item.Relocate; perhaps ItemHistory should be part of Item?  or ItemHistory should be deriving from BasesModel?
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                string fetchsql = "SELECT date_installed FROM ItemHistorys WHERE item_id = @item_id AND location_id = @location_id AND date_removed IS NULL;";
                using (SqlCommand fetchCommand = new SqlCommand(fetchsql, connection))
                {
                    fetchCommand.Parameters.Add(new SqlParameter("item_id", item.id));
                    fetchCommand.Parameters.Add(new SqlParameter("location_id", currentLocationID));
                    fetchCommand.Parameters.Add(new SqlParameter("date_removed", date_moved));
                    var date_installed = (DateTime?)fetchCommand.ExecuteScalar();
                    if (date_installed.HasValue && date_installed.Value > date_moved)
                    {
                        throw new ArgumentException("date_moved cannot be earlier than date_installed", "date_moved");
                    }
                }


                string updatesql = "UPDATE ItemHistorys SET date_removed = @date_removed WHERE item_id = @item_id AND location_id = @location_id AND date_removed IS NULL;";
                using (SqlCommand updateCommand = new SqlCommand(updatesql, connection))
                {
                    updateCommand.Parameters.Add(new SqlParameter("item_id", item.id));
                    updateCommand.Parameters.Add(new SqlParameter("location_id", currentLocationID));
                    updateCommand.Parameters.Add(new SqlParameter("date_removed", date_moved));
                    updateCommand.ExecuteNonQuery();
                }

                string insertsql = "INSERT INTO ItemHistorys (item_id, location_id, date_installed) VALUES (@item_id, @location_id, @date_installed);";
                using (SqlCommand insertCommand = new SqlCommand(insertsql, connection))
                {
                    insertCommand.Parameters.Add(new SqlParameter("item_id", item.id));
                    insertCommand.Parameters.Add(new SqlParameter("location_id", newLocationID));
                    insertCommand.Parameters.Add(new SqlParameter("date_installed", date_moved));
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public override void Validate()
        {
            //do nothing
        }

        public override void Save()
        {
            //do nothing
        }
    }
}