using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace core.Attributes
{
    public class InfoAttribute : Attribute
    {
        public static string SelectQueryWithoutGeometry(Type type, bool onlySerializable)
        {
            var result = "";
            var props = type.GetProperties();

            foreach (var p in props)
            {
                InfoAttribute ia = p.GetCustomAttribute(typeof(InfoAttribute)) as InfoAttribute;
                ColumnAttribute ca = p.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                JsonPropertyAttribute jp = p.GetCustomAttribute(typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;

                if (ca == null || ia == null || (onlySerializable ? jp == null : false))
                    continue;

                result += ("".Equals(result) ? "" : ",") + ca.Name;
            }

            return result;
        }
    }
}
