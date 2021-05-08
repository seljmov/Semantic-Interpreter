using System;
using System.IO;
using System.Text;
using System.Xml;
using Semantic_Interpreter.Core;

namespace Semantic_Interpreter.Library
{
    public static class DocumentRW
    {
        private const string Demo = @"E:\Education\Github\Own\Semantic-Interpreter\Semantic-Interpreter\Demo\";
        private const string Filename = "pr2.xml";
        
        public static void Save(SemanticTree semanticTree)
        {
            var buffer = new StringBuilder();
            var writer = XmlWriter.Create(buffer);
            writer.WriteStartDocument();
            SaveNode(semanticTree.Root, writer);
            writer.WriteEndDocument();
            writer.Close();

            using var sw = new StreamWriter(Demo + Filename, false, Encoding.Default);
            sw.Write(buffer.ToString());
        }

        private static void SaveNode(SemanticOperator node, XmlWriter writer)
        {
            writer.WriteStartElement(node.GetType().Name);

            switch (node)
            {
                case Module module:
                    writer.WriteAttributeString("Name", module.Name);
                    // writer.WriteAttributeString("Id", Guid.NewGuid().ToString());
                    break;
                case While @while:
                    
                    break;
                case Variable variable:
                    writer.WriteAttributeString("Name", variable.Name);
                    writer.WriteAttributeString("Type", variable.Type.ToString());
                    writer.WriteAttributeString("Expression", variable.Expression.ToString() ?? string.Empty);
                    break;
                case Output output:
                    writer.WriteAttributeString("Expression", output.Expression.ToString() ?? string.Empty);
                    break;
                case Input input:
                    writer.WriteAttributeString("Expression", input.Name ?? string.Empty);
                    break;
            }

            if (node.Child != null)
            {
                SaveNode(node.Child, writer);
            }
            
            if (node.Next != null)
            {
                SaveNode(node.Next, writer);
            }
        }
    }
}