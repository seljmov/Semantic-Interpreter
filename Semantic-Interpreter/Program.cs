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
            using var reader = new StreamReader(Demo + "array4.slang");
            var program = reader.ReadToEnd();
            
            var lexer = new Lexer(program);
            var tokens = lexer.Tokenize();
            
            // PrintTokens(tokens);
            
            var tree = new Parser.Parser(tokens).Parse();
            tree.TraversalTree();
        }
        
        private static void PrintTokens(List<Token> tokens)
        {
            tokens.ForEach(token => 
                Console.WriteLine(token.Text == "" 
                    ? $"{token.Type}" 
                    : $"{token.Type} -> {token.Text}"));
        }
    }
}