/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;

namespace WBEADMS.Models
{
    public class PreparationSection : BaseSection
    {
        private static readonly string[] CocFields = new string[] {
            "chain_of_custody_id",
            "created_by",
            "sample_type_id",
            "schedule_id",
            "date_sampling_scheduled"};

        SampleType _sampleType;
        DateTime? _dateSamplingScheduled;

        public PreparationSection(SampleType sampleType) : base(sampleType, "Prepare", "ChainOfCustodys", "chain_of_custody_id", PreparationSection.CocFields) { }

        public PreparationSection(SampleType sampleType, string chainOfCustodyID)
            : this(sampleType)
        {
            chain_of_custody_id = chainOfCustodyID;
            LoadData();
        }

        #region Properties
        public string chain_of_custody_id
        {
            get
            {
                return _data["chain_of_custody_id"];
            }

            private set
            {
                _data["chain_of_custody_id"] = value;
            }
        }

        public string status
        {
            get
            {
                return _data["status"];
            }

            set
            {
                _data["status"] = value;
            }
        }

        public string created_by
        {
            get
            {
                return _data["created_by"];
            }

            set
            {
                _data["created_by"] = value;
            }
        }

        public User UserCreated
        {
            get
            {
                return LoadRelated<User>("created_by");
            }
        }

        public string sample_type_id
        {
            get
            {
                return _data["sample_type_id"];
            }

            set
            {
                _data["sample_type_id"] = value;
            }
        }

        public string schedule_id
        {
            get
            {
                return _data["schedule_id"];
            }

            set
            {
                _data["schedule_id"] = value;
            }
        }

        public Schedule Schedule
        {
            get
            {
                return LoadRelated<Schedule>();
            }
        }

        public string date_sampling_scheduled
        {
            get
            {
                return _data["date_sampling_scheduled"];
            }

            set
            {
                _data["date_sampling_scheduled"] = value;
            }
        }

        public DateTime? ScheduledSamplingDate
        {
            get
            {
                if (!_dateSamplingScheduled.HasValue && !date_sampling_scheduled.IsBlank())
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(date_sampling_scheduled, out parsedDate))
                    {
                        _dateSamplingScheduled = parsedDate;
                    }
                }

                return _dateSamplingScheduled;
            }
        }
        #endregion

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                errors.AddError(date_sampling_scheduled.CheckIfDateTime("date_sampling_scheduled", "Sample Schedule Date"));
            }
        }

        protected override void ValidateStrict()
        {
            using (ModelException errors = new ModelException())
            {
                errors.AddError(date_sampling_scheduled.CheckRequired("date_sampling_scheduled", "Sample Schedule Date"));

                // Make sure at least 1 sample has been added to the CoC before saving
                //int numSamples = 
                ChainOfCustody coc = ChainOfCustody.Load(chain_of_custody_id);
                if (coc.Samples.Count == 0)
                {
                    errors.AddError("sample_count", "Please add at least one sample to the Chain of Custody.");
                }
            }
        }

        public override void CommittedBy(string userId)
        {
            //not needed do nothing.
        }

        public override void UpdateSectionAuditLog(string user_id)
        {
            base.UpdateAuditLog(user_id);
        }
    }
}