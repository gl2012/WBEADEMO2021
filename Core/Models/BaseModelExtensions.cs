/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Collections.Generic;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public static class BaseModelExtensions
    {

        /// <summary>Determines whether an element is in System.Collections.Generic.List&lt;BaseModel&gt;.</summary>
        /// <typeparam name="TModel">BaseModel type.  Do not specify this; use the non-generic overload version instead. --James</typeparam>
        /// <param name="id">The id of the object to locate in System.Collections.Generic.List&lt;BaseModel&gt;. The value can be null for uncommitted models.</param>
        /// <returns> true if item is found in the System.Collections.Generic.List<T>; otherwise, false.</returns>
        public static bool Contains<TModel>(this List<TModel> list, string id) where TModel : BaseModel
        {
            foreach (var listItem in list)
            {
                if (listItem.id == id)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>SelectList created from List of BaseModel. Also ensures selected model exists in the list, and loads it if it isn't.</summary>
        /// <typeparam name="TModel">BaseModel</typeparam>
        /// <param name="dataValueField">The data value field.</param>
        /// <param name="dataTextField">The data text field.</param>
        /// <param name="list">System.Generics.Collections.List of BaseModel.</param>
        /// <param name="selectedModelId">The selected model's id.</param>
        internal static SelectList SelectList<TModel>(string dataValueField, string dataTextField, List<TModel> list, object selectedModelId) where TModel : BaseModel, new()
        {
            if (selectedModelId != null)
            {
                string id = selectedModelId.ToString();
                if (!id.IsBlank() && !list.Contains(id))
                {
                    list.Add(BaseModel.Load<TModel>(id));
                }
            }

            return new SelectList(list, dataValueField, dataTextField, selectedModelId);
        }

        /// <summary>FetchAll of a BaseModel and returns it as a SelectList. Also ensures selected model exists in the list, even if it is active.</summary>
        /// <typeparam name="TModel">BaseModel</typeparam>
        /// <param name="dataValueField">The data value field.</param>
        /// <param name="dataTextField">The data text field.</param>
        /// <param name="clauses">new object containing where, order, and join properties specifying the clauses</param>
        /// <param name="selectedModelId">The selected model's id.</param>
        internal static SelectList FetchAllSelectList<TModel>(string dataValueField, string dataTextField, object clauses, object selectedModelId) where TModel : BaseModel, new()
        {
            return SelectList(dataValueField, dataTextField, BaseModel.FetchAll<TModel>(clauses), selectedModelId);
        }

        /// <summary>FetchAll where "active=1" of a BaseModel and returns it as a SelectList.</summary>
        /// <typeparam name="TModel">BaseModel</typeparam>
        /// <param name="dataValueField">The data value field.</param>
        /// <param name="dataTextField">The data text field.</param>
        internal static SelectList FetchAllActiveSelectList<TModel>(string dataValueField, string dataTextField) where TModel : BaseModel, new()
        {
            return FetchAllActiveSelectList<TModel>(dataValueField, dataTextField, null /* clauses */, null /* id */);
        }

        /// <summary>FetchAll where "active=1" of a BaseModel and returns it as a SelectList.</summary>
        /// <typeparam name="TModel">BaseModel</typeparam>
        /// <param name="dataValueField">The data value field.</param>
        /// <param name="dataTextField">The data text field.</param>
        /// <param name="clauses">new object containing where, order, and join properties specifying the clauses</param>
        internal static SelectList FetchAllActiveSelectList<TModel>(string dataValueField, string dataTextField, object clauses) where TModel : BaseModel, new()
        {
            return FetchAllActiveSelectList<TModel>(dataValueField, dataTextField, clauses, null /* selectedModelId */);
        }

        /// <summary>FetchAll where "active=1" of a BaseModel and returns it as a SelectList. Also ensures selected model exists in the list, even if it is active.</summary>
        /// <typeparam name="TModel">BaseModel</typeparam>
        /// <param name="dataValueField">The data value field.</param>
        /// <param name="dataTextField">The data text field.</param>
        /// <param name="clauses">new object containing where, order, and join properties specifying the clauses</param>
        /// <param name="selectedModelId">The selected model's id.</param>
        internal static SelectList FetchAllActiveSelectList<TModel>(string dataValueField, string dataTextField, object clauses, object selectedModelId) where TModel : BaseModel, new()
        {
            var clausesNVC = BaseModel.ObjectToDictionary(clauses);

            string order = clausesNVC["order"];
            string join = clausesNVC["join"];
            string where = "active = 1";
            if (clausesNVC["where"] != null)
            {
                where += " AND " + clausesNVC["where"];
            }

            var list = BaseModel.FetchAll<TModel>(new { where = where, join = join, order = order });

            return SelectList(dataValueField, dataTextField, list, selectedModelId);
        }


    }
}