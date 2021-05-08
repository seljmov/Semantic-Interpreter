using System;
using System.Collections.Generic;
using System.IO;
using Semantic_Interpreter.Parser;

namespace Semantic_Interpreter.Library
{
    public static class Test
    {
        private const string Demo = @"E:\Education\Github\Own\Semantic-Interpreter\Semantic-Interpreter\Demo\";
        private static readonly List<string> Programs = new List<string>()
        {
            "vars", "arith", "if", "factorial", "while"
        };

        public static void Run()
        {
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("             Запуск тестовых програм             ");
            Console.WriteLine("-------------------------------------------------");

            foreach (var program in Programs)
            {
                RunProgram(program + ".txt");
                VariablesStorage.Clear();
                Console.WriteLine("\n");
            }
        }
        
        public static void Run(string filename)
        {
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("            Запуск тестовой программы            ");
            Console.WriteLine("-------------------------------------------------");

            RunProgram(filename + ".txt");
        }

        private static void RunProgram(string filename)
        {
            Console.WriteLine();
            using var reader = new StreamReader(Demo + filename);
            var program = reader.ReadToEnd();
            
            var lexer = new Lexer(program);
            var tokens = lexer.Tokenize();
            
            // PrintTokens(tokens);
            
            var tree = new Parser.Parser(tokens).Parse();
            tree.TraversalTree();
        }
        
        private static void PrintTokens(List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                Console.Write(token.Type);
                if (token.Text != "") Console.Write($" -> {token.Text}");
                Console.WriteLine();
            }
        }
    }
}