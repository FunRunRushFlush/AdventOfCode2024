using BenchmarkDotNet.Running;
using Day02;




string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");
string input = File.ReadAllText(path);

//BenchmarkRunner.Run<Benchmarks>();
Part01.Result(input);

//Console.WriteLine("Part01.Result(input); {}",Part01.Result(input));
//Console.WriteLine("Part01.Result_Improved(input); {}", Part01.Result_Improved(input));
//Console.WriteLine("Part02.Result(input); {}", Part02.Result(input));
//Console.WriteLine("Part02.Result_Improved(input); {}", Part02.Result_Improved(input));
//Console.WriteLine($"Part02.Result(input) : {part02} "); 
