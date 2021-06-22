using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using core.Attributes;

namespace core.Tools
{
    public sealed class ReflectionTool
    {
        public static ReflectionTool Instance => _Instance.Value;

        private static readonly Lazy<ReflectionTool> _Instance =
            new Lazy<ReflectionTool>(() => new ReflectionTool());

        private ReflectionTool()
        {
        }

        public IEnumerable<Type> GetTypesInNamespace(
            Assembly _assymbly,
            string _nameSpace,
            Type _baseType = null,
            Type _implementedInterface = null)
        {
            IEnumerable<Type> _result = null;

            try
            {
                _result = _assymbly.
                    GetTypes().
                    Where(t => string.Equals(t.Namespace, _nameSpace, StringComparison.Ordinal));

                if (_implementedInterface != null)
                {
                    _result = _result.Where(x =>
                        ((TypeInfo)x).ImplementedInterfaces.Contains(_implementedInterface));
                }
                else if (_baseType != null)
                {
                    _result = _result.Where(x => x.BaseType == _baseType);
                }
            }
            catch (Exception _ex)
            {
                LogTool.Instance.Error(_ex);
            }

            return _result;
        }

        public Type GetEntityType(string _entityName) =>
            Type.GetType($"dal.Entities.{_entityName}, dal");

        public Dictionary<ColumnAttribute, JsonPropertyAttribute> GetInfoAttributes(Type _type) =>
            _type.GetProperties().
                Where(x => x.GetCustomAttribute<InfoAttribute>() != null).
                ToDictionary(
                    x => x.GetCustomAttribute<ColumnAttribute>(),
                    x => x.GetCustomAttribute<JsonPropertyAttribute>());

        public KeyValuePair<ColumnAttribute, JsonPropertyAttribute> GetIdentityAttribute(
            Type _type)
        {
            var _property = _type.
                GetProperties().
                FirstOrDefault(x => x.GetCustomAttribute<ColumnAttribute>().Identity);

            return new KeyValuePair<ColumnAttribute, JsonPropertyAttribute>(
                _property.GetCustomAttribute<ColumnAttribute>(),
                _property.GetCustomAttribute<JsonPropertyAttribute>()
            );
        }

        public JsonObjectAttribute GetTableJsonAttribute(Type _type) =>
            _type.GetCustomAttribute<JsonObjectAttribute>();
    }
}
