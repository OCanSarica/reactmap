using core.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Reflection;
using core.Tools;

namespace dal.Models
{
    public partial class DalDbContext : DbContext
    {
        public DalDbContext(DbContextOptions<DalDbContext> _options): base(_options)
        {
        }

        protected override void OnModelCreating(ModelBuilder _modelBuilder)
        {
            _modelBuilder.HasPostgresExtension("postgis");

            var _types = ReflectionTool.Instance.GetTypesInNamespace(
                Assembly.GetExecutingAssembly(), "dal.Entities");

            foreach (var _type in _types)
            {
                _modelBuilder.Entity(_type.FullName, _entity =>
                {
                    var _tableAttribute = _type.GetCustomAttribute<TableAttribute>();

                    _entity.ToTable(_tableAttribute.Name);

                    foreach (var _property in _type.GetProperties())
                    {
                        var _columnAttribute = _property.GetCustomAttribute<ColumnAttribute>();

                        var _propBuilder = _entity.Property(_property.Name);

                        if (_columnAttribute.Spatial)
                        {
                            _propBuilder.
                                HasColumnName(_columnAttribute.Name).
                                HasColumnType("geometry");

                            continue;
                        }

                        _propBuilder.HasColumnName(_columnAttribute.Name);

                        if (_columnAttribute.Identity)
                        {
                            _entity.HasKey(_property.Name);

                            if (!string.IsNullOrEmpty(_columnAttribute.SequenceName))
                            {
                                _propBuilder.HasValueGenerator(
                                    (a, b) => new SequenceValueGenerator(
                                        "public",
                                        _columnAttribute.SequenceName,
                                        _property.PropertyType));
                            }
                        }
                    }
                });
            }

            OnModelCreatingPartial(_modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        internal class SequenceValueGenerator : ValueGenerator<object>
        {
            private readonly string _Schema;

            private readonly string _SequenceName;

            private readonly Type _Type;

            public SequenceValueGenerator(string _schema, string _sequenceName, Type _type)
            {
                _Schema = _schema;

                _SequenceName = _sequenceName;

                _Type = _type;
            }

            public override bool GeneratesTemporaryValues => false;

            public override object Next(EntityEntry _entry)
            {
                using var _command = _entry.Context.Database.GetDbConnection().CreateCommand();

                _command.CommandText = $"SELECT nextval('{_SequenceName}')";

                _entry.Context.Database.OpenConnection();

                using var _reader = _command.ExecuteReader();

                _reader.Read();

                var _value = _reader.GetValue(0);

                return Convert.ChangeType(_value, _Type);
            }
        }
    }
}
