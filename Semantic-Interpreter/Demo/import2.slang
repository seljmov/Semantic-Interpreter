import files;
module main
start
    variable-Files.File myfile := Files.OpenFile("file.txt");
    output "1. pos -> ${myfile.Position()} \n";
    output "int -> ${myfile.ReadInteger()} \n";
    output "int -> ${myfile.ReadInteger()} \n";
    output "int -> ${myfile.ReadInteger()} \n";
    output "2. pos -> ${myfile.Position()} \n";
    call myfile.Close();
end main.