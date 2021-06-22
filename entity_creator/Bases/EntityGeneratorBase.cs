using entity_creator.Models;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace entity_creator.Bases
{
    internal abstract class EntityGeneratorBase
    {
        protected CodeCompileUnit _TargetUnit;

        protected CodeTypeDeclaration _TargetClass;

        protected string _OutputFileName;

        protected virtual void GenerateCSharpCode(string _fileName)
        {
            var _provider = CodeDomProvider.CreateProvider("CSharp");

            var _options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };

            using var _sourceWriter = new StreamWriter(_fileName);

            _provider.GenerateCodeFromCompileUnit(_TargetUnit, _sourceWriter, _options);
        }
    }
}
