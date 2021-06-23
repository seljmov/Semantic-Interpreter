module main
start
    variable-integer n;
    input n;
    
    variable-integer result := 1;
    if n > 0 then
        variable-integer i := 1;
        while i <= n do
            let result := result * i;
            let i := i + 1;
        end while;
    end if;
    
    output n;
    output "! = ";
    output result;
end main.