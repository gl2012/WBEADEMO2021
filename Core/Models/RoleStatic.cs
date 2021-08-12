/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Collections.Generic;

namespace WBEADMS.Models
{
    public partial class Role
    {
        public static List<Role> FetchPage(int pageNumber, int pageSize)
        {
            return FetchPage<Role>(pageNumber, pageSize, "Roles", "role_id", new { order = "Name" });
        }

        public static int TotalCount()
        {
            return TotalCount("Roles");
        }

        public static Role Load(string id)
        {
            return Load<Role>(id);
        }

        // NOTE: only used in User/Index view; probably should replace that with a Role.FetchAll();
        public static Dictionary<string, string> FetchDictionary()
        {
            return FetchDictionary<Role>("name");
        }

        public static System.Web.Mvc.SelectList FetchSelectList(object selectedValue)
        {
            return new System.Web.Mvc.SelectList(BaseModel.FetchAll<Role>(), "role_id", "name", selectedValue);
        }
    }
}