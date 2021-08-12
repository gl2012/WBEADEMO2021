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
    public partial class ChainOfCustody : BaseModel
    {
        #region properties and private members
        private const string _table_name = "ChainOfCustodys";
        private const string _id_field = "chain_of_custody_id";

        #endregion

        public static ChainOfCustody Load(string id)
        {
            return Load<ChainOfCustody>(id);
        }

        // TODO: consider moving this method to Schedule.cs instead and be parameterless; even tho it is querying CoC table, it really has to do with the Schedule model
        public static DateTime FetchDateLastScheduled(Schedule schedule)
        {
            object coc_id;
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                // select the id of the latest scheduled or deployed coc for a schedule
                string sql =
                    @"SELECT TOP 1 chain_of_custody_id 
                        FROM ChainOfCustodys 
                        WHERE schedule_id = @schedule_id 
                          AND (date_sample_start IS NOT NULL OR date_sampling_scheduled IS NOT NULL) 
                        ORDER BY 
                            CASE WHEN date_sampling_scheduled IS NULL 
                                    THEN date_sample_start 
                                 WHEN date_sample_start IS NULL 
                                    THEN date_sampling_scheduled 
                                 WHEN date_sample_start > date_sampling_scheduled 
                                    THEN date_sample_start 
                                    ELSE date_sampling_scheduled 
                            END DESC";
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    selectCommand.Parameters.AddWithValue("schedule_id", schedule.id);

                    coc_id = selectCommand.ExecuteScalar();
                }
            }

            if (coc_id == null) { return DateTime.MinValue; } // return epoch if the schedule has never been run

            var coc = ChainOfCustody.Load(coc_id.ToString());
            DateTime coc_scheduled;
            DateTime coc_start;
            DateTime.TryParse(coc.Preparation.date_sampling_scheduled, out coc_scheduled);
            DateTime.TryParse(coc.Deployment.date_sample_start, out coc_start);
            if (coc_scheduled > coc_start)
            {
                return coc_scheduled;
            }
            else
            {
                return coc_start;
            }
        }

        /// <summary>
        /// Get all ChainOfCustody, filtering by date_sample_start and optionally by location and/or sampletype
        /// </summary>
        public static List<ChainOfCustody> FetchScheduled(DateTime startDate, DateTime endDate, string location_id, string sample_type_id)
        {
            var whereClauseList = new List<string>();
            whereClauseList.Add(
                "((date_sample_start IS NOT NULL AND " +
                "date_sample_start >= '" + startDate.ToISODate() + "' AND " +
                "date_sample_start <= '" + endDate.ToISODate() + "') OR " +
                "(date_sample_start IS NULL AND " +
                "date_sampling_scheduled >= '" + startDate.ToISODate() + "' AND " +
                "date_sampling_scheduled <= '" + endDate.ToISODate() + "'))");

            // build a list of schedule Ids to filter CoCs by
            if (!location_id.IsBlank())
            {
                whereClauseList.Add("location_id = " + location_id);
            }

            // find sampleType to filter CoCs by
            if (sample_type_id == "XPASS")
            {
                whereClauseList.Add("sample_type_id != " + SampleType.PASS.id);
            }
            else if (!sample_type_id.IsBlank())
            {
                whereClauseList.Add("sample_type_id = " + sample_type_id);
            }

            // create Where clause
            string whereClause = (whereClauseList.Count > 0)
                ? String.Join(" AND ", whereClauseList.ToArray())
                : null;

            // get datasheet info
            var cocs = BaseModel.FetchAll<ChainOfCustody>(whereClause);

            return cocs;
        }

        /// <summary>Takes in a wbeaId and tires to find the related CoC id, return null if not found</summary>
        public static string FindCoCIdWithWbeaId(string wbeaId)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                // select the id of the latest scheduled or deployed coc for a schedule
                string sql =
                    @"SELECT ChainOfCustodys_Samples.chain_of_custody_id 
                      FROM Samples
                      INNER JOIN ChainOfCustodys_Samples ON Samples.sample_id = ChainOfCustodys_Samples.sample_id
                      WHERE wbea_id = @wbea_id";
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    selectCommand.Parameters.AddWithValue("wbea_id", wbeaId);

                    int? coc_id = (int?)selectCommand.ExecuteScalar();
                    if (coc_id.HasValue)
                    {
                        return coc_id.ToString();
                    }

                    return null;
                }
            }
        }

        public static string[] FetchRecentWaybill()
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                string sql =
                    @"SELECT DISTINCT TOP 10 waybill_number
                      FROM ChainOfCustodys
                      WHERE waybill_number IS NOT NULL
                      ORDER BY waybill_number ASC";
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        var list = new List<string>();
                        while (reader.Read())
                        {
                            list.Add(reader[0].ToString());
                        }
                        return list.ToArray();
                    }
                }
            }
        }

        public static bool jQueryCheckBoxEnabled()
        {
            return false;
        }
    }
}