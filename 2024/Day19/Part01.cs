
namespace Day19;
public class Part01
{
    private HashSet<string> Designs = new();

    public long Result(ReadOnlySpan<string> input)
    {
        InputParser(input);
        int possibleLogos = 0;

        for (int i = 2; i < input.Length; i++)
        {
            GlobalLog.LogLine($"Line; {i}");
            var line = input[i].AsSpan();
           int index = 0;
            List<int> nodeStart = new List<int>();
            
            for(int j = line.Length; j>0;j--)
            {
                var subString = line.Slice(0,j);
                if (Designs.Contains(subString.ToString()))
                {
                    var newline = line.Slice(j);
                    if (CheckForExistingDesign(newline))
                    {
                        possibleLogos++;
                        break;
                    };
                }
            }
            
        }

        return possibleLogos;
    }

    //private bool CheckForExistingDesign(ReadOnlySpan<char> subString)
    //{
    //    GlobalLog.LogLine($"CheckForExistingDesign; {subString}");
    //    bool result = false;
    //    if ( Designs.Contains(subString.ToString()))
    //    {
    //        return true;
    //    }
    //    for (int j = subString.Length; j > 0; j--)
    //    {
    //        GlobalLog.LogLine($" subString.Length; {j}");
    //        var subSubString = subString.Slice(0, j);
    //        if (Designs.Contains(subSubString.ToString()))
    //        {
    //            result = CheckForExistingDesign(subString.Slice(j));
    //        }
    //    }
    //    return result;
    //}
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