using BenchmarkDotNet.Attributes;


namespace Day02;

[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class Benchmarks
{
    string inputRaw = string.Empty;
    string[] input;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");
        inputRaw = File.ReadAllText(path);
        input = File.ReadAllLines(path).ToArray();
    }
    [Benchmark]
    public void Part01_Result()
    {
        Day02.Part01.Result(inputRaw);
    }

    [Benchmark]
    public void Part01_Result_Improved()
    {
        Day02.Part01.Result_Improved(inputRaw);
    }
    [Benchmark]
    public void Part02_Result()
    {
        Day02.Part02.Result(inputRaw);
    }

    [Benchmark]
    public void Part02_Result_Improved()
    {
        Day02.Part02.Result_Improved(inputRaw);
    }

    [Benchmark]
    public void Part01_LinqParser()
    {
        int[][] lines = inputRaw
        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
        .Select(line => line.Split(' ').Select(int.Parse).ToArray())
        .ToArray();
    }

    [Benchmark]
    public void Part01_CustomParser()
    {
        Day02.Part01.InputParser(inputRaw);
    }



}
