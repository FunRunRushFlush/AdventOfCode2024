﻿
namespace Day02;
public class Part02 : IPart
{
    public string Result(Input input)
    {
        int safeReports = 0;
        foreach (var item in input.SpanLines)
        {
            List<int> ints = item.Split(' ').Select(int.Parse).ToList();
            if (CheckIfRepoertIsSafe(ints))
            {
                safeReports++;
            }
            else
            {                   
                for (int i = 0; i < ints.Count; i++)
                {
                    var safeInt = ints[i];
                    ints.RemoveAt(i);
                    if (CheckIfRepoertIsSafe(ints))
                    {
                        safeReports++;
                        break;
                    }
                    ints.Insert(i,safeInt);
                }
            }
        }

        return safeReports.ToString();
    }

    private bool CheckIfRepoertIsSafe(List<int> ints)
    {
        bool? up = CheckIfUp(ints.First(), ints.Last());
        if (up == null) return false;
        for (int i = 0; i < ints.Count - 1; i++)
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
