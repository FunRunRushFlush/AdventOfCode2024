

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day10/InputData/Input.txt");

//var inputLines = File.ReadAllLines(path);
var inputLines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();


//long solution = Day09.Part01.Result(inputLines);

//var solution = Day10.Part01.Result(inputLines);
var solution01 = Day10.Part02.Result(inputLines);
var solution02 = Day10.Part02.Result_Improved01(inputLines);

Console.WriteLine(solution01-solution02);


