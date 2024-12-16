


namespace Year_2024.Test;

public class Day12Tests
{
    private string TestDay = "Day12";
    [SetUp]
    public void Setup()
    {

    }

    //[Test, TestCaseSource(nameof(Part1TestCases))]
    //public void Part1_Test(string filePath, int expectedOutput)
    //{
    //    var inputLines = File.ReadAllLines(filePath);

    //    // Führe die Funktion aus, die getestet werden soll
    //    var actualOutput = Day12.Part01.Result(inputLines);

    //    Assert.That(expectedOutput, Is.EqualTo(actualOutput), $"Test failed for file: {filePath}");
    //}

    //[Test, TestCaseSource(nameof(Part2TestCases))]
    //public void Part2(string filePath, int expectedOutput)
    //{
    //    var inputLines = File.ReadAllLines(filePath);

    //    // Führe die Funktion aus, die getestet werden soll
    //    var actualOutput = Day12.Part02Try.Result(inputLines);

    //    Assert.That(expectedOutput,Is.EqualTo( actualOutput), $"Test failed for file: {filePath}");
    //}
    [TestCaseSource(nameof(Part2TestCases))]
    public void Part2_Corner(string filePath, int expectedOutput)
    {
        var inputLines = File.ReadAllLines(filePath);

        // Führe die Funktion aus, die getestet werden soll
        var actualOutput = Day12.Part02Block.Result(inputLines);

        Assert.That(expectedOutput, Is.EqualTo(actualOutput), $"Test failed for file: {filePath}");
    }




    public static IEnumerable<TestCaseData> Part1TestCases
    {
        get
        {
            string testDay = "Day12";
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{testDay}/InputData");

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Test data folder not found: {folderPath}");
            }

            foreach (var filePath in Directory.GetFiles(folderPath, "Part01_*.txt"))
            {
                // Extrahiere den erwarteten Wert aus dem Dateinamen (z. B. _140 aus Part01_01Example_140.txt)
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var expectedValue = int.Parse(fileName.Split('_').Last());

                yield return new TestCaseData(filePath, expectedValue)
                    .SetName($"Part1_{fileName}"); // Setzt den Testnamen dynamisch
            }
        }
    }

    public static IEnumerable<TestCaseData> Part2TestCases
    {
        get
        {
            string testDay = "Day12";
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{testDay}/InputData");

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Test data folder not found: {folderPath}");
            }

            foreach (var filePath in Directory.GetFiles(folderPath, "Part02_*.txt"))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var expectedValue = int.Parse(fileName.Split('_').Last());

                yield return new TestCaseData(filePath, expectedValue)
                    .SetName($"Part2_{fileName}");
            }

            var specificFiles = new Dictionary<string, int>
            {
                { $"{folderPath}/Input.txt", 893790 }
                
            };
            foreach (var specificFile in specificFiles)
            {
                yield return new TestCaseData(specificFile.Key, specificFile.Value)
                    .SetName($"Part2_{Path.GetFileNameWithoutExtension(specificFile.Key)}");
            }
        }
    }

}
