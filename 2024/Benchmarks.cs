using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;


[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class Benchmarks
{
    string inputRaw = string.Empty;
    string[] input;
    string[] rules;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day08/InputData/Input.txt");
        inputRaw = File.ReadAllText(path);
    
         input = File.ReadAllLines(path);


    }
    [Benchmark]
    public void Part01_Result()
    {
        Day09.Part01.Result(inputRaw);
    }





}
