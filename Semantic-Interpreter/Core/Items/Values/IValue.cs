namespace Semantic_Interpreter.Library
{
    public interface IValue
    {
        int AsInteger();

        double AsReal();

        bool AsBoolean();

        string AsString();
    }
}