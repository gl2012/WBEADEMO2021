/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;

namespace WBEADMS.Models
{
    public class RetrievalSection : BaseSection
    {
        private static readonly string[] CocFields = new string[] {
            "chain_of_custody_id",
            "elapsed_sampling_duration",
            "sample_volume",
            "sample_volume_unit",
            "date_actual_sample_start",
            "date_actual_sample_end",
            "date_secondary_sample_start",
            "date_secondary_sample_end",
            "average_station_temperature",
            "average_station_temperature_unit",
            "average_ambient_temperature",
            "average_ambient_temperature_unit",
            "average_barometric_pressure",
            "average_barometric_pressure_unit",
            "average_relative_humidity",
            "voc_final_cannister_pressure",
            "voc_final_cannister_pressure_unit",
            "voc_final_sampler_pressure",
            "voc_final_sampler_pressure_unit",
            "voc_valve_closed",
            "date_sample_retrieved",
            "retrieval_initials",
            "retrieved_by",
            "field_user_flag",
            "wet_week"};

        public static System.Collections.Generic.Dictionary<string, string> FieldUserFlags
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> retVal = new System.Collections.Generic.Dictionary<string, string>();
                string[] flags = System.Configuration.ConfigurationManager.AppSettings["FieldUserFlags"].Split(new string[] { ";$;" }, StringSplitOptions.None);
                foreach (string flag in flags)
                {
                    string[] pair = flag.Split(new string[] { ";*;" }, StringSplitOptions.None);
                    if (pair.Length >= 2)
                    {
                        retVal.Add(pair[0], pair[0] + " - " + pair[1]);
                    }
                }
                return retVal;
            }
            private set { }
        }

        User _retrievedBy;
        TimeSpan? _elapsedSamplingDuration;
        int? _sampleCount;

        public RetrievalSection(SampleType sampleType) : base(sampleType, "Retrieve", "ChainOfCustodys", "chain_of_custody_id", RetrievalSection.CocFields) { }

        public RetrievalSection(SampleType sampleType, string chainOfCustodyID)
            : this(sampleType)
        {
            chain_of_custody_id = chainOfCustodyID;
            LoadData();
        }

        #region Properties

        public string field_user_flag
        {
            get
            {
                return _data["field_user_flag"];
            }
            set
            {
                _data["field_user_flag"] = value;
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

        public string elapsed_sampling_duration
        {
            get
            {
                return _data["elapsed_sampling_duration"];
            }

            set
            {
                _data["elapsed_sampling_duration"] = value;
            }
        }

        string elapsedHours;
        public string elapsed_hours
        {
            get
            {
                return elapsedHours;
            }
            set
            {
                elapsedHours = value;
            }
        }

        string elapsedMinutes;
        public string elapsed_minutes
        {
            get
            {
                return elapsedMinutes;
            }
            set
            {
                elapsedMinutes = value;
            }
        }

        string elapsedSeconds;
        public string elapsed_seconds
        {
            get
            {
                return elapsedSeconds;
            }
            set
            {
                elapsedSeconds = value;
            }
        }

        public TimeSpan? ElapsedSamplingDuration
        {
            get
            {
                if (!_elapsedSamplingDuration.HasValue && !elapsed_sampling_duration.IsBlank())
                {
                    int seconds;
                    if (int.TryParse(elapsed_sampling_duration, out seconds))
                    {
                        _elapsedSamplingDuration = new TimeSpan(0, 0, seconds);
                    }
                }

                return _elapsedSamplingDuration;
            }
        }

        public string sample_volume
        {
            get
            {
                return _data["sample_volume"];
            }

            set
            {
                _data["sample_volume"] = value;
            }
        }

        public string sample_volume_unit
        {
            get
            {
                return _data["sample_volume_unit"];
            }

            set
            {
                _data["sample_volume_unit"] = value;
            }
        }

        public Unit SampleVolumeUnit
        {
            get
            {
                return LoadRelated<Unit>("sample_volume_unit");
            }
        }

        public string date_actual_sample_start
        {
            get
            {
                return _data["date_actual_sample_start"];
            }

            set
            {
                _data["date_actual_sample_start"] = value;
            }
        }

        public string date_actual_sample_end
        {
            get
            {
                return _data["date_actual_sample_end"];
            }

            set
            {
                _data["date_actual_sample_end"] = value;
            }
        }

        public string date_secondary_sample_start
        {
            get
            {
                return _data["date_secondary_sample_start"];
            }

            set
            {
                _data["date_secondary_sample_start"] = value;
            }
        }

        public string date_secondary_sample_end
        {
            get
            {
                return _data["date_secondary_sample_end"];
            }

            set
            {
                _data["date_secondary_sample_end"] = value;
            }
        }

        public string average_station_temperature
        {
            get
            {
                return _data["average_station_temperature"];
            }

            set
            {
                _data["average_station_temperature"] = value;
            }
        }

        public string average_station_temperature_unit
        {
            get
            {
                return _data["average_station_temperature_unit"];
            }

            set
            {
                _data["average_station_temperature_unit"] = value;
            }
        }

        public Unit AverageStationTemperatureUnit
        {
            get
            {
                return LoadRelated<Unit>("average_station_temperature_unit");
            }
        }

        public string average_ambient_temperature
        {
            get
            {
                return _data["average_ambient_temperature"];
            }

            set
            {
                _data["average_ambient_temperature"] = value;
            }
        }

        public string average_ambient_temperature_unit
        {
            get
            {
                return _data["average_ambient_temperature_unit"];
            }

            set
            {
                _data["average_ambient_temperature_unit"] = value;
            }
        }

        public Unit AverageAmbientTemperatureUnit
        {
            get
            {
                return LoadRelated<Unit>("average_ambient_temperature_unit");
            }
        }

        public string average_barometric_pressure
        {
            get
            {
                return _data["average_barometric_pressure"];
            }

            set
            {
                _data["average_barometric_pressure"] = value;
            }
        }

        public string average_barometric_pressure_unit
        {
            get
            {
                return _data["average_barometric_pressure_unit"];
            }

            set
            {
                _data["average_barometric_pressure_unit"] = value;
            }
        }

        public Unit AverageBarometricPressureUnit
        {
            get
            {
                return LoadRelated<Unit>("average_barometric_pressure_unit");
            }
        }

        public string average_relative_humidity
        {
            get
            {
                return _data["average_relative_humidity"];
            }

            set
            {
                _data["average_relative_humidity"] = value;
            }
        }

        public string voc_final_cannister_pressure
        {
            get
            {
                return _data["voc_final_cannister_pressure"];
            }

            set
            {
                _data["voc_final_cannister_pressure"] = value;
            }
        }

        public string voc_final_cannister_pressure_unit
        {
            get
            {
                return _data["voc_final_cannister_pressure_unit"];
            }

            set
            {
                _data["voc_final_cannister_pressure_unit"] = value;
            }
        }

        public Unit VOCFinalCannisterPressureUnit
        {
            get
            {
                return LoadRelated<Unit>("voc_final_cannister_pressure_unit");
            }
        }

        public string voc_final_sampler_pressure
        {
            get
            {
                return _data["voc_final_sampler_pressure"];
            }

            set
            {
                _data["voc_final_sampler_pressure"] = value;
            }
        }

        public string voc_final_sampler_pressure_unit
        {
            get
            {
                return _data["voc_final_sampler_pressure_unit"];
            }

            set
            {
                _data["voc_final_sampler_pressure_unit"] = value;
            }
        }

        public Unit VOCFinalSamplerPressureUnit
        {
            get
            {
                return LoadRelated<Unit>("voc_final_sampler_pressure_unit");
            }
        }

        public string voc_valve_closed
        {
            get
            {
                return _data["voc_valve_closed"];
            }

            set
            {
                _data["voc_valve_closed"] = value;
            }
        }

        public string retrieved_by
        {
            get
            {
                return _data["retrieved_by"];
            }

            set
            {
                _data["retrieved_by"] = value;
            }
        }

        public User RetrievedBy
        {
            get
            {
                if (_retrievedBy == null && !String.IsNullOrEmpty(retrieved_by))
                {
                    _retrievedBy = new User(retrieved_by);
                }

                return _retrievedBy;
            }
        }

        public string date_sample_retrieved
        {
            get
            {
                return _data["date_sample_retrieved"];
            }

            set
            {
                _data["date_sample_retrieved"] = value;
            }
        }

        public string retrieval_initials
        {
            get
            {
                return _data["retrieval_initials"];
            }

            set
            {
                _data["retrieval_initials"] = value;
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

        public string wet_week
        {
            get
            {
                return _data["wet_week"];
            }
            set
            {
                _data["wet_week"] = value;
            }
        }
        #endregion

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {

                //elapsed_sampling_duration
                if (!errors.AddError(elapsed_hours.CheckIfDecimal("elapsed_hours")))
                {
                    errors.AddError(elapsed_hours.CheckIfDecimalAndPositive("elapsed_hours"));
                }

                if (!errors.AddError(elapsed_minutes.CheckIfInt("elapsed_minutes")))
                {
                    errors.AddError(elapsed_minutes.CheckIfIntAndPositive("elapsed_minutes"));
                }

                if (!errors.AddError(elapsed_seconds.CheckIfInt("elapsed_seconds")))
                {
                    errors.AddError(elapsed_seconds.CheckIfIntAndPositive("elapsed_seconds"));
                }

                // NOTE: This is a bit of a hack as code order matters. 
                // Elapsed time is validated first. So if there are any errors I don't want to update the 
                // Elapsed time in the back end. 
                // TODO: find a better way to do this.
                if (!errors.hasErrors)
                {
                    errors.AddError(UpdateElapsedDuration());
                }

                errors.AddError(sample_volume.CheckIfDecimal("sample_volume"));
                errors.AddError(sample_volume_unit.CheckIfInt("sample_volume_unit"));

                errors.AddError(sample_volume.CheckIfDecimal("average_station_temperature"));

                errors.AddError(date_actual_sample_start.CheckIfDateTime("date_actual_sample_start", "Actual Start Date"));
                errors.AddError(date_actual_sample_end.CheckIfDateTime("date_actual_sample_end", "Actual End Date"));
                errors.AddError(date_secondary_sample_start.CheckIfDateTime("date_secondary_sample_start", "Secondary Start Date"));
                errors.AddError(date_secondary_sample_end.CheckIfDateTime("date_secondary_sample_end", "Secondary End Date"));

                errors.AddError(average_station_temperature.CheckIfDecimal("average_station_temperature"));
                errors.AddError(average_ambient_temperature.CheckIfDecimal("average_ambient_temperature"));
                errors.AddError(average_barometric_pressure.CheckIfDecimal("average_barometric_pressure"));
                errors.AddError(average_relative_humidity.CheckIfDecimal("average_relative_humidity"));
                if (!IsNA(voc_final_cannister_pressure))
                    errors.AddError(voc_final_cannister_pressure.CheckIfDecimal("voc_final_cannister_pressure", "VOC Final Cannister Pressure"));
                errors.AddError(voc_final_sampler_pressure.CheckIfDecimal("voc_final_sampler_pressure", "VOC Final Sampler Pressure"));

                if (SampleType == SampleType.VOC)
                {
                    errors.AddError(voc_valve_closed.CheckIfBool("voc_valve_closed", "The cannister valve closed is specified as an invalid data type. Please contact your administrator."));
                }

                errors.AddError(retrieved_by.CheckIfInt("retrieved_by"));
                errors.AddError(date_sample_retrieved.CheckIfDateTime("date_sample_retrieved", "Retrieval Date"));
            }
        }

        protected override void ValidateStrict()
        {
            using (ModelException errors = new ModelException())
            {

                //elapsed_sampling_duration
                //This assums that validation has already run UpdateElapsedDuration.
                if (SampleType != SampleType.PRECIP && SampleType != SampleType.PASS)
                {
                    if (!errors.AddError(elapsed_sampling_duration.CheckRequired("elapsed_sampling_duration")))
                    {
                        if (elapsed_sampling_duration == "0")
                        {
                            errors.AddError(new ValidationError("elapsed_sampling_duration", "Elapsed Sampling Duration must have a value greater then 0"));
                        }
                    }
                }

                //sample_volume
                // * cannot be null
                if (SampleType != SampleType.VOC && SampleType != SampleType.PASS && SampleType != SampleType.PRECIP)
                {
                    errors.AddError(sample_volume.CheckRequired("sample_volume"));
                    errors.AddError(sample_volume_unit.CheckRequired("sample_volume_unit"));
                }

                bool hasActualStart;
                bool hasActualEnd;
                bool hasSecondaryStart;
                bool hasSecondaryEnd;

                hasActualStart = !errors.AddError(date_actual_sample_start.CheckRequired("date_actual_sample_start", "Actual Sample Start Date"));
                hasActualEnd = !errors.AddError(date_actual_sample_end.CheckRequired("date_actual_sample_end", "Actual Sample End Date"));
                hasSecondaryStart = !date_secondary_sample_start.IsBlank(); // Note: if it is not blank it has been validated to be a DateTime in Validate()
                hasSecondaryEnd = !date_secondary_sample_end.IsBlank(); // Note: if it is not blank it has been validated to be a DateTime in Validate()
                /*
                if (date_secondary_sample_start.IsBlank() && !date_secondary_sample_end.IsBlank()) {
                    //missing secondary start date.
                    errors.AddError("date_secondary_sample_start", "If you have a secondary sample end date then you must enter the secondary start date");
                }

                if (!date_secondary_sample_start.IsBlank() && date_secondary_sample_end.IsBlank()) {
                    //missing secondary start end.
                    errors.AddError("date_secondary_sample_end", "If you have a secondary sample start date then you must enter the secondary end date");
                }
                */

                if (hasActualStart && hasActualEnd)
                {
                    if (date_actual_sample_start.ToDateTime() >= date_actual_sample_end.ToDateTime())
                    {
                        errors.AddError("date_actual_sample_end", "Actual Sample End Date must be greater then Actual Sample Start Date");
                    }
                }

                if (hasActualEnd && hasSecondaryStart)
                {
                    if (date_actual_sample_end.ToDateTime() >= date_secondary_sample_start.ToDateTime())
                    {
                        errors.AddError("date_secondary_sample_start", "Secondary Sample Start Date must be greater then Actual Sample End Date");
                    }
                }

                if (hasSecondaryStart && hasSecondaryEnd)
                {
                    if (date_secondary_sample_start.ToDateTime() >= date_secondary_sample_end.ToDateTime())
                    {
                        errors.AddError("date_secondary_sample_end", "Secondary Sample End Date must be greater then Secondary Sample Start Date");
                    }
                }

                // check for temperature/pressure
                if (SampleType != SampleType.PRECIP && SampleType != SampleType.PASS)
                {
                    errors.AddError(average_station_temperature.CheckRequired("average_station_temperature"));
                    errors.AddError(average_station_temperature_unit.CheckRequired("average_station_temperature_unit"));

                    errors.AddError(average_ambient_temperature.CheckRequired("average_ambient_temperature"));
                    errors.AddError(average_ambient_temperature_unit.CheckRequired("average_ambient_temperature_unit"));

                    errors.AddError(average_barometric_pressure.CheckRequired("average_barometric_pressure"));
                    errors.AddError(average_barometric_pressure_unit.CheckRequired("average_barometric_pressure_unit"));

                    errors.AddError(average_relative_humidity.CheckRequired("average_relative_humidity"));
                }
                else
                {
                    if (!average_station_temperature.IsBlank())
                    {
                        errors.AddError(average_station_temperature_unit.CheckRequired("average_station_temperature_unit", "Average Station Temperature Unit", "cannot be left blank if a temperature is provided."));
                    }

                    if (!average_ambient_temperature.IsBlank())
                    {
                        errors.AddError(average_ambient_temperature_unit.CheckRequired("average_ambient_temperature_unit", "Average Ambient Temperature Unit", "cannot be left blank if a temperature is provided."));
                    }

                    if (!average_barometric_pressure.IsBlank())
                    {
                        errors.AddError(average_barometric_pressure_unit.CheckRequired("average_barometric_pressure_unit", "Average Barometric Pressure Unit", "cannot be left blank if a pressure is provided."));
                    }
                }


                if (SampleType == SampleType.VOC)
                {
                    /* removed checked as per Sanjay email 2010-03-15
                    errors.AddError(voc_final_cannister_pressure.CheckRequired("voc_final_cannister_pressure", "Final Cannister Pressure"));
                    errors.AddError(voc_final_cannister_pressure_unit.CheckRequired("voc_final_cannister_pressure_unit", "Final Cannister Pressure Unit"));
                     */
                    if (voc_final_cannister_pressure.IsBlank())
                    {
                        errors.AddError(new ValidationError("voc_final_cannister_pressure", "VOC Final Cannister Pressure cannot be left blank. Defaulted to N/A"));
                    }
                    else if (IsNA(voc_final_cannister_pressure))
                    {
                        voc_final_cannister_pressure = string.Empty;
                    }

                    if (!voc_final_cannister_pressure.IsBlank())
                    {
                        errors.AddError(voc_final_cannister_pressure_unit.CheckRequired("voc_final_cannister_pressure_unit", "Final Cannister Pressure Unit", "cannot be left blank if a pressure is provided."));
                    }

                    errors.AddError(voc_final_sampler_pressure.CheckRequired("voc_final_sampler_pressure", "Final Sampler Pressure"));
                    errors.AddError(voc_final_sampler_pressure_unit.CheckRequired("voc_final_sampler_pressure_unit", "Final Sampler Pressure Unit"));
                    errors.AddError(voc_valve_closed.CheckRequired("voc_valve_closed", "Valve Closed"));
                }

                // if this value is supplied then it must be set to yes. 
                if (!voc_valve_closed.IsBlank())
                {
                    bool valveClosed;
                    if (bool.TryParse(voc_valve_closed, out valveClosed))
                    {
                        if (!valveClosed)
                        {
                            errors.AddError("voc_valve_closed", "You must close the valve on the cannister before you can commit.");
                        }
                    }
                }

                errors.AddError(retrieved_by.CheckRequired("retrieved_by"));
                if (SampleType.name != "PASS")
                {
                    errors.AddError(date_sample_retrieved.CheckRequired("date_sample_retrieved", "Date Retrieved"));
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
            retrieved_by = userId;
        }

        public ValidationError UpdateElapsedDuration()
        {
            decimal hours;
            long minutes, seconds;

            if (!elapsed_hours.IsBlank())
            {
                hours = decimal.Parse(elapsed_hours);
            }
            else
            {
                hours = 0;
            }

            if (!elapsed_minutes.IsBlank())
            {
                minutes = long.Parse(elapsed_minutes);
            }
            else
            {
                minutes = 0;
            }

            if (!elapsed_seconds.IsBlank())
            {
                seconds = long.Parse(elapsed_seconds);
            }
            else
            {
                seconds = 0;
            }

            long totalSeconds = (long)(hours * 3600) + (minutes * 60) + seconds;
            if (totalSeconds > int.MaxValue)
            {
                return new ValidationError("elapsed_sampling_duration", "Elapsed Sampling Duration cannot exceed " + int.MaxValue + " seconds or (596523h 14m 7s) in length.");
            }

            // no need to cast to an int because of the validation check we know that its less then the max int.
            elapsed_sampling_duration = totalSeconds.ToString();
            return null;
        }

        public override void Save()
        {
            base.Save();
        }

        public override void UpdateSectionAuditLog(string user_id)
        {
            base.UpdateAuditLog(user_id);
        }
    }
}