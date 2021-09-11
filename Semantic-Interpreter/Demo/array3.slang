module main
    public procedure fillArray(var array[] integer arr, in integer n)
        variable-integer i := 0;
        while i < n do
            let arr[i] := i;
            let i := i + 1;
        end while;
    end fillArray;
start
    variable-integer N := 5;
    variable-array[N] integer arr;
    
    call fillArray(arr, N);
    
    variable-integer i := 0;
    while i < N do
        output arr[i];
        output " ";
        let i := i + 1;
    end while;
end main.