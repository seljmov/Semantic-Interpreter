namespace Semantic_Interpreter.Core
{
    public class ClassValue : Value
    {
        public ClassValue(Class @class) => Class = @class;
        
        public Class Class { get; set; }
        
        public override object AsObject() => Class;
    }
}