using entity_creator.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace entity_creator.Models
{
    class DbContextGenerator : EntityGeneratorBase
    {
        private readonly IEnumerable<string> _Entities;

        public DbContextGenerator(IEnumerable<string> _entities, string _className)
        {
            _Entities = _entities;

            _TargetUnit = new CodeCompileUnit();

            var _namespace = new CodeNamespace("dal.Models");

            _TargetClass = new CodeTypeDeclaration(_className)
            {
                IsClass = true,

                IsPartial = true,

                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            _TargetClass.BaseTypes.Add(typeof(DbContext));

            _namespace.Types.Add(_TargetClass);

            _namespace.Imports.Add(new CodeNamespaceImport("Microsoft.EntityFrameworkCore"));

            _TargetUnit.Namespaces.Add(_namespace);

            var _path = Environment.CurrentDirectory + "/temp/Entities";

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            _OutputFileName = Path.Combine(_path, _className + ".cs");
        }

        private void AddDbSets()
        {
            foreach (var _entity in _Entities)
            {
                var _property = new CodeMemberField
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final
                };

                _property.Name = _entity + " { get; set; }//";

                _property.Type = new CodeTypeReference($"DbSet<dal.Entities.{_entity}>");

                _TargetClass.Members.Add(_property);
            }
        }

        public void Generate()
        {
            AddDbSets();

            GenerateCSharpCode(_OutputFileName);
        }
    }
}
