namespace Year_2024.Days.Day07;

public class Part01 : IPart
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


        }

        return solution;
    }

    //Vorwärts
    //private long CanBeCalculated(long targetNum, long[] nums)
    //{
    //    long solution = 0;
    //    if (nums[0] > targetNum) return 0;

    //    if (nums.Length == 1)
    //    {
    //        if (nums[0] == targetNum) return 1;
    //        return 0;
    //    }

    //    if (nums.Length >= 2)
    //    {
    //        var add = nums[1..].ToArray();
    //        var mult = nums[1..].ToArray();

    //        add[0] += nums[0];
    //        solution += CanBeCalculated(targetNum, add);

    //        mult[0] *= nums[0];
    //        solution += CanBeCalculated(targetNum, mult);
    //    }

    //    return solution;
    //}
}