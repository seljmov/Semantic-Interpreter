module main
start
    output "Программа: ветвление (if-else if-else) \n";
    variable-integer i;
    output "Введите число: ";
    input i;
    
    if i < 5 then
        variable-string here := "im here";
        output "${here}: ${i} <= 5";
    else if i == 5 then
        variable-string here2 := "im here2";
        output "${here2}: ${i} == 5";
    else
        variable-string here3 := "im here3";
        output "${here3}: ${i} > 5";
    end if;
end main.