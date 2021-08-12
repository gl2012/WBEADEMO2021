/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Data;
using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public static class WBEAIdGenerator
    {
        const int MAX_DIGITS = 99999;

        /// <summary>Returns a new WBEA Sample Id</summary>
        public static string ReserveNew()
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int uniqueDigits = GetNextLastGeneratedDigits(year);
            return GenerateWBEAId(year, month, uniqueDigits);
        }

        /// <summary>Checks if wbea is valid or not and updates the WBEAId table to the latest digits</summary>
        public static bool Reserve(string sample_id, string wbeaId)
        {
            int temp;
            if (!int.TryParse(wbeaId, out temp)) { return false; } // scrubbing data against sql injection

            var samples = Sample.FetchAll("sample_id != " + sample_id + " AND wbea_id = '" + wbeaId + "'");
            if (samples.Count > 0)
            {
                return false;
            }

            int year = int.Parse(wbeaId.Substring(0, 2));
            int month = int.Parse(wbeaId.Substring(2, 2));
            int digits = int.Parse(wbeaId.Substring(4));

            return SetLastGeneratedDigits(year, digits);
        }

        /// <summary>Returns a CSV of a set of WBEA Sample Ids</summary>
        public static string[] ReserveNewSet(string set_size)
        {
            int setSize;
            if (!int.TryParse(set_size, out setSize))
            {
                throw new ModelException(new ValidationError("set_size", "Set Size must be a numerical value. (1 - 99999)"));
            }

            return ReserveNewSet(setSize);
        }

        /// <summary>Returns a CSV of a set of WBEA Sample Ids</summary>
        public static string[] ReserveNewSet(int setSize)
        {
            if (setSize <= 0)
            {
                throw new ModelException(new ValidationError("set_size", "Set Size must be greater than 0."));
            }

            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            int prevDigits;
            int nextDigits;
            GetNextLastGeneratedDigits(year, setSize, out prevDigits, out nextDigits);

            var list = new string[setSize];
            for (int i = 1; i <= setSize; i++)
            {
                list[i - 1] = GenerateWBEAId(year, month, prevDigits + i);
            }

            return list;
        }

        public static void FetchDetails(int year, out int lastGeneratedDigits, out int lastSetSize, out DateTime dateLastGenerated)
        {
            using (var connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                string sql = "SELECT * FROM WBEAIds WHERE year = " + year;
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            lastGeneratedDigits = (int)dataReader["last_generated_digits"];
                            lastSetSize = (int)dataReader["last_set_size"];
                            dateLastGenerated = (DateTime)dataReader["date_last_generated"];
                        }
                        else
                        {
                            lastGeneratedDigits = -1;
                            lastSetSize = 0;
                            dateLastGenerated = DateTime.MinValue;
                        }
                    }
                }
            }
        }

        public static string GenerateWBEAId(int year, int month, int uniqueDigits)
        {
            return LeadingZero(year % 100) + LeadingZero(month) + LeadingZero(uniqueDigits, 5);
        }

        private static string LeadingZero(int i)
        {
            return LeadingZero(i, 2);
        }

        private static string LeadingZero(int i, int digits)
        {
            return i.ToString().PadLeft(digits, '0');
        }

        private static int GetNextLastGeneratedDigits(int year)
        {
            int prevDigits;
            int nextDigits;
            GetNextLastGeneratedDigits(year, 1 /* increment */, out prevDigits, out nextDigits);
            return nextDigits;
        }

        private static void GetNextLastGeneratedDigits(int year, int increment, out int prevDigits, out int nextDigits)
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
                command.Parameters.AddWithValue("size", increment);
                command.Parameters.AddWithValue("lastgen", DateTime.Now);

                try
                {
                    command.CommandText = "SELECT last_generated_digits FROM WBEAIds WHERE year = @year";
                    var result = command.ExecuteScalar();

                    if (result == null)
                    {
                        prevDigits = -1;
                        nextDigits = increment - 1;
                        command.CommandText = "INSERT INTO WBEAIds (year, last_generated_digits, last_set_size, date_last_generated) VALUES (@year, @digits, @size, @lastgen);";
                    }
                    else
                    {
                        prevDigits = (int)result;
                        nextDigits = prevDigits + increment;
                        if (nextDigits == MAX_DIGITS) { throw new ArgumentException("Last generated WBEA Sample ID is already at " + MAX_DIGITS + "; please contact your system administrator."); }
                        if (nextDigits > MAX_DIGITS) { throw new ArgumentException("Set Size is too large. Maximum available value is " + (MAX_DIGITS - prevDigits)); }

                        command.CommandText = "UPDATE WBEAIds SET last_generated_digits = @digits, last_set_size = @size, date_last_generated = @lastgen WHERE year = @year";
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

        private static bool SetLastGeneratedDigits(int year, int digits)
        {
            using (var connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                var command = connection.CreateCommand();
                var transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                command.Connection = connection;
                command.Transaction = transaction;

                command.Parameters.AddWithValue("year", year);

                try
                {
                    command.CommandText = "SELECT last_generated_digits FROM WBEAIds WHERE year = @year";
                    var result = command.ExecuteScalar();

                    if (result == null)
                    {
                        command.CommandText = "INSERT INTO WBEAIds (year, last_generated_digits) VALUES (@year, @digits);";
                    }
                    else
                    {
                        if ((int)result > digits)
                        {
                            return true; // don't update table if requested digits is below the latest generated digits
                        }

                        command.CommandText = "UPDATE WBEAIds SET last_generated_digits = @digits WHERE year = @year";
                    }

                    command.Parameters.AddWithValue("digits", digits);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new Exception("Sql error. Unable to set next LastGeneratedDigits. Please contact an administrator.");
                }
            }

            return true;
        }
    }
}