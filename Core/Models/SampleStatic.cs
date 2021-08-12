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
    partial class Sample
    {
        public const int OldCoCByDate = 21;

        public static Sample Load(string id)
        {
            return BaseModel.Load<Sample>(id);
        }

        public static Sample FetchByWBEAId(string wbea_id)
        {
            return Fetch<Sample>("wbea_id", wbea_id);
        }

        public static List<Sample> FetchAll()
        {
            return FetchAll<Sample>();
        }

        public static List<Sample> FetchAll(string whereClause)
        {
            return FetchAll<Sample>(new { where = whereClause, order = "wbea_id DESC" });
        }

        public static List<Sample> FetchAllUnassigned()
        {
            return FetchAll("sample_id Not IN(select sample_id from ChainOfCustodys_Samples) AND is_travel_blank = 0");
        }

        public static List<Sample> FetchAllUnassigned(string sampleTypeID)
        {

            if (sampleTypeID.IsBlank())
            {
                return FetchAllUnassigned();
            }

            return FetchAll("sample_id Not IN(select sample_id from ChainOfCustodys_Samples) AND is_travel_blank = 0 AND sample_type_id = " + sampleTypeID);
        }

        public static SelectList FetchSelectListAllUnassigned(string sampleTypeID)
        {
            var unassigned = FetchAllUnassigned(sampleTypeID);
            var dropdownData = new Dictionary<string, string>();
            foreach (var sample in unassigned)
            {
                dropdownData.Add(sample.sample_id, GetSampleText(sample.wbea_id, sample.media_serial_number, sample.lab_sample_id));
            }

            return new SelectList(dropdownData, "key", "value");
        }

        #region  sample list without Orphaned Samples
        public static SelectList FetchSelectListAllUnassignedWithoutOrphaned(string sampleTypeID)
        {
            var unassigned = FetchAllUnassignedWithoutOrphaned(sampleTypeID);
            var dropdownData = new Dictionary<string, string>();
            foreach (var sample in unassigned)
            {
                dropdownData.Add(sample.sample_id, GetSampleText(sample.wbea_id, sample.media_serial_number, sample.lab_sample_id));
            }

            return new SelectList(dropdownData, "key", "value");
        }
        public static List<Sample> FetchAllUnassignedWithoutOrphaned()
        {
            // return FetchAll("sample_id Not IN(select sample_id from ChainOfCustodys_Samples) AND is_travel_blank = 0");
            return FetchAll("sample_id Not IN(select sample_id from ChainOfCustodys_Samples) and isnull(is_orphaned_sample,0)<>1 AND is_travel_blank = 0");
        }

        public static List<Sample> FetchAllUnassignedWithoutOrphaned(string sampleTypeID)
        {

            if (sampleTypeID.IsBlank())
            {
                return FetchAllUnassignedWithoutOrphaned();
            }

            // return FetchAll("sample_id Not IN(select sample_id from ChainOfCustodys_Samples) AND is_travel_blank = 0 AND sample_type_id = " + sampleTypeID);
            return FetchAll("sample_id Not IN(select sample_id from ChainOfCustodys_Samples) AND isnull(is_orphaned_sample,0)<>1 AND is_travel_blank = 0 AND sample_type_id = " + sampleTypeID);
        }

        #endregion

        public static SelectList FetchAvailableTravelBlanksSelectList(ChainOfCustody coc)
        {
            if (coc == null)
            {
                return null;
            }
            Dictionary<string, string> travelBlanks = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand dbCommand = new SqlCommand(@"
                    SELECT sample_id, wbea_id, media_serial_number, lab_sample_id FROM Samples WHERE sample_id IN (
                    SELECT s.sample_id FROM Samples s
                    WHERE is_travel_blank = 1 
                    AND sample_type_id = @sample_type_id
                    AND date_created + @days >= @coc_date_created -- (+ 21) adds days
                    AND date_created - @days <= @coc_date_created -- (- 21) subtracts days
                    EXCEPT
                    SELECT s.sample_id FROM Samples s
                    INNER JOIN ChainOfCustodys_Samples cs ON s.sample_id = cs.sample_id AND cs.chain_of_custody_id = @chain_of_custody_id
                    WHERE is_travel_blank = 1 AND sample_type_id = @sample_type_id)", connection))
                {

                    dbCommand.Parameters.AddWithValue("@sample_type_id", coc.sample_type_id);
                    dbCommand.Parameters.AddWithValue("@coc_date_created", coc.date_opened);
                    dbCommand.Parameters.AddWithValue("@chain_of_custody_id", coc.chain_of_custody_id);
                    dbCommand.Parameters.AddWithValue("@days", OldCoCByDate);

                    using (SqlDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            travelBlanks.Add(dataReader[0].ToString(), GetSampleText(dataReader[1].ToString(), dataReader[2].ToString(), dataReader[3].ToString()));
                        }
                    }
                }
            }

            return new SelectList(travelBlanks, "key", "value");
        }

        // format the text of Samples dropdown so that you get the media id with the WBEA ID
        private static string GetSampleText(string wbea_id, string media_serial_number, string lab_sample_id)
        {
            string sampleText = wbea_id;
            if (!media_serial_number.IsBlank())
            {
                sampleText += " (" + media_serial_number + ")";
            }
            else if (!lab_sample_id.IsBlank())
            {
                sampleText += " [" + lab_sample_id + "]";
            }

            return sampleText;
        }
    }
}