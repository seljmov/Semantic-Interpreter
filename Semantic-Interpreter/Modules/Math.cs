using System;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Modules
{
    public class Math : Module
    {
        // TODO: Добавить проверку кол-ва аргументов
        public Math(string name = "Math") : base(name) 
            => OperatorId = GenerateOperatorId();

        public override void Execute()
        {
            FunctionStorage.Add("Abs", new Abs());
            FunctionStorage.Add("Max", new Max());
            FunctionStorage.Add("Min", new Min());
        }

        public override string OperatorId { get; }

        private class Abs : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new IntegerValue(System.Math.Abs(i)),
                    double d => new RealValue(System.Math.Abs(d)),
                    _ => throw new Exception("Math abs exception")
                };
            }
        }
        
        private class Max : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new IntegerValue(System.Math.Max(i, args[1].AsInteger())),
                    double d => new RealValue(System.Math.Max(d, args[1].AsReal())),
                    _ => throw new Exception("Math abs exception")
                };
            }
        }
        
        private class Min : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new IntegerValue(System.Math.Min(i, args[1].AsInteger())),
                    double d => new RealValue(System.Math.Min(d, args[1].AsReal())),
                    _ => throw new Exception("Math abs exception")
                };
            }
        }
    }
}