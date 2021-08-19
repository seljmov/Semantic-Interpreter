using System;

namespace Semantic_Interpreter.Core
{
    public interface IValue
    {
        long AsInteger();

        double AsReal();

        bool AsBoolean();

        char AsChar();

        string AsString();

        IValue[] AsArray();

        object AsObject();
    }
}