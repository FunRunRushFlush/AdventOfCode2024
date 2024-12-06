using BenchmarkDotNet.Running;
using CommandLine;
using CommunityToolkit.HighPerformance;
using Day05;
using System.Xml;




string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");



var lines = File.ReadAllLines(path);
int separatorIndex = Array.IndexOf(lines, string.Empty);
int funf = 5;
var test2 = funf / 2;
Span2D<bool> rule2D = new bool[100, 100];
for (int i = 0; i < separatorIndex; i++)
{
    var test =lines[i].Split('|');
    rule2D[int.Parse(test[0]), int.Parse(test[1])] = true;
}

//int rows = rule2D.Height;
//int cols = rule2D.Width;
//Console.Write("#######################");
//Console.WriteLine();
//for (int i = 0; i < rows; i++)
//{
//    for (int j = 0; j < cols; j++)
//    {

//        Console.Write(rule2D[i, j]);
//        Console.ResetColor();
//        Console.Write(" ");
//    }
//    Console.WriteLine();
//}
//Console.Write("#######################");

var input = lines.AsSpan(separatorIndex + 1);
Span<int[]> inputSpan = new int[input.Length][];
for (int i = 0; i < input.Length; i++)
{
    inputSpan[i] = input[i].Split(',').Select(int.Parse).ToArray();
}



BenchmarkRunner.Run<Benchmarks>();
Part01.Result(lines);
//Part02.Result(input);

//Console.WriteLine("Part01.Result(input); {0}", Part01.Result(inputLinesAsSpan));
//Console.WriteLine("Part01.Result_Improved(input); {0}", Part01.Result_Improved(inputSpan));
//Console.WriteLine("Part01.Result_Improved_02(input); {0}", Part01.Result_Improved_02(inputSpan));
//Console.WriteLine("Part01.Result_Improved_03(input); {0}", Part01.Result_Improved_03(inputSpan));

//Console.WriteLine("Part02.Result(input); {0}", Part02.Result(input));
//Console.WriteLine("Part02.Result_Improved(input); {}", Part02.Result_Improved(input));

