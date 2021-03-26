using System;
using System.IO;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Parser;

namespace Semantic_Interpreter
{
    public static class Program
    {
        private const string Demo = @"C:\Github\Semantic-Interpreter\Semantic-Interpreter\Demo\";
        private const string Filename = "program1.txt";

        public static void Main(string[] args)
        {

            using var reader = new StreamReader(Demo + Filename);
            var program = reader.ReadToEnd();

            var lexer = new Lexer(program);
            var tokens = lexer.Tokenize();

            foreach (var token in tokens)
            {
                Console.WriteLine(token.Type + " -> " + token.Text);
            }

            /*
            var tree = new SemanticTree();
            var parser = new Parser_old(tree, program);
            parser.BuildTree();
            
            tree.TraversalTree();
            */
        }
    }
}