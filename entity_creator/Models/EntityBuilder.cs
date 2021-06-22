using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Geometries;
using core.Attributes;

namespace entity_creator.Models
{
    public class EntityBuilder
    {
        private readonly EntityDbContext _Context;

        private readonly IConfiguration _Config;

        private readonly string _ConnectionString;

        public EntityBuilder(EntityDbContext _context, IConfiguration _config)
        {
            _Context = _context;

            _Config = _config;

            _ConnectionString = _Config.GetConnectionString("Base");
        }

        public void Build()
        {
            try
            {
                var _tableMetadatas = GetTableMetadata();

                foreach (var _tableMetadata in _tableMetadatas)
                {
                    var _entityCreator = new EntityGenerator(_tableMetadata);

                    _entityCreator.Generate();
                }

                new DbContextGenerator(
                    _tableMetadatas.Select(x => x.EntityName),
                    "DalDbContext").
                Generate();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Dictionary<string, bool> GetTableNames()
        {
            var _result = new Dictionary<string, bool>();

            try
            {
                var _ignoredRegex = new string[]
                {
                    "^tables$",
                    "^table_columns$",
                    "^table_attribute",
                    "^column_$",
                    "^attribute",
                    "^RELATION",
                    "^MDRT",
                    "^pg_"
                };

                using var _connection = new Npgsql.NpgsqlConnection(_ConnectionString);

                _connection.Open();

                for (int i = 0; i < 2; i++)
                {
                    using var _command = _connection.CreateCommand();

                    if (i == 0)
                    {
                        _command.CommandText = "SELECT TABLE_NAME FROM " +
                            "INFORMATION_SCHEMA.TABLES " +
                            "WHERE TABLE_TYPE = 'BASE TABLE'";
                    }
                    else
                    {
                        _command.CommandText = "SELECT TABLE_NAME FROM " +
                            "INFORMATION_SCHEMA.VIEWS ";
                    }

                    using var _reader = _command.ExecuteReader();

                    if (_reader != null)
                    {
                        while (_reader.Read())
                        {
                            var _found = false;

                            var _tableName = _reader.GetString(0);

                            foreach (var pattern in _ignoredRegex)
                            {
                                if (Regex.Matches(_tableName, pattern).Count > 0)
                                {
                                    _found = true;
                                }
                            }

                            if (!_found && !_result.ContainsKey(_tableName))
                            {
                                _result.Add(_tableName, i == 1);
                            }
                        }
                    }

                    _reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _result;
        }


        private List<EntityMetadata> GetTableMetadata()
        {
            var _result = new List<EntityMetadata>();

            try
            {
                var _dbTables = GetTableNames();

                var _dbTableNames = _dbTables.Select(x => x.Key);

                var _tables = _Context.Table.
                    Where(x => _dbTableNames.Contains(x.TableName)).
                    ToList();

                foreach (var _table in _tables)
                {
                    var _data = new EntityMetadata
                    {
                        AssemblyName = "dal.Dynamic",
                        EntityName = _table.TableName,
                        AttributeList = new List<EntityAttributeMetadata>(),
                        IsView = _dbTables[_table.TableName],
                        PropertyList = GetColumnMetaData(_table.TableName)
                    };

                    _data.EntityName = _table.EntityName;

                    _data.AttributeList = DefaultTableMappingAttribute(
                        _table.TableName,
                        _table.DisplayName,
                        _table.SerializableColumn);

                    var _attributes = _Context.Table.
                            Where(x => x.TableName == _table.TableName).
                            Join(_Context.TableAttribute,
                                t => t.Id,
                                tc => tc.TableId,
                                (t, tc) => new
                                {
                                    TableAttribute = tc,
                                    Tables = t
                                }).
                            Join(_Context.Attribute,
                                a => a.TableAttribute.AttributeId,
                                b => b.Id,
                                (a, b) => new
                                {
                                    a.TableAttribute,
                                    a.Tables,
                                    Attribute = b
                                });

                    var _attributeProperties = _attributes.
                            Join(_Context.AttributeProperty,
                                c => c.Attribute.Id,
                                d => d.AttributeId,
                                (c, d) => new
                                {
                                    c.TableAttribute,
                                    c.Tables,
                                    c.Attribute,
                                    AttributeProperty = d
                                }).
                            Join(_Context.TableAttributeProperty,
                                aa => aa.AttributeProperty.Id,
                                bb => bb.AttributePropteryId,
                                (aa, bb) => new
                                {
                                    aa.TableAttribute,
                                    aa.Tables,
                                    aa.Attribute,
                                    aa.AttributeProperty,
                                    TableAttributeProperty = bb
                                }).
                            Where(x => x.Tables.Id == _table.Id).
                            Where(x => x.TableAttributeProperty.TableAttributeId ==
                                x.TableAttribute.Id).
                            ToList();

                    foreach (var _attribute in _attributes.Where(x => x.Tables.Id == _table.Id))
                    {
                        var _attributeMetadata = new EntityAttributeMetadata
                        {
                            ConstructorTypes = new List<Type>(),
                            ConstructorValues = new List<object>(),
                            PropertyNames = new List<string>(),
                            PropertyValues = new List<object>(),
                            AttributeType = Type.GetType(_attribute.Attribute.Name)
                        };

                        foreach (var attr in _attributeProperties.
                            Where(x => x.TableAttribute.Id == _attribute.TableAttribute.Id))
                        {
                            _attributeMetadata.PropertyNames.Add(attr.AttributeProperty.Name);

                            _attributeMetadata.PropertyValues.Add(
                                attr.TableAttributeProperty.Value);
                        }

                        _data.AttributeList.Add(_attributeMetadata);
                    }

                    _result.Add(_data);
                }
            }
            catch (Exception _ex)
            {
                throw;
            }

            return _result;
        }

        private static List<EntityAttributeMetadata> DefaultTableMappingAttribute(
            string _tableName,
            string _displayName,
            bool _serializable)
        {
            var _result = new List<EntityAttributeMetadata>();

            try
            {
                var _tableAttribute = new EntityAttributeMetadata
                {
                    AttributeType = typeof(TableAttribute),
                    ConstructorTypes = new List<Type>(),
                    ConstructorValues = new List<object>(),
                    PropertyNames = new List<string>() { "Name" },
                    PropertyValues = new List<object>() { _tableName }
                };

                _result.Add(_tableAttribute);

                if (_serializable)
                {
                    var _serializableAttribute = new EntityAttributeMetadata
                    {
                        AttributeType = typeof(JsonObjectAttribute),
                        ConstructorTypes = new List<Type>() { typeof(string) },
                        ConstructorValues = new List<object>() { _displayName },
                        PropertyNames = new List<string>(),
                        PropertyValues = new List<object>()
                    };

                    _result.Add(_serializableAttribute);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _result;
        }

        private List<EntityPropertyMetadata> GetColumnMetaData(string _tableName)
        {
            var _result = new List<EntityPropertyMetadata>();

            try
            {
                Console.WriteLine($"{_tableName} is starting.");

                using var _connection = new Npgsql.NpgsqlConnection(_ConnectionString);

                _connection.Open();

                using var _command = _connection.CreateCommand();

                _command.CommandText = "SELECT " + _tableName +
                    ".* from " + _tableName + " where 1 = 2";

                using var _reader = _command.ExecuteReader(CommandBehavior.KeyInfo);

                if (_reader != null)
                {
                    var _tableColumns = _Context.Table.
                            Where(x => x.TableName == _tableName).
                            Join(
                                _Context.TableColumn,
                                t => t.Id,
                                tc => tc.TableId,
                                (t, tc) => new { TableColumn = tc, Tables = t }).
                            OrderBy(x => x.TableColumn.OrderBy).
                            ToList();

                    var _columnAttributes = _tableColumns.
                        Join(_Context.ColumnAttribute,
                            r => r.TableColumn.Id,
                            c => c.ColumnId,
                            (r, c) => new
                            {
                                r.TableColumn,
                                r.Tables,
                                ColumnAttribute = c
                            }).
                        Join(_Context.Attribute,
                            rr => rr.ColumnAttribute.AttributeId,
                            a => a.Id,
                            (rr, a) => new
                            {
                                rr.TableColumn,
                                rr.Tables,
                                rr.ColumnAttribute,
                                Attribute = a
                            }).
                        ToList();

                    var _attributeProperties = _Context.AttributeProperty.ToList();

                    var _columnAttributeProperties = _Context.ColumnAttributeProperty.ToList();

                    foreach (var _tableColumn in _tableColumns)
                    {
                        for (int i = 0; i < _reader.FieldCount; i++)
                        {
                            var _columnName = _reader.GetName(i);

                            if (_columnName != _tableColumn.TableColumn.ColumnName)
                            {
                                continue;
                            }

                            var _columnType = _reader.GetFieldType(i);

                            if (_columnName == "versiyon" && _columnType == typeof(decimal))
                            {
                                _columnType = typeof(decimal);
                            }
                            else
                            {
                                _columnType = _columnType == typeof(DateTime) ?
                                    typeof(DateTime?) :
                                    _columnType;
                            }

                            var _isSpatial = _columnName == "geoloc";

                            var _metaData = new EntityPropertyMetadata
                            {
                                PropertyName = _columnName,
                                PropertyType = _isSpatial ? typeof(Geometry) : _columnType,
                                AttributeList = new List<EntityAttributeMetadata>(),
                            };

                            var _columnData = _tableColumns.
                                Where(x => x.TableColumn.ColumnName == _columnName).
                                Select(x => x.TableColumn).
                                FirstOrDefault();

                            if (_columnData != null)
                            {
                                _metaData.PropertyName = _columnData.PropertyName;

                                _metaData.IsIdentity = _columnData.IdentityColumn;

                                if (_result.Any(x =>
                                    x.PropertyName == _metaData.PropertyName &&
                                    x.PropertyType == _metaData.PropertyType))
                                {
                                    continue;
                                }

                                _result.Add(_metaData);

                                _metaData.AttributeList = DefaultPropertyMappingAttribute(
                                    _columnData.ColumnName,
                                    _columnData.DisplayName,
                                    _columnData.SerializableColumn,
                                    _columnData.IdentityColumn,
                                    _columnData.SequenceName,
                                    _isSpatial);

                                var _attributes = _columnAttributes.
                                    Join(_attributeProperties,
                                        aa => aa.ColumnAttribute.AttributeId,
                                        bb => bb.AttributeId,
                                        (aa, bb) => new
                                        {
                                            aa.TableColumn,
                                            aa.Tables,
                                            aa.ColumnAttribute,
                                            aa.Attribute,
                                            AttributeProperty = bb
                                        }).
                                    Join(_columnAttributeProperties,
                                        aa => aa.AttributeProperty.Id,
                                        bb => bb.AttributePropteryId,
                                        (aa, bb) => new
                                        {
                                            aa.TableColumn,
                                            aa.Tables,
                                            aa.ColumnAttribute,
                                            aa.Attribute,
                                            aa.AttributeProperty,
                                            ColumnAttributeProperty = bb
                                        }).
                                     Where(x => x.TableColumn.Id == _columnData.Id).
                                     ToList();

                                foreach (var _columnAttribute in _columnAttributes.
                                    Where(x => x.TableColumn.Id == _columnData.Id))
                                {
                                    var _assembly = _columnAttribute.Attribute.Name.Split('.')[0];

                                    var _attributeMetadata = new EntityAttributeMetadata
                                    {
                                        ConstructorTypes = new List<Type>(),
                                        ConstructorValues = new List<object>(),
                                        PropertyNames = new List<string>(),
                                        PropertyValues = new List<object>(),
                                        AttributeType = Type.GetType(
                                            $"{_columnAttribute.Attribute.Name},{_assembly}")
                                    };

                                    foreach (var _attribute in _attributes.Where(x =>
                                        x.ColumnAttribute.Id ==
                                            _columnAttribute.ColumnAttribute.Id))
                                    {
                                        _attributeMetadata.PropertyNames.Add(
                                            _attribute.AttributeProperty.Name);

                                        _attributeMetadata.PropertyValues.Add(
                                            _attribute.ColumnAttributeProperty.Value);
                                    }

                                    _metaData.AttributeList.Add(_attributeMetadata);
                                }

                                Console.WriteLine($"\t{_columnName} is done.");
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _result;
        }

        private List<EntityAttributeMetadata> DefaultPropertyMappingAttribute(
            string _columnName,
            string _displayName,
            bool _serializable,
            bool _identity,
            string _sequenceName,
            bool _spatial)
        {
            var _result = new List<EntityAttributeMetadata>();

            try
            {
                var _tableAttribute = new EntityAttributeMetadata
                {
                    AttributeType = typeof(ColumnAttribute),
                    ConstructorTypes = new List<Type>(),
                    ConstructorValues = new List<object>(),
                    PropertyNames = new List<string>()
                    {
                        "Name", "Identity", "Spatial", "SequenceName"
                    },
                    PropertyValues = new List<object>()
                    {
                        _columnName, _identity, _spatial, _sequenceName
                    }
                };

                _result.Add(_tableAttribute);

                if (!_spatial && _serializable)
                {
                    var _jsonAttribute = new EntityAttributeMetadata
                    {
                        AttributeType = typeof(JsonPropertyAttribute),
                        ConstructorTypes = new List<Type>() { typeof(string) },
                        ConstructorValues = new List<object>() { _displayName },
                        PropertyNames = new List<string>(),
                        PropertyValues = new List<object>()
                    };

                    _result.Add(_jsonAttribute);
                }
                else
                {
                    var _jsonIgnoreAttribute = new EntityAttributeMetadata
                    {
                        AttributeType = typeof(JsonIgnoreAttribute),
                        ConstructorTypes = new List<Type>(),
                        ConstructorValues = new List<object>(),
                        PropertyNames = new List<string>() { },
                        PropertyValues = new List<object>() { }
                    };

                    _result.Add(_jsonIgnoreAttribute);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _result;
        }
    }
}