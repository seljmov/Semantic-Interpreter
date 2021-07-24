module main
    public class Time inherits Object
        public field-string p0 := "parent field";

        public method p1(Time time)()
            output "parent method";
        end Hours;
    end Time;        
start
    variable-Time time;
    output "before -> ${time.p0} \n";
    let time.p0 := "non parent";
    output "after -> ${time.p0}";
end main.