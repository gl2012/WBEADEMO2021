/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;

namespace WBEADMS.Models
{
    public partial class ChainOfCustody : BaseModel
    {
        SampleType _sampleType;
        List<Sample> _samples;
        List<Sample> _travelBlanks;
        List<Note> _note;
        Dictionary<string, BaseModel> _sections;

        public ChainOfCustody()
            : base("ChainOfCustodys", "chain_of_custody_id", new string[] {
                "chain_of_custody_id",
                "created_by",
                "date_opened",
                "sample_type_id",
                "schedule_id",
                "date_sampling_scheduled",
                "location_id",
                "status_id"})
        {
            _sections = new Dictionary<string, BaseModel>();
        }

        public ChainOfCustody(string chainOfCustodyID)
            : this()
        {
            chain_of_custody_id = chainOfCustodyID;
            LoadData();
        }

        #region Properties
        public PreparationSection Preparation
        {
            get
            {
                if (!_sections.ContainsKey("Prepare"))
                {
                    if (chain_of_custody_id.IsBlank())
                    {
                        _sections["Prepare"] = new PreparationSection(SampleType);
                    }
                    else
                    {
                        _sections["Prepare"] = new PreparationSection(SampleType, chain_of_custody_id);
                    }
                }

                return (PreparationSection)_sections["Prepare"];
            }
        }

        public DeploymentSection Deployment
        {
            get
            {
                if (!_sections.ContainsKey("Deploy"))
                {
                    if (chain_of_custody_id.IsBlank())
                    {
                        _sections["Deploy"] = new DeploymentSection(SampleType);
                    }
                    else
                    {
                        _sections["Deploy"] = new DeploymentSection(SampleType, chain_of_custody_id);
                    }
                }

                return (DeploymentSection)_sections["Deploy"];
            }
        }

        public RetrievalSection Retrieval
        {
            get
            {
                if (!_sections.ContainsKey("Retrieve"))
                {
                    if (chain_of_custody_id.IsBlank())
                    {
                        _sections["Retrieve"] = new RetrievalSection(SampleType);
                    }
                    else
                    {
                        _sections["Retrieve"] = new RetrievalSection(SampleType, chain_of_custody_id);
                    }
                }

                return (RetrievalSection)_sections["Retrieve"];
            }
        }

        public ShippingSection Shipping
        {
            get
            {
                if (!_sections.ContainsKey("Ship"))
                {
                    if (chain_of_custody_id.IsBlank())
                    {
                        _sections["Ship"] = new ShippingSection(SampleType);
                    }
                    else
                    {
                        _sections["Ship"] = new ShippingSection(SampleType, chain_of_custody_id);
                    }
                }

                return (ShippingSection)_sections["Ship"];
            }
        }

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

        public string status_id
        {
            get
            {
                return _data["status_id"];
            }

            set
            {
                _data["status_id"] = value;
            }
        }

        public Status Status
        {
            get
            {
                return LoadRelated<Status>();
            }

            private set
            {
                _data["status_id"] = value.id;
            }
        }

        public string created_by
        {
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

        public string date_opened
        {
            get
            {
                return _data["date_opened"];
            }
            set
            {
                _data["date_opened"] = value;
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

        public SampleType SampleType
        {
            get
            {
                if (_sampleType == null && !String.IsNullOrEmpty(sample_type_id))
                {
                    _sampleType = new SampleType(sample_type_id);
                }

                return _sampleType;
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

        public string location_id
        {
            set
            {
                _data["location_id"] = value;
            }
            get
            {
                return _data["location_id"];
            }
        }
        public string shipped_by
        {
            set
            {
                _data["shipped_by"] = value;
            }
            get
            {
                return _data["shipped_by"];
            }
        }
        public List<Sample> Samples
        {
            get
            {

                if (_samples == null)
                {
                    LoadRelatedSamples();
                }

                return _samples;
            }
        }

        public List<Sample> TravelBlanks
        {
            get
            {

                if (_travelBlanks == null)
                {
                    LoadRelatedSamples();
                }

                return _travelBlanks;
            }
        }
        public List<Note> Note
        {
            get
            {

                if (_note == null)
                {
                    LoadRelatedNote();
                }

                return _note;
            }
        }
        private void LoadRelatedNote()
        {
            // This abuses the fact that we are check on null to populate this list.

            _note = new List<Note>();

            foreach (Note n in LoadRelatedJoin<Note>("Notes_ChainOfCustodys"))
            {
                _note.Add(n);
            }
        }

        private void LoadRelatedSamples()
        {
            // This abuses the fact that we are check on null to populate this list.
            _travelBlanks = new List<Sample>();
            _samples = new List<Sample>();

            foreach (Sample sample in LoadRelatedJoin<Sample>())
            {
                if (sample.is_travel_blank.ToBool())
                {
                    _travelBlanks.Add(sample);
                }
                else
                {
                    _samples.Add(sample);
                }
            }
        }

        public string collecting_duplicate
        {
            get
            {
                return "false";
            }
        }

        public Note[] Notes
        {
            get
            {
                var note_list = LoadRelatedJoin<Note>("Notes_ChainOfCustodys");
                Array.Sort(note_list);
                return note_list;
            }
        }
        #endregion
        private List<string> GetCOCId_Schedule()
        {
            if (String.IsNullOrEmpty(schedule_id))
                return null;
            else
                return BaseModel.FetchList("select  chain_of_custody_id from chainofcustodys where schedule_id =" + schedule_id, "chain_of_custody_id");
        }

        public string Previous()
        {
            int currIndex = 0;
            string preCOCId = null;
            if (GetCOCId_Schedule().Count > 0)
            {
                currIndex = GetCOCId_Schedule().IndexOf(chain_of_custody_id);
                if (currIndex > 0)
                    preCOCId = GetCOCId_Schedule()[currIndex - 1];
            }

            return preCOCId;
        }
        public string Next()
        {
            int currIndex = 0;
            string NextCOCId = null;
            if (GetCOCId_Schedule().Count > 0)
            {
                currIndex = GetCOCId_Schedule().IndexOf(chain_of_custody_id);
                if (currIndex < GetCOCId_Schedule().Count - 1)
                    NextCOCId = GetCOCId_Schedule()[currIndex + 1];
            }

            return NextCOCId;
        }

        public override void Validate()
        {
            ////throw new NotImplementedException("TODO: Implement validation for commit of CoC.  Replace ValidatePreparation() ?");
        }

        public void ValidatePreparation(bool isStrict)
        {
            ModelException errors = new ModelException();

            /* created_by 
             *   cannot be null.
             *   must be an integer.

             * sample_type_id 
             *   cannot be null
             *   must be an integer.
             */

            if (errors.hasErrors)
            {
                throw errors;
            }
        }

        public override void Save()
        {
            throw new NotSupportedException("You cannot use save method use save(sectionName) instead");
        }

        /// <summary>Adds a related Sample.  Save the samples to the join table with SaveRelatedSamples().</summary>
        public void AddSample(string id)
        {
            AddToRelatedList<Sample>(id);
        }

        public void RemoveSample(string id)
        {
            RemoveFromRelatedList<Sample>(id);
        }

        /// <summary>Saves a related Samples.  Set the IDs with AddRelatedSample().</summary>
        public void SaveRelatedSamples()
        {
            SaveRelatedJoin<Sample>();
        }

        public void Create()
        {

            if (!status_id.IsBlank())
            {
                throw new Exception("Chain of custody has already been created. Use Save(sectionName) instead");
            }

            Status = Status.Opened;
            base.Save();
        }
        public void CreatePassiveSampleCoc()
        {


            //  Status = Status.Opened;
            base.Save();
        }

        public void Save(BaseSection section, User user)
        {
            if (section == null)
            {
                return;
            }

            section.Save(user);

            if (!Status.IsComplete)
            {
                Status = Status.NextState(section.SectionName, false);
            }
            else
            {
                throw new NotSupportedException("Invalid call. Use Commit() instead.");
            }

            base.Save();
        }

        public void Commit(BaseSection section, User user)
        {
            if (section == null)
            {
                return;
            }

            section.Commit(user);

            // TODO: Fix this for HISTORICAL
            /*
            if (Status.IsLastStatusBeforeCommitted || Status.IsComplete) {
                Validate();
            }
            */

            if (!Status.IsComplete)
            {
                Status = Status.NextState(section.SectionName, true);
            }

            base.Save();
        }

        public void CommitEdits(BaseSection section, User user)
        {
            if (section == null)
            {
                return;
            }

            section.CommitEdits(user);

            base.Save();
        }

        public List<string> GetComponents()
        {
            List<string> components = new List<string>();
            Status statusMarker = Status;

            while (statusMarker != null)
            {
                if (Status.SectionName == statusMarker.SectionName)
                {
                    if (!statusMarker.form_view.IsBlank())
                    {
                        components.Add(statusMarker.form_view);
                    }
                }
                else if (!statusMarker.details_view.IsBlank())
                {
                    components.Insert(0, statusMarker.details_view);
                }

                statusMarker = statusMarker.PreviousState();
            }

            return components;
        }

        public List<string> GetDetailComponents()
        {
            List<string> components = new List<string>();
            Status statusMarker = Status;

            while (statusMarker != null)
            {
                if (Status.SectionName != statusMarker.SectionName)
                {
                    if (!statusMarker.details_view.IsBlank())
                    {
                        components.Insert(0, statusMarker.details_view);
                    }
                }

                statusMarker = statusMarker.PreviousState();
            }

            return components;
        }

        public override string ToString()
        {
            return id.ToString();
        }


    }
}