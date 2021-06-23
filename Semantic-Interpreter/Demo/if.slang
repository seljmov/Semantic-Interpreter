module main
start
    variable-integer i := 0;
    
    
    if i <= 5 then
        output "${i} <= 5";
    else if i == 6 then
        output "${i} == 6";
    else if i == 8 then
        output "${i} == 8";
    else
        variable-integer n := 1;
        output "${i} > 8";
    end if;
    
    output "\n";
    output n;
end main.