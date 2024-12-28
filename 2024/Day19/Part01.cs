
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
           var line = input[i].AsSpan();
            int index = 0;
            
            for (int j = 1; j <= input[i].Length; j++)
            {
                if (Designs.Contains(line.Slice(index, j-index).ToString()))
                {
                    index=j;
                }
                else if(index == input[i].Length)

                if(index== input[i].Length)
                {
                    possibleLogos++;
                    break;
                }
            }


            //GlobalLog.LogLine($"String: i:{i}={input[i]}");
            //GlobalLog.LogLine($"possibleLogos: {possibleLogos}");

        }


        foreach (var item in Designs)
        {
            GlobalLog.LogLine(item);
        }
        return possibleLogos;
    }

    private (bool match,int length) CheckIfMatching(ReadOnlySpan<char> instrucLine)
    {
        //GlobalLog.LogLine($"instrucLine {instrucLine}");
        if (Designs.Contains(instrucLine.ToString()))
        {
            //GlobalLog.LogLine($"Return instrucLine {instrucLine}");
            return (true, instrucLine.Length);
        }
        else if(instrucLine.Length == 1) return (false,0);

        var subLine = instrucLine.Slice(0, instrucLine.Length-1);
        var res = CheckIfMatching(subLine);
        if (res.match) return (true, res.length);

        return (false, 0);
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

    private void InputParserOld(ReadOnlySpan<string> input)
    {
        var line = input[0];
        ReadOnlySpan<char> restSpan;
        while (true)
        {
            int commaIndex = line.IndexOf(',');
            if (commaIndex == -1) break;

            var instructionSpan = line.Substring(0, commaIndex);
            line = line.Substring(commaIndex + 2); //+2 wegen komma+space ', '

            Designs.Add(instructionSpan.ToString());
        }
    }


}