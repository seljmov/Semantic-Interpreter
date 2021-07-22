namespace Semantic_Interpreter.Core
{
    public class ClassValue : IValue
    {
        public ClassValue(Class @class) => Class = @class;
        
        public Class Class { get; set; }

        public int AsInteger()
        {
            throw new System.NotImplementedException();
        }

        public double AsReal()
        {
            throw new System.NotImplementedException();
        }

        public bool AsBoolean()
        {
            throw new System.NotImplementedException();
        }

        public char AsChar()
        {
            throw new System.NotImplementedException();
        }

        public string AsString()
        {
            throw new System.NotImplementedException();
        }

        public IValue[] AsArray()
        {
            throw new System.NotImplementedException();
        }
    }
}