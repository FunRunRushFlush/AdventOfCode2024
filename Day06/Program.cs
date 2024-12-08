﻿//#define LOGGING_ENABLED
using BenchmarkDotNet.Running;
using CommandLine;
using CommunityToolkit.HighPerformance;
using Day06;
using System.Xml;


string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");



var lines = File.ReadAllLines(path);


BenchmarkRunner.Run<Benchmarks>();
GlobalLog.Log("Starte Benchmark...");
GlobalLog.Log($"Part02.Result(input): {Part02.Result(lines)}");

