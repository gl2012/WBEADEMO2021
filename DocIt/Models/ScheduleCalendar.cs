
using System;
using System.Collections.Generic;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    public class ScheduleCalendar
    {

        public ScheduleCalendar(int month, int year, string location_id, string sample_type_id)
        {
            ChainOfCustodys = GetCoCsInMonth(month, year, location_id, sample_type_id);
            GetCalendarMonth(month, year);
        }

        public List<CalendarDate[]> Weeks { get; private set; }

        public int Count { get; set; }

        Dictionary<DateTime, List<ChainOfCustody>> ChainOfCustodys { get; set; }

        /// <summary>Gets a list of weeks in the month/year.</summary>
        void GetCalendarMonth(int month, int year)
        {
            Weeks = new List<CalendarDate[]>();
            DateTime date = new DateTime(year, month, 1);
            DateTime nextMonth = date.AddMonths(1);
            if (date.DayOfWeek != DayOfWeek.Sunday) { date = date.AddDays(-(int)date.DayOfWeek); }

            while (date < nextMonth)
            {
                Weeks.Add(GetCalendarWeek(date));
                date = date.AddDays(7);
            }
        }

        /// <summary>Gets a week of CalendarDate starting from the specified date.</summary>
        CalendarDate[] GetCalendarWeek(DateTime date)
        {
            var week = new CalendarDate[7];
            for (int day = 0; day < 7; day++)
            {
                DateTime dateInWeek = date.AddDays(day).Date;
                if (ChainOfCustodys.ContainsKey(dateInWeek))
                {
                    week[day] = new CalendarDate(dateInWeek, ChainOfCustodys[dateInWeek]);
                }
                else
                {
                    week[day] = new CalendarDate(dateInWeek);
                }
            }

            return week;
        }

        /// <summary>Gets all CoCs (including forecasted CoCs) in a month.</summary>
        Dictionary<DateTime, List<ChainOfCustody>> GetCoCsInMonth(int month, int year, string location_id, string sample_type_id)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var monthList = new List<ChainOfCustody>();

            // find existing chain of custodies
            monthList.AddRange(ChainOfCustody.FetchScheduled(startDate, endDate, location_id, sample_type_id));

            // determine forecasted chain of custodies
            foreach (var schedule in Schedule.FetchAll(startDate, endDate, location_id, sample_type_id))
            {
                DateTime lastScheduledDate = ChainOfCustody.FetchDateLastScheduled(schedule);

                // forecast only after the last chain of custody assigned to that schedule
                DateTime forecastStart;
                if (lastScheduledDate < startDate)
                {
                    forecastStart = startDate;
                }
                else
                {
                    forecastStart = lastScheduledDate.Date.AddDays(1);
                }

                var forecastedCoCs = schedule.ForecastMonth(forecastStart, endDate);
                monthList.AddRange(forecastedCoCs);
            }

            // update count
            Count = monthList.Count;

            // put list into datetime indexed hash
            var cocHash = new Dictionary<DateTime, List<ChainOfCustody>>();
            foreach (var coc in monthList)
            {
                DateTime sampleDate;
                if (DateTime.TryParse(coc.Deployment.date_sample_start, out sampleDate))
                {  // NOTE: we don't use date_actual_sample_start
                    sampleDate = sampleDate.Date;
                }
                else if (DateTime.TryParse(coc.Preparation.date_sampling_scheduled, out sampleDate))
                {
                    sampleDate = sampleDate.Date;
                }
                else
                {
                    continue; // unscheduled coc; drop it like it's hot
                }

                if (!cocHash.ContainsKey(sampleDate))
                {
                    cocHash.Add(sampleDate, new List<ChainOfCustody>());
                }

                cocHash[sampleDate].Add(coc);
            }

            return cocHash;
        }
    }
}