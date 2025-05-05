using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _1221kr.Helpers
{
    public static class EnumHelper
    {
        public static SelectList GetEnumSelectList<TEnum>() where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = e.ToString()
                });
            return new SelectList(values, "Value", "Text");
        }
    }
}