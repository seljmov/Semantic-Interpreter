module main
    public function first(in string a):string
        output "message -> ${a} \n";
        return 1;
    end first;        
start
    variable-string hi := "Wazzup!";
    output first(hi);
end main.