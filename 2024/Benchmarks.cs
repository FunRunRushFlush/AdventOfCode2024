﻿using BenchmarkDotNet.Attributes;
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
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day10/InputData/Input.txt");
        inputRaw = File.ReadAllText(path);
    
         input = File.ReadAllLines(path);


    }
    [Benchmark]
    public void Part01_Result()
    {
        Day10.Part01.Result(input);
    }
    [Benchmark]
    public void Part02_Result()
    {
        Day10.Part02.Result(input);
    }
    [Benchmark]
    public void Part02_Result_Improve01()
    {
        Day10.Part02.Result_Improved01(input);
    }






}
