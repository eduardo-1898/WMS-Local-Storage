using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASTSoft.Desarrollos.Utils
{
    public static class DropDownUtilString
    {
        public static List<SelectListItem> GetDropDownPappingGeneric<T>(IList<T> data, string key, string value, List<String> keySelected = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in data)
            {
                var view = item;
                var itemKey = (String)(view.GetType().GetProperty(key).GetValue(view, null));
                var itemValue = view.GetType().GetProperty(value).GetValue(view, null).ToString();
                var selected = keySelected != null && keySelected.Any(x => x == itemKey);
                items.Add(new SelectListItem { Text = itemValue, Value = itemKey.ToString(), Selected = selected });
            }

            return items;
        }

        public static List<SelectListItem> GetDropDownPappingGeneric<T>(IList<T> data, string key, string value, String? keySelected = null)
        {
            return GetDropDownPappingGeneric(data, key, value, new List<String> { keySelected == null ? String.Empty : keySelected });
        }


        public static List<SelectListItem> GetDropDownPappingGeneric<T>(IList<T> data, string key, string value)
        {
            return GetDropDownPappingGeneric(data, key, value, new List<String> { String.Empty });
        }

        public static IEnumerable<SelectListItem> GetDropDownPappingGenericBool(bool selected = false)
        {
            var list = new List<SelectListItem> { new SelectListItem() { Text = "si", Value = "true", Selected = selected == true }, new SelectListItem() { Text = "no", Value = "false", Selected = selected == false } };
            return list;
        }

        public static MultiSelectList GetDropDownMultiplePappingGeneric<T>(List<T> data, string key, string value, List<String> keySelected = null)
        {
            var items = new MultiSelectList(data, key, value, keySelected);

            return items;
        }
    }
}
