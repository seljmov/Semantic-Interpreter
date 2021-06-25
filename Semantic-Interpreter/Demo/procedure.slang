module main
    public procedure test(in integer a, var integer b)
        let a := a + 3;
        variable-integer c := a + b - 2;
        output "${a} + ${b} - 2 = ${c} \n";
        while c >= 0 do
            output "c = ${c} \n";
            let c := c - 1;
        end while;
    end test;
start
    variable-integer a := 1;
    variable-integer b := 2;
    call test(a, b);
end main.