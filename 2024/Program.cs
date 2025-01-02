using BenchmarkDotNet.Running;
using System;


string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day14/InputData/Input.txt");

Input input = new(
      File.ReadAllBytes(path),
      File.ReadAllText(path),
      File.ReadAllLines(path));


//BenchmarkRunner.Run<DayBenchmark>();
BenchmarkRunner.Run<Benchmarks>();


Day14.Part01 dayP01 = new();
//Day20.Part02 dayP02 = new();
Day14.Part02 dayP02 = new();

var solution = dayP01.Result(input);
var solutionTry = dayP02.Result(input);
//long solution02 = dayP02.Result(inputLines);



Console.WriteLine($"solution : {solution}");
//Console.WriteLine($"solution : {solution02}");
Console.WriteLine($"solution : {solutionTry}");







