module main
    public class Parent inherits Object
        public field-string p0 := "parent field";

        public method p1(Time time)()
            output "parent method";
        end Hours;
    end Time;
    
    public class Time inherits Parent

    end Time;        
start
    variable-Time time;
    call time.p1();
end main.