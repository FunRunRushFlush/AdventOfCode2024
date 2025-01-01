using BenchmarkDotNet.Running;
using System;


string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day21/InputData/Input.txt");


var inputText= File.ReadAllText(path);
var inputLines = File.ReadAllLines(path);


//BenchmarkRunner.Run<Benchmarks>();


Day21.Part01Old dayP01 = new();
Day21.Part01Try dayP01Try = new();
Day21.Part02 dayP02 = new();
//Day21.Part02Try dayP02 = new();

//long solution = dayP01.Result(inputLines);
//Console.WriteLine($"solution : {solution}");

var solutionTry = dayP01Try.Result(inputLines);
//long solution02 = dayP02.Result(inputLines);



//Console.WriteLine($"solution : {solution02}");
Console.WriteLine($"solutionTry : {solutionTry}");







