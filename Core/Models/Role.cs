/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
namespace WBEADMS.Models
{
    public partial class Role : BaseModel
    {
        #region Properties
        public string role_id
        {
            get
            {
                return _data["role_id"];
            }

            private set
            {
                _data["role_id"] = value;
            }
        }

        public string policy_id
        {
            get
            {
                return _data["policy_id"];
            }

            set
            {
                _data["policy_id"] = value;
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

        public string description
        {
            get
            {
                return _data["description"];
            }

            set
            {
                _data["description"] = value;
            }
        }

        public int UsersCount
        {
            get
            {
                return User.UsersCountByRole(role_id);
            }
        }

        public Policy Policy
        {
            get
            {
                return LoadRelated<Policy>("policy_id");
            }
        }
        #endregion

        public Role() : base("Roles", "role_id", new string[] { "role_id", "policy_id", "name", "description" }) { }
        public Role(string roleID) : this()
        {
            role_id = roleID;
            LoadData();
        }

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                errors.AddError(name.CheckRequired("name"));
                errors.AddError(name.CheckMaxLength(50, "name"));
            }
        }

        public override void Save()
        {
            if (String.IsNullOrEmpty(role_id))
            {
                // create new location
                Validate();
                SaveNew();
            }
            else
            {
                // Save Edits.
                Validate();
                SaveEdits();
            }
        }
    }
}