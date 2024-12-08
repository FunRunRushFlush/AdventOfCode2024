

using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day08/InputData/Input.txt");

var inputLines = File.ReadAllLines(path);


BenchmarkAction.




Day08.Part01.Result(inputLines);
