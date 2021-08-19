using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Semantic_Interpreter.Core;
using Semantic_Interpreter.Core.Items;

namespace Semantic_Interpreter.Modules
{
    public class FilesBase : Module
    {
        public FilesBase(string name = "FilesBase") : base(name)
        {
            OperatorId = GenerateOperatorId();
            
            FunctionStorage.Add("Exists", new Exists());
            FunctionStorage.Add("Open", new Open());
            FunctionStorage.Add("Close", new Close());
            FunctionStorage.Add("ReadInteger", new ReadInteger());
            FunctionStorage.Add("ReadReal", new ReadReal());
            FunctionStorage.Add("ReadString", new ReadString());
            FunctionStorage.Add("ReadChar", new ReadChar());
            FunctionStorage.Add("ReadBoolean", new ReadBoolean());
            FunctionStorage.Add("Write", new Write());
            FunctionStorage.Add("Position", new Position());
            FunctionStorage.Add("Seek", new Seek());
            FunctionStorage.Add("SeekBeg", new SeekBeg());
            FunctionStorage.Add("SeekCur", new SeekCur());
            FunctionStorage.Add("SeekEnd", new SeekEnd());
        }

        public override string OperatorId { get; }
        private static readonly Dictionary<long, SemanticFileInfo> Files = new();
        private static uint _fileNumber;

        private class SemanticFileInfo
        {
            private readonly string _path;
            private readonly FileMode _mode;
            private readonly FileAccess _access;
            public uint CursorPosition;

            public FileStream CreateStream() => File.Open(_path, _mode, _access);
            public FileStream CreateEmptyStream() => File.Open(_path, FileMode.Truncate, _access);

            public SemanticFileInfo(string path, FileMode mode, FileAccess access)
            {
                _path = path;
                _mode = mode;
                _access = access;
                CursorPosition = 0;
            }
        }

        private static void VerifyIndex(int size, long index)
        {
            if (index >= size)
            {
                throw new Exception($"Выход за границы файла! Размер файла -> {size}, указанный индекс -> {index}.");
            }
        }
        
        private class Exists : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                
                var path = args[0].AsString();
                var cond = File.Exists(path);

                return new BooleanValue(cond);
            }
        }
        
        private class Open : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = _fileNumber;
                var path = args[0].AsString();
                var mode = args[1].AsString() switch
                {
                    "Read" => FileAccess.Read,
                    "Write" => FileAccess.Write,
                    "ReadWrite" => FileAccess.ReadWrite,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var fileInfo = new SemanticFileInfo(path, FileMode.OpenOrCreate, mode);
                Files.Add(number, fileInfo);
                _fileNumber++;

                return new IntegerValue(number);
            }
        }
        
        private class Close : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                Files.Remove(number);
                
                return new BooleanValue(true);
            }
        }
        
        private class Position : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                var sf = Files[number];
                
                return new IntegerValue(sf.CursorPosition);
            }
        }
        
        private class Seek : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                var offset = args[1].AsInteger();
                var seekMode = args[2].AsInteger();
                var fs = Files[number];
                
                switch (seekMode)
                {
                    case 0:
                        fs.CursorPosition = (uint) offset;
                        break;
                    case 1:
                        fs.CursorPosition += (uint) offset;
                        break;
                    case 2:
                    {
                        using var sr = new StreamReader(fs.CreateStream());
                
                        var fileString = sr
                            .ReadToEnd()
                            .Replace("\r\n", " ");
                        var count = fileString.Split(" ").Length;
                        fs.CursorPosition = (uint) (count - offset);
                        break;
                    }
                }
                
                return new BooleanValue(true);
            }
        }
        
        private class SeekBeg : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                return new IntegerValue(0);
            }
        }
        
        private class SeekCur : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                return new IntegerValue(1);
            }
        }
        
        private class SeekEnd : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                return new IntegerValue(2);
            }
        }
        
        private class ReadInteger : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                var fs = Files[number];
                using var sr = new StreamReader(fs.CreateStream());
                
                var fileString = sr
                    .ReadToEnd()
                    .Replace("\r\n", " ");
                var items = fileString.Split(" ");

                VerifyIndex(items.Length, fs.CursorPosition);
                
                var item = items[fs.CursorPosition];
                var value = Convert.ToInt64(item);
                fs.CursorPosition++;
                
                return new IntegerValue(value);
            }
        }
        
        private class ReadReal : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                var fs = Files[number];
                using var sr = new StreamReader(fs.CreateStream());
                
                var fileString = sr
                    .ReadToEnd()
                    .Replace("\r\n", " ");
                var items = fileString.Split(" ");
                var item = items[fs.CursorPosition];
                
                VerifyIndex(items.Length, fs.CursorPosition);
                
                IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                var value = Convert.ToDouble(item, formatter);
                fs.CursorPosition++;
                
                return new RealValue(value);
            }
        }
        
        private class ReadString : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                var fs = Files[number];
                using var sr = new StreamReader(fs.CreateStream());
                
                var fileString = sr
                    .ReadToEnd()
                    .Replace("\r\n", " ");
                var items = fileString.Split(" ");
                
                VerifyIndex(items.Length, fs.CursorPosition);
                
                var item = items[fs.CursorPosition];
                var value = Convert.ToString(item);
                fs.CursorPosition++;
                
                return new StringValue(value);
            }
        }
        
        private class ReadChar : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                var fs = Files[number];
                using var sr = new StreamReader(fs.CreateStream());
                
                var fileString = sr
                    .ReadToEnd()
                    .Replace("\r\n", " ");
                var items = fileString.Split(" ");
                
                VerifyIndex(items.Length, fs.CursorPosition);
                
                var item = items[fs.CursorPosition];
                var value = Convert.ToChar(item);
                fs.CursorPosition++;
                
                return new CharValue(value);
            }
        }
        
        private class ReadBoolean : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                var fs = Files[number];
                using var sr = new StreamReader(fs.CreateStream());
                
                var fileString = sr
                    .ReadToEnd()
                    .Replace("\r\n", " ");
                var items = fileString.Split(" ");
                
                VerifyIndex(items.Length, fs.CursorPosition);
                
                var item = items[fs.CursorPosition];
                var value = Convert.ToBoolean(item);
                fs.CursorPosition++;
                
                return new BooleanValue(value);
            }
        }
        
        private class Write : IFunction
        {
            public IValue Execute(params IValue[] args)
            {
                var number = args[0].AsInteger();
                var fs = Files[number];
                var value = args[1].AsString();
                
                using var sr = new StreamReader(fs.CreateStream());
                var fileString = sr
                    .ReadToEnd()
                    .Replace("\r\n", " ");
                var items = fileString.Split(" ").ToList();
                
                VerifyIndex(items.Count, fs.CursorPosition);
                
                items.Insert((int) fs.CursorPosition, value);
                sr.Close();
                
                using var sw = new StreamWriter(fs.CreateEmptyStream());
                var info = string.Join(" ", items);
                sw.Write(info);
                fs.CursorPosition++;

                return new BooleanValue(true);
            }
        }
    }
}