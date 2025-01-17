namespace Year_2024.Days.Day11;

public class Part02 : IPart
{
    private int BlinkLimit;
    private long StoneCounter;
    private Dictionary<(long, int), long> Cache;
    public string Result(Input input)
    {
        StoneCounter = 0;
        BlinkLimit = 75;
        Cache = new Dictionary<(long, int), long>();

        List<long> stoneList = input.Text
            .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        foreach (var element in stoneList)
        {
            StoneCounter += CheckStoneRule(element, 0);
        }

        GlobalLog.LogLine($"StoneCounter: {StoneCounter} - Result_Rec");
        return StoneCounter.ToString();
    }

    private long CheckStoneRule(long stoneNum, int blinkNum)
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