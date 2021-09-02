module main
    public procedure func2(in integer b)
        output "b = ${b} \n";
    end func2;
    
    public procedure func1(in integer a)
        output "a = ${a} \n";
        call func2(a);
    end func1;
start
    variable-integer a := 1;
    call func1("yes -> ${a + 2 * 4 + 3}");
end main.