using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;



//[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class Benchmarks
{
    string inputRaw = string.Empty;
    string[] input;
    string[] rules;
    private Day20.Part01 _part01;
    private Day20.Part02 _part02;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day20/InputData/Input.txt");
        inputRaw = File.ReadAllText(path);
    
         input = File.ReadAllLines(path);

        _part01 = new Day20.Part01();
        _part02 = new Day20.Part02();
    }
    [Benchmark]
    public void Part01_Result()
    {
        _part01.Result(input);
    }
    //[Benchmark]
    //public void Part01_ResultV2()
    //{
    //    _part01.ResultV2(input);
    //}

    //[Benchmark]
    //public void Part02_Result()
    //{
    //    _part02.Result(input);
    //}

    //[Benchmark]
    //public void Part02_ResultBackwards()
    //{
    //    _part02.ResultBackwards(input);
    //}






}
