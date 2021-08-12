/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public static class SampleMediaIdGenerator
    {
        const int MAX_DIGITS = 9999;
        public static readonly List<string> ParameterList = new List<string> { "SO2", "O3", "NO2", "NH3", "HNO3", "H2S", "IER-TF", "IER-O" };

        /// <summary>Returns a CSV of a set of Sample Media Ids</summary>
        public static string[] ReserveNewSet(string parameter, string set_size, string year, string month)
        {
            int setSize;
            if (!int.TryParse(set_size, out setSize))
            {
                throw new ModelException(new ValidationError("set_size", "Set Size must be a numerical value. (1 - 9999)"));
            }

            if (!ParameterList.Contains(parameter))
            {
                throw new ModelException(new ValidationError("parameter", "Invalid parameter selected. " + parameter));
            }

            if (setSize <= 0)
            {
                throw new ModelException(new ValidationError("set_size", "Set Size must be greater than 0."));
            }

            int monthNumber = int.Parse(month);
            int yearNumber = int.Parse(year);

            int prevDigits;
            int nextDigits;
            GetNextLastGeneratedDigits(yearNumber, parameter, setSize, out prevDigits, out nextDigits);

            var list = new string[setSize];
            for (int i = 1; i <= setSize; i++)
            {
                list[i - 1] = GenerateSampleMediaId(yearNumber, monthNumber, parameter, prevDigits + i);
            }

            return list;
        }

        public static void FetchDetails(int year, out string parameter, out int lastGeneratedDigits, out int lastSetSize, out DateTime dateLastGenerated)
        {
            using (var connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                string sql = "SELECT TOP 1 * FROM SampleMediaIds WHERE year = " + year + " ORDER BY date_last_generated DESC";
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            lastGeneratedDigits = (int)dataReader["last_generated_digits"];
                            lastSetSize = (int)dataReader["last_set_size"];
                            dateLastGenerated = (DateTime)dataReader["date_last_generated"];
                            parameter = dataReader["parameter"].ToString();
                        }
                        else
                        {
                            lastGeneratedDigits = -1;
                            lastSetSize = 0;
                            dateLastGenerated = DateTime.MinValue;
                            parameter = string.Empty;
                        }
                    }
                }
            }
        }

        public static string GenerateSampleMediaId(int year, int month, string parameter, int uniqueDigits)
        {
            return (new DateTime(year, month, 1)).ToString("MMM") + "-" + LeadingZero(year % 100) + "-" + parameter + "-" + LeadingZero(uniqueDigits, 4);
        }

        private static string LeadingZero(int i)
        {
            return LeadingZero(i, 2);
        }

        private static string LeadingZero(int i, int digits)
        {
            return i.ToString().PadLeft(digits, '0');
        }

        private static void GetNextLastGeneratedDigits(int year, string parameter, int increment, out int prevDigits, out int nextDigits)
        {
            if (increment > MAX_DIGITS)
            {
                throw new ArgumentException("Set Size must not exceed " + MAX_DIGITS + ".");
            }

            using (var connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                var command = connection.CreateCommand();
                var transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                command.Connection = connection;
                command.Transaction = transaction;

                command.Parameters.AddWithValue("year", year);
                command.Parameters.AddWithValue("parameter", parameter);
                command.Parameters.AddWithValue("size", increment);
                command.Parameters.AddWithValue("lastgen", DateTime.Now);

                try
                {
                    command.CommandText = "SELECT last_generated_digits FROM SampleMediaIds WHERE year = @year AND parameter = @parameter";
                    var result = command.ExecuteScalar();

                    if (result == null)
                    {
                        prevDigits = -1;
                        nextDigits = increment - 1;
                        command.CommandText = "INSERT INTO SampleMediaIds (year, last_generated_digits, last_set_size, date_last_generated, parameter) VALUES (@year, @digits, @size, @lastgen, @parameter);";
                    }
                    else
                    {
                        prevDigits = (int)result;
                        nextDigits = prevDigits + increment;
                        if (nextDigits == MAX_DIGITS) { throw new ArgumentException("Last generated Sample Media ID is already at " + MAX_DIGITS + "; please contact your system administrator."); }
                        if (nextDigits > MAX_DIGITS) { throw new ArgumentException("Set Size is too large. Maximum available value is " + (MAX_DIGITS - prevDigits)); }

                        command.CommandText = "UPDATE SampleMediaIds SET last_generated_digits = @digits, last_set_size = @size, date_last_generated = @lastgen WHERE year = @year AND parameter = @parameter";
                    }

                    command.Parameters.AddWithValue("digits", nextDigits);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (ArgumentException)
                {
                    transaction.Rollback();
                    throw;
                }
                catch
                {
                    transaction.Rollback();
                    throw new Exception("Sql error. Unable to get next LastGeneratedDigits. Please contact an administrator.");
                    ////throw;
                }
            }
        }
    }
}