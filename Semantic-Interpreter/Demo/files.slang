import FilesBase;

module Files
    public class File inherits Object
        private field-integer number;
        private field-string path;
        
        public method Position(File this)():integer
            return FilesBase.Position(this.number);
        end Position;
        
         public method Seek(File this)(in integer offset, in integer seekMode):integer
             return FilesBase.Seek(this.number, offset, seekMode);
         end Seek;       
        
        public method ReadInteger(File this)():integer
            return FilesBase.ReadInteger(this.number);
        end ReadInteger;
        
        public method ReadReal(File this)():real
            return FilesBase.ReadReal(this.number);
        end ReadReal;
        
        public method ReadString(File this)():string
            return FilesBase.ReadString(this.number);
        end ReadString;
        
        public method ReadChar(File this)():char
            return FilesBase.ReadChar(this.number);
        end ReadChar;
        
        public method ReadBoolean(File this)():boolean
            return FilesBase.ReadBoolean(this.number);
        end ReadBoolean;
        
        public method WriteInteger(File this)(in integer x)
            call FilesBase.Write(this.number, x);
        end WriteInteger;
        
        public method WriteReal(File this)(in real x)
            call FilesBase.Write(this.number, x);
        end WriteReal;
        
        public method WriteString(File this)(in string x)
            call FilesBase.Write(this.number, x);
        end WriteString;
        
        public method WriteChar(File this)(in char x)
            call FilesBase.Write(this.number, x);
        end WriteChar;
        
        public method WriteBoolean(File this)(in boolean x)
            call FilesBase.Write(this.number, x);
        end WriteBoolean;
        
        public method Close(File this)()
            call FilesBase.Close(this.number);
        end Close;
    end File;        
    
    public class InputFile inherits Object
        private field-integer number;
        private field-string path;
        
        public method Position(InputFile this)():integer
            return FilesBase.Position(this.number);
        end Position;
        
         public method Seek(InputFile this)(in integer offset, in integer seekMode):integer
             return FilesBase.Seek(this.number, offset, seekMode);
         end Seek;       
        
        public method ReadInteger(InputFile this)():integer
            return FilesBase.ReadInteger(this.number);
        end ReadInteger;
        
        public method ReadReal(InputFile this)():real
            return FilesBase.ReadReal(this.number);
        end ReadReal;
        
        public method ReadString(InputFile this)():string
            return FilesBase.ReadString(this.number);
        end ReadString;
        
        public method ReadChar(InputFile this)():char
            return FilesBase.ReadChar(this.number);
        end ReadChar;
        
        public method ReadBoolean(InputFile this)():boolean
            return FilesBase.ReadBoolean(this.number);
        end ReadBoolean;
        
        public method Close(InputFile this)()
            call FilesBase.Close(this.number);
        end Close;
    end InputFile;  
    
    public class OutputFile inherits Object
        private field-integer number;
        private field-string path;
        
        public method Position(OutputFile this)():integer
            return FilesBase.Position(this.number);
        end Position;
        
         public method Seek(OutputFile this)(in integer offset, in integer seekMode):integer
             return FilesBase.Seek(this.number, offset, seekMode);
         end Seek;       
        
        public method WriteInteger(OutputFile this)(in integer x)
            call FilesBase.Write(this.number, x);
        end WriteInteger;
        
        public method WriteReal(OutputFile this)(in real x)
            call FilesBase.Write(this.number, x);
        end WriteReal;
        
        public method WriteString(OutputFile this)(in string x)
            call FilesBase.Write(this.number, x);
        end WriteString;
        
        public method WriteChar(OutputFile this)(in char x)
            call FilesBase.Write(this.number, x);
        end WriteChar;
        
        public method WriteBoolean(OutputFile this)(in boolean x)
            call FilesBase.Write(this.number, x);
        end WriteBoolean;
        
        public method Close(OutputFile this)()
            call FilesBase.Close(this.number);
        end Close;
    end OutputFile;  
    
    public function Exists(in string path):bool
        return FilesBase.Exists(path);
    end Exists;
    
    public function SeekBeg():integer
        return FilesBase.SeekBeg;
    end SeekBeg;
    
    public function SeekCur():integer
        return FilesBase.SeekCur;
    end SeekCur;
    
    public function SeekEnd():integer
        return FilesBase.SeekEnd;
    end SeekEnd;
    
    public function OpenFile(in string path):File
        variable-integer fileNumber := FilesBase.Open(path, "ReadWrite");
        variable-File file;
        let file.number := fileNumber;
        let file.path := path;
        return file;
    end OpenFile;
    
    public function OpenInputFile(in string path):File
        variable-integer fileNumber := FilesBase.Open(path, "Read");
        variable-InputFile file;
        let file.number := fileNumber;
        let file.path := path;
        return file;
    end OpenInputFile;
    
    public function OpenOutputFile(in string path):File
        variable-integer fileNumber := FilesBase.Open(path, "Write");
        variable-OutputFile file;
        let file.number := fileNumber;
        let file.path := path;
        return file;
    end OpenOutputFile;
    
start

end 