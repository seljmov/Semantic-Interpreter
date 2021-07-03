module main
start
    variable-integer i := 0;
    input i; 
   
    if i <= 5 then
        output "${i} <= 5";
    else if i == 6 then
        output "${i} == 6";
    else if i == 8 then
        output "${i} == 8";
    else
        output "${i} > 8";
    end if;
end main.