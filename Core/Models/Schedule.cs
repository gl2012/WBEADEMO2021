/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public partial class Schedule : BaseModel
    {

        #region private members and properties
        public string schedule_id { get { return _data["schedule_id"]; } set { _data["schedule_id"] = value; } }
        public string name { get { return _data["name"]; } set { _data["name"] = value; } }
        public string location_id { get { return _data["location_id"]; } set { _data["location_id"] = value; } }
        public string contact_id { get { return _data["contact_id"]; } set { _data["contact_id"] = value; } }
        public string sample_type_id { get { return _data["sample_type_id"]; } set { _data["sample_type_id"] = value; } }
        public string date_start { get { return _data["date_start"]; } set { _data["date_start"] = value; } }
        public string date_end { get { return _data["date_end"]; } set { _data["date_end"] = value; } }
        public string interval_id { get { return _data["interval_id"]; } set { _data["interval_id"] = value; } }
        public string frequency_data
        {
            get
            {
                if (!_data["frequency_data"].IsBlank() && !_data["frequency_data"].Contains(","))
                {
                    // casting 12.000 to 12; this is old data from Ironspeed that is saved as decimal
                    decimal frequency;
                    if (System.Decimal.TryParse(_data["frequency_data"], out frequency))
                    {
                        return ((int)frequency).ToString();
                    }
                }

                return _data["frequency_data"];
            }

            set
            {
                _data["frequency_data"] = value;
            }
        }

        public string comments { get { return _data["comments"]; } set { _data["comments"] = value; } }
        public string is_active { get { return _data["is_active"]; } set { _data["is_active"] = value; } }
        public string created_by { get { return _data["created_by"]; } set { _data["created_by"] = value; } }
        public string date_created { get { return _data["date_created"].ToDateTimeFormat(); } set { _data["date_created"] = value.ToDateTimeFormat(); } }
        public string modified_by { get { return _data["modified_by"]; } set { _data["modified_by"] = value; } }
        public string date_modified { get { return _data["date_modified"].ToDateTimeFormat(); } set { _data["date_modified"] = value.ToDateTimeFormat(); } }

        public string DateStart { get { return _data["date_start"].ToDateFormat(); } }
        public string DateEnd { get { return _data["date_end"].ToDateFormat(); } }

        public Location Location { get { return LoadRelated<Location>(); } }
        public User contact { get { return LoadRelated<User>("contact_id"); } }
        public User created_user { get { return LoadRelated<User>("created_by"); } }
        public User modified_user { get { return LoadRelated<User>("modified_by"); } }
        public SampleType SampleType { get { return SampleType.Load(_data["sample_type_id"]); } }
        public Interval interval { get { return Interval.Load(interval_id); } }
        public Note[] Notes
        {
            get
            {
                var note_list = LoadRelatedJoin<Note>("Notes_Schedules");
                Array.Sort(note_list);
                return note_list;
            }
        }
        public int ChainOfCustodyCount { get { if (id == null) return 0; else return ChainOfCustody.TotalCount("ChainOfCustodys", "schedule_id = " + id); } }
        #endregion

        public Schedule() : base() { }

        // instance methods
        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                location_id.CheckRequired(errors, "location_id", "Location");
                location_id.CheckIfInt(errors, "location_id", "Location");

                sample_type_id.CheckRequired(errors, "sample_type_id", "Sample Type");
                sample_type_id.CheckIfInt(errors, "sample_type_id", "Sample Type");

                if (!name.CheckRequired(errors, "name"))
                {
                    name.CheckMinLength(errors, 2, "name");
                    name.CheckMaxLength(errors, 50, "name");
                }

                contact_id.CheckRequired(errors, "contact_id", "Contact");
                contact_id.CheckIfInt(errors, "contact_id", "Contact");

                bool hasStartDate = !date_start.CheckRequired(errors, "date_start", "Start Date") &&
                                    !date_start.CheckIfDateTime(errors, "date_start", "Start Date");

                bool hasEndDate = !date_end.IsBlank() && !date_end.CheckIfDateTime(errors, "date_end", "End Date");

                if (hasStartDate && hasEndDate)
                {
                    if (date_start.ToDateTime() > date_end.ToDateTime())
                    {
                        errors.AddError("date_end", "End Date must be later then Start Date.");
                    }
                }

                if (!interval_id.CheckRequired(errors, "interval_id", "Interval") &&
                   !interval_id.CheckIfInt(errors, "interval_id", "Interval"))
                {
                    switch (interval.type)
                    {
                        case IntervalType.every_n_days:
                            if (!frequency_data.CheckRequired(errors, "frequency_data") &&
                                !frequency_data.CheckIfInt(errors, "frequency_data"))
                            {
                                frequency_data.CheckIfIntInRange(2, 365, "frequency_data");
                            }
                            break;
                        case IntervalType.every_n_months:
                            frequency_data.CheckRequired(errors, "frequency_data");
                            frequency_data.CheckIfInt(errors, "frequency_data");
                            break;
                        case IntervalType.monthly_on_days:
                            frequency_data.CheckRequired(errors, "frequency_data");
                            string[] freqSplit = frequency_data.Split(',');
                            foreach (string strFreq in freqSplit)
                            {
                                strFreq.CheckIfInt(errors, "frequency_data", "Frequency Data", "Must be a comma separated list of days of the month.");
                                strFreq.CheckIfIntInRange(errors, 1, 31, "frequency_data", "Frequency Data", "Days in Frequency must be within 1 to 31.");
                            }

                            break;
                    }
                }

                is_active.CheckRequired(errors, "is_active", "Active");

            }
        }

        public List<ChainOfCustody> ForecastMonth(DateTime forecastStart, DateTime forecastEnd)
        {
            DateTime dateEnd;
            if (DateTime.TryParse(date_end, out dateEnd))
            {
                if (dateEnd < forecastEnd)
                { // if schedule's date_end is earlier, then stop forecast earlier
                    forecastEnd = dateEnd;
                }
            }
            else
            {
                dateEnd = DateTime.MaxValue;
            }
            if (forecastStart > forecastEnd) { return new List<ChainOfCustody>(); } // ignore start dates that are past the forecast end date

            var cocList = new List<ChainOfCustody>();

            var freqList = Interval.ParseFrequencyData(frequency_data);
            int freq = (freqList.Count > 0) ? freqList[0] : 1;

            DateTime dateStart = DateTime.Parse(date_start);

            switch (interval.type)
            {
                case IntervalType.daily:
                    return ForecastByFrequency(forecastStart, 1, 0);

                case IntervalType.every_n_months:
                    // calculate months since start
                    int thisStartMonths = dateStart.Year * 12 + dateStart.Month;
                    int forecastMonths = forecastStart.Date.Year * 12 + forecastStart.Date.Month;
                    int monthDiff = forecastMonths - thisStartMonths;

                    // display forecasts if month falls on every n monthly frequency
                    if (monthDiff % freq == 0)
                    {
                        return ForecastByFrequency(forecastStart.Date, forecastStart.AddMonths(1).AddDays(-forecastStart.Day).Day, dateStart.Day - forecastStart.Day);
                    }
                    else
                    {
                        return new List<ChainOfCustody>();
                    }

                case IntervalType.every_n_days:
                    int dateDiff = (dateStart.Date - forecastStart.Date).Days % freq;
                    return ForecastByFrequency(forecastStart, freq, dateDiff);

                case IntervalType.weekly:
                    return ForecastByFrequency(forecastStart, 7, dateStart.DayOfWeek - forecastStart.DayOfWeek);

                case IntervalType.monthly_on_days:
                    foreach (int forecastedDay in freqList)
                    {
                        DateTime forecastedDate = new DateTime(forecastStart.Year, forecastStart.Month, forecastedDay);
                        if (forecastedDate < dateStart) { continue; }
                        if (forecastedDate < forecastStart) { continue; }
                        if (forecastedDate > dateEnd) { continue; }
                        if (forecastedDate > forecastEnd) { continue; }
                        cocList.Add(ForecastDay(forecastedDate));
                    }

                    return cocList;

                case IntervalType.weekdaily:
                    for (var forecastedDate = forecastStart; forecastedDate <= forecastEnd; forecastedDate.AddDays(1))
                    {
                        if (forecastedDate < dateStart) { continue; }
                        if (forecastedDate > dateEnd) { break; }
                        if (forecastedDate.DayOfWeek == DayOfWeek.Saturday || forecastedDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            continue;
                        }

                        cocList.Add(ForecastDay(forecastedDate));
                    }

                    return cocList;

                default:
                    return null;
            }
        }

        public ChainOfCustody ForecastDay(DateTime date_sample_start)
        {
            var coc = new ChainOfCustody();
            coc.schedule_id = schedule_id;
            coc.sample_type_id = sample_type_id;
            coc.Preparation.date_sampling_scheduled = date_sample_start.ToISODate();
            coc.Deployment.location_id = location_id;
            return coc;
        }

        /// <summary></summary>
        /// <param name="forecastStart">start of month; i.e. Nov 1, 2009</param>
        /// <param name="frequency">add days after each forecast, i.e. 5 makes forecast go Nov 1, Nov 6, Nov 11, etc.</param>
        /// <param name="dayDiff">add days to start of forecast, i.e. 2 makes forecast start at Nov 3, 2009</param>
        private List<ChainOfCustody> ForecastByFrequency(DateTime forecastStart, int frequency, int dayDiff)
        {
            DateTime dateStart = DateTime.Parse(date_start);
            var cocList = new List<ChainOfCustody>();

            DateTime forecastEnd = forecastStart.AddMonths(1).AddDays(-forecastStart.Day); // forecast for one month
            DateTime dateEnd;
            if (DateTime.TryParse(date_end, out dateEnd))
            {
                if (forecastEnd > dateEnd)
                { // do not forecast past the end of the schedule
                    forecastEnd = dateEnd;
                }
            }

            for (var forecastedDay = forecastStart.AddDays(dayDiff); forecastedDay <= forecastEnd; forecastedDay = forecastedDay.AddDays(frequency))
            {
                if (forecastedDay < dateStart) { continue; }
                if (forecastedDay < forecastStart) { continue; }
                cocList.Add(ForecastDay(forecastedDay));
            }

            return cocList;
        }

        public static SelectList Fetchschedulelist(string sampletypeid)
        {
            List<SelectListItem> newList = new List<SelectListItem>();
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))

            {
                SelectListItem selListItem1 = new SelectListItem() { Value = "", Text = "" };
                newList.Add(selListItem1);
                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"select * from schedules where  is_active = 1 and sample_type_id=" + sampletypeid, connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {

                            SelectListItem selListItem = new SelectListItem() { Value = dataReader["schedule_id"].ToString(), Text = (string)dataReader["name"] };
                            newList.Add(selListItem);
                        }
                    }
                }

                connection.Close();
            }
            return new SelectList(newList, "Value", "Text");
            // return BaseModelExtensions.SelectList("location_id", "name", userSampleTypeList, selectedValue);
        }
    }
}