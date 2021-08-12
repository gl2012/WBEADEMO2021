/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;

namespace WBEADMS.Models
{
    public class ValidationError
    {
        private string _field;
        private string _message;

        public ValidationError(string field, string message)
        {
            _field = field;
            _message = message;
        }

        #region properties
        public string Field
        {
            get
            {
                return _field;
            }

            set
            {
                string validationField = value.Trim();
                if (string.IsNullOrEmpty(validationField))
                {
                    throw new ArgumentNullException("field", "ValidationError object must have a specified target field");
                }

                _field = value;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                string validationMessage = value.Trim();
                if (string.IsNullOrEmpty(validationMessage))
                {
                    throw new ArgumentNullException("message", "ValidationError object must have a specified message");
                }

                _message = validationMessage;
            }
        }
        #endregion
    }
}