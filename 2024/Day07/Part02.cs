
using CommunityToolkit.HighPerformance;

namespace Day07;
public class Part02 : IPart
{
    
    public string Result(Input rawInput)
    {
        long resCounter = 0;
        foreach (var line in rawInput.Lines)
        {
            long[] calc = line.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse).ToArray();

            if (CanBeCalculated(calc[0], calc[1..]) > 0)
                resCounter += calc[0];
        }

        return resCounter.ToString();
    }

    //Rückwärts
    private long CanBeCalculated(long targetNum, long[] nums)
    {
        long solution = 0;

        if (nums.Length == 2)
        {
            if (targetNum / nums[1] == nums[0] && targetNum % nums[1] == 0)
                return 1;
            if (targetNum - nums[1] == nums[0])
                return 1;
            if (nums[0].ToString()+ nums[1].ToString() == targetNum.ToString())
                return 1;

            return 0;
        }

        if (nums.Length > 2)
        {

            if (targetNum % nums[^1] == 0)
            {
                solution += CanBeCalculated(targetNum / nums[^1], nums[..(nums.Length - 1)]);
            }
            if (targetNum - nums[^1] >= 0)
            {
                solution += CanBeCalculated(targetNum - nums[^1], nums[..(nums.Length - 1)]);
            }
            string strTargetNum = targetNum.ToString();
            string suffix = nums[^1].ToString();
            //TODO: EndWith() kannte ich nicht
            if (strTargetNum.EndsWith(suffix)&& suffix.Length !=strTargetNum.Length)
            {
                var newTargetNum = long.Parse(strTargetNum.Substring(0, strTargetNum.Length - suffix.Length));
                solution += CanBeCalculated(newTargetNum, nums[..(nums.Length - 1)]);
            }
        }

        return solution;
    }
}
