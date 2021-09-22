module main
    public function getArray(in integer n):array[] integer
        variable-array[n] integer arr;
        variable-integer i := 0;
        while i < n do
            let arr[i] := i;
            let i := i + 1;
        end while;
        return arr;
    end getArray;
start
    variable-integer N := 5;
    variable-array[N] integer arr := getArray(N);
    
    variable-integer i := 0;
    while i < N do
        output arr[i];
        output " ";
        let i := i + 1;
    end while;
end main.