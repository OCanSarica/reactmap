using entity_creator.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using core.Attributes;
using core.Tools;

namespace entity_creator.Models
{
    public partial class EntityDbContext : DbContext
    {
        public virtual DbSet<Entities.Table> Table { get; set; }

        public virtual DbSet<Entities.TableAttribute> TableAttribute { get; set; }

        public virtual DbSet<Entities.TableAttributeProperty> TableAttributeProperty { get; set; }

        public virtual DbSet<Entities.TableColumn> TableColumn { get; set; }

        public virtual DbSet<Entities.ColumnAttribute> ColumnAttribute { get; set; }

        public virtual DbSet<Entities.Attribute> Attribute { get; set; }

        public virtual DbSet<Entities.AttributeProperty> AttributeProperty { get; set; }

        public virtual DbSet<Entities.ColumnAttributeProperty> ColumnAttributeProperty
        {
            get; set;
        }

        public EntityDbContext(DbContextOptions<EntityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder _modelBuilder)
        {
            _modelBuilder.HasPostgresExtension("postgis");

            var _types = ReflectionTool.Instance.
                GetTypesInNamespace(
                    Assembly.GetExecutingAssembly(),
                    "entity_creator.Entities",
                    null,
                    typeof(IEntity));

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

                        if (_columnAttribute.Identity &&
                            !string.IsNullOrEmpty(_columnAttribute.SequenceName))
                        {
                            _entity.HasKey(_property.Name);

                            _property.GetType();
                        }
                    }
                });
            }

            OnModelCreatingPartial(_modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
