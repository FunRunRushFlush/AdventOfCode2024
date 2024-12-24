
using System.Collections.Generic;

namespace Day19;
public class Part01
{
    private HashSet<string> Designs = new();
    private HashSet<string> BlackList = new();

    public long Result(ReadOnlySpan<string> input)
    {
        InputParser(input);

         int possibleLogos = 0;

        for (int i = 2; i < input.Length; i++)
        {
            var instrucLineOrig = input[i].AsSpan();
            var instrucLine = input[i].AsSpan();
            int counter = 0;

            while(true)
            {
                var res = CheckIfMatching(instrucLine);
                if (!res.match)
                {
                    if (counter !=0)
                    {
                    GlobalLog.LogLine($"{input[i]}");
                        SpecialEdgeCase(instrucLineOrig);
                    }
                    break;
                }
                instrucLine = instrucLine.Slice(res.length);

                counter += res.length;
                if (counter == input[i].Length)
                {
                    possibleLogos++;
                    break;
                }
                if (counter > input[i].Length)
                {
                    GlobalLog.LogLine($"counter > input[{i}].Length");
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

    private bool SpecialEdgeCase(ReadOnlySpan<char> instrucLine)
    {
        GlobalLog.LogLine($"SpecialEdgeCase");
        GlobalLog.LogLine($"instrucLine; {instrucLine}");
        if (instrucLine.IsEmpty || instrucLine.Length ==1)
            return true;

        for (int i = 1; i< instrucLine.Length; i++)
        {
            if (Designs.Contains(instrucLine.ToString()))
            {
            GlobalLog.LogLine($"forloop: {i}");
                return true;
                
            }            
            var rest = instrucLine.Slice(0, i);

            var slice = instrucLine.Slice(i);
            if (Designs.Contains(slice.ToString()))
            {
                GlobalLog.LogLine($"forloop: {i}");
                SpecialEdgeCase(rest);
            }
        }

        return false;

    }
    private bool SpecialEdgeCase_test(ReadOnlySpan<char> instrucLine)
    {
            var instrucLineOrig = instrucLine;
            int counter = 0;
        for (int i = 0; i < instrucLine.Length; i++)
        {


            while (true)
            {
                var res = CheckIfMatching(instrucLine);
                if (!res.match)
                {
                    if (counter != 0)
                    {
                        GlobalLog.LogLine($"{input[i]}");
                        SpecialEdgeCase(instrucLineOrig);
                    }
                    break;
                }
                instrucLine = instrucLine.Slice(res.length);

                counter += res.length;
                if (counter == input[i].Length)
                {
                    possibleLogos++;
                    break;
                }
                if (counter > input[i].Length)
                {
                    GlobalLog.LogLine($"counter > input[{i}].Length");
                }

            }

        }

    private (bool match, int length) CheckIfMatchingV2(ReadOnlySpan<char> instrucLine)
    {
        //GlobalLog.LogLine($"instrucLine {instrucLine}");
        if (Designs.Contains(instrucLine.ToString()) && Blacklist.C)
        {
            //GlobalLog.LogLine($"Return instrucLine {instrucLine}");
            return (true, instrucLine.Length);
        }
        else if (instrucLine.Length == 1) return (false, 0);

        var subLine = instrucLine.Slice(0, instrucLine.Length - 1);
        var res = CheckIfMatching(subLine);
        if (res.match) return (true, res.length);

        return (false, 0);
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


    //public long ResultOld(ReadOnlySpan<string> input)
    //{
    //    InputParserOld(input);


    //    foreach (var item in Designs)
    //    {
    //        GlobalLog.LogLine(item);
    //    }
    //    return 0;
    //}

    private void InputParser(ReadOnlySpan<string> input)
    {
        var line = input[0].AsSpan();
        ReadOnlySpan<char> restSpan;
        while (true)
        {
            int commaIndex = line.IndexOf(',');
            if (commaIndex == -1) break;

            var instructionSpan = line.Slice(0, commaIndex);
            line = line.Slice(commaIndex + 2); //+2 wegen komma+space ', '

            Designs.Add(instructionSpan.ToString());
        }
        Designs.Add(line.ToString()); // mir fehklt sonst das letzte
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