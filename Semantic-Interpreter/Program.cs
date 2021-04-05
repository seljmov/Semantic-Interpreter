using System;
using System.IO;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Parser;
using VariablesStorage = Semantic_Interpreter.Library.VariablesStorage;

namespace Semantic_Interpreter
{
    public static class Program
    {
        private const string Demo = @"E:\Education\Github\Own\Semantic-Interpreter\Semantic-Interpreter\Demo\";
        private const string Filename = "program1.txt";

        public static void Main()
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
            
            Console.WriteLine("\n-----------------\n");
            
            foreach (var @operator in operators)
            {
                @operator.Execute();
            }
        }
    }
}