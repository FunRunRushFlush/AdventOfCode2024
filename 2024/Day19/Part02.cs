
namespace Day19;
public class Part02
{
    private HashSet<string> Designs = new();
    private HashSet<string> UsedTowels = new();

    public long Result(ReadOnlySpan<string> input)
    {
        InputParser(input);
        int possibleLogos = 0;

        for (int i = 2; i < input.Length; i++)
        {
            UsedTowels.Clear();
            GlobalLog.LogLine($"Line; {i}");
            var line = input[i].AsSpan();
            int index = 0;

            for (int j = line.Length; j > 0; j--)
            {
                var subString = line.Slice(0, j);
                if (Designs.Contains(subString.ToString()))
                {
                    var newline = line.Slice(j);

                    if (CheckForExistingDesign(newline))
                    {
                        possibleLogos++;
                    }
                    //possibleLogos += CheckForExistingDesignInt(newline);
                }
            }
            foreach(var ele in UsedTowels)
            {
                for (int j = ele.Length; j > 0; j--)
                {
                    var subString = ele.AsSpan().Slice(0, j);
                    if (Designs.Contains(subString.ToString()))
                    {
                        var newline = ele.AsSpan().Slice(j);

                        if (CheckForExistingDesignV2(newline))
                        {
                            possibleLogos++;
                        }
                        //possibleLogos += CheckForExistingDesignInt(newline);
                    }
                }
            }

        }


        foreach (var item in Designs)
        {
            GlobalLog.LogLine(item);
        }
        return possibleLogos;
    }

    //private int CheckForExistingDesignInt(ReadOnlySpan<char> subString)
    //{
    //    GlobalLog.LogLine($"CheckForExistingDesign; {subString}");
    //    int counter = 0;
    //    if (Designs.Contains(subString.ToString()) && !UsedTowels.Contains(subString.ToString()))
    //    {
    //        UsedTowels.Add(subString.ToString());
    //        return counter++;
    //    }
    //    for (int j = subString.Length; j > 0; j--)
    //    {
    //        var subSubString = subString.Slice(0, j);
    //        if (Designs.Contains(subSubString.ToString()) && !UsedTowels.Contains(subSubString.ToString()))
    //        {
    //            counter += (CheckForExistingDesignInt(subString.Slice(j)));
    //        }
    //    }


    //    return counter;
    //}

    private bool CheckForExistingDesign(ReadOnlySpan<char> subString)
    {
        if (Designs.Contains(subString.ToString()))
        {
            UsedTowels.Add(subString.ToString());
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
    private bool CheckForExistingDesignV2(ReadOnlySpan<char> subString)
    {
        if (Designs.Contains(subString.ToString()))
        {
            //UsedTowels.Add(subString.ToString());
            return true;
        }

        for (int j = subString.Length; j > 0; j--)
        {
            var subSubString = subString.Slice(0, j);
            if (Designs.Contains(subSubString.ToString()))
            {
                if (CheckForExistingDesignV2(subString.Slice(j)))
                {
                    return true;
                }
            }
        }
        return false;
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