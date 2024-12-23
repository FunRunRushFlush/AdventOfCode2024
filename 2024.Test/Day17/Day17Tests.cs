


using Day17;

namespace Year_2024.Test;

public class Day17Tests
{
    private Part01 _part01;

    public static string TestDay { get; set; } = "Day17";

    [SetUp]
    public void Setup()
    {
        // Initialisieren des gemockten Loggers
       
        // Erstellen einer Instanz von Part01 mit dem gemockten Logger
        _part01 = new Part01();
    }


    //[Test, TestCaseSource(nameof(Part1TestCases))]
    //public void Part1_Test(string filePath, int expectedOutput)
    //{
    //    var inputLines = File.ReadAllLines(filePath);

    //    // Führe die Funktion aus, die getestet werden soll
    //    var actualOutput = Day17.Part01.Result(inputLines);

    //    Assert.That(expectedOutput, Is.EqualTo(actualOutput), $"Test failed for file: {filePath}");
    //}

    //[Test,]
    //public void Part1_Test01()
    //{

    //    var inputLines = File.ReadAllLines(filePath);

    //    // Führe die Funktion aus, die getestet werden soll
    //    var actualOutput = Day17.Part01.Result(inputLines);

    //    Assert.That(expectedValue, Is.EqualTo(actualOutput), $"Test failed for file: {filePath}");
    //}
    [Test, TestCaseSource(nameof(Part1TestCases))]
    public void Part1_Test(string filePath, long expectedOutput)
    {
        var inputLines = File.ReadAllLines(filePath).AsSpan();
        var actualOutput = _part01.Result(inputLines);

        Assert.That(actualOutput, Is.EqualTo(expectedOutput), $"Test failed for file: {filePath}");
    }





    public static IEnumerable<TestCaseData> Part1TestCases
    {
        get
        {
            string testDay = TestDay;
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TestDay}/InputData");

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Test data folder not found: {folderPath}");
            }

            foreach (var filePath in Directory.GetFiles(folderPath, "*Example*.txt"))
            {
                // Extrahiere den erwarteten Wert aus dem Dateinamen (z. B. _140 aus Part01_01Example_140.txt)
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var expectedValue = long.Parse(fileName.Split('_').Last());

                yield return new TestCaseData(filePath, expectedValue)
                    .SetName($"{fileName}");
            }
        }
    }


}
