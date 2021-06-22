using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using core.Models;

namespace core.Tools
{
    public sealed class EnumTool
    {
        private static readonly Lazy<EnumTool> _Instance =
            new Lazy<EnumTool>(() => new EnumTool());

        public static EnumTool Instance => _Instance.Value;

        public T StringToEnum<T>(string _name)
        {
            return (T)Enum.Parse(typeof(T), _name);
        }

        public List<T> GetEnumItems<T>()
        {
            return Enum.GetValues(typeof(T)).
                Cast<T>().
                ToList();
        }
    }
}
