using System;
using System.Collections.Generic;
using System.IO;
using Semantic_Interpreter.Parser;

namespace Semantic_Interpreter
{
    public static class Program
    {
        private const string Demo = @"E:\Education\Github\Own\Semantic-Interpreter\Semantic-Interpreter\Demo\";
        private const string Filename = "factorial.txt";
        
        public static void Main()
        {
            Console.WriteLine();
            using var reader = new StreamReader(Demo + Filename);
            var program = reader.ReadToEnd();
            
            var lexer = new Lexer(program);
            var tokens = lexer.Tokenize();
            
            // PrintTokens(tokens);
            
            var tree = new Parser.Parser(tokens).Parse();
            tree.TraversalTree();

            // DocumentRW.Save(tree);
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