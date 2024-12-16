


using BenchmarkDotNet.Running;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day15/InputData/Part03Example.txt");


var inputText= File.ReadAllText(path);
var inputLines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();


//long solution01 = Day15.Part01.Result(inputText);

long solution = Day15.Part02.Result(inputText);

//GlobalLog.LogLine($"{solution01}");



