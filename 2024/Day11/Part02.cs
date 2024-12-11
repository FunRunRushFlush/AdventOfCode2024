namespace Day11;

public static class Part02
{
    private static int BlinkLimit;
    private static long StoneCounter;
    private static Dictionary<(long, int), long> Cache;
    public static long Result(string input)
    {
        StoneCounter = 0;
        BlinkLimit = 75;
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