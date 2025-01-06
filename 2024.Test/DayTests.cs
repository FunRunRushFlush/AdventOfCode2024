using NUnit.Framework;
using System;
using System.IO;

namespace Year_2024.Test;
[TestFixture]
public class DayTests
{
    private static readonly string[] Days =
    {
         "Day01", "Day02", "Day03", "Day04", "Day05", "Day06", "Day07", "Day08",
         "Day09", "Day10", "Day11", "Day12", "Day13", "Day14", "Day15", "Day16",
         "Day17", "Day18", "Day19", "Day20", "Day21", "Day22", "Day23", "Day24", "Day25"
     };

    //private static readonly string[] Days =
    //{
    //   "Day01", "Day02", "Day03", "Day04", "Day05", "Day06", "Day07", "Day08",
    //};

    // private static readonly string[] TestCases = { "Case01", "Case02", "Case03" }; 
    private static readonly string[] TestCases = { "Case01", "Case02" };


    [TestCaseSource(nameof(GetPart01TestCases))]
    public void Part01_ShouldReturnExpectedResult(string day, string testCase)
    {
        RunTest(day, testCase, "Part01");
    }

    [TestCaseSource(nameof(GetPart02TestCases))]
    public void Part02_ShouldReturnExpectedResult(string day, string testCase)
    {
        RunTest(day, testCase, "Part02");
    }


    private IPart CreateInstance(string typeName)
    {
        var type = Type.GetType($"{typeName}, Year_2024");
        if (type == null)
        {
            throw new ArgumentException($"Type {typeName} not found in assembly Year_2024.");
        }

        return (IPart)Activator.CreateInstance(type);
    }

    private static IEnumerable<TestCaseData> GetPart01TestCases()
    {
        foreach (var day in Days)
        {
            foreach (var testCase in TestCases)
            {
                yield return new TestCaseData(day, testCase).SetName($"{day}_{testCase}_Part01");
            }
        }
    }

    private static IEnumerable<TestCaseData> GetPart02TestCases()
    {
        foreach (var day in Days)
        {
            foreach (var testCase in TestCases)
            {
                yield return new TestCaseData(day, testCase).SetName($"{day}_{testCase}_Part02");
            }
        }
    }


    private void RunTest(string day, string testCase, string part)
    {
        string inputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"TestData/{day}/{testCase}/Input.txt");
        string expectedOutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"TestData/{day}/{testCase}/Expected{part}.txt");

        AssertFileExists(inputPath, expectedOutputPath, day, testCase);

        var input = new Input(
            File.ReadAllBytes(inputPath),
            File.ReadAllText(inputPath),
            File.ReadAllLines(inputPath));

        string expectedOutput = File.ReadAllText(expectedOutputPath);
        var instance = CreateInstance($"{day}.{part}");

        var result = instance.Result(input);

        Assert.That(result, Is.EqualTo(expectedOutput), $"Failed for {day} {testCase} {part}");
    }
    private static void AssertFileExists(string inputPath, string expectedOutputPath, string day, string testCase)
    {
        if (!File.Exists(inputPath))
            Assert.Fail($"Input file for {day}/{testCase} not found at {inputPath}");
        if (!File.Exists(expectedOutputPath))
            Assert.Fail($"Expected output file for {day}/{testCase} not found at {expectedOutputPath}");
    }



}
