module main
    public procedure swap(var integer a, var integer b)
        variable-integer temp := a;
        let a := b;
        let b := temp;
    end swap;       
start
    variable-integer var1 := 4;
    variable-integer var2 := 16;
    output "Before swap: var1 = ${var1}, var2 = ${var2} \n";
    call swap(var1, var2);
    output "After swap: var1 = ${var1}, var2 = ${var2} \n";
end main.
