using System;
using System.IO;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Parser;
using VariablesStorage = Semantic_Interpreter.Library.VariablesStorage;

namespace Semantic_Interpreter
{
    public static class Program
    {
        private const string Demo = @"C:\Github\Semantic-Interpreter\Semantic-Interpreter\Demo\";
        private const string Filename = "program3.txt";

        public static void Main(string[] args)
        {
            Console.WriteLine();
            using var reader = new StreamReader(Demo + Filename);
            var program = reader.ReadToEnd();
            
            var lexer = new Lexer(program);
            var tokens = lexer.Tokenize();

            
            foreach (var token in tokens)
            {
                Console.Write(token.Type);
                if (token.Text != "") Console.Write($" -> {token.Text}");
                Console.WriteLine();
            }   
            
            var operators = new Parser.Parser(tokens).Parse();
            foreach (var @operator in operators)
            {
                Console.WriteLine(@operator);
            }
            
            Console.WriteLine("\n-----------------\n");
            
            foreach (var @operator in operators)
            {
                @operator.Execute();
            }

            var variable = VariablesStorage.At("name");
            Console.WriteLine($"{(variable.Type)} {variable.Name} := {variable.Expression}");
        }
    }
}