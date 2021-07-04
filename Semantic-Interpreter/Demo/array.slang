module main
start
    variable-integer N := 5;
    variable-array[N] integer arr;
    
    variable-integer i := 0;
    while i < N do
        let arr[i] := i;
        let i := i + 1;
    end while;
    
    let i := 0;
    while i < N do
        output arr[i];
        output " ";
        let i := i + 1;
    end while;
end main.