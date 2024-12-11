

using Perfolizer.Mathematics.Randomization;

namespace Day11;
public static class Part01
{

    private static int BlinkLimit;
    private static long StoneCounter;
    private static Dictionary<(long, int), long> Cache;
    //1 2024 1 0 9 9 2021976
    public static long Result(string input)
    {
        BlinkLimit = 25;
        List<string> stoneList = input
            .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        for (int x = 0; x < BlinkLimit; x++)
        {
            GlobalLog.Log($"{string.Join(", ", stoneList)}");
            for (int i = 0; i < stoneList.Count; i++)
            {
                string stone = stoneList[i];
                if (stone == "0") stoneList[i] = "1";
                else if (stone.Length % 2 == 0)
                {
                    stoneList.Insert(i, stone.Substring(0, stone.Length / 2));
                    stoneList[i + 1] = int.Parse(stone.Substring(stone.Length / 2, stone.Length / 2)).ToString();
                    i++;
                }
                else
                {
                    stoneList[i] = (long.Parse(stone) * 2024).ToString();
                }
            }
        }

        return stoneList.Count;
    }

  

    public static long Result_Improved(string input)
    {
        StoneCounter = 0;
        BlinkLimit = 25;
        Cache = new Dictionary<(long, int), long>();

        List<long> stoneList = input
            .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        foreach (var element in stoneList)
        {
            StoneCounter += CheckStoneRule(element, 0);
        }

        GlobalLog.Log($"StoneCounter: {StoneCounter} - Result_Rec");
        return StoneCounter;
    }

    private static long CheckStoneRule(long stoneNum, int blinkNum)
    {
        if (blinkNum == BlinkLimit)
        {
            return 1;
        }

        if (Cache.TryGetValue((stoneNum, blinkNum), out long cachedResult))
        {
            return cachedResult;
        }

        long result = 0;

        if (stoneNum == 0)
        {
            result = CheckStoneRule(1, blinkNum + 1);
        }
        else if (stoneNum.ToString().Length % 2 == 0)
        {
            long divisor = (long)Math.Pow(10, stoneNum.ToString().Length / 2);
            result += CheckStoneRule(stoneNum / divisor, blinkNum + 1);
            result += CheckStoneRule(stoneNum % divisor, blinkNum + 1);
        }
        else
        {
            result = CheckStoneRule(stoneNum * 2024, blinkNum + 1);
        }

        Cache[(stoneNum, blinkNum)] = result;
        return result;
    }
}

