


namespace Year_2024.Test;

public class Day16Tests
{
    public static string TestDay { get; set; } = "Day16";
    [SetUp]
    public void Setup()
    {

    }

    //[Test, TestCaseSource(nameof(Part1TestCases))]
    //public void Part1_Test(string filePath, int expectedOutput)
    //{
    //    var inputLines = File.ReadAllLines(filePath);

    //    // Führe die Funktion aus, die getestet werden soll
    //    var actualOutput = Day16.Part01.Result(inputLines);

    //    Assert.That(expectedOutput, Is.EqualTo(actualOutput), $"Test failed for file: {filePath}");
    //}

    [Test,]
    public void Part1_Test01()
    {
        string testDay = TestDay;
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TestDay}/InputData/Part01Example_7036.txt");
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var expectedValue = int.Parse(fileName.Split('_').Last());
        var inputLines = File.ReadAllLines(filePath);

        // Führe die Funktion aus, die getestet werden soll
        var actualOutput = Day16.Part01.Result(inputLines);

        Assert.That(expectedValue, Is.EqualTo(actualOutput), $"Test failed for file: {filePath}");
    }
    [Test,]
    public void Part1_Test02()
    {
        string testDay = TestDay;
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TestDay}/InputData/Part02Example_11048.txt");
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var expectedValue = int.Parse(fileName.Split('_').Last());
        var inputLines = File.ReadAllLines(filePath);

        // Führe die Funktion aus, die getestet werden soll
        var actualOutput = Day16.Part01.Result(inputLines);

        Assert.That(expectedValue, Is.EqualTo(actualOutput), $"Test failed for file: {filePath}");
    }





    //public static IEnumerable<TestCaseData> Part1TestCases
    //{
    //    get
    //    {
    //        string testDay = TestDay;
    //        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TestDay}/InputData");

    //        if (!Directory.Exists(folderPath))
    //        {
    //            throw new DirectoryNotFoundException($"Test data folder not found: {folderPath}");
    //        }

    //        foreach (var filePath in Directory.GetFiles(folderPath, "*Example*.txt"))
    //        {
    //            // Extrahiere den erwarteten Wert aus dem Dateinamen (z. B. _140 aus Part01_01Example_140.txt)
    //            var fileName = Path.GetFileNameWithoutExtension(filePath);
    //            var expectedValue = int.Parse(fileName.Split('_').Last());

    //            yield return new TestCaseData(filePath, expectedValue)
    //                .SetName($"{fileName}"); 
    //        }
    //    }
    //}


}
