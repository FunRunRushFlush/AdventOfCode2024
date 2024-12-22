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
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day16/InputData/Input.txt");
        inputRaw = File.ReadAllText(path);
    
         input = File.ReadAllLines(path);
    }
    //[Benchmark]
    //public void Part01_Result()
    //{
    //    Day16.Part01.Result(input);
    //}

    //[Benchmark]
    //public void Result_ToArray()
    //{
    //    Day16.Part01.Result_ToArray(input);
    //}






}
