using core.Bases;
using entity_creator.Bases;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace entity_creator.Models
{
    internal class EntityGenerator : EntityGeneratorBase
    {
        private readonly EntityMetadata _EntityMetadata;

        public EntityGenerator(EntityMetadata _entityMetadata)
        {
            _EntityMetadata = _entityMetadata;

            _TargetUnit = new CodeCompileUnit();

            var _namespace = new CodeNamespace("dal.Entities");

            _TargetClass = new CodeTypeDeclaration(_EntityMetadata.EntityName)
            {
                IsClass = true,

                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            foreach (var _attributeMetadata in _entityMetadata.AttributeList)
            {
                if (_attributeMetadata.AttributeType == null)
                {
                    continue;
                }

                var _attribute = new CodeAttributeDeclaration(
                       new CodeTypeReference(_attributeMetadata.AttributeType));

                for (int i = 0; i < _attributeMetadata.PropertyNames.Count; i++)
                {
                    var _attributePropertyName = _attributeMetadata.PropertyNames[i];

                    var _attributePropertyValue = _attributeMetadata.PropertyValues[i];

                    _attribute.Arguments.Add(
                        new CodeAttributeArgument
                        {
                            Name = _attributePropertyName,
                            Value = new CodePrimitiveExpression(_attributePropertyValue)
                        }
                    );
                }

                foreach (var _attributeValue in _attributeMetadata.ConstructorValues)
                {
                    _attribute.Arguments.Add(
                        new CodeAttributeArgument(
                            new CodePrimitiveExpression(_attributeValue.ToString())));
                }

                _TargetClass.CustomAttributes.Add(_attribute);
            }

            _TargetClass.BaseTypes.Add(typeof(EntityBase));

            _namespace.Types.Add(_TargetClass);

            _TargetUnit.Namespaces.Add(_namespace);

            var _path = Environment.CurrentDirectory + "/temp/Entities";

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            _OutputFileName = Path.Combine(_path, _EntityMetadata.EntityName + ".cs");
        }

        private void AddProperties()
        {
            foreach (var _propertyMetadata in _EntityMetadata.PropertyList)
            {
                var _property = new CodeMemberProperty
                {
                    Attributes = MemberAttributes.Public | 
                        (!_propertyMetadata.IsIdentity ?
                            MemberAttributes.Final :
                            MemberAttributes.Override)
                };

                foreach (var _attributeMetadata in _propertyMetadata.AttributeList)
                {
                    if (_attributeMetadata.AttributeType == null)
                    {
                        continue;
                    }

                    var _attribute = new CodeAttributeDeclaration(
                        new CodeTypeReference(_attributeMetadata.AttributeType));

                    for (int i = 0; i < _attributeMetadata.PropertyNames.Count; i++)
                    {
                        var _attributePropertyName = _attributeMetadata.PropertyNames[i];

                        var _attributePropertyValue = _attributeMetadata.PropertyValues[i];

                        _attribute.Arguments.Add(
                            new CodeAttributeArgument
                            {
                                Name = _attributePropertyName,
                                Value = new CodePrimitiveExpression(_attributePropertyValue)
                            }
                        );
                    }

                    foreach (var _attributeValue in _attributeMetadata.ConstructorValues)
                    {
                        _attribute.Arguments.Add(
                            new CodeAttributeArgument(
                                new CodePrimitiveExpression(_attributeValue.ToString())));
                    }

                    _property.CustomAttributes.Add(_attribute);
                }

                _property.Name = _propertyMetadata.PropertyName;

                _property.Type = new CodeTypeReference(
                    _propertyMetadata.PropertyType);

                _property.GetStatements.Add(
                    new CodeMethodReturnStatement(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(), "_" + _property.Name)));

                _property.SetStatements.Add(
                    new CodeAssignStatement(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(), "_" + _property.Name),
                        new CodePropertySetValueReferenceExpression()));

                _TargetClass.Members.Add(_property);

                var _field = new CodeMemberField
                {
                    Attributes = MemberAttributes.Private | MemberAttributes.Final
                };

                _field.Name = "_" + _property.Name;

                _field.Type = new CodeTypeReference(
                    _propertyMetadata.PropertyType);

                _TargetClass.Members.Add(_field);
            }
        }

        public void Generate()
        {
            AddProperties();

            GenerateCSharpCode(_OutputFileName);
        }
    }
}
