using Day17;


using BenchmarkDotNet.Running;


string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day17/InputData/Input.txt");


var inputText= File.ReadAllText(path);
var inputLines = File.ReadAllLines(path);


//BenchmarkRunner.Run<Benchmarks>();


//long solution01 = Day15.Part01.Result(inputText);
Part01 day17P01 = new();
Day17.Part02 day17P02 = new();

long solution = day17P01.Result(inputLines);
long solution02 = day17P02.Result(inputLines);

Console.WriteLine($"solution : {solution}");
Console.WriteLine($"solution : {solution02}");






