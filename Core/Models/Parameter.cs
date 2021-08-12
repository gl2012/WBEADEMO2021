
using System.Collections.Generic;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public class Parameter : BaseModel
    {
        #region properties and private members
        private const string _table_name = "Parameters";
        private const string _id_field = "parameter_id";

        private static readonly string[] _table_fields = new string[] {
            "parameter_id",
            "name",
            "description",
            "hidden"
        };

        private static readonly string[] _required_fields = new string[] {
            "name"
        };

        public string parameter_id { get { return _data["parameter_id"]; } private set { _data["parameter_id"] = value; } }
        public string name { get { return _data["name"]; } set { _data["name"] = value; } }
        public string description { get { return _data["description"]; } set { _data["description"] = value; } }
        public string hidden { get { return _data["hidden"]; } set { _data["hidden"] = value; } }
        #endregion

        public Parameter() : base(_table_name, _id_field, _table_fields) { }

        #region list & pagination
        public static List<Parameter> FetchAll()
        {
            return FetchAll<Parameter>();
        }

        public static List<Parameter> FetchAll(string field, string value)
        {
            return FetchAll<Parameter>(field + "='" + value.Replace("'", "''") + "'");
        }
        #endregion

        public static Parameter Load(string id)
        {
            return Load<Parameter>(id);
        }

        public override void Validate()
        {
            var v = new Validator(_data);
            v.RequiredFields(_required_fields);
            v.BetweenLength("name", 2, 50);
            v.Validate();
        }

        public static SelectList FetchSelectListActive(object selectedValue)
        {
            return new SelectList(FetchAll<Parameter>(), "parameter_id", "name", selectedValue);
        }
        public static SelectList FetchSelectListActive()
        {
            return new SelectList(FetchAll<Parameter>(), "parameter_id", "name");
        }
    }
}