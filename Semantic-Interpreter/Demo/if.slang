module main
start
    output "Программа: ветвление (if-else if-else) \n";
    variable-integer i;
    output "Введите число: ";
    input i;
    
    if i <= 5 then
        output "Number <= 5";
    else if i == 6 then
        output "Number 6";
    else if i == 8 then
        output "Number 8";
    else
        output "Number > 8";
    end if;
end main.