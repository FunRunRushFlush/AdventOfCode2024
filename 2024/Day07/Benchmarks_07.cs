﻿using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;
using Day07;


namespace _2024.Day07;

[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class Benchmarks_07
{
    string inputRaw = string.Empty;
    string[] input;
    string[] rules;

    //[GlobalSetup]
    //public void Setup()
    //{
    //    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");
    //    inputRaw = File.ReadAllText(path);

    //    input = File.ReadAllLines(path);


    //}
    //[Benchmark]
    //public void Part01_Result()
    //{
    //    Part01.Result(input);
    //}

    //[Benchmark]
    //public void Part02_Result()
    //{
    //    Part02.Result(input);
    //}



}
