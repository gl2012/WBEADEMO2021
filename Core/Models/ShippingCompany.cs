/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public class ShippingCompany : BaseModel
    {

        public ShippingCompany() : base() { }

        #region properties
        public string shipping_company_id
        {
            get
            {
                return _data["shipping_company_id"];
            }
        }

        public string name
        {
            get
            {
                return _data["name"];
            }
            set
            {
                _data["name"] = value;
            }
        }

        public string active
        {
            get
            {
                return _data["active"];
            }
            set
            {
                _data["active"] = value;
            }
        }
        #endregion

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                if (!errors.AddError(name.CheckRequired("name")))
                {
                    errors.AddError(name.CheckMaxLength(100, "name"));
                }

                if (!errors.AddError(active.CheckRequired("active")))
                {
                    errors.AddError(active.CheckIfBool("active"));
                }
            }
        }

        #region Static Methods
        public static ShippingCompany Load(string shipping_company_id)
        {
            return BaseModel.Load<ShippingCompany>(shipping_company_id);
        }

        public static SelectList FetchAllActiveSelectList(string shipping_company_id)
        {
            return BaseModelExtensions.FetchAllActiveSelectList<ShippingCompany>("id", "name", null /* clauses */, shipping_company_id);
        }
        #endregion
    }
}