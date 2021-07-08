module main
    public procedure first()
        output "first \n";
    end first;  
    
    public procedure second()
        output "second \n";
    end second;      
start
    call first();
    call second();
end main.