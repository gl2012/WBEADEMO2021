/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Text.RegularExpressions;
using WBEADMS.Models;

namespace WBEADMS
{
    public static class StringModelValidationExtensions
    {
        #region CheckRequired
        public static ValidationError CheckRequired(this string value, string field)
        {
            return CheckRequired(value, field, field.ToTitleCase(), " is a required field.");
        }

        public static ValidationError CheckRequired(this string value, string field, string fieldName)
        {
            return CheckRequired(value, field, fieldName, " is a required field.");
        }

        public static ValidationError CheckRequired(this string value, string field, string fieldName, string message)
        {
            if (value.IsBlank())
            {
                return new ValidationError(field, fieldName + " " + message);
            }

            return null;
        }

        public static bool CheckRequired(this string value, ModelException me, string field)
        {
            return me.AddError(CheckRequired(value, field));
        }

        public static bool CheckRequired(this string value, ModelException me, string field, string fieldName)
        {
            return me.AddError(CheckRequired(value, field, fieldName));
        }

        public static bool CheckRequired(this string value, ModelException me, string field, string fieldName, string message)
        {
            return me.AddError(CheckRequired(value, field, fieldName, message));
        }
        #endregion

        #region CheckExactLength
        public static ValidationError CheckExactLength(this string value, int length, string field)
        {
            return CheckExactLength(value, length, field, field.ToTitleCase() + " must be of exactly length " + length + " .");
        }

        public static ValidationError CheckExactLength(this string value, int length, string field, string message)
        {
            if (!value.IsBlank() && !value.IsExactLength(length))
            {
                return new ValidationError(field, message);
            }

            return null;
        }

        public static bool CheckExactLength(this string value, ModelException me, int length, string field)
        {
            return me.AddError(CheckExactLength(value, length, field));
        }

        public static bool CheckExactLength(this string value, ModelException me, int length, string field, string message)
        {
            return me.AddError(CheckExactLength(value, length, field, message));
        }
        #endregion 

        #region CheckMaxLength
        public static ValidationError CheckMaxLength(this string value, int maxLength, string field)
        {
            return CheckMaxLength(value, maxLength, field, field.ToTitleCase() + " is too long. Must be less than " + maxLength + " in length.");
        }

        public static ValidationError CheckMaxLength(this string value, int maxLength, string field, string message)
        {
            if (!value.IsBlank() && !value.IsMaxLength(maxLength))
            {
                return new ValidationError(field, message);
            }

            return null;
        }

        public static bool CheckMaxLength(this string value, ModelException me, int maxLength, string field)
        {
            return me.AddError(CheckMaxLength(value, maxLength, field));
        }

        public static bool CheckMaxLength(this string value, ModelException me, int maxLength, string field, string message)
        {
            return me.AddError(CheckMaxLength(value, maxLength, field, message));
        }

        #endregion

        #region CheckMinLength
        public static ValidationError CheckMinLength(this string value, int min, string field)
        {
            return CheckMinLength(value, min, field, field.ToTitleCase() + " is too short. Must be at least " + min + " characters in length.");
        }

        public static ValidationError CheckMinLength(this string value, int min, string field, string message)
        {
            if (!value.IsBlank() && !value.IsMinLength(min))
            {
                return new ValidationError(field, message);
            }

            return null;
        }

        public static bool CheckMinLength(this string value, ModelException me, int min, string field)
        {
            return me.AddError(CheckMinLength(value, min, field));
        }

        public static bool CheckMinLength(this string value, ModelException me, int min, string field, string message)
        {
            return me.AddError(CheckMinLength(value, min, field, message));
        }
        #endregion

        #region CheckIfInt
        public static ValidationError CheckIfInt(this string value, string field)
        {
            return CheckIfInt(value, field, field.ToTitleCase());
        }

        public static ValidationError CheckIfInt(this string value, string field, string fieldName)
        {
            return CheckIfInt(value, field, fieldName, "must be an integer.");
        }

        public static ValidationError CheckIfInt(this string value, string field, string fieldName, string message)
        {
            if (!value.IsBlank() && !value.IsInt())
            {
                return new ValidationError(field, fieldName.Trim() + " " + message.Trim());
            }

            return null;
        }

        public static bool CheckIfInt(this string value, ModelException me, string field)
        {
            return me.AddError(CheckIfInt(value, field));
        }

        public static bool CheckIfInt(this string value, ModelException me, string field, string fieldName)
        {
            return me.AddError(CheckIfInt(value, field, fieldName));
        }

        public static bool CheckIfInt(this string value, ModelException me, string field, string fieldName, string message)
        {
            return me.AddError(CheckIfInt(value, field, fieldName, message));
        }
        #endregion

        #region CheckIfIntInRange
        public static ValidationError CheckIfIntInRange(this string value, int min, int max, string field)
        {
            return CheckIfIntInRange(value, min, max, field, field.ToTitleCase());
        }

        public static ValidationError CheckIfIntInRange(this string value, int min, int max, string field, string fieldName)
        {
            return CheckIfIntInRange(value, min, max, field, field.ToTitleCase(), "must be in the range " + min + " to " + max + ".");
        }

        public static ValidationError CheckIfIntInRange(this string value, int min, int max, string field, string fieldName, string message)
        {
            if (!value.IsBlank() && !value.IsIntAndInRange(min, max))
            {
                return new ValidationError(field, fieldName.Trim() + " " + message.Trim());
            }

            return null;
        }

        public static bool CheckIfIntInRange(this string value, ModelException me, int min, int max, string field)
        {
            return me.AddError(CheckIfIntInRange(value, min, max, field));
        }

        public static bool CheckIfIntInRange(this string value, ModelException me, int min, int max, string field, string fieldName)
        {
            return me.AddError(CheckIfIntInRange(value, min, max, field, fieldName));
        }

        public static bool CheckIfIntInRange(this string value, ModelException me, int min, int max, string field, string fieldName, string message)
        {
            return me.AddError(CheckIfIntInRange(value, min, max, field, fieldName, message));
        }
        #endregion

        #region CheckIfDecimalInRange
        public static ValidationError CheckIfDecimalInRange(this string value, decimal min, decimal max, string field)
        {
            return CheckIfDecimalInRange(value, min, max, field, field.ToTitleCase() + " must be in the range " + min + " to " + max + ".");
        }

        public static ValidationError CheckIfDecimalInRange(this string value, decimal min, decimal max, string field, string message)
        {
            if (!value.IsBlank() && !value.IsDecimalAndInRange(min, max))
            {
                return new ValidationError(field, message);
            }

            return null;
        }

        public static bool CheckIfDecimalInRange(this string value, ModelException me, decimal min, decimal max, string field)
        {
            return me.AddError(CheckIfDecimalInRange(value, min, max, field));
        }

        public static bool CheckIfDecimalInRange(this string value, ModelException me, decimal min, decimal max, string field, string message)
        {
            return me.AddError(CheckIfDecimalInRange(value, min, max, field, message));
        }
        #endregion

        #region CheckIfIntAndPositive
        public static ValidationError CheckIfIntAndPositive(this string value, string field)
        {
            return CheckIfIntAndPositive(value, field, field.ToTitleCase() + " must be a positive number.");
        }

        public static ValidationError CheckIfIntAndPositive(this string value, string field, string message)
        {
            if (!value.IsBlank() && !value.IsIntAndPositive())
            {
                return new ValidationError(field, message);
            }

            return null;
        }

        public static bool CheckIfIntAndPositive(this string value, ModelException me, string field)
        {
            return me.AddError(CheckIfIntAndPositive(value, field));
        }

        public static bool CheckIfIntAndPositive(this string value, ModelException me, string field, string message)
        {
            return me.AddError(CheckIfIntAndPositive(value, field, message));
        }
        #endregion

        #region CheckIfDecimalAndPositive
        public static ValidationError CheckIfDecimalAndPositive(this string value, string field)
        {
            return CheckIfDecimalAndPositive(value, field, field.ToTitleCase() + " must be a positive number.");
        }

        public static ValidationError CheckIfDecimalAndPositive(this string value, string field, string message)
        {
            if (!value.IsBlank() && !value.IsDecimalAndPositive())
            {
                return new ValidationError(field, message);
            }

            return null;
        }

        private static bool IsDecimalAndPositive(this string value)
        {
            decimal decimalValue;
            if (!decimal.TryParse(value, out decimalValue))
            {
                return false;
            }

            return (decimalValue >= 0);
        }
        #endregion

        #region CheckIfDecimal
        public static ValidationError CheckIfDecimal(this string value, string field)
        {
            return CheckIfDecimal(value, field, field.ToTitleCase());
        }

        public static ValidationError CheckIfDecimal(this string value, string field, string fieldName)
        {
            return CheckIfDecimal(value, field, fieldName, "must be a decimal.");
        }

        public static ValidationError CheckIfDecimal(this string value, string field, string fieldName, string message)
        {
            if (!value.IsBlank() && !value.IsDecimal())
            {
                return new ValidationError(field, fieldName.Trim() + " " + message.Trim());
            }

            return null;
        }

        public static bool CheckIfDecimal(this string value, ModelException me, string field)
        {
            return me.AddError(CheckIfDecimal(value, field));
        }

        public static bool CheckIfDecimal(this string value, ModelException me, string field, string fieldName)
        {
            return me.AddError(CheckIfDecimal(value, field, fieldName));
        }

        public static bool CheckIfDecimal(this string value, ModelException me, string field, string fieldName, string message)
        {
            return me.AddError(CheckIfDecimal(value, field, fieldName, message));
        }
        #endregion

        #region CheckIfBool
        public static ValidationError CheckIfBool(this string value, string field)
        {
            return CheckIfBool(value, field, field.ToTitleCase() + " must be true or false.");
        }

        public static ValidationError CheckIfBool(this string value, string field, string message)
        {
            if (!value.IsBlank() && !value.IsBool())
            {
                return new ValidationError(field, message);
            }

            return null;
        }

        public static bool CheckIfBool(this string value, ModelException me, string field)
        {
            return me.AddError(CheckIfBool(value, field));
        }

        public static bool CheckIfBool(this string value, ModelException me, string field, string message)
        {
            return me.AddError(CheckIfBool(value, field, message));
        }
        #endregion

        #region CheckIfDateTime
        public static ValidationError CheckIfDateTime(this string value, string field)
        {
            return CheckIfDateTime(value, field, field.ToTitleCase());
        }

        public static ValidationError CheckIfDateTime(this string value, string field, string fieldName)
        {
            return CheckIfDateTime(value, field, fieldName, "must be a valid date.");
        }

        public static ValidationError CheckIfDateTime(this string value, string field, string fieldName, string message)
        {
            if (!value.IsBlank() && !value.IsDateTime())
            {
                return new ValidationError(field, fieldName.Trim() + " " + message.Trim());
            }

            return null;
        }

        public static bool CheckIfDateTime(this string value, ModelException me, string field)
        {
            return me.AddError(CheckIfDateTime(value, field));
        }

        public static bool CheckIfDateTime(this string value, ModelException me, string field, string fieldName)
        {
            return me.AddError(CheckIfDateTime(value, field, fieldName));
        }

        public static bool CheckIfDateTime(this string value, ModelException me, string field, string fieldName, string message)
        {
            return me.AddError(CheckIfDateTime(value, field, fieldName, message));
        }
        #endregion

        #region CheckValidEmail
        public static ValidationError CheckValidEmail(this string value, string field)
        {
            return CheckValidEmail(value, field, field.ToTitleCase() + " address provided is invalid.");
        }

        public static ValidationError CheckValidEmail(this string value, string field, string message)
        {
            Regex emailPattern = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            if (!value.IsBlank() && !emailPattern.IsMatch(value))
            {
                return new ValidationError(field, message);
            }

            return null;
        }

        public static bool CheckValidEmail(this string value, ModelException me, string field)
        {
            return me.AddError(CheckValidEmail(value, field));
        }

        public static bool CheckValidEmail(this string value, ModelException me, string field, string message)
        {
            return me.AddError(CheckValidEmail(value, field, message));
        }
        #endregion 
    }
}