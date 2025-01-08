﻿using BenchmarkDotNet.Running;
using System;


//string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day16/InputData/InputSecAcc.txt");
string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day16/InputData/Input.txt");


Input input = new(
      File.ReadAllBytes(path),
      File.ReadAllText(path),
      File.ReadAllLines(path));


BenchmarkRunner.Run<DayBenchmark>();
BenchmarkRunner.Run<Benchmarks>();


Day16.Part01 dayP01 = new();
//Day16.Part01Old dayP01Old = new();
Day16.Part02 dayP02 = new();
Day16.Part02Old dayP02Old = new();


//var solution = dayP01.Result(input);
//var solutionOld = dayP01Old.Result(input);
var solution02 = dayP02.Result(input);
var solution02Old = dayP02Old.Result(input);




//Console.WriteLine($"solution : {solution}");
//Console.WriteLine($"solution : {solutionOld}");

Console.WriteLine($"solution : {solution02}");
Console.WriteLine($"solution : {solution02Old}");
//Console.WriteLine($"solution Block : {solution02Block}");








