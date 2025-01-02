using BenchmarkDotNet.Attributes;
using System;
using System.IO;

[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class DayBenchmark
{
    [Params("Day01", "Day02", "Day03")] // Liste der Tage, die getestet werden
    public string Day { get; set; }

    private string inputRaw;
    private string[] inputLines;
    private IPart part01Instance;
    private IPart part02Instance;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Day}/InputData/Input.txt");
        inputRaw = File.ReadAllText(path);
        inputLines = File.ReadAllLines(path);

        part01Instance = CreateInstance($"{Day}.Part01");
        part02Instance = CreateInstance($"{Day}.Part02");
    }

    private IPart CreateInstance(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type == null)
        {
            throw new ArgumentException($"Typ {typeName} nicht gefunden.");
        }

        return (IPart)Activator.CreateInstance(type);
    }

    [Benchmark]
    public void Part01_Result_String()
    {
        part01Instance.Result(inputRaw);
    }

    [Benchmark]
    public void Part02_Result_String()
    {
        part02Instance.Result(inputRaw);
    }

}
