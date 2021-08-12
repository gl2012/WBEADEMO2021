/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;

namespace WBEADMS.Models
{
    public class ShippingSection : BaseSection
    {
        private static readonly string[] CocFields = new string[] {
            "chain_of_custody_id",
            "date_shipped_to_lab",
            "shipped_to",
            "shipping_company",
            "waybill_number",
            "voc_cannister_pressure_before_shipping",
            "voc_cannister_pressure_before_shipping_unit",
            "printed",
            "shipped_by"};

        User _shippedBy;

        public ShippingSection(SampleType sampleType) : base(sampleType, "Ship", "ChainOfCustodys", "chain_of_custody_id", ShippingSection.CocFields) { }

        public ShippingSection(SampleType sampleType, string chainOfCustodyID)
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

        public string date_shipped_to_lab
        {
            get
            {
                return _data["date_shipped_to_lab"];
            }

            set
            {
                _data["date_shipped_to_lab"] = value;
            }
        }

        public string shipped_to
        {
            get
            {
                return _data["shipped_to"];
            }
            set
            {
                _data["shipped_to"] = value;
            }
        }

        public Lab ShippedTo
        {
            get
            {
                return Lab.Load(shipped_to);
            }
        }

        public string shipping_company
        {
            get
            {
                return _data["shipping_company"];
            }

            set
            {
                _data["shipping_company"] = value;
            }
        }

        public ShippingCompany ShippingCompany
        {
            get { return ShippingCompany.Load(shipping_company); }
        }
        public string waybill_number
        {
            get
            {
                return _data["waybill_number"];
            }

            set
            {
                _data["waybill_number"] = value;
            }
        }

        public string voc_cannister_pressure_before_shipping
        {
            get
            {
                return _data["voc_cannister_pressure_before_shipping"];
            }

            set
            {
                _data["voc_cannister_pressure_before_shipping"] = value;
            }
        }

        public string voc_cannister_pressure_before_shipping_unit
        {
            get
            {
                return _data["voc_cannister_pressure_before_shipping_unit"];
            }

            set
            {
                _data["voc_cannister_pressure_before_shipping_unit"] = value;
            }
        }


        public Unit VOCCannisterPressureBeforeShippingUnit
        {
            get
            {
                return LoadRelated<Unit>("voc_cannister_pressure_before_shipping_unit");
            }
        }

        public string shipped_by
        {
            get
            {
                return _data["shipped_by"];
            }

            set
            {
                _data["shipped_by"] = value;
            }
        }

        public User ShippedBy
        {
            get
            {
                if (_shippedBy == null && !String.IsNullOrEmpty(shipped_by))
                {
                    _shippedBy = new User(shipped_by);
                }

                return _shippedBy;
            }
        }

        public string printed
        {
            get
            {
                return _data["printed"];
            }

            set
            {
                _data["printed"] = value;
            }
        }
        #endregion

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                errors.AddError(date_shipped_to_lab.CheckIfDateTime("date_shipped_to_lab"));

                errors.AddError(shipping_company.CheckMaxLength(30, "shipping_company"));
                errors.AddError(waybill_number.CheckMaxLength(30, "waybill_number"));

                errors.AddError(shipped_to.CheckIfInt("shipped_to"));
                errors.AddError(printed.CheckIfBool("printed"));

                if (!IsNA(voc_cannister_pressure_before_shipping))
                    errors.AddError(voc_cannister_pressure_before_shipping.CheckIfDecimal("voc_cannister_pressure_before_shipping"));
                errors.AddError(voc_cannister_pressure_before_shipping_unit.CheckIfInt("voc_cannister_pressure_before_shipping_unit"));

                errors.AddError(shipped_by.CheckIfInt("shipped_by"));
            }
        }

        protected override void ValidateStrict()
        {
            using (ModelException errors = new ModelException())
            {

                errors.AddError(date_shipped_to_lab.CheckRequired("date_shipped_to_lab"));
                errors.AddError(shipping_company.CheckRequired("shipping_company"));
                errors.AddError(waybill_number.CheckRequired("waybill_number"));
                errors.AddError(shipped_to.CheckRequired("shipped_to"));

                if (!errors.AddError(printed.CheckRequired("printed")))
                {
                    if (printed != "True" && SampleType != SampleType.PASS)
                    {
                        errors.AddError("printed", "You must print the chain of custody before final commit.");
                    }
                }

                if (SampleType == SampleType.VOC)
                {
                    if (voc_cannister_pressure_before_shipping.IsBlank())
                    {
                        errors.AddError(new ValidationError("voc_cannister_pressure_before_shipping", "VOC Cannister Pressure Before Shipping cannot be left blank. Defaulted to N/A."));
                    }
                    else if (IsNA(voc_cannister_pressure_before_shipping))
                    {
                        voc_cannister_pressure_before_shipping = string.Empty;
                    }

                    if (!voc_cannister_pressure_before_shipping.IsBlank())
                    {
                        errors.AddError(voc_cannister_pressure_before_shipping_unit.CheckRequired("voc_cannister_pressure_before_shipping_unit", "VOC Cannister Pressure Before Shipping Unit", "cannot be left blank if a pressure is provided."));
                    }
                }

                errors.AddError(shipped_by.CheckRequired("shipped_by", "Shipped By", shipped_by));
            }
        }

        public override void CommittedBy(string userId)
        {
            shipped_by = userId;
        }

        public override void UpdateSectionAuditLog(string user_id)
        {
            base.UpdateAuditLog(user_id);
        }
    }
}