/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;

namespace WBEADMS.Models
{
    public class DeploymentSection : BaseSection
    {
        private static readonly string[] Fields = new string[] {
            "chain_of_custody_id",
            "date_deployed",
            "schedule_id",
            "voc_cannister_pressure",
            "voc_cannister_pressure_unit",
            "created_by",
            "deployed_by",
            "deployment_initials",
            "location_id",
            "other_location",
            "date_sampler_last_calibrated",
            "date_sampler_last_leak_checked",
            "leak_check",
            "leak_check_unit",
            "date_sampling_head_cleaned_on",
            "sampler_item_id",
            "sampler_flowrate",
            "sampler_flowrate_unit",
            "sampler_setpoint",
            "date_sample_start",
            "date_sample_end",
            "collecting_duplicate",
            "travel_blank_present",
            "voc_valve_open",
            "voc_cannister_pressure_after_connection",
            "voc_cannister_pressure_after_connection_unit",
            "voc_sampler_pressure_after_connection",
            "voc_sampler_pressure_after_connection_unit"};

        Item _sampler;
        int? _sampleCount;

        public DeploymentSection(SampleType sampleType) : base(sampleType, "Deploy", "ChainOfCustodys", "chain_of_custody_id", DeploymentSection.Fields) { }

        public DeploymentSection(SampleType sampleType, string chainOfCustodyID)
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

        public string voc_cannister_pressure
        {
            get
            {
                return _data["voc_cannister_pressure"];
            }

            set
            {
                _data["voc_cannister_pressure"] = value;
            }
        }

        public string voc_cannister_pressure_unit
        {
            get
            {
                return _data["voc_cannister_pressure_unit"];
            }

            set
            {
                _data["voc_cannister_pressure_unit"] = value;
            }
        }

        public Unit VOCCannisterPressureUnit
        {
            get
            {
                return LoadRelated<Unit>("voc_cannister_pressure_unit");
            }
        }

        public string date_deployed
        {
            get
            {
                return _data["date_deployed"];
            }

            set
            {
                _data["date_deployed"] = value;
            }
        }

        public string deployment_initials
        {
            get
            {
                return _data["deployment_initials"];
            }

            set
            {
                _data["deployment_initials"] = value;
            }
        }

        public string deployed_by
        {
            get
            {
                return _data["deployed_by"];
            }

            private set
            {
                _data["deployed_by"] = value;
            }
        }

        public User DeployedBy
        {
            get
            {
                return LoadRelated<User>("deployed_by");
            }
        }

        public string location_id
        {
            get
            {
                return _data["location_id"];
            }

            set
            {
                _data["location_id"] = value;
            }
        }

        public Location Location
        {
            get
            {
                return LoadRelated<Location>();
            }
        }

        public string other_location
        {
            get
            {
                return _data["other_location"];
            }

            set
            {
                _data["other_location"] = value;
            }
        }

        public string sampler_item_id
        {
            get
            {
                return _data["sampler_item_id"];
            }

            set
            {
                _data["sampler_item_id"] = value;
            }
        }

        public Item Sampler
        {
            get
            {
                if (_sampler == null && !String.IsNullOrEmpty(sampler_item_id))
                {
                    _sampler = Item.Load(sampler_item_id);
                }

                return _sampler;
            }
        }

        public string date_sampler_last_calibrated
        {
            get
            {
                return _data["date_sampler_last_calibrated"];
            }

            set
            {
                _data["date_sampler_last_calibrated"] = value;
            }
        }

        public string date_sampler_last_leak_checked
        {
            get
            {
                return _data["date_sampler_last_leak_checked"];
            }

            set
            {
                _data["date_sampler_last_leak_checked"] = value;
            }
        }

        public string leak_check
        {
            get
            {
                return _data["leak_check"];
            }
            set
            {
                _data["leak_check"] = value;
            }
        }

        public string leak_check_unit
        {
            get
            {
                return _data["leak_check_unit"];
            }
            set
            {
                _data["leak_check_unit"] = value;
            }
        }

        public string date_sampling_head_cleaned_on
        {
            get
            {
                return _data["date_sampling_head_cleaned_on"];
            }

            set
            {
                _data["date_sampling_head_cleaned_on"] = value;
            }
        }

        public string sampler_flowrate
        {
            get
            {
                return _data["sampler_flowrate"];
            }

            set
            {
                _data["sampler_flowrate"] = value;
            }
        }

        public string sampler_flowrate_unit
        {
            get
            {
                return _data["sampler_flowrate_unit"];
            }

            set
            {
                _data["sampler_flowrate_unit"] = value;
            }
        }

        public Unit LeakCheckFlowRateUnit
        {
            get
            {
                return LoadRelated<Unit>("leak_check_unit");
            }
        }

        public Unit SamplerFlowrateUnit
        {
            get
            {
                return LoadRelated<Unit>("sampler_flowrate_unit");
            }
        }

        public string sampler_setpoint
        {
            get
            {
                return _data["sampler_setpoint"];
            }

            set
            {
                _data["sampler_setpoint"] = value;
            }
        }

        public string date_sample_start
        {
            get
            {
                return _data["date_sample_start"];
            }

            set
            {
                _data["date_sample_start"] = value;
            }
        }

        public string date_sample_end
        {
            get
            {
                return _data["date_sample_end"];
            }

            set
            {
                _data["date_sample_end"] = value;
            }
        }

        public string voc_valve_open
        {
            get
            {
                return _data["voc_valve_open"];
            }

            set
            {
                _data["voc_valve_open"] = value;
            }
        }

        public string voc_cannister_pressure_after_connection
        {
            get
            {
                return _data["voc_cannister_pressure_after_connection"];
            }

            set
            {
                _data["voc_cannister_pressure_after_connection"] = value;
            }
        }

        public string voc_cannister_pressure_after_connection_unit
        {
            get
            {
                return _data["voc_cannister_pressure_after_connection_unit"];
            }

            set
            {
                _data["voc_cannister_pressure_after_connection_unit"] = value;
            }
        }

        public Unit VOCCannisterPressureAfterConnection
        {
            get
            {
                return LoadRelated<Unit>("voc_cannister_pressure_after_connection_unit");
            }
        }

        public string voc_sampler_pressure_after_connection
        {
            get
            {
                return _data["voc_sampler_pressure_after_connection"];
            }

            set
            {
                _data["voc_sampler_pressure_after_connection"] = value;
            }
        }

        public string voc_sampler_pressure_after_connection_unit
        {
            get
            {
                return _data["voc_sampler_pressure_after_connection_unit"];
            }

            set
            {
                _data["voc_sampler_pressure_after_connection_unit"] = value;
            }
        }

        public Unit VOCSamplerPressureAfterConnectionUnit
        {
            get
            {
                return LoadRelated<Unit>("voc_sampler_pressure_after_connection_unit");
            }
        }

        public string collecting_duplicate
        {
            get
            {
                return _data["collecting_duplicate"];
            }

            set
            {
                _data["collecting_duplicate"] = value;
            }
        }

        public string travel_blank_present
        {
            get
            {
                return _data["travel_blank_present"];
            }

            set
            {
                _data["travel_blank_present"] = value;
            }
        }

        public int? SampleCount
        {
            get
            {

                if (_sampleCount == null)
                {
                    _sampleCount = LoadRelatedJoin<Sample>().Length;
                }

                return _sampleCount;
            }
        }
        #endregion

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {

                if (!IsNA(voc_cannister_pressure))
                {
                    errors.AddError(voc_cannister_pressure.CheckIfDecimal("voc_cannister_pressure", "VOC Cannister Pressure"));
                    errors.AddError(voc_cannister_pressure_unit.CheckIfInt("voc_cannister_pressure_unit", "VOC Cannister Pressure Unit"));
                }
                errors.AddError(_data["date_deployed"].CheckIfDateTime("date_deployed"));

                errors.AddError(deployed_by.CheckIfInt("deployed_by"));

                errors.AddError(location_id.CheckIfInt("location_id", "Selected Location"));
                if (!location_id.IsBlank() && !other_location.IsBlank())
                {
                    errors.AddError("other_location", "You must select a Location from the drop down or specify an Other Location but not both.");
                }

                errors.AddError(sampler_item_id.CheckIfInt("sampler_item_id", "Selected Sampler"));

                errors.AddError(date_sampler_last_calibrated.CheckIfDateTime("date_sampler_last_calibrated", "Last Calibrated Date"));

                errors.AddError(date_sampler_last_leak_checked.CheckIfDateTime("date_sampler_last_leak_checked", "Date of Last Leak Check"));
                errors.AddError(date_sampling_head_cleaned_on.CheckIfDateTime("date_sampling_head_cleaned_on", "Date of Last Sampling Head Clean"));

                errors.AddError(sampler_flowrate.CheckIfDecimal("sampler_flowrate"));
                errors.AddError(sampler_flowrate_unit.CheckIfInt("sampler_flowrate_unit"));

                if (!IsNA(sampler_setpoint))
                    errors.AddError(sampler_setpoint.CheckIfDecimal("sampler_setpoint"));

                if (!IsNA(leak_check))
                {
                    errors.AddError(leak_check.CheckIfDecimal("leak_check"));
                    errors.AddError(leak_check_unit.CheckIfInt("leak_check_unit"));
                }
                errors.AddError(date_sample_start.CheckIfDateTime("date_sample_start", "Programmed Sample Start Date"));

                errors.AddError(date_sample_end.CheckIfDateTime("date_sample_end", "Programmed Sample End Date"));

                errors.AddError(voc_valve_open.CheckIfBool("voc_valve_open", "The cannister valve open is specified as an invalid data type. Please contact your administrator."));

                if (!IsNA(voc_cannister_pressure_after_connection))
                {
                    errors.AddError(voc_cannister_pressure_after_connection.CheckIfDecimal("voc_cannister_pressure_after_connection", "VOC Cannister Pressure After Connection"));
                    errors.AddError(voc_cannister_pressure_after_connection_unit.CheckIfInt("voc_cannister_pressure_after_connection_unit", "VOC Cannister Pressure After Connection Unit"));
                }

                errors.AddError(voc_sampler_pressure_after_connection.CheckIfDecimal("voc_sampler_pressure_after_connection", "VOC Sampler Pressure After Connection"));
                errors.AddError(voc_sampler_pressure_after_connection_unit.CheckIfDecimal("voc_sampler_pressure_after_connection_unit", "VOC Sampler Pressure After Connection Unit"));

                errors.AddError(collecting_duplicate.CheckIfBool("collecting_duplicate", "Collecting duplicate is an invalid data type. Please contact your administrator."));

                errors.AddError(travel_blank_present.CheckIfBool("travel_blank_present", "Travel blank present is an invalid data type. Please contact your administrator."));
            }
        }

        protected override void ValidateStrict()
        {
            using (ModelException errors = new ModelException())
            {

                if (SampleType == SampleType.VOC)
                {
                    /* removed checked as per Sanjay email 2010-03-15
                    errors.AddError(voc_cannister_pressure.CheckRequired("voc_cannister_pressure", "VOC Cannister Pressure"));
                    errors.AddError(voc_cannister_pressure_unit.CheckRequired("voc_cannister_pressure_unit", "VOC Cannister Pressure Unit"));
                     */
                    if (voc_cannister_pressure.IsBlank())
                    {
                        errors.AddError(new ValidationError("voc_cannister_pressure", "VOC Cannister Pressure cannot be left blank. Defaulted to N/A."));
                    }
                    else if (IsNA(voc_cannister_pressure))
                    {
                        voc_cannister_pressure = string.Empty;
                    }

                    if (!voc_cannister_pressure.IsBlank())
                    {
                        errors.AddError(voc_cannister_pressure_unit.CheckRequired("voc_cannister_pressure_unit", "VOC Cannister Pressure Unit", "cannot be left blank if a pressure is provided."));
                    }
                }

                if (SampleType.name != "PASS")
                {
                    errors.AddError(date_deployed.CheckRequired("date_deployed", "Deployment Date"));
                    errors.AddError(deployed_by.CheckRequired("deployed_by", "User Deployed"));
                }

                if (Schedule != null)
                {
                    // * if there is a associated schedule then the location_id must match that schedule.
                    if (!Schedule.location_id.IsBlank() && Schedule.location_id != location_id)
                    {
                        errors.AddError("location_id", "Location specified must match the associated schedule. Please contact your administrator.");
                    }
                }
                else
                {
                    if (other_location.IsBlank())
                    {
                        errors.AddError(location_id.CheckRequired("location_id", "Location"));
                    }
                }

                if (SampleType != SampleType.PASS)
                {
                    errors.AddError(sampler_item_id.CheckRequired("sampler_item_id", "Sampler"));
                }

                if (SampleType != SampleType.PASS)
                {
                    errors.AddError(date_sampler_last_calibrated.CheckRequired("date_sampler_last_calibrated", "Sampler Last Calibrated Date"));
                    errors.AddError(date_sampler_last_leak_checked.CheckRequired("date_sampler_last_leak_checked", "Sampler Last Leak Check Date"));
                }

                if (SampleType != SampleType.VOC && SampleType != SampleType.PRECIP && SampleType != SampleType.PASS)
                {
                    errors.AddError(date_sampling_head_cleaned_on.CheckRequired("date_sampling_head_cleaned_on", "Sampler Head Last Clean Date"));
                }

                if (SampleType != SampleType.PASS && SampleType != SampleType.PRECIP)
                {
                    errors.AddError(sampler_flowrate.CheckRequired("sampler_flowrate"));
                    errors.AddError(sampler_flowrate_unit.CheckRequired("sampler_flowrate_unit"));
                    /* removed checked as per Sanjay email 2010-03-15
                    errors.AddError(sampler_setpoint.CheckRequired("sampler_setpoint"));
                     */
                    if (sampler_setpoint.IsBlank())
                    {
                        errors.AddError(new ValidationError("sampler_setpoint", "Sampler Setpoint cannot be left blank. Defaulted to N/A."));
                    }
                    else if (IsNA(sampler_setpoint))
                    {
                        sampler_setpoint = string.Empty;
                    }
                }
                if (SampleType == SampleType.PAH)
                {
                    if (leak_check.IsBlank())
                    {
                        errors.AddError(new ValidationError("leak_check", "Leak Check cannot be left blank. Defaulted to N/A."));
                    }
                    else if (IsNA(leak_check))
                    {
                        leak_check = string.Empty;
                    }
                }

                bool hasStartDateTime, hasEndDateTime;
                hasStartDateTime = !errors.AddError(date_sample_start.CheckRequired("date_sample_start", "Programmed Sample Start Date"));

                hasEndDateTime = !errors.AddError(date_sample_end.CheckRequired("date_sample_end", "Programmed Sample End Date"));

                if (hasStartDateTime && hasEndDateTime)
                {
                    if (date_sample_start.ToDateTime() >= date_sample_end.ToDateTime())
                    {
                        errors.AddError("date_sample_end", "Programmed Sample End Date must be greater then Programmed Sample Start Date");
                    }
                }

                //voc_valve_open
                // * cannot be null         
                if (SampleType == SampleType.VOC)
                {
                    if (!errors.AddError(voc_valve_open.CheckRequired("voc_valve_open", "VOC Valve Open")))
                    {
                        // * must be set to 'Yes'.
                        if (voc_valve_open != "True")
                        {
                            errors.AddError("voc_valve_open", "You must open the valve on the cannister before you can commit.");
                        }
                    }
                }

                if (SampleType == SampleType.VOC)
                {
                    /* removed checked as per Sanjay email 2010-03-15
                    errors.AddError(voc_cannister_pressure_after_connection.CheckRequired("voc_cannister_pressure_after_connection", "VOC Cannister Pressure After Connection"));
                    errors.AddError(voc_cannister_pressure_after_connection_unit.CheckRequired("voc_cannister_pressure_after_connection_unit", "VOC Cannister Pressure After Connection Unit"));
                     */
                    if (voc_cannister_pressure_after_connection.IsBlank())
                    {
                        errors.AddError(new ValidationError("voc_cannister_pressure_after_connection", "VOC Cannister Pressure After Connection cannot be blank. Defaulted to N/A."));
                    }
                    else if (IsNA(voc_cannister_pressure_after_connection))
                    {
                        voc_cannister_pressure_after_connection = string.Empty;
                    }

                    if (!voc_cannister_pressure_after_connection.IsBlank())
                    {
                        errors.AddError(voc_cannister_pressure_after_connection_unit.CheckRequired("voc_cannister_pressure_after_connection_unit", "VOC Cannister Pressure After Connection Unit", "cannot be left blank if a pressure is provided."));
                    }

                    errors.AddError(voc_sampler_pressure_after_connection.CheckRequired("voc_sampler_pressure_after_connection", "VOC Sampler Pressure After Connection"));
                    errors.AddError(voc_sampler_pressure_after_connection_unit.CheckRequired("voc_sampler_pressure_after_connection_unit", "VOC Sampler Pressure After Connection Unit"));
                }

                if (SampleType == SampleType.VOC || SampleType == SampleType.PASS)
                {
                    errors.AddError(collecting_duplicate.CheckRequired("collecting_duplicate"));
                }

                if (SampleType == SampleType.PM10 || SampleType == SampleType.PM10E || SampleType == SampleType.PM25 || SampleType == SampleType.PASS)
                {
                    errors.AddError(travel_blank_present.CheckRequired("travel_blank_present"));
                }

                //SAMPLES
                // * must have at least one sample associated with the COC
                if (SampleCount.Value == 0)
                {
                    errors.AddError("Sample Count", "You must have at least on associated sample before you can commit.");
                }
            }
        }

        public override void CommittedBy(string userId)
        {
            deployed_by = userId;
        }

        public override void UpdateSectionAuditLog(string user_id)
        {
            List<string> ignore = new List<string>();
            base.UpdateAuditLog(ignore, user_id);
        }
    }
}