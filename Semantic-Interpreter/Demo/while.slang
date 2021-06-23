module main
start
    variable-integer i := 0;
    while i <= 5 do
        output "${i} \n";
        let i := i + 1;
    end while;
end main.