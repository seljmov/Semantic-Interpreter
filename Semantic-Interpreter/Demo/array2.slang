module main
start
    variable-integer N := 3;
    variable-array[N] array[N] integer arr;
    
    variable-integer c := 0;
    variable-integer i := 0;
    while i < N do
        variable-integer j := 0;
        while j < N do
            let arr[i][j] := c;
            let c := c + 1;
            let j := j + 1;
        end while;
        let i := i + 1;
    end while;
    
    let i := 0;
    while i < N do
        variable-integer j := 0;
        while j < N do
            output arr[i][j];
            output " ";
            let j := j + 1;
        end while;
        output "\n";
        let i := i + 1;
    end while;
end main.