
namespace Day17;

public class Part02
{
    private long aReg;
    private long bReg;
    private long cReg;
    private long[] ProgramCode;
    private HashSet<string> ProgramCodeHash = new HashSet<string>();
    private long n=1_003_000_000_000_000;
                    //72_338_647_597
    private List<int> OutList = new();
    private long Pointer = 0;




    public long Result(ReadOnlySpan<string> input)
    {
        ParseInstructions(input);
        //TODO: muss ich wiederholen!!!
        string bits = "01110000010101000101101";
        ulong fixedBits = Convert.ToUInt64(bits, 2); // 23 feste Bits, die ich schon gebrutforced habe
        int fixedBitLength = bits.Length;      
        int remainingBits = 64 - fixedBitLength;
        ulong maxIterations = 1UL << remainingBits; // 2^(remainingBits)


        ProgramCodeHash.Add(string.Join(",", ProgramCode));
        ulong aTemp = fixedBits;
        var bTemp = bReg;
        var cTemp = cReg;

        bool innerLoop = true;

        ulong index = 0;
        GlobalLog.LogLine($"aReg: {aReg}");
        GlobalLog.LogLine($"bReg: {bReg}");
        GlobalLog.LogLine($"cReg: {cReg}");
        GlobalLog.LogLine($"ProgramCode: {ProgramCode.Length}");
        while (true)
        { 
            aReg = (long)aTemp;
            bReg = bTemp;
            cReg = cTemp;
            while (innerLoop)
            {
                if (Pointer >= ProgramCode.Length)
                {
                    innerLoop = false;
                    break;
                }

                if(OutList.Count > 0)
                {
                    var ind = 0;
                    for (int i = 0; i < OutList.Count; i++)
                    {
                        if (OutList[i] != ProgramCode[i])
                        {
                            if (ind > 13)
                            {
                                GlobalLog.LogLine($"-----------I; {i} --------------");

                                GlobalLog.LogLine($"First Match: {aTemp}");
                                GlobalLog.LogLine($"In Bits: {Convert.ToString((long)aTemp, 2)}"); 
                                GlobalLog.LogLine($".Mod8: {aTemp%8}");
                                GlobalLog.LogLine($".div8: {aTemp /8}");
                                GlobalLog.LogLine($".div8.Mod8: {(aTemp / 8)%8}");

                            }
                            innerLoop= false;
                            break;
                        }

                        ind = i;
                    }
                    if (!innerLoop) break;
                }
                if(!innerLoop) break;
                OperationPicker(ProgramCode[Pointer], ProgramCode[Pointer + 1]);
                Pointer += 2;
            }

            if (ProgramCodeHash.Contains(string.Join(',', OutList)))
            {
                break;
            }
            OutList.Clear();
            
            innerLoop = true;
            Pointer = 0;

            index += 1;
            //TODO: Muss ich umbedingt wiederholen!!!
            aTemp = (index << fixedBitLength) | fixedBits;
            //Console.WriteLine(Convert.ToString((long)aTemp, 2).PadLeft(64, '0'));

            //GlobalLog.LogLine($"aReg: {aReg}");
        }

        GlobalLog.LogLine($"bReg: {bReg}");
        GlobalLog.LogLine($"cReg: {cReg}");
        GlobalLog.LogLine($"ProgramCode: {ProgramCode.Length}");



        return (long)aTemp;
    }

    private long ComboOperandPicker(long progValue)
    {
        if (progValue < 4) return (long)progValue;
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
        //denominator = (long)Math.Pow(2, combo);
        aReg = (long)(aReg / denominator);
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
        OutList.Add((int)(combo % 8));

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