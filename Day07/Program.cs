#define LOGGING_ENABLED
using BenchmarkDotNet.Running;
using Day07;




//string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Part01Example_01.txt");
string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");



var lines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();
GlobalLog.Log("Starte Benchmark...");
GlobalLog.Log($"Part01.Result(input): {Part01.Result(lines)}");
GlobalLog.Log($"Part02.Result(input): {Part02.Result(lines)}");

