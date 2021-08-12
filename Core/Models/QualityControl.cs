/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */

namespace WBEADMS.Models
{
    public class QualityControl : BaseModel
    {
        #region private static members
        private static readonly string[] _table_fields = new string[] {
            "quality_control_id",
            "batch_id",
            "compound_name",
            "retention_time",
            "target_ion",
            "qualifier_ion",
            "standard_solution_concentration_level",
            "calibration_linear_regression",
            "calibration_linear_regression_slope",
            "average_laboratory_blank_concentration",
            "laboratory_blank_concentration_standard_deviation",
            "instrument_detection_limit",
            "instrument_detection_units",
            "method_detection_limit",
            "method_detection_limit_units",
            "average_recovery_efficiency_precent",
            "average_recovery_efficiency_standard_deviation"
        };

        private static readonly string[] _required_fields = new string[] {
            "batch_id"
        };
        #endregion

        public QualityControl() : base(_table_fields) { }

        #region properties
        public string batch_id
        {
            get
            {
                return _data["batch_id"];
            }

            set
            {
                _data["batch_id"] = value;
            }
        }

        public string quality_control_id
        {
            get
            {
                return _data["quality_control_id"];
            }

            set
            {
                _data["quality_control_id"] = value;
            }
        }

        public Batch Batch
        {
            get
            {
                return LoadRelated<Batch>();
            }
        }

        public string compound_name
        {
            get
            {
                return _data["compound_name"];
            }

            set
            {
                _data["compound_name"] = value;
            }
        }

        public string retention_time
        {
            get
            {
                return _data["retention_time"];
            }

            set
            {
                _data["retention_time"] = value;
            }
        }

        public string qualifier_ion
        {
            get
            {
                return _data["qualifier_ion"];
            }

            set
            {
                _data["qualifier_ion"] = value;
            }
        }

        public string standard_solution_concentration_level
        {
            get
            {
                return _data["standard_solution_concentration_level"];
            }

            set
            {
                _data["standard_solution_concentration_level"] = value;
            }
        }

        public string calibration_linear_regression
        {
            get
            {
                return _data["calibration_linear_regression"];
            }

            set
            {
                _data["calibration_linear_regression"] = value;
            }
        }

        public string calibration_linear_regression_slope
        {
            get
            {
                return _data["calibration_linear_regression_slope"];
            }

            set
            {
                _data["calibration_linear_regression_slope"] = value;
            }
        }

        public string average_laboratory_blank_concentration
        {
            get
            {
                return _data["average_laboratory_blank_concentration"];
            }

            set
            {
                _data["average_laboratory_blank_concentration"] = value;
            }
        }

        public string laboratory_blank_concentration_standard_deviation
        {
            get
            {
                return _data["laboratory_blank_concentration_standard_deviation"];
            }

            set
            {
                _data["laboratory_blank_concentration_standard_deviation"] = value;
            }
        }

        public string instrument_detection_limit
        {
            get
            {
                return _data["instrument_detection_limit"];
            }

            set
            {
                _data["instrument_detection_limit"] = value;
            }
        }

        public string instrument_detection_units
        {
            get
            {
                return _data["instrument_detection_units"];
            }

            set
            {
                _data["instrument_detection_units"] = value;
            }
        }

        public string method_detection_limit
        {
            get
            {
                return _data["method_detection_limit"];
            }

            set
            {
                _data["method_detection_limit"] = value;
            }
        }

        public string method_detection_limit_units
        {
            get
            {
                return _data["method_detection_limit_units"];
            }

            set
            {
                _data["method_detection_limit_units"] = value;
            }
        }

        public string average_recovery_efficiency_precent
        {
            get
            {
                return _data["average_recovery_efficiency_precent"];
            }

            set
            {
                _data["average_recovery_efficiency_precent"] = value;
            }
        }

        public string average_recovery_efficiency_standard_deviation
        {
            get
            {
                return _data["average_recovery_efficiency_standard_deviation"];
            }

            set
            {
                _data["average_recovery_efficiency_standard_deviation"] = value;
            }
        }
        #endregion

        public static QualityControl Load(string id)
        {
            return Load<QualityControl>(id);
        }

        public override void Validate()
        {
            using (var errors = new ModelException())
            {
                errors.AddError(batch_id.CheckRequired("batch_id"));
            }
        }
    }
}