using BenchmarkDotNet.Attributes;
using System;
using System.IO;

[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class DayBenchmark
{
    //[Params("Day01", "Day02", "Day03", "Day04", "Day05", "Day06")]
    [Params("Day01", "Day02", "Day03", "Day04", "Day05", "Day06", "Day07", "Day08", "Day09", "Day10", "Day11", "Day12", "Day13", "Day14")]
    public string Day { get; set; }

    private Input _input;
    private string[] inputLines;
    private IPart part01Instance;
    private IPart part02Instance;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Day}/InputData/Input.txt");
        _input = new(
            File.ReadAllBytes(path),
            File.ReadAllText(path),
            File.ReadAllLines(path));

    }

    private IPart CreateInstance(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type == null)
        {
            throw new ArgumentException($"Typ {typeName} not found");
        }

        return (IPart)Activator.CreateInstance(type);
    }

    [IterationSetup]
    public void SetupIteration()
    {
        // Vor jedem Iterationslauf eine neue Instanz anlegen bekomme sonst Bugs
        part01Instance = CreateInstance($"{Day}.Part01");
        part02Instance = CreateInstance($"{Day}.Part02");
    }

    [Benchmark]
    public void Part01()
    {
        part01Instance.Result(_input);
    }

    [Benchmark]
    public void Part02()
    {
        part02Instance.Result(_input);
    }

}
