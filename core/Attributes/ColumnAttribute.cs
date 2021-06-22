using System;

namespace core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// Kolon adi
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kimlik bilgisi
        /// </summary>
        public bool Identity { get; set; }

        /// <summary>
        /// Kimlik bilgisi
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// Spatial kolok bilgisi
        /// </summary>
        public bool Spatial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type Relation { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public ColumnAttribute()
        {
            Identity = false;
            Spatial = false;
        }
    }
}
