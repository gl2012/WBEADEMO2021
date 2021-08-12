/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public class Lab : BaseModel
    {
        private static readonly string[] _table_fields = new string[] {
            "lab_id",
            "name",
            "active"
        };

        public Lab() : base(_table_fields) { }

        #region properties
        public string lab_id
        {
            get
            {
                return _data["lab_id"];
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
        public static Lab Load(string lab_id)
        {
            return BaseModel.Load<Lab>(lab_id);
        }

        public static SelectList FetchAllActiveSelectList(string lab_id)
        {
            return BaseModelExtensions.FetchAllActiveSelectList<Lab>("id", "name", null /* clauses */, lab_id);
        }
        #endregion
    }
}