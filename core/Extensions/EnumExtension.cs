using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using core.Tools;

namespace core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum _value)
        {
            var _field = _value.GetType().GetField(_value.ToString());

            var _attributes = (DescriptionAttribute[])_field.
                GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (_attributes != null && _attributes.Length > 0)
            {
                return _attributes[0].Description;
            }
            else
            {
                return _value.ToString();
            }
        }

        public static List<int> ToIds(this Enum _value)
        {
            var _result = new List<int>();

            try
            {
                _result = _value.
                    ToDescriptionString().
                    Split(',').
                    Select(x => Convert.ToInt32(x)).
                    ToList();
            }
            catch (Exception _ex)
            {
                LogTool.Instance.Error(_ex);
            }

            return _result;
        }
    }
}
