using System;
using System.Collections.Generic;
using System.IO;
using Semantic_Interpreter.Parser;

namespace Semantic_Interpreter
{
    public static class Program
    {
        private const string Demo = @"E:\Education\Github\Own\Semantic-Interpreter\Semantic-Interpreter\Demo\";
        public static void Main()
        {
            Console.WriteLine();
            using var reader = new StreamReader(Demo + "arith.slang");
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