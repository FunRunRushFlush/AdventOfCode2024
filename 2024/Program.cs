

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day09/InputData/Input.txt");

//var inputLines = File.ReadAllLines(path);
var inputLines = File.ReadAllText(path);


BenchmarkRunner.Run<Benchmarks>();


//long solution = Day09.Part01.Result(inputLines);

var solution = Day09.Part02.Result(inputLines);

Console.WriteLine(solution);


