namespace Year_2024.Days.Day19;
public class Part02 : IPart
{
    private HashSet<string> Designs = new();
    private Dictionary<string, long> memo = new();

    public string Result(Input input)
    {
        InputParser(input.Lines);
        long possibleLogos = 0;

        for (int i = 2; i < input.Lines.Length; i++)
        {
            GlobalLog.LogLine($"Line; {i}");
            var line = input.Lines[i].AsSpan();
            possibleLogos += CountCombinations(input.Lines[i]);

        }

        return possibleLogos.ToString();
    }


    private bool CheckForExistingDesign(ReadOnlySpan<char> subString)
    {
        if (Designs.Contains(subString.ToString()))
        {
            return true;
        }

        for (int j = subString.Length; j > 0; j--)
        {
            var subSubString = subString.Slice(0, j);
            if (Designs.Contains(subSubString.ToString()))
            {
                if (CheckForExistingDesign(subString.Slice(j)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    //TODO: Cache Implementierung üben/verstehen gleiche wie in Day11 
    public long CountCombinations(string s)
    {

        if (memo.TryGetValue(s, out long savedCount))
            return savedCount;

        long totalWays = 0;


        if (Designs.Contains(s))
        {
            totalWays++;
        }


        for (int i = 1; i < s.Length; i++)
        {
            string left = s[..i];
            if (Designs.Contains(left))
            {
                string right = s[i..];
                totalWays += CountCombinations(right);
            }
        }

        memo[s] = totalWays;
        return totalWays;
    }


    private void InputParser(ReadOnlySpan<string> input)
    {
        var line = input[0].AsSpan();

        while (true)
        {
            int commaIndex = line.IndexOf(',');
            if (commaIndex == -1) break;

            var instructionSpan = line.Slice(0, commaIndex);
            line = line.Slice(commaIndex + 2); //+2 wegen komma+space ', '

            Designs.Add(instructionSpan.ToString());
        }
        Designs.Add(line.ToString()); // mir fehlt sonst das letzte
    }




}