module main
start
    output "Программа: цикл while \n";
    variable-integer i := 0;
    while i <= 5 do
        output "${i} \n";
        let i := i + 1;
    end while;
end main.