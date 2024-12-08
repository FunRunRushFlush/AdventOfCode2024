using BenchmarkDotNet.Attributes;


namespace Day03;

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
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputData/Input.txt");
        inputRaw = File.ReadAllText(path);
        input = File.ReadAllLines(path).ToArray();
    }
    [Benchmark]
    public void Part01_Result()
    {
        Day03.Part01.Result(inputRaw);
    }

    [Benchmark]
    public void Part01_Result_Improved()
    {
        Day03.Part01.Result_Improved(inputRaw);
    }
    [Benchmark]
    public void Part01_Result_Improved_02()
    {
        Day03.Part01.Result_Improved_02(inputRaw);
    }
    [Benchmark]
    public void Part01_Result_Improved_03()
    {
        Day03.Part01.Result_Improved_03(inputRaw);
    }
    [Benchmark]
    public void Part02_Result()
    {
        Day03.Part02.Result(inputRaw);
    }

    [Benchmark]
    public void Part02_CheckIfCharIsNum()
    {
        foreach (char ch in inputRaw)
        {
            CheckIfCharIsNum(ch);
        }
    }
        private static bool CheckIfCharIsNum(char input) => ((input - '0') >= 0 && (input - '0') <= 9) ? true : false;

    [Benchmark]
    public void Part02_IsDigit()
    {
        foreach (char ch in inputRaw)
        {
            char.IsDigit(ch);
        }
    }



    //[Benchmark]
    //public void Part01_LinqParser()
    //{
    //    int[][] lines = inputRaw
    //.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
    //.Select(line => line.Split(' ').Select(int.Parse).ToArray())
    //.ToArray();
    //}

    //[Benchmark]
    //public void Part01_CustomParser()
    //{
    //    Day03.Part01.InputParser(inputRaw);
    //}



}
