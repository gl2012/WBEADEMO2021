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
    public enum IntervalType
    {
        none = 0,
        daily = 1,
        every_n_months = 2, // every month
        every_n_days = 3,
        weekly = 4, // once a week starting on start_date; so it always stays on the same day of week
        monthly_on_days = 5,
        weekdaily = 6 // every day except Sat/Sun
    }

    public class Interval
    {
        private static int[] name_order = { 1, 6, 3, 4, 2, 5 }; // TODO: perhaps do names in an enum?  but it must preserve historical ids

        public string name { get { return ((IntervalType)id).ToString().ToTitleCase(); } }
        public int id { get; private set; }
        public string interval_id { get { return id.ToString(); } } // NOTE: this is a hack it imitate database field
        public IntervalType type { get { return (IntervalType)id; } }

        public Interval(int id)
        {
            this.id = id;
        }

        public Interval(string id)
        {
            this.id = Int32.Parse(id);
        }

        public static Interval Load(int id) { return new Interval(id); }
        public static Interval Load(string id) { return new Interval(id); }

        public static List<Interval> FetchAll()
        {
            // NOTE: old sql would be: SELECT * FROM ScheduleIntervals ORDER BY sortorder ASC
            var list = new List<Interval>();
            foreach (int i in name_order)
            {
                list.Add(new Interval(i));
            }

            return list;
        }

        public static SelectList FetchSelectList(object selectedValue)
        {
            return new SelectList(Interval.FetchAll(), "interval_id", "name", selectedValue);
        }

        public static List<int> ParseFrequencyData(string frequency_data)
        {
            var freqList = new List<int>();
            if (frequency_data.IsBlank())
            {
                return freqList; // return empty list if frequency_data is blank
            }

            foreach (string s in frequency_data.Split(','))
            {
                freqList.Add((int)double.Parse(s));
            }

            return freqList;
        }

        public string ToSentence(string frequency_data)
        {
            List<int> freqList = ParseFrequencyData(frequency_data);

            string msg;
            switch (this.type)
            {
                case IntervalType.every_n_days:
                    if (freqList.Count == 0)
                    {
                        msg = "Special";
                    }
                    else if (freqList[0] > 1)
                    {
                        msg = "Every " + freqList[0].ToString("0") + " days";
                    }
                    else
                    {
                        msg = "Daily";
                    }

                    break;

                case IntervalType.monthly_on_days:
                    if (freqList.Count == 1)
                    {
                        msg = "Monthly on day ";
                    }
                    else
                    {
                        msg = "Monthly on days: ";
                    }

                    var freqListString = new List<string>();
                    foreach (int i in freqList) { freqListString.Add(i.ToString()); }
                    msg += String.Join(", ", freqListString.ToArray());
                    break;

                case IntervalType.every_n_months:
                    if (freqList.Count > 0 && freqList[0] > 1)
                    {
                        msg = "Every " + freqList[0].ToString("0") + " months";
                    }
                    else
                    {
                        msg = "Monthly";
                    }

                    break;

                default:
                    msg = name;
                    break;
            }

            return msg;
        }
    }
}