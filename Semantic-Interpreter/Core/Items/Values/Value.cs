using System;

namespace Semantic_Interpreter.Core
{
    public class Value
    {
        public virtual long AsInteger() 
            => throw new Exception("Преобразование в целое число невозможно!");

        public virtual double AsReal() 
            => throw new Exception("Преобразование в вещественное число невозможно!");

        public virtual bool AsBoolean() 
            => throw new Exception("Преобразование в булев тип невозможно!");

        public virtual char AsChar() 
            => throw new Exception("Преобразование в символ невозможно!");

        public virtual string AsString()
            => throw new Exception("Преобразование в строку невозможно!");

        public virtual Value[] AsArray() 
            => throw new Exception("Преобразование в массив невозможно!");

        public virtual object AsObject() 
            => throw new Exception("Преобразование в объект невозможно!");
    }
}