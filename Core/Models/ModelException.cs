/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;

namespace WBEADMS.Models
{
    public class ModelException : Exception, IDisposable
    {
        private List<ValidationError> _errors;

        public ModelException()
        {
            _errors = new List<ValidationError>();
        }

        #region Constructors
        public ModelException(List<ValidationError> errors)
        {
            _errors = errors;
        }

        public ModelException(ValidationError error) : this()
        {
            AddError(error);
        }

        public ModelException(Exception error) : this()
        {
            AddError(error);
        }
        #endregion

        #region AddError
        /// <summary>Returns True if an error is added. nulls are ignored;</summary>
        public bool AddError(ValidationError error)
        {
            if (error != null)
            {
                _errors.Add(error);
                return true;
            }

            return false;
        }

        public bool AddError(ModelException me)
        {
            foreach (ValidationError ve in me.Errors)
            {
                this._errors.Add(ve);
            }

            return me.hasErrors;
        }

        public bool AddError(string field, string message)
        {
            return AddError(new ValidationError(field, message));
        }

        public bool AddError(Exception e)
        {
            return AddError("Unknown", e.Message);
        }
        #endregion

        public bool hasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        public List<ValidationError> Errors
        {
            get
            {
                // TODO: warning this is an exposed list....
                return _errors;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (hasErrors)
            {
                throw this;
            }
        }

        #endregion
    }
}