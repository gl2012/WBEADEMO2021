/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Collections.Generic;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public class ItemMake : BaseModel
    {
        #region properties and private members
        public const string _table_name = "ItemMakes";
        public const string _id_field = "make_id";

        private static readonly string[] _table_fields = new string[] {
            "make_id",
            "name"
        };

        public string make_id { get { return _data["make_id"]; } private set { _data["make_id"] = value; } }
        public string name { get { return _data["name"]; } set { _data["name"] = value; } }
        public List<ItemModel> models { get { return ItemModel.FetchAll("make_id", make_id); } }
        #endregion

        public ItemMake() : base(_table_name, _id_field, _table_fields) { }

        #region list & pagination
        public static List<ItemMake> FetchAll(string field, string value)
        {
            return FetchAll<ItemMake>(_table_name, _id_field, field + "=" + value);
        }

        public static List<ItemMake> FetchPage(int pageNumber, int pageSize)
        {
            return FetchPage<ItemMake>(pageNumber, pageSize, _table_name, _id_field);
        }

        public static int TotalCount()
        {
            return TotalCount(_table_name);
        }
        #endregion

        public static ItemMake Load(string id)
        {
            return Load<ItemMake>(id);
        }

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                errors.AddError(name.CheckRequired("name"));
                errors.AddError(name.CheckMaxLength(50, "name"));
                errors.AddError(name.CheckMinLength(2, "name"));
            }
        }

        #region static methods
        public static SelectList FetchSelectList()
        {
            return FetchSelectList(null);
        }

        public static SelectList FetchSelectList(object selectedValue)
        {
            return new SelectList(FetchAll<ItemMake>("ItemMakes", "make_id", null /* whereClause */), "make_id", "name", selectedValue);
        }
        #endregion 

        ////public override void Save() { } // NOTE: use the BaseModel Save()
    }
}