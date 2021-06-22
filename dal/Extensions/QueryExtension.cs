using core.Attributes;
using core.Models;
using dynamic_linq;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace dal.Extensions
{
    public static class QueryExtension
    {
        private static MethodInfo _WhereMethod = null;

        public static IQueryable Search(this IQueryable _source,
            string[] _propertyNames,
            string[] _searchValues,
            bool _and)
        {
            var _searchMethod = typeof(QueryExtension).FindMethod("Search", true, 4).
                MakeGenericMethod(_source.ElementType);

            var _searchObj = _searchMethod.Invoke(null,
                new object[] { _source, _propertyNames, _searchValues, _and }) as IQueryable;

            return _searchObj;
        }

        public static IQueryable SearchAnd(this IQueryable _source,
            string[] _propertyNames,
            string[] _searchValues)
        {
            var _searchMethod = typeof(QueryExtension).FindMethod("SearchAnd", true, 3).
                MakeGenericMethod(_source.ElementType);

            var _searchObj = _searchMethod.Invoke(null,
                new object[] { _source, _propertyNames, _searchValues }) as IQueryable;

            return _searchObj;
        }

        public static IQueryable SearchOr(this IQueryable _source,
            string[] _propertyNames,
            string[] _searchValues)
        {
            var _searchMethod = typeof(QueryExtension).FindMethod("SearchOr", true, 3).
                MakeGenericMethod(_source.ElementType);

            var _searchObj = _searchMethod.Invoke(null,
                new object[] { _source, _propertyNames, _searchValues }) as IQueryable;

            return _searchObj;
        }

        public static IQueryable<T> SearchAnd<T>(this IQueryable<T> _source,
            string[] _propertyNames,
            string[] _searchValues)
        {
            return Search(_source, _propertyNames, _searchValues, true);
        }

        public static IQueryable<T> SearchOr<T>(this IQueryable<T> _source,
            string[] _propertyNames,
            string[] _searchValues)
        {
            return Search(_source, _propertyNames, _searchValues, false);
        }

        private static IQueryable<T> Search<T>(this IQueryable<T> _source,
            string[] _propertyNames,
            string[] _searchValues, bool and)
        {
            if (_propertyNames != null &&
                _searchValues != null &&
               _propertyNames.Length == _searchValues.Length &&
               _propertyNames.Length != 0)
            {
                Type _type = typeof(T);

                ParameterExpression _parameter = Expression.Parameter(_type, "x");

                Expression _body = null;

                for (int i = 0; i < _propertyNames.Length; i++)
                {
                    var _propertyName = _propertyNames[i];

                    var _searchValue = _searchValues[i] ?? "";

                    if (String.IsNullOrEmpty(_searchValue))
                    {
                        continue;
                    }

                    Expression _property = Expression.Property(_parameter, _propertyName);

                    PropertyInfo _info = _type.GetProperty(_propertyName);

                    if (_info.PropertyType == typeof(int))
                    {
                        if (int.TryParse(_searchValue.ToUpper(), out int _val))
                        {
                            _property = Expression.Equal(_property, Expression.Constant(_val));
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (_info.PropertyType == typeof(string))
                    {
                        CultureInfo _culture = new CultureInfo("tr");

                        _property = Expression.Call(_property, "ToUpper", null);

                        _property = Expression.Call(_property,
                            "Contains",
                            null,
                            new[] { Expression.Constant(_searchValue.ToUpper(_culture)) });
                    }
                    else if (_info.PropertyType == typeof(DateTime?))
                    {
                        DateTime _end;

                        if (DateTime.TryParseExact(_searchValue.ToUpper(),
                            "MM/dd/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime _start))
                        {
                            _end = _start.AddHours(23);

                            _end = _end.AddMinutes(59);

                            _end = _end.AddSeconds(59);

                            _end = _end.AddMilliseconds(999);

                            Expression _exp1 = Expression.GreaterThanOrEqual(_property,
                                Expression.Convert(Expression.Constant(_start), _property.Type));

                            Expression _exp2 = Expression.LessThanOrEqual(_property,
                                Expression.Convert(Expression.Constant(_end), _property.Type));

                            _property = Expression.AndAlso(_exp1, _exp2);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        try
                        {
                            _property = Expression.Equal(_property,
                                Expression.Constant(
                                    Convert.ChangeType(_searchValue.ToUpper(), _info.PropertyType)));
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (_body == null)
                    {
                        _body = _property;
                    }
                    else if (and)
                    {
                        _body = Expression.AndAlso(_body, _property);
                    }
                    else if (!and)
                    {
                        _body = Expression.OrElse(_body, _property);
                    }
                }

                if (_body == null)
                {
                    return _source;
                }

                var _lambda = Expression.Lambda<Func<T, bool>>(_body, _parameter);

                if (_WhereMethod == null)
                {
                    _WhereMethod = typeof(Queryable).GetMethods().
                        Where(x => x.Name == "Where").
                        Select(x => new { M = x, P = x.GetParameters() }).
                        Where(x => x.P.Length == 2 &&
                            x.P[0].ParameterType.IsGenericType &&
                            x.P[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>) &&
                            x.P[1].ParameterType.IsGenericType &&
                            x.P[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>)).
                        Select(x => new { x.M, A = x.P[1].ParameterType.GetGenericArguments() }).
                        Where(x => x.A[0].IsGenericType &&
                            x.A[0].GetGenericTypeDefinition() == typeof(Func<,>)).
                        Select(x => new { x.M, A = x.A[0].GetGenericArguments() }).
                        Where(x => x.A[0].IsGenericParameter && x.A[1] == typeof(bool)).
                        Select(x => x.M).
                        SingleOrDefault();
                }

                object _result = _WhereMethod.MakeGenericMethod(_type).
                    Invoke(null, new object[] { _source, _lambda });

                return (IQueryable<T>)_result;
            }

            return _source;
        }

        public static IOrderedQueryable OrderHelper(this IQueryable _source,
            string[] _order, string[] _property)
        {
            var method = typeof(QueryExtension).FindMethod("OrderHelper", true, 3).
                MakeGenericMethod(_source.ElementType);

            var _obj = method.Invoke(null,
                new object[] { _source, _order, _property }) as IOrderedQueryable;

            return _obj;
        }

        public static IOrderedQueryable OrderByHelper(this IQueryable _source, string _property)
        {
            var _method = typeof(QueryExtension).FindMethod("OrderByHelper", true, 2).
                MakeGenericMethod(_source.ElementType);

            var _obj = _method.Invoke(null, new object[] { _source, _property }) as IOrderedQueryable;

            return _obj;
        }

        public static IOrderedQueryable<T> OrderHelper<T>(this IQueryable<T> _source,
            string[] _order,
            string[] _property)
        {
            if (_order != null &&
                _property != null &&
                _order.Length == _property.Length &&
                _order.Length != 0)
            {
                IOrderedQueryable<T> _result = null;

                for (int i = 0; i < _property.Length; i++)
                {
                    if (_order[i] == "asc")
                    {
                        if (i == 0)
                        {
                            _result = OrderByHelper(_source, _property[i]);
                        }
                        else
                        {
                            _result = ThenBy(_result, _property[i]);
                        }
                    }
                    else if (_order[i] == "desc")
                    {
                        if (i == 0)
                        {
                            _result = OrderByDescendingHelper(_source, _property[i]);
                        }
                        else
                        {
                            _result = ThenByDescending(_result, _property[i]);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("_order sadece asc veya desc olabilir");
                    }
                }

                return _result;
            }

            return null;
        }

        public static IOrderedQueryable<T> OrderByHelper<T>(this IQueryable<T> _source, string _property)
        {
            return ApplyOrder<T>(_source, _property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescendingHelper<T>(this IQueryable<T> _source,
            string _property)
        {
            return ApplyOrder<T>(_source, _property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> _source,
            string _property)
        {
            return ApplyOrder<T>(_source, _property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> _source,
            string _property)
        {
            return ApplyOrder<T>(_source, _property, "ThenByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> _source,
            string _property,
            string _methodName)
        {
            string[] _props = _property.Split('.');

            Type _type = typeof(T);

            ParameterExpression _arg = Expression.Parameter(_type, "x");

            Expression _expr = _arg;

            foreach (string _prop in _props)
            {
                PropertyInfo _pi = _type.GetProperty(_prop);

                _expr = Expression.Property(_expr, _pi);

                _type = _pi.PropertyType;
            }

            Type _delegateType = typeof(Func<,>).MakeGenericType(typeof(T), _type);

            LambdaExpression _lambda = Expression.Lambda(_delegateType, _expr, _arg);

            object _result = typeof(Queryable).GetMethods().
                Single(method => method.Name == _methodName &&
                    method.IsGenericMethodDefinition &&
                    method.GetGenericArguments().Length == 2 &&
                    method.GetParameters().Length == 2).
                MakeGenericMethod(typeof(T), _type).
                Invoke(null, new object[] { _source, _lambda });

            return (IOrderedQueryable<T>)_result;
        }

        private static IOrderedQueryable<T> ApplyOrderAlt<T>(this IQueryable<T> _source,
            string _property,
            string _methodName)
        {
            string[] _props = _property.Split('.');

            var _baseType = typeof(T);

            if (_baseType == typeof(object))
            {
                var _first = _source.FirstOrDefault();

                if (_first != null)
                {
                    _baseType = _first.GetType();
                }
                else
                {
                    return (IOrderedQueryable<T>)_source;
                }
            }

            var _propertyType = typeof(object);

            ParameterExpression _arg = Expression.Parameter(_baseType, "x");

            Expression _expr = _arg;

            foreach (string _prop in _props)
            {
                PropertyInfo _pi = _baseType.GetProperty(_prop);

                _expr = Expression.Property(_expr, _pi);

                _propertyType = _pi.PropertyType;
            }

            Type _delegateType = typeof(Func<,>).MakeGenericType(_baseType, _propertyType);

            LambdaExpression _lambda = Expression.Lambda(_delegateType, _expr, _arg);

            object _result = typeof(Queryable).GetMethods().
                Single(method => method.Name == _methodName &&
                    method.IsGenericMethodDefinition &&
                    method.GetGenericArguments().Length == 2 &&
                    method.GetParameters().Length == 2).
                MakeGenericMethod(_baseType, _propertyType).
                Invoke(null, new object[] { _source, _lambda });

            return (IOrderedQueryable<T>)_result;
        }

        public static IQueryable<T> IfWhere<T>(this IQueryable<T> _source,
            bool _ifTrue,
            Expression<Func<T, bool>> _predicate)
        {
            if (_ifTrue)
            {
                return _source.Where(_predicate);
            }

            return _source;
        }

        public static MethodInfo FindMethod(this Type _type, string _method)
        {
            return FindMethod(_type, _method, null, null, null, new Type[] { });
        }

        public static MethodInfo FindMethod(this Type _type, string _method, bool? _isGenericMethod)
        {
            return FindMethod(_type, _method, _isGenericMethod, null, null, new Type[] { });
        }

        public static MethodInfo FindMethod(this Type _type,
            string _method,
            bool? _isGenericMethod,
            int? _parameterCount)
        {
            return FindMethod(_type, _method, _isGenericMethod, _parameterCount, null, new Type[] { });
        }

        public static MethodInfo FindMethod(this Type type,
            string _method,
            bool? _isGenericMethod,
            int? _parameterCount,
            params Type[] _parameterTypes)
        {
            return FindMethod(type, _method, _isGenericMethod, _parameterCount, null, _parameterTypes);
        }

        public static MethodInfo FindMethod(this Type _type,
            string _method,
            bool? _isGenericMethod,
            int? _parameterCount,
            BindingFlags? _bindingsFlag,
            params Type[] _parameterTypes)
        {
            var _methodInfoList = (_bindingsFlag == null ?
                _type.GetMethods() :
                _type.GetMethods(_bindingsFlag.Value)).
                Where(x => x.Name == _method &&
                    (_isGenericMethod != null ? x.IsGenericMethod == _isGenericMethod : true) &&
                    (_parameterCount != null ? x.GetParameters().Count() == _parameterCount : true)).
                OrderBy(x => x.GetParameters().Count());

            if (_parameterTypes != null && _parameterTypes.Length != 0)
            {
                foreach (var _info in _methodInfoList)
                {
                    var _parameterInfoList = _info.GetParameters();

                    if (_parameterInfoList.Length >= _parameterTypes.Length)
                    {
                        int i = 0;

                        for (; i < _parameterTypes.Length; i++)
                        {
                            var _param = _parameterTypes[i];

                            if (_parameterInfoList[i].ParameterType != _param)
                            {
                                break;
                            }
                        }

                        if (i == _parameterTypes.Length)
                        {
                            return _info;
                        }
                    }
                }

                return null;
            }

            return _methodInfoList.First();
        }

        public static IQueryable Union(this IQueryable _source, IQueryable _other)
        {
            if (_source == null)
            {
                throw new ArgumentNullException("source");
            }

            return _source.Provider.CreateQuery(
                Expression.Call(typeof(Queryable),
                    "Union",
                    new Type[] { _source.ElementType },
                    _source.Expression,
                    _other.Expression));
        }

        public static IEnumerable<string> IdPropertyNames(this Type _type)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute
                }).
                Where(x => x.a?.Identity == true).
                Select(x => x.p.Name);
        }

        public static IEnumerable<string> IdColumnNames(this Type _type)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute
                }).
                Where(x => x.a?.Identity == true).
                Select(x => x.a.Name);
        }

        public static IEnumerable<string> EntityPropertyNames(this Type _type)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute
                }).
                Select(x => x.a.Name);
        }

        public static IEnumerable<string> IdSerializablePropertyNames(this Type _type)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute,
                    b = x.a.FirstOrDefault(w =>
                        w.GetType() == typeof(JsonPropertyAttribute)) as JsonPropertyAttribute
                }).
                Where(x => x.a?.Identity == true && x.b != null).
                Select(x => x.b.PropertyName);
        }

        public static string GetGeometryBeautifulName(this Type _type)
        {
            foreach (var _item in _type.GetProperties())
            {

                if (_type.GeometryNames().ElementAt(0) + "WKT" == _item.Name)
                {
                    DataMemberAttribute _attr = (DataMemberAttribute)_item.GetCustomAttribute(
                        typeof(DataMemberAttribute));

                    return _attr.Name;
                }
            }

            return null;
        }

        public static IEnumerable<string> SerializablePropertyNames(this Type _type)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute,
                    b = x.a.FirstOrDefault(w =>
                        w.GetType() == typeof(JsonPropertyAttribute)) as JsonPropertyAttribute
                }).
                Where(x => x.b != null).
                Select(x => x.b.PropertyName);
        }

        public static IEnumerable<string> PropertyNames(this Type _type)
        {
            return _type.GetProperties().Select(i => i.Name);

        }

        public static IEnumerable<PropertyInfo> SerializableProperty(this Type _type)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute,
                    b = x.a.FirstOrDefault(w =>
                        w.GetType() == typeof(JsonPropertyAttribute)) as JsonPropertyAttribute
                }).
                Where(x => x.b != null).
                Select(x => x.p);
        }

        public static string SerializablePropertyOnlyName(this Type _type, string _columnName)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute,
                    b = x.a.FirstOrDefault(w =>
                        w.GetType() == typeof(JsonPropertyAttribute)) as JsonPropertyAttribute
                }).
                Where(x => x.b != null && x.p.Name == _columnName).Select(x => x.p.Name).FirstOrDefault();

        }

        public static IEnumerable<string> GeometryNames(this Type _type)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute
                }).
                Where(x => x.a?.Spatial == true).
                Select(x => x.a.Name);
        }


        public static IEnumerable<Attribute> FindAttribute(this Type _type, Type _attributeType)
        {
            return _type.GetCustomAttributes().Where(w => w.GetType() == _attributeType);
        }

        public static IEnumerable<Type> IdPropertyTypes(this Type _type)
        {
            return _type.GetProperties().
                Select(i => new { p = i, a = i.GetCustomAttributes() }).
                Select(x => new
                {
                    x.p,
                    a = x.a.FirstOrDefault(w => w.GetType() == typeof(ColumnAttribute)) as ColumnAttribute
                }).
                Where(x => x.a?.Identity == true).
                Select(x => x.p.PropertyType);
        }

        public static string ToLineWKt(this List<List<double>> _coordinates)
        {
            var _result = new List<Coordinate>();

            _coordinates.ForEach(x =>
            {
                _result.Add(new Coordinate(x.ElementAt(0), x.ElementAt(1)));
            });

            return WKTWriter.ToLineString(_result.ToArray());
        }

        public static double ToRadians(this double _angleIn10thofaDegree)
        {
            return (_angleIn10thofaDegree * Math.PI) / 1800;
        }

        public static IQueryable Paginate(this IQueryable _query,
            PaginatorDto _paginator,
            out int _count)
        {
            _count = 0;

            if (_paginator == null)
            {
                return _query;
            }

            _paginator.PrepareForPagination();

            var _order = _paginator.OrderValues;

            var _orderColumns = _paginator.OrderColumns;

            var _offset = _paginator.Offset;

            var _limit = _paginator.Limit;

            var _filterColumns = _paginator.FilterColumns;

            var _filterValues = _paginator.FilterValues;

            var _searchText = new string[_paginator.SearchColumns?.Length ?? 0];

            var _searchColumns = _paginator.SearchColumns;

            for (int i = 0; i < _searchText.Length; i++)
            {
                _searchText[i] = _paginator.SearchText;
            }

            if (_filterColumns != null && _filterValues != null && _filterColumns.Any())
            {
                _query = _query.SearchAnd(_filterColumns, _filterValues);
            }

            if (_searchColumns != null && _paginator.SearchText != null && _searchColumns.Any())
            {
                _query = _query.SearchOr(_searchColumns, _searchText);
            }

            if (_order != null && _orderColumns != null && _orderColumns.Any() && _order.Any())
            {
                _query = _query.OrderHelper(_order, _orderColumns);
            }

            _count = _query.Count();

            if (_offset != null)
            {
                _query = _query.Skip(_offset.Value);
            }

            if (_limit != null)
            {
                if (_limit == 0)
                {
                    _query = _query.Take(_count);
                }
                else
                {
                    _query = _query.Take(_limit.Value);
                }
            }

            return _query;
        }

        public static T[] ToArray<T>(this IQueryable _querable)
        {
            T[] _array = new T[1000];

            int _index = 0;

            foreach (var item in _querable)
            {
                var i = _index++;

                if (_array.Length <= i)
                {
                    Array.Resize(ref _array, _array.Length + 1000);
                }

                _array[i] = (T)item;
            }

            Array.Resize(ref _array, _index);

            return _array;
        }

        public static object[] ToArray(this IQueryable _querable)
        {
            object[] _array = new object[1000];

            int _index = 0;

            foreach (var _item in _querable)
            {
                var i = _index++;

                if (_array.Length <= i)
                {
                    Array.Resize(ref _array, _array.Length + 1000);
                }

                _array[i] = _item;
            }

            Array.Resize(ref _array, _index);

            return _array;
        }

        public static T[] ToArray<T>(this IQueryable _querable, int _size)
        {
            T[] _array = new T[_size];

            int _index = 0;

            foreach (var _item in _querable)
            {
                _array[_index++] = (T)_item;
            }

            return _array;
        }

        public static object[] ToArray(this IQueryable _querable, int _size)
        {
            try
            {
                object[] _array = new object[_size];

                int _index = 0;

                foreach (var _item in _querable)
                {
                    _array[_index++] = _item;
                }

                if (_index != _size)
                {
                    Array.Resize(ref _array, _index);
                }

                return _array;
            }
            catch (Exception _ex)
            {
                return new object[_size];
            }
        }

        public static IList EnumerableToList(this IEnumerable _querable)
        {
            List<object> _result = new List<object>();

            foreach (var _item in _querable)
            {
                _result.Add(_item);
            }

            return _result;
        }

        public static IList ToIList(this IQueryable _querable)
        {
            List<object> _result = new List<object>();

            foreach (var item in _querable)
            {
                _result.Add(item);
            }

            return _result;
        }


        public static IList<T> EnumerableToList<T>(this IEnumerable _querable)
        {
            List<T> _result = new List<T>();

            foreach (var _item in _querable)
            {
                _result.Add((T)_item);
            }

            return _result;
        }

        public static List<T> ToList<T>(this IQueryable _querable)
        {
            List<T> _result = new List<T>();

            foreach (var _item in _querable)
            {
                _result.Add((T)_item);
            }

            return _result;
        }

        public static List<T> ToListDynamic<T>(this IQueryable _querable)
        {
            List<T> _result = new List<T>();

            foreach (var item in _querable)
            {
                _result.Add((T)item);
            }

            return _result;
        }

        private static void PrepareForPagination(this PaginatorDto _paginator)
        {
            TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

            var _filterColumns = new List<string>();

            var _searchColumns = new List<string>();

            var _orderColumns = new List<string>();

            if (_paginator.FilterColumns != null)
            {
                foreach (var _item in _paginator.FilterColumns)
                {
                    _filterColumns.Add(_textInfo.ToTitleCase(_item));
                }
            }

            if (_paginator.SearchColumns != null)
            {
                foreach (var _item in _paginator.SearchColumns)
                {
                    _searchColumns.Add(_textInfo.ToTitleCase(_item));
                }
            }
            if (_paginator.OrderColumns != null)
            {
                foreach (var _item in _paginator.OrderColumns)
                {
                    _orderColumns.Add(_textInfo.ToTitleCase(_item));
                }
            }

            _paginator.FilterColumns = _filterColumns.ToArray();

            _paginator.SearchColumns = _searchColumns.ToArray();

            _paginator.OrderColumns = _orderColumns.ToArray();
        }
    }
}
