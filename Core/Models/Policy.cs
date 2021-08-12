/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */

namespace WBEADMS.Models
{
    public class Policy : BaseModel
    {

        #region Properties
        public string policy_id
        {
            get
            {
                return _data["policy_id"];
            }

            private set
            {
                _data["policy_id"] = value;
            }
        }

        public string admin_panel_access
        {
            get
            {
                return _data["admin_panel_access"];
            }

            set
            {
                _data["admin_panel_access"] = value;
            }
        }

        public string docit_access
        {
            get
            {
                return _data["docit_access"];
            }

            set
            {
                _data["docit_access"] = value;
            }
        }

        public string sampleit_access
        {
            get
            {
                return _data["sampleit_access"];
            }

            set
            {
                _data["sampleit_access"] = value;
            }
        }

        public bool AdminPanelAccess
        {
            get
            {
                if (admin_panel_access.IsBlank())
                {
                    return false;
                }

                return admin_panel_access == "True";
            }

            set
            {
                admin_panel_access = value.ToString();
            }
        }

        public bool DocItAccess
        {
            get
            {
                if (docit_access.IsBlank())
                {
                    return false;
                }

                return docit_access == "True";
            }

            set
            {
                docit_access = value.ToString();
            }
        }

        #endregion

        public Policy() : base() { }

        public Policy(string policyID)
            : this()
        {
            policy_id = policyID;
            LoadData();
        }

        public override void Validate()
        {
            return;
        }
    }
}