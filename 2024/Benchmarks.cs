using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;



[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class Benchmarks
{

    private Input _input;

    private Day01.Part01 _part01;
    private Day01.Part02 _part02;

    [GlobalSetup]
    public void Setup()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day01/InputData/Input.txt");
        _input = new(
         File.ReadAllBytes(path),
         File.ReadAllText(path),
         File.ReadAllLines(path));
    }

    [IterationSetup]
    public void SetupIteration()
    {
        // Vor jedem Iterationslauf eine neue Instanz anlegen
        _part01 = new Day01.Part01();
        _part02 = new Day01.Part02();
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







}
