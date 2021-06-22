using dynamic_linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{

    [Flags]
    public enum Operator
    {
        IN = 0,
        EQUAL = 1,
        NOT_IN = 2,
        IS_NULL = 4,
        NOT_EQUAL = 8,
        IS_NOT_NULL = 16
    }

    /// <summary>
    /// Eğer Values verilirse LookUpTable passif olur
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SelectMenuFilterAttribute : FilterBaseAttribute
    {
        // Properties

        public string LookUpTable { get; set; }

        public string IdName { get; set; }

        public string ValueName { get; set; }


        public object[] Ids { get; set; }

        public object[] Values { get; set; }

        public Dictionary<object, object> Values2 { get; set; }

        public Operator? Operators { get; set; }

        //////////////////////////

        public SelectMenuFilterAttribute()
        {
            IdName = "Id";
            ValueName = "Deger";
        }

        protected override FilterBase CreateFilter()
        {
            return new SelectMenuFilter();
        }

        public override void Prepare(System.Reflection.PropertyInfo property, String columName)
        {
            base.Prepare(property, columName);

            var selectMenuFilter = FilterBase as SelectMenuFilter;
            selectMenuFilter.Input = "select";

            List<string> olist = new List<string>();

            Array operatorArray = Enum.GetValues(typeof(Operator));

            foreach (var op in operatorArray)
            {
                if (Operators == null || OperatorExist(Operators.Value, (Operator)op))
                {
                    olist.Add(Enum.GetName(typeof(Operator), (Operator)op).ToLower(CultureInfo.GetCultureInfo("en")));
                }
            }

            selectMenuFilter.Operators = olist;

            Dictionary<object, object> valueHash = new Dictionary<object, object>();

            if (Values == null)
            {
                FetchLookUptable(ref valueHash, LookUpTable, IdName, ValueName);
            }
            else
            {
                FetchValues(Ids, Values, ref valueHash);
            }

            var _orderedDic = new Dictionary<object, object>();

            foreach (var _item in valueHash.OrderBy("Value"))
            {
                _orderedDic.Add(_item.Key, _item.Value);
            }

            Values2 = _orderedDic;

            selectMenuFilter.Values = _orderedDic;
        }

        public bool OperatorExist(Operator @operator, Operator filter)
        {
            return (@operator & filter) == filter;
        }
    }

    [DataContract]
    public class SelectMenuFilter : FilterBase
    {
        [DataMember(Name = "input")]
        public string Input { get; set; }

        [DataMember(Name = "values")]
        public Dictionary<object, object> Values { get; set; }

        [DataMember(Name = "operator")]
        public List<string> Operators { get; set; }
    }
}