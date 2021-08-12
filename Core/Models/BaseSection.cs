/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Specialized;

namespace WBEADMS.Models
{
    public abstract class BaseSection : BaseModel
    {
        SampleType _sampleType;
        string _sectionName;
        NameValueCollection _invalidData;

        protected BaseSection(SampleType sampleType, string sectionName, string[] fields) : this(sampleType, sectionName, null, null, fields) { }

        protected BaseSection(SampleType sampleType, string sectionName, string tableName, string primaryKey, string[] fields)
            : base(tableName, primaryKey, fields)
        {
            _sampleType = sampleType;
            _sectionName = sectionName;
            _invalidData = new NameValueCollection();
        }

        public SampleType SampleType
        {
            get
            {
                return _sampleType;
            }
        }

        public string SectionName
        {
            get
            {
                return _sectionName;
            }
        }

        public abstract void CommittedBy(string userId);

        protected abstract void ValidateStrict();

        public override void Save()
        {
            throw new NotSupportedException("This method should not be called use Save(User user) instead");
        }

        public void Save(User user)
        {
            ModelException errors = new ModelException();

            // do a standard validation, this will blank all invalid fields.
            try
            {
                Validate();
            }
            catch (ModelException me)
            {
                errors.AddError(me);
            }

            // removed remove all invalid fields.
            foreach (ValidationError validationError in errors.Errors)
            {
                InvalidDataField(validationError.Field);
            }

            // save all fields that are valid.
            SaveEdits();

            // update the Audit Log for all valid fields
            // Modified: we only update the audit log on edits after a commit has taken place.
            // UpdateSectionAuditLog(user.user_id);

            // restore invalid data fields for good user experience.
            RestoreInvalidDataFields();

            if (errors.hasErrors)
            {
                throw errors;
            }
        }

        public virtual void Commit(User user)
        {
            ModelException errors = new ModelException();

            // do a standard validation, this will blank all invalid fields.
            try
            {
                Validate();
            }
            catch (ModelException me)
            {
                errors.AddError(me);
            }

            // do a strict validation, add any errors to the list from validate.
            try
            {
                ValidateStrict();
            }
            catch (ModelException me)
            {
                errors.AddError(me);
            }

            // removed remove all invalid fields.
            foreach (ValidationError validationError in errors.Errors)
            {
                InvalidDataField(validationError.Field);
            }

            // save all fields that are valid.
            SaveEdits();

            // restore invalid data fields for good user experience.
            RestoreInvalidDataFields();

            if (errors.hasErrors)
            {
                throw errors;
            }
        }

        public virtual void CommitEdits(User user)
        {
            ModelException errors = new ModelException();

            // do a standard validation, this will blank all invalid fields.
            try
            {
                Validate();
            }
            catch (ModelException me)
            {
                errors.AddError(me);
            }

            // do a strict validation, add any errors to the list from validate.
            try
            {
                ValidateStrict();
            }
            catch (ModelException me)
            {
                errors.AddError(me);
            }

            // removed remove all invalid fields.
            foreach (ValidationError validationError in errors.Errors)
            {
                InvalidDataField(validationError.Field);
            }

            // save all fields that are valid.
            SaveEdits();

            // update the Audit Log for all valid fields
            UpdateSectionAuditLog(user.user_id);

            // restore invalid data fields for good user experience.
            RestoreInvalidDataFields();

            if (errors.hasErrors)
            {
                throw errors;
            }
        }

        protected virtual void RestoreInvalidDataFields()
        {
            if (_invalidData.Count > 0)
            {
                foreach (string key in _invalidData.Keys)
                {
                    _data[key] = _invalidData[key];
                }
            }

            _invalidData.Clear();
        }

        protected virtual void InvalidDataField(string key)
        {
            _invalidData[key] = _data[key];
            _data.Remove(key);
        }

        public abstract void UpdateSectionAuditLog(string user_id);

        ///<summary>A utility method to check strings for NA or N/A</summary>
        protected static bool IsNA(string s)
        {
            if (s == null) return false;
            s = s.ToLower();
            return s == "na" || s == "n/a" || s == "";
        }
    }
}