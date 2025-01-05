using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;



[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class Benchmarks
{

    private Input _input;

    private Day20.Part01 _part01;
    private Day20.Part02 _part02;
    private Day20.Part01Old _part01Old;
    private Day20.Part02Old _part02Old;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day20/InputData/Input.txt");
        _input = new(
         File.ReadAllBytes(path),
         File.ReadAllText(path),
         File.ReadAllLines(path));
    }

    [IterationSetup]
    public void SetupIteration()
    {
        // Vor jedem Iterationslauf eine neue Instanz anlegen
        _part01 = new Day20.Part01();
        _part02 = new Day20.Part02();
        _part01Old = new Day20.Part01Old();
        _part02Old = new Day20.Part02Old();
    }


    [Benchmark]
    public void Part01_Result()
    {
        _part01.Result(_input);
    }
    [Benchmark]
    public void Part02_Result()
    {
        _part02.Result(_input);
    }

    [Benchmark]
    public void Part01Old()
    {
        _part01Old.Result(_input);
    }
    [Benchmark]
    public void Part02Old()
    {
        _part02Old.Result(_input);
    }






}
