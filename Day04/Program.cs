using BenchmarkDotNet.Running;
using Day04;




string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");
string input = File.ReadAllText(path);
var inputLinesAsSpan = File.ReadAllLines(path).AsSpan();

BenchmarkRunner.Run<Benchmarks>();
//Part01.Result(inputLinesAsSpan);
//Part02.Result(input);

Console.WriteLine("Part01.Result(input); {0}", Part01.Result(inputLinesAsSpan));
//Console.WriteLine("Part01.Result_Improved(input); {0}", Part01.Result_Improved(inputSpan));
//Console.WriteLine("Part01.Result_Improved_02(input); {0}", Part01.Result_Improved_02(inputSpan));
//Console.WriteLine("Part01.Result_Improved_03(input); {0}", Part01.Result_Improved_03(inputSpan));

Console.WriteLine("Part02.Result(input); {0}", Part02.Result(inputLinesAsSpan));
//Console.WriteLine("Part02.Result_Improved(input); {}", Part02.Result_Improved(input));

