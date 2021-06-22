using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace core.Attributes.FilterAttributes
{

    public abstract class FilterBaseAttribute : Attribute
    {
        private FilterBase filterBase = null;

        public string Label { get; set; }

        public string Type { get; set; }

        internal FilterBase FilterBase { get { return filterBase; } }

        protected virtual AConverter Converter { get; set; }

        protected abstract FilterBase CreateFilter();

        protected object CallPropertyValue(object item, string propertyName)
        {
            var param = Expression.Parameter(item.GetType());
            var body = Expression.PropertyOrField(param, propertyName);
            var @delegate = Expression.Lambda(body, param).Compile();
            return @delegate.DynamicInvoke(item);
        }

        protected virtual void FetchLookUptable(ref Dictionary<object, object> values, 
            string lookUpTable, string idName, string valueName)
        {
            //using (DataRepository repo = new DataRepository())
            //{
            //    IEnumerable<object> list = repo.QueryLookUp(lookUpTable, valueName, idName);

            //    if (values == null)
            //    {
            //        values = new Dictionary<object, object>();
            //    }
            //    else
            //    {
            //        values.Clear();
            //    }

            //    foreach (var item in list)
            //    {


            //        values.Add(CallPropertyValue(item, idName), CallPropertyValue(item, valueName));


            //    }
            //}
        }

        protected virtual void FetchValues(object[] ids, object[] values, ref Dictionary<object, object> valueHash)
        {
            if (valueHash == null)
            {
                valueHash = new Dictionary<object, object>();
            }
            else
            {
                valueHash.Clear();
            }

            for (int i = 0; i < values.Length; i++)
            {
                object id;
                object value = values[i];

                if (ids != null && ids.Length > i)
                    id = ids[i];
                else
                    id = values[i];

                valueHash.Add(id, value);
            }
        }

        public virtual void Prepare(PropertyInfo property, String columnName)
        {
            filterBase = CreateFilter();

            if (Converter != null)
            {
                property = Converter.Convert();
            }

            filterBase.Id = columnName ?? property.Name;

            if (!string.IsNullOrEmpty(Label))
            {
                filterBase.Label = Label;
            }
            else
            {
                var _jsonPropertyAttribute = property.GetCustomAttribute<Newtonsoft.
                    Json.JsonPropertyAttribute>(false);

                if (_jsonPropertyAttribute != null)
                {
                    var _name = _jsonPropertyAttribute.GetType().
                        GetProperty("PropertyName").
                        GetValue(_jsonPropertyAttribute).ToString();

                    filterBase.Label = _name;
                }
                else
                {
                    filterBase.Label = property.Name;
                }
            }


            if (Type != null)
            {
                filterBase.Type = Type;
            }
            else
            {
                Type type = property.PropertyType;

                if (type.IsAssignableFrom(typeof(bool)))
                    filterBase.Type = "boolean";
                else if (type.IsAssignableFrom(typeof(DateTime)))
                    filterBase.Type = "datetime";
                else if (type.IsAssignableFrom(typeof(TimeSpan)))
                    filterBase.Type = "time";
                else if (type.IsAssignableFrom(typeof(int)) || type.IsAssignableFrom(typeof(Int64)) || type.IsAssignableFrom(typeof(short)))
                    filterBase.Type = "integer";
                else if (type.IsAssignableFrom(typeof(float)))
                    filterBase.Type = "double";
                else if (type.IsAssignableFrom(typeof(double)))
                    filterBase.Type = "double";
                else if (type.IsAssignableFrom(typeof(string)))
                    filterBase.Type = "string";
                else
                    filterBase.Type = type.Name.ToLower();
            }
        }
    }
}