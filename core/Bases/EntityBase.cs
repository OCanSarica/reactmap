using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace core.Bases
{
    [DataContract]
    public abstract class EntityBase : IEntity
    {
        public abstract long Id { get; set; }

        public virtual T Get<T>(string _propertyName)
        {
            var _type = GetType();

            var _property = _type.GetProperty(_propertyName);

            var _value = (T)Convert.ChangeType(_property.GetValue(this), typeof(T));
            
            return _value;
        }

        public virtual void Set(string _propertyName, object _value)
        {
            var _type = GetType();

            var _property = _type.GetProperty(_propertyName);

            _property.SetValue(this, _value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
                GetType() != obj.GetType())
            {
                return false;
            }

            return obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode() => Convert.ToInt32(Id);
    }
}
