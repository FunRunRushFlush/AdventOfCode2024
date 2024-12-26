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
    private Day25.Part01 _part01;
    private Day25.Part02 _part02;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day25/InputData/Input.txt");
        inputRaw = File.ReadAllText(path);
    
         input = File.ReadAllLines(path);

    }

    [IterationSetup]
    public void SetupIteration()
    {
        // Vor jedem Iterationslauf eine neue Instanz anlegen
        _part01 = new Day25.Part01();
        _part02 = new Day25.Part02();
    }

    //[Benchmark]
    //public void Part01_Parse()
    //{
    //    _part01.ParseOnly(input);
    //}
    //[Benchmark]
    //public void Part02_Parse()
    //{
    //    _part02.ParseOnly(input);
    //}
    [Benchmark]
    public void Part01_Result()
    {
        _part01.Result(input);
    }
    //[Benchmark]
    //public void Part02_Result()
    //{
    //    _part02.Result(input);
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
