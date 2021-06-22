using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{

    public static class FilterUtil
    {
        private static IList<FilterableModel> filterableList = new List<FilterableModel>();
        private static Dictionary<int, Type> typeHash = new Dictionary<int, Type>();
        private static Dictionary<Type, FilterableAttribute> typeFilterHash = new Dictionary<Type, FilterableAttribute>();

        private static List<Assembly> assemblyList = new List<Assembly>();
        private static List<Type> typeList = new List<Type>();

        public static void Register(Assembly assembly)
        {
            lock (assemblyList)
            {
                if (!assemblyList.Contains(assembly))
                {
                    assemblyList.Add(assembly);
                }
            }
        }

        public static void Register(Type type)
        {
            lock (typeList)
            {
                if (!typeList.Contains(type))
                {
                    typeList.Add(type);
                }
            }
        }

        public static void ClearCache()
        {
            assemblyList.Clear();
            typeList.Clear();
            filterableList.Clear();
            typeHash.Clear();
            typeFilterHash.Clear();
        }

        public static FilterableAttribute GetTypeAttributes(Type type)
        {
            return typeFilterHash[type];
        }
    }
}