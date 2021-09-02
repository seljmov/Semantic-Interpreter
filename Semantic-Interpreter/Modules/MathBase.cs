using System;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Modules
{
    /// <summary>
    ///     Math модуль
    ///     Предоставляет константы и методы для тригонометрических,
    ///     логарифмических и иных общих математических функций.
    /// </summary>
    public class MathBase : Module
    {
        // TODO: Добавить проверку кол-ва аргументов
        public MathBase(string name = "Math") : base(name)
        {
            OperatorId = GenerateOperatorId();
            
            FunctionStorage.Add("E", new E());
            FunctionStorage.Add("PI", new Pi());
            FunctionStorage.Add("Tau", new Tau());
            FunctionStorage.Add("Abs", new Abs());
            FunctionStorage.Add("Acos", new Acos());
            FunctionStorage.Add("Asin", new Asin());
            FunctionStorage.Add("Atan", new Atan());
            FunctionStorage.Add("Atanh", new Atanh());
            FunctionStorage.Add("Cos", new Cos());
            FunctionStorage.Add("Sin", new Sin());
            FunctionStorage.Add("Tan", new Tan());
            FunctionStorage.Add("Tanh", new Tanh());
            FunctionStorage.Add("Ceil", new Ceil());
            FunctionStorage.Add("Floor", new Floor());
            FunctionStorage.Add("Round", new Round());
            FunctionStorage.Add("Truncate", new Truncate());
            FunctionStorage.Add("Log", new Log());
            FunctionStorage.Add("Log10", new Log10());
            FunctionStorage.Add("Log2", new Log2());
            FunctionStorage.Add("Pow", new Pow());
            FunctionStorage.Add("Cbrt", new Cbrt());
            FunctionStorage.Add("Sqrt", new Sqrt());
            FunctionStorage.Add("Exp", new Exp());
            FunctionStorage.Add("Mod", new Mod());
            FunctionStorage.Add("Max", new Max());
            FunctionStorage.Add("Min", new Min());
        }

        public override string OperatorId { get; }
        
        /// <summary>
        ///     Представляет основание натурального логарифма, определяемое константой e.
        /// </summary>
        private class E : IFunction
        {
            public Value Execute(params Value[] args)
            {
                return new RealValue(2.7182818284590451);
            }
        }
        
        /// <summary>
        ///     Представляет отношение длины окружности к ее диаметру, определяемое константой π.
        /// </summary>
        private class Pi : IFunction
        {
            public Value Execute(params Value[] args)
            {
                return new RealValue(3.1415926535897931);
            }
        }
        
        /// <summary>
        ///     Представляет число радианов в полном обороте, заданное константой τ.
        /// </summary>
        private class Tau : IFunction
        {
            public Value Execute(params Value[] args)
            {
                return new RealValue(6.2831853071795862);
            }
        }
        
        /// <summary>
        ///     Возвращает абсолютное значение числа.
        /// </summary>
        private class Abs : IFunction
        {
            public Value Execute(params Value[] args)
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

        /// <summary>
        ///     Возвращает угол, косинус которого равен указанному числу.
        /// </summary>
        private class Acos : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Acos(i)),
                    double d => new RealValue(System.Math.Acos(d)),
                    _ => throw new Exception("Math acos exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает угол, гиперболический косинус которого равен указанному числу.
        /// </summary>
        private class Acosh : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Acosh(i)),
                    double d => new RealValue(System.Math.Acosh(d)),
                    _ => throw new Exception("Math acosh exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает угол, синус которого равен указанному числу.
        /// </summary>
        private class Asin : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Asin(i)),
                    double d => new RealValue(System.Math.Asinh(d)),
                    _ => throw new Exception("Math asin exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает угол, гиперболический синус которого равен указанному числу.
        /// </summary>
        private class Asinh : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Asinh(i)),
                    double d => new RealValue(System.Math.Asinh(d)),
                    _ => throw new Exception("Math asinh exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает угол, тангенс которого равен указанному числу.
        /// </summary>
        private class Atan : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Atan(i)),
                    double d => new RealValue(System.Math.Atan(d)),
                    _ => throw new Exception("Math atan exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает угол, гиперболический тангенс которого равен указанному числу.
        /// </summary>
        private class Atanh : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Atanh(i)),
                    double d => new RealValue(System.Math.Atanh(d)),
                    _ => throw new Exception("Math atanh exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает косинус указанного угла.
        /// </summary>
        private class Cos : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Cos(i)),
                    double d => new RealValue(System.Math.Cos(d)),
                    _ => throw new Exception("Math cos exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает гиперболический косинус указанного угла.
        /// </summary>
        private class Cosh : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Cosh(i)),
                    double d => new RealValue(System.Math.Cosh(d)),
                    _ => throw new Exception("Math cosh exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает синус указанного угла.
        /// </summary>
        private class Sin : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Sin(i)),
                    double d => new RealValue(System.Math.Sin(d)),
                    _ => throw new Exception("Math sin exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает гиперболический синус указанного угла.
        /// </summary>
        private class Sinh : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Sinh(i)),
                    double d => new RealValue(System.Math.Sinh(d)),
                    _ => throw new Exception("Math sinh exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает тангенс указанного угла.
        /// </summary>
        private class Tan : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Tan(i)),
                    double d => new RealValue(System.Math.Tan(d)),
                    _ => throw new Exception("Math tan exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает гиперболический тангенс указанного угла.
        /// </summary>
        private class Tanh : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new RealValue(System.Math.Tanh(i)),
                    double d => new RealValue(System.Math.Tanh(d)),
                    _ => throw new Exception("Math tanh exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает наименьшее целое число,
        ///     которое больше или равно заданному десятичному числу.
        /// </summary>
        private class Ceil : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Ceiling(d)),
                    _ => throw new Exception("Math ceil exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает наибольшее целое число, которое
        ///     меньше или равно указанному десятичному числу.
        /// </summary>
        private class Floor : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Floor(d)),
                    _ => throw new Exception("Math floor exception")
                };
            }
        }

        /// <summary>
        ///     Округляет значение с плавающей запятой двойной точности
        ///     до ближайшего целого значения; значения посередине
        ///     округляются до ближайшего четного числа.
        /// </summary>
        private class Round : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return args.Length == 1 
                    ? raw switch
                        {
                            double d => new RealValue(System.Math.Round(d)),
                            _ => throw new Exception("Math round exception")
                        }
                    : raw switch
                        {
                            double d => new RealValue(System.Math.Round(d, (int) args[1].AsInteger())),
                            _ => throw new Exception("Math round exception")
                        };
            }
        }

        /// <summary>
        ///     Вычисляет целую часть заданного десятичного числа.
        /// </summary>
        private class Truncate : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Truncate(d)),
                    _ => throw new Exception("Math truncate exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает логарифм указанного числа в
        ///     системе счисления с указанным основанием.
        /// </summary>
        private class Log : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return args.Length == 1 
                    ? raw switch
                    {
                        double d => new RealValue(System.Math.Log(d)),
                        _ => throw new Exception("Math log exception")
                    }
                    : raw switch
                    {
                        double d => new RealValue(System.Math.Log(d, args[1].AsReal())),
                        _ => throw new Exception("Math log exception")
                    };
            }
        }

        /// <summary>
        ///     Возвращает логарифм с основанием 10 указанного числа.
        /// </summary>
        private class Log10 : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Log10(d)),
                    _ => throw new Exception("Math log10 exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает логарифм с основанием 2 указанного числа.
        /// </summary>
        private class Log2 : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Log2(d)),
                    _ => throw new Exception("Math log2 exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает указанное число, возведенное в указанную степень.
        /// </summary>
        private class Pow : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Pow(d, args[1].AsReal())),
                    _ => throw new Exception("Math pow exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает кубический корень из указанного числа.
        /// </summary>
        private class Cbrt : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Cbrt(d)),
                    _ => throw new Exception("Math cbrt exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает квадратный корень из указанного числа.
        /// </summary>
        private class Sqrt : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Sqrt(d)),
                    _ => throw new Exception("Math sqrt exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает e, возведенное в указанную степень.
        /// </summary>
        private class Exp : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.Exp(d)),
                    _ => throw new Exception("Math exp exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает остаток от деления одного
        ///     указанного числа на другое указанное число.
        /// </summary>
        private class Mod : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    double d => new RealValue(System.Math.IEEERemainder(d, args[1].AsReal())),
                    _ => throw new Exception("Math mod exception")
                };
            }
        }

        /// <summary>
        ///     Возвращает большее из двух десятичных чисел.
        /// </summary>
        private class Max : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new IntegerValue(System.Math.Max(i, args[1].AsInteger())),
                    double d => new RealValue(System.Math.Max(d, args[1].AsReal())),
                    _ => throw new Exception("Math max exception")
                };
            }
        }
        
        /// <summary>
        ///     Возвращает меньшее из двух десятичных чисел.
        /// </summary>
        private class Min : IFunction
        {
            public Value Execute(params Value[] args)
            {
                var raw = args[0].AsObject();

                return raw switch
                {
                    int i => new IntegerValue(System.Math.Min(i, args[1].AsInteger())),
                    double d => new RealValue(System.Math.Min(d, args[1].AsReal())),
                    _ => throw new Exception("Math min exception")
                };
            }
        }
    }
}