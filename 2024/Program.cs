using BenchmarkDotNet.Running;


string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day25/InputData/Input.txt");


var inputText= File.ReadAllText(path);
var inputLines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();



Day25.Part01 dayP01 = new();
//Day20.Part02 dayP02 = new();
Day20.Part02 dayP02 = new();

long solution = dayP01.Result(inputLines);
long solutionTry = dayP02.Result(inputLines);
//long solution02 = dayP02.Result(inputLines);



Console.WriteLine($"solution : {solution}");
//Console.WriteLine($"solution : {solution02}");
Console.WriteLine($"solution : {solutionTry}");







