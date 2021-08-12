using System;
using System.Collections.Generic;
using System.IO;
using Semantic_Interpreter.Parser;

namespace Semantic_Interpreter
{
    public static class Program
    {
        private const string Demo = @"E:\Education\Own\Semantic-Language\Semantic-Interpreter\Demo\";
        public static void Main()
        {
            Console.WriteLine(Math.Acos(1));
            using var reader = new StreamReader(Demo + "import.slang");
            var program = reader.ReadToEnd();
            
            var lexer = new Lexer(program);
            var tokens = lexer.Tokenize();
            
            // PrintTokens(tokens);
            
            var tree = new Parser.Parser(tokens).Parse();
            tree.TraversalTree();
        }
        
        private static void PrintTokens(List<Token> tokens)
        {
            // tokens.ForEach(token => Console.WriteLine(token.Text == "" ? $"{token.Type} \n" : $"{token.Type} -> {token.Text} \n"));
            
            foreach (var token in tokens)
            {
                Console.Write(token.Type);
                if (token.Text != "") Console.Write($" -> {token.Text}");
                Console.WriteLine();
            }
        }
    }
}