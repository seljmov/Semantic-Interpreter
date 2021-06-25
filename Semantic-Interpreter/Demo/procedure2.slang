module main
    public procedure first()
        output "first \n";
    end first;
    
    public procedure second()
        output "second \n";
    end second;
    
    public procedure third()
        output "third \n";
    end third;        
start
    call first();
    call second();
    call third();
end main.