module main
    public procedure test(in integer a)
        output "a = ${a} \n";
    end test;
start
    variable-integer a := 1;
    call test("yes -> ${a + 2 * 4 + 3}");
end main.