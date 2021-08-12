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
    public partial class Note : BaseModel
    {
        #region private static members
        ////private static readonly string _table_name = "Notes";
        ////private static readonly string _id_field = "note_id";

        private static readonly string[] _table_fields = new string[] {
            "note_id",
            "location_id",
            "date_occurred",
            "body",
            "is_starred",
            "is_committed",
            "is_deleted",
            "created_by",
            "date_created",
            "modified_by",
            "date_modified",
            "parent_type_id",
            "dsc_tag"
        };

        private static readonly string[] _required_fields = new string[] {
            "location_id",
            "date_occurred",
            "body",
            "is_starred",
            "is_committed",
            "is_deleted"
        };

        public enum ParentType
        {
            None = 0, Note, Item, Schedule, ChainOfCustody, Sample, Location = 100
        }
        #endregion

        #region list & pagination

        public static List<Note> FetchPage(int pageNumber, int pageSize)
        {
            return FetchPage<Note>(pageNumber, pageSize);
        }

        public static List<Note> FetchPage(int pageNumber, int pageSize, object clauses)
        {
            return FetchPage<Note>(pageNumber, pageSize, clauses);
        }

        public static int TotalCount()
        {
            return TotalCount<Note>(null /* whereClause */);
        }
        #endregion

        public static Note Load(string id)
        {
            return Load<Note>(id);
        }

        public static void CommitNotes()
        {

            string sql = "UPDATE Notes SET is_committed = 1 WHERE is_committed = 0 AND ABS(DATEDIFF(d, date_created, getdate())) >= 1";
            using (var connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (var updateCommand = new SqlCommand(sql, connection))
                {
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public static Dictionary<string, DateTime> GetLatestDailySystemChecks()
        {
            var list = new Dictionary<string, DateTime>();
            string sql = "SELECT location_id, MAX(date_occurred) AS date_occurred FROM Notes WHERE dsc_tag IS NOT NULL GROUP BY location_id";
            using (var connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                using (var selectCommand = new SqlCommand(sql, connection))
                {
                    using (var dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            list.Add(dataReader["location_id"].ToString(), DateTime.Parse(dataReader["date_occurred"].ToString()));
                        }
                    }
                }
            }

            return list;
        }
    }
}