using System;
using System.Collections.Generic;

namespace Semantic_Interpreter.Core
{
    public class Class : MultilineOperator, ICloneable
    {
        public Class() => OperatorId = GenerateOperatorId();
        
        public VisibilityType VisibilityType { get; set; }
        public string Name { get; set; }
        public string BaseClass { get; set; }
        public List<Field> Fields { get; set; }
        public List<DefineFunction> Methods  { get; set; }
        public sealed override string OperatorId { get; }

        public override void Execute()
        {
            GetRoot().Module.ClassStorage.Add(Name, (Class) Clone());
        }
        public object Clone()
        {
            return new Class
            {
                VisibilityType = VisibilityType,
                Name = Name,
                BaseClass = BaseClass,
                Fields = Fields,
                Methods = Methods
            };
        }
    }
}