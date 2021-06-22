using System;

namespace core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Tablo Adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public TableAttribute()
        {
        }
    }
}
