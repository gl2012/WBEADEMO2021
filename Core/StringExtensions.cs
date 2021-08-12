/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;

namespace WBEADMS
{
    public static class StringExtensions
    {
        /// <summary>Determines if string is blank.</summary><returns>True if string is null, if string is empty, or if string contains only spaces.</returns>
        public static bool IsBlank(this string s)
        {
            return (s == null || s.Trim().Length == 0);
        }

        public static bool IsExactLength(this string value, int length)
        {
            return (value == null || value.Length == length);
        }

        public static bool IsMaxLength(this string value, int maxLength)
        {
            return (value == null || value.Length <= maxLength);
        }

        public static bool IsMinLength(this string value, int min)
        {
            return (value == null || value.Length >= min);
        }

        public static bool IsInt(this string value)
        {
            int intValue;
            return int.TryParse(value, out intValue);
        }

        public static bool IsIntAndInRange(this string value, int min, int max)
        {
            int intValue;
            if (!int.TryParse(value, out intValue))
            {
                return false;
            }

            return (intValue >= min && intValue <= max);
        }

        public static bool IsIntAndPositive(this string value)
        {
            int intValue;
            if (!int.TryParse(value, out intValue))
            {
                return false;
            }

            return (intValue >= 0);
        }

        public static bool IsDecimal(this string value)
        {
            decimal decimalValue;
            return decimal.TryParse(value, out decimalValue);
        }

        public static bool IsDecimalAndInRange(this string value, decimal min, decimal max)
        {
            decimal decValue;
            if (!decimal.TryParse(value, out decValue))
            {
                return false;
            }

            return (decValue >= min && decValue <= max);
        }

        public static string ToDecimalFormat(this string value)
        {
            if (value.IsBlank())
            {
                return "";
            }

            decimal decVal;
            if (decimal.TryParse(value, out decVal))
            {
                return decVal.ToString("0.0######");
            }

            return "";
        }

        public static bool IsBool(this string value)
        {
            bool boolValue;
            return bool.TryParse(value, out boolValue);
        }

        public static bool IsDateTime(this string value)
        {
            DateTime dateTimeValue;
            return DateTime.TryParse(value, out dateTimeValue);
        }

        /// <summary>Convert underscore_separated or TitleCase strings to space separated strings.</summary>
        public static string ToHuman(this string s)
        {
            if (s.Contains("_"))
            {
                return s.Replace("_", " ");  // convert "underscore_case" to "underscore case"
            }
            else
            {
                var sb = new System.Text.StringBuilder();
                sb.Append(s[0]);
                s = s.Substring(1);
                foreach (char c in s)
                {
                    if (Char.IsUpper(c)) { sb.Append(' '); }
                    sb.Append(c);
                }

                return sb.ToString().ToLower(); // convert "TitleCase" to "title case"
            }
        }

        /// <summary>Convert underscore or space separated strings to title case</summary>
        public static string ToTitleCase(this string s)
        {
            string[] words = s.ToHuman().Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = String.Format("{0}{1}", words[i][0].ToString().ToUpper(), words[i].Substring(1));
            }

            return String.Join(" ", words);
        }

        /// <summary>Convert space separated string to lower case underscore separated string</summary>
        public static string ToUnderscore(this string s)
        {
            return s.IsBlank()
                    ? String.Empty
                    : s.ToHuman().Replace(' ', '_');
        }

        /// <summary>Formats a string date to ISO 8601 (yyyy-MM-dd) format</summary>
        /// <returns>Returns date in yyyy-MM-dd format.  Returns String.Empty if input is an invalid date or null.</returns>
        public static string ToDateFormat(this string s)
        {
            if (s.IsBlank())
            {
                return String.Empty;
            }
            else
            {
                DateTime d;
                if (DateTime.TryParse(s, out d))
                {
                    return d.ToISODate();
                    ////return d.ToString(WBEADMS.ViewsCommon.FetchDateFormat(false)); // NOTE: TSQL cannot save dd-MM-yyyy format, so explicit converts is required and that opens a can of worms.
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public static string ToDateTimeFormat(this string s)
        {
            if (s.IsBlank())
            {
                return String.Empty;
            }
            else
            {
                DateTime d;
                if (DateTime.TryParse(s, out d))
                {
                    return d.ToISODateTime();
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public static DateTime? ToDateTime(this string s)
        {
            if (s.IsBlank()) { return null; }

            DateTime d;
            if (DateTime.TryParse(s, out d)) { return d; }

            return null;
        }
        /// <summary>Formats a date to ISO 8601 (yyyy-MM-dd) format</summary>
        /// <returns>Returns date in yyyy-MM-dd format.</returns>
        public static string ToISODate(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd");
        }

        /// <summary>Formats a date to ISO 8601 (yyyy-MM-dd HH:mm) format</summary>
        /// <returns>Returns date in yyyy-MM-dd HH:mm format.</returns>
        public static string ToISODateTime(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>Returns false if blank, 0, or "false"; otherwise returns true.</summary>
        public static bool ToBool(this string s)
        {
            if (s.IsBlank()) { return false; }

            switch (s.ToLower())
            {
                case "0":
                case "false":
                    return false;
                default:
                    return true;
            }
        }

        //return Yes if "True" no if "False" and "" otherwise
        public static string ToHumanBool(this string value)
        {
            switch (value.ToLower())
            {
                case "true":
                    return "Yes";
                case "false":
                    return "No";
                default:
                    return "";
            }
        }
    }
}