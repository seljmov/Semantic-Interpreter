module main
    public procedure first(in integer a)
        output "a = ${a} \n";
        let a := 123;
        output "a = ${a} \n";
        if a == 123 then
            variable-integer q := a + 19999;
            let a := q;
            output "a = ${a} \n";
        end if;
    end first;      
start
    variable-integer qw := 1;
    call first(qw);
end main.