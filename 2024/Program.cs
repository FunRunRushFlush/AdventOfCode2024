using BenchmarkDotNet.Running;


string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day19/InputData/CustomInput.txt");


var inputText= File.ReadAllText(path);
var inputLines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();



Day19.Part01 dayP01 = new();
//Day19.Part01Try dayP01Try = new();
//Day19.Part02 dayP02 = new();

long solution = dayP01.Result(inputLines);
//long solutionTry = dayP01Try.Result(inputLines);
//long solution02 = dayP02.Result(inputLines);



Console.WriteLine($"solution : {solution}");
//Console.WriteLine($"solution : {solutionTry}");
//Console.WriteLine($"solution : {solution02}");







