import Math;
module main
start
    variable-integer i := -1;
    output "before -> ${i} \n";
    let i := Math.Abs(i);
    output "after -> ${i} \n";
end main.