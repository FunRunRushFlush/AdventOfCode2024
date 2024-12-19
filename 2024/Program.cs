


using BenchmarkDotNet.Running;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day16/InputData/Custom.txt");


var inputText= File.ReadAllText(path);
var inputLines = File.ReadAllLines(path);


//BenchmarkRunner.Run<Benchmarks>();


//long solution01 = Day15.Part01.Result(inputText);

long solution = Day16.Part01.Result(inputLines);

Console.WriteLine($"solution : {solution}");


//GlobalLog.LogLine($"{solution01}");



