using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CustomNumberFilterAttribute : NumberFilterAttribute
    {
        public override void Prepare(PropertyInfo property, string columName)
        {
            try
            {
                base.Prepare(property, columName);

                //var type = property.DeclaringType;
                //var properties = type.GetProperties();

                //var customFilter = new CustomFilterElement[3] { new CustomFilterElement(), new CustomFilterElement(), new CustomFilterElement() };

                //AssemblyName asmName = new AssemblyName("temp");
                //AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);

                //DynamicModuleCreator creator = new DynamicModuleCreator(assemblyBuilder, typeof(DynamicObjectBase), "tempModule", false);
                //creator.AddCustomAttribute(typeof(JsonPropertyAttribute), typeof(string)).Build(null, typeof(CustomFilterElement).Name);
                //Dictionary<string, object> propValues = new Dictionary<string, object>();

                //foreach (var prop in properties)
                //{
                //    var columnAttribute = prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as ColumnAttribute;
                //    var jsonObjectAttribute = prop.GetCustomAttributes(typeof(JsonPropertyAttribute), true).FirstOrDefault() as JsonPropertyAttribute;
                //    var numberFilterAttribute = prop.GetCustomAttributes(typeof(NumberFilterAttribute), true).FirstOrDefault() as NumberFilterAttribute;
                //    var isnumber = IsNumericType(prop.PropertyType);
                //    if (isnumber && numberFilterAttribute != null && columnAttribute != null && jsonObjectAttribute != null)
                //    {
                //        creator.CreateProperty(prop.Name, typeof(string))
                //            .AddCustomAttribute(typeof(JsonPropertyAttribute), typeof(string))
                //            .Build(null, columnAttribute.Name);
                //        propValues.Add(prop.Name, jsonObjectAttribute.PropertyName);
                //    }
                //}
                //DynamicObjectBase obj = creator.CreateNewObject() as DynamicObjectBase;

                //foreach (var propVal in propValues)
                //    obj.Set(propVal.Key, propVal.Value);

                //propValues.Clear();

                //customFilter[0].Id = "category";
                //customFilter[0].Label = "Category";
                //customFilter[0].Type = "string";
                //customFilter[0].Input = "select";
                //customFilter[0].Values = obj;

                //customFilter[1].Id = "operator";
                //customFilter[1].Label = "Operator";
                //customFilter[1].Type = "string";
                //customFilter[1].Input = "select";

                //List<string> props = new string[] { "=", ">", "<=", "<", ">=" }.ToList();
                //creator = new DynamicModuleCreator(assemblyBuilder, typeof(DynamicObjectBase), "tempOperatorModule", false);
                //creator.AddCustomAttribute(typeof(JsonObjectAttribute), typeof(string)).Build(null, typeof(CustomFilterElement).Name);
                //var propertySetter = new Dictionary<string, object>();
                //propertySetter.Add("Name", typeof(CustomFilterElement).Name);
                //creator.AddCustomAttribute(typeof(DataContractAttribute)).Build(propertySetter);
                //foreach (var prop in props)
                //{
                //    var propcreator = creator.CreateProperty("ops" + props.IndexOf(prop), typeof(string));
                //    propcreator.AddCustomAttribute(typeof(JsonPropertyAttribute), typeof(string)).Build(null, prop);
                //    propertySetter = new Dictionary<string, object>();
                //    propertySetter.Add("Name", prop);
                //    propcreator.AddCustomAttribute(typeof(DataMemberAttribute)).Build(propertySetter);
                //    propValues.Add("ops" + props.IndexOf(prop), prop);
                //}

                //obj = creator.CreateNewObject() as DynamicObjectBase;
                //foreach (var propVal in propValues)
                //    obj.Set(propVal.Key, propVal.Value);

                //customFilter[1].Values = obj;
                //customFilter[2].Id = "sam";
                //customFilter[2].Label = "Identifier";
                //customFilter[2].Type = "double";
                //customFilter[2].Input = "number";

                //var filter = FilterBase as CustomNumberFilter;
                //filter.CustomFilter = customFilter;
            }
            catch (Exception _ex)
            {
                //_ex.Error("");
            }
        }

        protected override FilterBase CreateFilter()
        {
            return new CustomNumberFilter();
        }

        private bool IsNumericType(Type o)
        {
            switch (System.Type.GetTypeCode(o))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }

    [DataContract]
    public class CustomNumberFilter : NumberFilter
    {
        [DataMember(Name = "customFilter")]
        public FilterBase[] CustomFilter { get; set; }
    }

    [DataContract]
    public class CustomFilterElement : FilterBase
    {
        [DataMember(Name = "input")]
        public string Input { get; set; }

        [DataMember(Name = "values")]
        public object Values { get; set; }
    }
}