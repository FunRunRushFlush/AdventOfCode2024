using BenchmarkDotNet.Running;
using Day18;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day18/InputData/Input.txt");


var inputText= File.ReadAllText(path);
var inputLines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();



Part01 dayP01 = new();
Day18.Part02 dayP02 = new();

long solution = dayP01.Result(inputLines);
long solution02 = dayP02.Result(inputLines);
long solution03 = dayP02.ResultBackwards(inputLines);


Console.WriteLine($"solution : {solution}");
Console.WriteLine($"solution : {solution02}");
Console.WriteLine($"solution : {solution03}");






