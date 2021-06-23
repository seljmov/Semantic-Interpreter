module main
start
    variable-integer i := 0;
    variable-integer a := 1;
    while i <= 5 do
        variable-integer a := 0;
        output "${i} \n";
        let i := i + 1;
        let i := i + a;
        let a := a + 1;
    end while;
    output a;
end main.