namespace Day17;
public class Part01Old:IPart
{
    private long aReg;
    private long bReg;
    private long cReg;
    private long[] ProgramCode;

    private List<long> OutList = new();
    private long Pointer = 0;

    public string Result(Input input)
    {
        ParseInstructions(input.Lines);

        GlobalLog.LogLine($"aReg: {aReg}");
        GlobalLog.LogLine($"bReg: {bReg}");
        GlobalLog.LogLine($"cReg: {cReg}");
        GlobalLog.LogLine($"ProgramCode: {ProgramCode.Length}");
        while(true)
        {
            if(Pointer>=ProgramCode.Length) break;

            OperationPicker(ProgramCode[Pointer], ProgramCode[Pointer + 1]);

            Pointer += 2;
        }
        GlobalLog.LogLine($"aReg: {aReg}");
        GlobalLog.LogLine($"bReg: {bReg}");
        GlobalLog.LogLine($"cReg: {cReg}");
        GlobalLog.LogLine($"ProgramCode: {ProgramCode.Length}");

        string solution = string.Empty;
        foreach (long i in OutList)
        {
            GlobalLog.Log($"{i}, ");
            solution += $"{i}";
        }
        GlobalLog.LogLine($"solution: {solution}");

        return solution;
    }

    private long ComboOperandPicker(long progValue)
    {
        if(progValue< 4) return (long)progValue;
        if (progValue == 4) return aReg;
        if (progValue == 5) return bReg;
        if (progValue == 6) return cReg;
        if (progValue == 7) return 7;

        throw new Exception();
    }

    private void OperationPicker(long opcode, long literalOperand)
    {
        long combo = ComboOperandPicker(literalOperand);
        //TODO: Switch case!
        if (opcode == 0)
        {
            AdvOperation(combo);
        }
        if (opcode == 1)
        {
            BxlOperation(literalOperand);
        }
        if (opcode == 2)
        {
            BstOperation(combo);
        }
        if (opcode == 3)
        {
            JnzOperation(literalOperand);
        }
        if (opcode == 4)
        {
            BxcOperation(combo);
        }
        if (opcode == 5)
        {
            OutOperation(combo);
        }
        if (opcode == 6)
        {
            BdvOperation(combo);
        }
        if (opcode == 7)
        {
            CdvOperation(combo);
        }
    }
    private void AdvOperation(long combo)
    {
        //TODO: alternativ: Math.Pow(2, combo);
        long denominator = 1 << (int)combo;
        denominator = (long)Math.Pow(2, combo);
        aReg = (long)(aReg/ denominator);
    }
    private void BxlOperation(long literal)
    {
        GlobalLog.LogLine($"literal: {literal}");
        GlobalLog.LogLine($"BxlOperation-Before: {bReg}");
        var test = bReg ^ literal;
        bReg = test;
        GlobalLog.LogLine($"BxlOperation-After: {bReg}");
    }
    private void BstOperation(long combo)
    {
        //var WikiPedia = """
        //    Modulo operations might be implemented such that a division with a remainder is calculated each time. For special cases, on some hardware, faster alternatives exist. For example, the modulo of powers of 2 can alternatively be expressed as a bitwise AND operation (assuming x is a positive integer, or using a non-truncating definition):

        //        x % 2n == x & (2n - 1)

        //    Examples:

        //        x % 2 == x & 1
        //        x % 4 == x & 3
        //        x % 8 == x & 7

        //    In devices and software that implement bitwise operations more efficiently than modulo, these alternative forms can result in faster calculations.[7]

        //    Compiler optimizations may recognize expressions of the form expression % constant where constant is a power of two and automatically implement them as expression & (constant-1), allowing the programmer to write clearer code without compromising performance.

        //    """;


        GlobalLog.LogLine($"combo: {combo}");
        var test = (long)(combo % 8);
        var test02 = combo & 7;
        GlobalLog.LogLine($"### BstOperation-Before: {bReg}");
        GlobalLog.LogLine($"### BstOperation-Modulo: {test}");
        bReg = test02;
        GlobalLog.LogLine($"### BstOperation-After: {bReg}");
    }
    private void JnzOperation(long literal)
    {
        if (aReg > 0)
        {
            Pointer = literal - 2;
        }
    }
    private void BxcOperation(long combo)
    {
        bReg = bReg ^ cReg;
    }

    private void OutOperation(long combo)
    {
        OutList.Add((long)(combo % 8));
    }
    private void BdvOperation(long combo)
    {
        long denominator = 1 << (int)combo;
        bReg = (long)(aReg / denominator);
    }

    private void CdvOperation(long combo)
    {
        long denominator = 1 << (int)combo;
        cReg = (long)(aReg / denominator);
    }


    private void ParseInstructions(ReadOnlySpan<string> input)
    {
        aReg = long.Parse(input[0].Split(' ').Last());
        bReg = long.Parse(input[1].Split(' ').Last());
        cReg = long.Parse(input[2].Split(' ').Last());

        //TODO: Ich habe immer noch Probleme mit dem parsen!
        ProgramCode = input[4].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries) 
                .Where(x => long.TryParse(x, out _)) 
                .Select(x => long.Parse(x))
                .ToArray();
    }
}