using BenchmarkDotNet.Running;
using CommandLine;
using CommunityToolkit.HighPerformance;
using Day05;
using System.Xml;




string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");



var lines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();
//Part01.Result(lines);

//Part02.Result(lines);

//Console.WriteLine("Part01.Result(input); {0}", Part01.Result(inputLinesAsSpan));
//Console.WriteLine("Part01.Result_Improved(input); {0}", Part01.Result_Improved(inputSpan));
//Console.WriteLine("Part01.Result_Improved_02(input); {0}", Part01.Result_Improved_02(inputSpan));
//Console.WriteLine("Part01.Result_Improved_03(input); {0}", Part01.Result_Improved_03(inputSpan));

//Console.WriteLine("Part02.Result(input); {0}", Part02.Result(input));
//Console.WriteLine("Part02.Result_Improved(input); {}", Part02.Result_Improved(input));

