using Day02;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");
string input = File.ReadAllText(path);

Part01.Result(input);
Part02.Result(input);

