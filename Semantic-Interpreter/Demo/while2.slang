module main
start
    variable-integer i := 0;
    while i <= 2 do
        variable-integer j := 0;
        output "${i} \n";
        let i := i + 1;
    end while;
end main.