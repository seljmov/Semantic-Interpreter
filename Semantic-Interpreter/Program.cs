using System;
using System.IO;
using Semantic_Interpreter.Library;
using Semantic_Interpreter.Parser;

namespace Semantic_Interpreter
{
    public static class Program
    {
        private const string Demo = @"E:\Education\Github\Own\Semantic-Interpreter\Semantic-Interpreter\Demo\";
        public static void Main()
        {
            Console.WriteLine();
            using var reader = new StreamReader(Demo + "if.txt");
            var program = reader.ReadToEnd();
            
            var lexer = new Lexer(program);
            var tokens = lexer.Tokenize();
            
            // PrintTokens(tokens);
            
            var tree = new Parser.Parser(tokens).Parse();
            // tree.TraversalTree();

            DocumentRW.Save(tree);
        }
    }
}