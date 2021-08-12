/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public partial class Schedule
    {
        public static Schedule Load(string id)
        {
            return Load<Schedule>(id);
        }

        public static SelectList FetchSelectListActive(object schedule_id)
        {
            string where = "(date_end >= '" + DateTime.Today.ToISODate() + "' OR date_end IS NULL) AND is_active = 1";
            return BaseModelExtensions.FetchAllSelectList<Schedule>("schedule_id", "name", new { where = where }, schedule_id);
        }
        public static SelectList FetchSelectedSampleListActive(string sampleTypeId, object schedule_id)
        {
            string where = "(date_end >= '" + DateTime.Today.ToISODate() + "' OR date_end IS NULL) AND is_active = 1 and sample_type_id=" + sampleTypeId;
            return BaseModelExtensions.FetchAllSelectList<Schedule>("schedule_id", "name", new { where = where }, schedule_id);
        }

        public static List<Schedule> FetchAll()
        {
            return FetchAll<Schedule>();
        }

        public static List<Schedule> FetchAll(DateTime startDate, DateTime endDate, string location_id, string sample_type_id)
        {
            var whereClauseList = new List<string>();
            whereClauseList.Add("date_start <= '" + endDate.ToISODate() + "'");
            whereClauseList.Add("(date_end >= '" + startDate.ToISODate() + "' OR date_end IS NULL)");

            // build a list of schedule Ids to filter CoCs by
            if (location_id.IsInt())
            {
                whereClauseList.Add("location_id = " + location_id);
            }

            // find sampleType to filter CoCs by
            if (sample_type_id == "XPASS")
            {
                whereClauseList.Add("sample_type_id != " + SampleType.PASS.id);
            }
            else if (sample_type_id.IsInt())
            {
                whereClauseList.Add("sample_type_id = " + sample_type_id);
            }

            // filter by non-deleted
            whereClauseList.Add("is_active = 1");

            // create Where clause
            string whereClause = (whereClauseList.Count > 0)
                ? String.Join(" AND ", whereClauseList.ToArray())
                : null;

            // get schedules
            return BaseModel.FetchAll<Schedule>(whereClause);
        }

        /// <summary>Return all schedules belonging to a location.</summary>
        public static List<Schedule> FetchAll(Location location)
        {
            return FetchAll<Schedule>("location_id=" + location.id.ToString());
        }
    }
}