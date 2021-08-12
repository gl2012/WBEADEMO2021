
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.Views.ItemHelpers
{
    public static class ItemHelper
    {
        public static string ParameterNames(this Item item)
        {
            var list = new List<string>();
            foreach (var parameter in item.parameters)
            {
                list.Add(parameter.name);
            }
            return String.Join(", ", list.ToArray());
        }

        public static string ItemListedName(this Item item)
        {
            return item.serial_number + " - " + item.ParameterNames() + " - " + item.model.display_name;
        }

        public static string ItemListedName(this HtmlHelper helper, Item item)
        {
            return helper.Encode(item.ItemListedName());
        }
    }
}