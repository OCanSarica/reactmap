using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace core.Attributes.FilterAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TextBoxAutoCompleteFilterAttribute : Attribute
    {
        public string classname { get; set; }
        public string QueryTable { get; set; }
        public string QueryField { get; set; }

        public TextBoxAutoCompleteFilterAttribute()
        {
            classname = "autocomplete";
        }
    }
}
