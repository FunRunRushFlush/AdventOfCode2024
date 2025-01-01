using BenchmarkDotNet.Running;
using System;


string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day24/InputData/InputCustom.txt");


var inputText= File.ReadAllText(path);
var inputLines = File.ReadAllLines(path);


//BenchmarkRunner.Run<Benchmarks>();


//Day24.Part01 dayP01 = new();
Day24.Part02Try dayP02Try = new();
//Day24.Part02 dayP02 = new();
//Day24.Part02Try dayP02 = new();

//long solution = dayP01.Result(inputLines);
//Console.WriteLine($"solution : {solution}");

var solutionTry = dayP02Try.Result(inputLines);
//long solution02 = dayP02.Result(inputLines);



//Console.WriteLine($"solution : {solution02}");
Console.WriteLine($"solutionTry : {solutionTry}");







