using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters;
using System;
using System.IO;
using BenchmarkDotNet.Columns;

//[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
[Config(typeof(Config))]
public class DayBenchmark
{
    [Params("Day01", "Day02", "Day03", "Day04", "Day05", "Day06", "Day07", "Day08", "Day09", "Day10", "Day11", "Day12", "Day13", "Day14", "Day15", "Day16", "Day17", "Day18", "Day19", "Day20", "Day21", "Day22", "Day23", "Day24", "Day25")]
    public string Day { get; set; }

    private Input _input;
    private IPart part01Instance;
    private IPart part02Instance;

    private class Config : ManualConfig
    {
        public Config()
        {
            AddColumnProvider(DefaultColumnProviders.Instance);
            AddExporter(CsvExporter.Default);
            AddExporter(MarkdownExporter.GitHub);

            SummaryStyle = BenchmarkDotNet.Reports.SummaryStyle.Default
                .WithTimeUnit(Perfolizer.Horology.TimeUnit.Millisecond)
                .WithRatioStyle(BenchmarkDotNet.Columns.RatioStyle.Percentage);
        }
    }

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Days/{Day}/InputData/Input.txt");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Input file for {Day} not found at {path}");
        }

        _input = new(
            File.ReadAllBytes(path),
            File.ReadAllText(path),
            File.ReadAllLines(path));

        Console.WriteLine($"Setup completed for {Day}");
    }

    private IPart CreateInstance(string typeName)
    {
        string fullTypeName = $"Year_2024.Days.{typeName}, Year_2024";
        var type = Type.GetType(fullTypeName) ?? throw new ArgumentException($"Type {fullTypeName} not found");
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