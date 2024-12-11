

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day11/InputData/Input.txt");

var inputLines = File.ReadAllText(path);
//var inputLines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();


//long solution = Day11.Part01.Result(inputLines);
//long solutionRec = Day11.Part01.Result_test(inputLines);

//var solutionTest = Day11.Part01.Result_Rec(inputLines);
//var solution01 = Day11.Part02.Result(inputLines);

//GlobalLog.Log($"{solution}");
//GlobalLog.Log($"{solutionTest}");


//GlobalLog.Log($"{solution01}");


