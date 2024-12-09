

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day09/InputData/Input.txt");

//var inputLines = File.ReadAllLines(path);
var inputLines = File.ReadAllText(path);


BenchmarkRunner.Run<Benchmarks>();

var numchar = char.IsDigit( inputLines[0]);

var num = (int)inputLines[0];

Day09.Part01.Result(inputLines);


