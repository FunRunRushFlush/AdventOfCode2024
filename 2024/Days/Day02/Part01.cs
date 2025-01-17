namespace Year_2024.Days.Day02;

public class Part01 : IPart
{
    public string Result(Input input)
    {
        int safeReports = 0;
        foreach (var item in input.SpanLines)
        {
            int[] ints = item.Split(' ').Select(int.Parse).ToArray();
            if (CheckIfRepoertIsSafe(ints)) safeReports++;
        }

        return safeReports.ToString();
    }

    private bool CheckIfRepoertIsSafe(int[] ints)
    {
        bool? up = CheckIfUp(ints.First(), ints.Last());
        if (up == null) return false;
        for (int i = 0; i < ints.Length - 1; i++)
        {
            if (CheckIfUp(ints[i], ints[i + 1]) != up) return false;
            if (Math.Abs(ints[i + 1] - ints[i]) > 3) return false;
        }
        return true;
    }
    private bool? CheckIfUp(int start, int end)
    {
        if (start == end) return null;

        return end - start > 0 ? true : false;
    }
}
