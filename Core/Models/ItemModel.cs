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
    public class ItemModel : BaseModel
    {
        #region properties and private members
        public const string _table_name = "ItemModels";
        public const string _id_field = "model_id";

        private static readonly string[] _table_fields = new string[] {
            "model_id",
            "make_id",
            "name"
        };

        private static readonly string[] _required_fields = new string[] {
            "make_id",
            "name"
        };

        public string model_id { get { return _data["model_id"]; } private set { _data["model_id"] = value; } }
        public string make_id { get { return _data["make_id"]; } set { _data["make_id"] = value; } }
        public string name { get { return _data["name"]; } set { _data["name"] = value; } }

        public ItemMake make { get { return ItemMake.Load(make_id); } }
        public string display_name
        {
            get
            {
                if (String.IsNullOrEmpty(make_id))
                {
                    return name;
                }
                else
                {
                    return make.name + " - " + name;
                }
            }
        }
        #endregion

        public ItemModel() : base(_table_name, _id_field, _table_fields) { }

        #region list & pagination
        public static List<ItemModel> FetchAll()
        {
            return FetchAll<ItemModel>(_table_name, _id_field, null /* whereClause */);
        }

        public static List<ItemModel> FetchAll(string field, string value)
        {
            return FetchAll<ItemModel>(_table_name, _id_field, field + "=" + value);
        }

        public static List<ItemModel> FetchPage(int pageNumber, int pageSize)
        {
            return FetchPage<ItemModel>(pageNumber, pageSize, _table_name, _id_field);
        }

        public static int TotalCount()
        {
            return TotalCount(_table_name);
        }
        #endregion

        public static ItemModel Load(string id)
        {
            return Load<ItemModel>(id);
        }

        public override void Validate()
        {
            var v = new Validator(_data);
            v.RequiredFields(_required_fields);
            v.BetweenLength("name", 2, 50);
            v.Validate();
        }

        #region static methods
        public static SelectList FetchSelectList(object selected)
        {
            return new SelectList(FetchAll<ItemModel>("ItemModels", "model_id", ""), "model_id", "display_name", selected);
        }
        #endregion 

        ////public override void Save() { } // NOTE: use the BaseModel Save()
    }
}