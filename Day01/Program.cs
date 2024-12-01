
string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");
string day01Input = File.ReadAllText(path);


Part01.Result(day01Input);
Part02.Result(day01Input);