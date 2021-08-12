/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public partial class Item : BaseModel
    {
        #region properties and private members
        private const string _table_name = "Items";
        private const string _id_field = "item_id";

        private static readonly string[] _table_fields = new string[] {
            "item_id",
            "serial_number",
            "name",
            "model_id",
            "comment",
            "date_created",
            "range",
            "asset_number",
            "location_id",
            "is_integrated",
            "sample_type_id"
        };
        #endregion

        #region list & pagination
        public static List<Item> FetchAll()
        {
            return FetchAll<Item>();
        }

        public static List<Item> FetchAll(string field, string value)
        {
            return FetchAll<Item>(field + "='" + value.Replace("'", "''") + "'");
        }

        public static List<Item> FetchPage(int pageNumber, int pageSize)
        {
            return FetchPage<Item>(pageNumber, pageSize);
        }

        public static int TotalCount()
        {
            return TotalCount(_table_name);
        }
        #endregion

        public static Item Load(string id)
        {
            var item = Load<Item>(id);

            return item;
        }

        public static SelectList FetchSelectList(object selectedValue)
        {
            return new SelectList(BaseModel.FetchAll<Item>(), "item_id", "DisplayName", selectedValue);
        }

        public static SelectList FetchSelectList(string sampleTypeId, string locationId, object selectedValue)
        {
            var whereExpressions = new List<string>();

            if (!sampleTypeId.IsBlank())
            {
                if (!sampleTypeId.IsInt()) { throw new ArgumentException("Invalid sampleTypeId : " + sampleTypeId, "sampleTypeId"); }

                whereExpressions.Add("sample_type_id = " + int.Parse(sampleTypeId).ToString());
            }

            if (!locationId.IsBlank())
            {
                if (!locationId.IsInt()) { throw new ArgumentException("Invalid locationId : " + locationId, "locationId"); }

                whereExpressions.Add("location_id = " + int.Parse(locationId).ToString());
            }

            string whereClause = String.Join(" AND ", whereExpressions.ToArray());
            var list = BaseModel.FetchAll<Item>(whereClause);
            return BaseModelExtensions.SelectList("item_id", "DisplayName", list, selectedValue); //which is more useful DisplayName? or Serial Number?  If you don't filter by Location, then DisplayName is more useful. If there are multiple items at the same location, then Serial Number is more useful.
        }
    }
}