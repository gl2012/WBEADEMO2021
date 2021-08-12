/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WBEADMS.Models
{
    public class Validator : IDisposable
    {

        #region members
        private NameValueCollection _data;

        #region properties

        public List<ValidationError> Errors { get; private set; }

        public Dictionary<string, int> ErrorFields { get; private set; }

        public List<string> AllowMultipleErrorField { get; set; }

        #endregion
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the Validator class. 
        /// This stores all errors as you run its validation and parser methods 
        /// and throws them all as a ModelException upon calling Validate()
        /// </summary>
        /// <param name="data">_data object of a BaseModel</param>
        public Validator(NameValueCollection data)
        {
            Errors = new List<ValidationError>();
            ErrorFields = new Dictionary<string, int>();
            AllowMultipleErrorField = new List<string>();
            this._data = data;
        }
        #endregion

        #region static parser methods
        public static Nullable<DateTime> GetDateTime(string text)
        {
            DateTime date;
            if (DateTime.TryParse(text, out date))
            {
                return new Nullable<DateTime>(date);
            }
            else
            {
                return new Nullable<DateTime>();
            }
        }

        public static Nullable<int> GetInt(string text)
        {
            int i;
            if (int.TryParse(text, out i))
            {
                return new int?(i);
            }
            else
            {
                return new int?();
            }
        }

        public static Nullable<Guid> GetGuid(string text)
        {
            Guid? nullGuid = null;

            if (!String.IsNullOrEmpty(text))
            {
                try
                { // NOTE: do we really need try/catch?  or just a simple null or empty string check is enough?                
                    Guid guid = new Guid(text.Trim());
                    nullGuid = guid;
                }
                catch (FormatException)
                {
                    // do nothing; leave guid as null
                }
            }

            return nullGuid;
        }

        public static bool GetBool(string text)
        {
            return !String.IsNullOrEmpty(text) && text.Contains("true");
        }
        #endregion

        #region validation + parser methods
        /// <summary>Throws Errors as ModelException if there are any errors.</summary>
        public void Validate()
        {
            if (Errors.Count > 0)
            {
                throw new ModelException(Errors);
            }
        }

        /// <summary>Ensures that fields are not empty.  Used for string fields.</summary>
        /// <param name="requiredList">list of field names to check</param>
        public void RequiredFields(string[] requiredList)
        {
            foreach (string requiredField in requiredList)
            {
                string value = _data[requiredField];
                if (value == null || value.Trim() == String.Empty)
                {
                    AddRequiredError(requiredField);
                }
            }
        }

        public void MinLength(string name, int minimum)
        {
            var text = _data[name];
            if (text == null || text.Length < minimum)
            {
                AddHumanError(name, "must be at least " + minimum.ToString() + " characters long.");
            }
        }

        public void MaxLength(string name, int maximum)
        {
            var text = _data[name];
            if (text != null && text.Length > maximum)
            {
                AddHumanError(name, "must not be longer than " + maximum.ToString() + " characters.");
            }
        }

        public void BetweenLength(string name, int minLength, int maxLength)
        {
            var text = _data[name];
            if ((text == null || text.Length < minLength) || (text != null && text.Length > maxLength))
            {
                AddHumanError(name, "must be between " + minLength.ToString() + " and " + maxLength.ToString() + " characters long.");
            }
        }

        /// <summary>Validates a string as a GUID and returns it.  Ensures it is of proper type or non-blank.</summary>
        /// <param name="name">name of field</param>
        public bool TryGuid(string name)
        {
            var text = _data[name];
            if (text.IsBlank()) { return false; }
            try
            {
                Guid guid = new Guid(text.Trim());
            }
            catch (FormatException)
            {
                AddHumanError(name, "is not a valid GUID.");
                return false;
            }

            return true;
        }

        /// <summary>Validates a string as a Int and returns it.  Ensures it is of proper type or non-blank.</summary>
        /// <param name="name">name of field</param>
        public bool TryInt(string name)
        {
            int i;
            var text = _data[name];
            if (text.IsBlank()) { return false; }
            if (Int32.TryParse(text, out i))
            {
                return true;
            }
            else
            {
                AddHumanError(name, "is not a valid number.");
                return false;
            }
        }

        /// <summary>Validates a string as a DateTime and returns true.  Ensures it is of proper type or non-blank.</summary>
        /// <param name="name">name of field</param>
        public bool TryDateTime(string name)
        {
            DateTime d;
            return TryDateTime(name, out d);
        }

        /// <summary>Validates a string as a DateTime and returns true.  Ensures it is of proper type or non-blank.</summary>
        /// <param name="name">name of field</param>
        /// <param name="date">parsed date</param>
        public bool TryDateTime(string name, out DateTime date)
        {
            date = DateTime.MinValue;
            var text = _data[name];
            if (text.IsBlank()) { return false; }
            if (DateTime.TryParse(text.Trim(), out date))
            {
                return true;
            }
            else
            {
                AddHumanError(name, "is not a valid date.");
                return false;
            }
        }
        #endregion

        #region add error methods
        public void AddError(string name, string message)
        {
            if (ErrorFields.ContainsKey(name))
            {
                ErrorFields[name] += 1;
                if (!AllowMultipleErrorField.Contains(name))
                {
                    return; // do not process field unless it is marked for multiple errors
                }
            }
            else
            {
                ErrorFields.Add(name, 1);
            }

            Errors.Add(new ValidationError(name, message));
        }

        public void AddHumanError(string name, string message)
        {
            string label = name;
            if (label.EndsWith("_id"))
            {
                label = label.Substring(0, label.IndexOf("_id"));
            }

            AddError(name, label.ToTitleCase() + " " + message);
        }

        public void AddRequiredError(string name)
        {
            AddHumanError(name, "is required.");
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Validate();
        }

        #endregion
    }
}