module main
start
    output "Программа: нахождение факториала \n";
    variable-integer n;
    output "Введите число: ";
    input n;
    
    variable-integer result := 1;
    if n < 0 then
        let result := 0;
    else if n == 0 then
        let result := 1;
    else
        variable-integer i := 1;
        while i <= n repeat
            let result := result * i;
            let i := i + 1;
        end while;
    end if;
    
    output n;
    output "! = ";
    output result;
end main.