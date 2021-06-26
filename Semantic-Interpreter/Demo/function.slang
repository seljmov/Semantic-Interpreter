module main
    public function first(in string a):string
        return 1 == 2;
    end first;        
start
    variable-string hi := "Wazzup!";
    output first(hi);
end main.