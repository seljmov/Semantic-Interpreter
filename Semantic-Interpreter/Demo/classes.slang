module main    
    public class Time inherits Object
        public field-string p0 := "field p0";
        public field-string p3 := "field p3";

        public method p1(Time time)()
            output "parent method";
        end p1;
        
        public method p2(Time this)():string
            return this.p3;
        end p2;
    end Time;        
start
    variable-Time time;
    output "p2 -> ${time.p2()} \n";
end main.