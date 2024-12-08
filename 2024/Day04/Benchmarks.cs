using BenchmarkDotNet.Attributes;


namespace Day04;

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
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Part01Example_01.txt");
        inputRaw = File.ReadAllText(path);
        input = File.ReadAllLines(path);
    }
    [Benchmark]
    public void Part01_Result()
    {
        Part01.Result(input);
    }

    //[Benchmark]
    //public void Part01_Result_Improved()
    //{
    //    Part01.Result_Improved(inputRaw);
    //}
    //[Benchmark]
    //public void Part01_Result_Improved_02()
    //{
    //    Part01.Result_Improved_02(inputRaw);
    //}
    //[Benchmark]
    //public void Part01_Result_Improved_03()
    //{
    //    Part01.Result_Improved_03(inputRaw);
    //}
    //[Benchmark]
    //public void Part02_Result()
    //{
    //    Part02.Result(inputRaw);
    //}

    //[Benchmark]
    //public void Part02_CheckIfCharIsNum()
    //{
    //    foreach (char ch in inputRaw)
    //    {
    //        CheckIfCharIsNum(ch);
    //    }
    //}
    //private static bool CheckIfCharIsNum(char input) => ((input - '0') >= 0 && (input - '0') <= 9) ? true : false;

    //[Benchmark]
    //public void Part02_IsDigit()
    //{
    //    foreach (char ch in inputRaw)
    //    {
    //        char.IsDigit(ch);
    //    }
    //}






}
