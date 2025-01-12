using System;
using System.Numerics;

namespace Year_2024.Days.Day17;
public class Part02 : IPart
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

        string programmCodeAsString = string.Join(",", ProgramCode);

        GlobalLog.LogLine($"aReg: {aReg}");
        GlobalLog.LogLine($"bReg: {bReg}");
        GlobalLog.LogLine($"cReg: {cReg}");
        GlobalLog.LogLine($"ProgramCode: {ProgramCode.Length}");

        bool innerLoop = true;
        ulong aTemp = 0;
        var bTemp = bReg;
        var cTemp = cReg;

        ulong fixedBits = 0;
        int fixedBitsLength = 0;
        ulong index = 0;
        int foundOuterlist = 2;
        bool firstFound = false;

        while (true)
        {
            aReg = (long)aTemp;
            bReg = bTemp;
            cReg = cTemp;

            if (aTemp / 8 % 8 != 5) innerLoop = false;

            while (innerLoop)
            {
                if (Pointer >= ProgramCode.Length)
                {
                    innerLoop = false;
                    break;
                }
                if (OutList.Count > 0)
                {

                    for (int i = 0; i < OutList.Count; i++)
                    {
                        if (OutList[i] != ProgramCode[i])
                        {
                            if (i > foundOuterlist)
                            {


                                GlobalLog.LogLine($"-----------I; {i} --------------");
                                int bitLength = 64 - BitOperations.LeadingZeroCount(aTemp);
                                int deleteBits = 7;

                                ulong mask = (1UL << bitLength - deleteBits) - 1;
                                fixedBits = aTemp & mask;

                                fixedBitsLength = 64 - BitOperations.LeadingZeroCount(fixedBits);
                                GlobalLog.LogLine($"fixedBitsLength; {fixedBitsLength}");

                                index = 0;
                                foundOuterlist += 1;
                                firstFound = true;

                                GlobalLog.LogLine($"First Match: {aTemp}");
                                GlobalLog.LogLine($"In Bits: {Convert.ToString((long)aTemp, 2)}");
                                GlobalLog.LogLine($".Mod8: {aTemp % 8}");
                                GlobalLog.LogLine($".div8: {aTemp / 8}");
                                GlobalLog.LogLine($".div8.Mod8: {aTemp / 8 % 8}");

                            }
                            innerLoop = false;
                            break;
                        }


                    }
                    if (!innerLoop) break;
                }
                if (!innerLoop) break;
                OperationPicker(ProgramCode[Pointer], ProgramCode[Pointer + 1]);
                Pointer += 2;
            }
            if (OutList.Count > 0 && programmCodeAsString == string.Join(',', OutList))
            {
                break;
            }
            OutList.Clear();

            innerLoop = true;
            Pointer = 0;

            index += 1;
            if (firstFound)
            {
                aTemp = index << fixedBitsLength | fixedBits;
                //GlobalLog.LogLine($"In Bits: {Convert.ToString((long)aTemp, 2)}");

            }
            else
            {
                aTemp = index;
            }

            //aTemp = index;


        }


        return aTemp.ToString();
    }

    private long ComboOperandPicker(long progValue)
    {
        if (progValue < 4) return progValue;
        if (progValue == 4) return aReg;
        if (progValue == 5) return bReg;
        if (progValue == 6) return cReg;
        if (progValue == 7) return 7;

        throw new Exception();
    }


    private void OperationPicker(long opcode, long literalOperand)
    {
        long combo = ComboOperandPicker(literalOperand);

        if (opcode == 0) AdvOperation(combo);
        if (opcode == 1) BxlOperation(literalOperand);
        if (opcode == 2) BstOperation(combo);
        if (opcode == 3) JnzOperation(literalOperand);
        if (opcode == 4) BxcOperation(combo);
        if (opcode == 5) OutOperation(combo);
        if (opcode == 6) BdvOperation(combo);
        if (opcode == 7) CdvOperation(combo);
    }
    private void AdvOperation(long combo)
    {
        long denominator = 1 << (int)combo;
        aReg = aReg / denominator;
    }
    private void BxlOperation(long literal)
    {
        bReg = bReg ^ literal;
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


        bReg = combo & 7;
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
        OutList.Add(combo & 7);
    }
    private void BdvOperation(long combo)
    {
        long denominator = 1 << (int)combo;
        bReg = aReg / denominator;
    }

    private void CdvOperation(long combo)
    {
        long denominator = 1 << (int)combo;
        cReg = aReg / denominator;
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