

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day08/InputData/Input.txt");

var inputLines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();




Day08.Part01.Result(inputLines);

Day08.Part02.Result(inputLines);
