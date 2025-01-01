
namespace Day24;
public class Part02Try
{
    private Dictionary<string, int> Rule = new Dictionary<string, int>();
    private Dictionary<string, int> RuleBackUp = new Dictionary<string, int>();
    private Dictionary<string, int> OriginalRule = new Dictionary<string, int>();
    private HashSet<string> UniqueSortedPairs = new HashSet<string>();

    private List<GateLogic> Gates = new List<GateLogic>();
    private List<GateLogic> BackUp = new List<GateLogic>();
    private HashSet<string> SwapedGates = new HashSet<string>();
    private List<string> SwapedGatesList = new List<string>();


    private int[] MusterLoesung = new int[64];


    private int StartIndex;
    public void ParseOnly(ReadOnlySpan<string> input)
    {
        ParseInput(input);
    }

    public long Result(ReadOnlySpan<string> input)
    {
        ParseInput(input);

        for (int i = 0; i < Rule.Count; i++)
        {
            string xkey = $"x{i:D2}";
            string ykey = $"y{i:D2}";

            if (!Rule.TryGetValue(xkey, out int output)) break;

            if (Rule[ykey] + Rule[xkey] + MusterLoesung[i] >= 2)
            {
                MusterLoesung[i+1] = 1;
                if(Rule[ykey] + Rule[xkey] + MusterLoesung[i] == 2)
                {
                    MusterLoesung[i] = 0;
                }
                else
                {
                    MusterLoesung[i] = 1;
                }
            }
            else
            {
                MusterLoesung[i] = Rule[ykey] + Rule[xkey] + MusterLoesung[i];
            }

        }
        BackUp = new List<GateLogic>(Gates);
        RuleBackUp = new Dictionary<string, int>(Rule);
        int sim = 0;
        int[] bitArray = new int[64];

        int bestNumOfMatches = 0;
        while (sim < 1_000_000_00)
        {
            Gates = new List<GateLogic>(BackUp);
            Rule = new Dictionary<string, int>(RuleBackUp);
            
            if (sim > 0)
            {
                SwapedGates.Clear();
                SwapedGatesList.Clear();
                Gates = SwapGates(Gates);
            }
            
            while (true)
            {
                bool etwasBerechnet = false;

                for (int i = Gates.Count - 1; i >= 0; i--)
                {
                    var gate = Gates[i];
                    if (Rule.ContainsKey(gate.InputVar01) && Rule.ContainsKey(gate.InputVar02))
                    {
                        CalculateGateLogic(gate);
                        etwasBerechnet = true;
                    }
                }

                if (!etwasBerechnet)
                {
                    
                    break;
                }

                if (Gates.Count == 0)
                {
                    
                    break;
                }
            }
            int numOfMatches = 0;
            for (int i = 0; i < MusterLoesung.Length; i++)
            {
                string key = $"z{i:D2}";
                if (!Rule.TryGetValue(key, out int output)) break;

                if (MusterLoesung[i] != output)
                {
                    //if(i>bestNumOfMatches) 
                        GlobalLog.LogLine($"{key} --> {MusterLoesung[i]} != {output}");
                    //break;
                }

                if (MusterLoesung[i] == output) numOfMatches++;
            }

            if (bestNumOfMatches < numOfMatches)
            {
                GlobalLog.LogLine($"Num of Matches: {numOfMatches} ");

                bestNumOfMatches = numOfMatches;
                GlobalLog.LogLine("Swapped Gates: ");
                for (int i = 0;i < SwapedGatesList.Count;i++)
                {
                    GlobalLog.LogLine($"    {SwapedGatesList[i]}");          
                }

            }

            if (sim == 0)
            {
                OriginalRule = new Dictionary<string, int>(Rule);
            }

            sim++;
        }
        //return ConvertBitArrayToInt_String(bitArray);
        return bestNumOfMatches;

    }

  
    

    private List<GateLogic> SwapGates(List<GateLogic> gates)
    {
        var swappedGates = new List<GateLogic>();
        HashSet<string> localSwapedGates = new HashSet<string>();
        List<string> localSwapedGatesList = new List<string>();
        int limit = gates.Count;


        Random random = new Random();



        while (true)
        {
            if (localSwapedGates.Count >= 8) break;

            var rng01 = random.Next(limit);
            var rng02 = random.Next(limit);
            string temp01 = gates[rng01].OutputVar01;
            string temp02 = gates[rng02].OutputVar01;
            if (localSwapedGates.Contains(temp01)
                || localSwapedGates.Contains(temp02)) continue;


            gates[rng01].ChangeOutputVar01(temp02);
            gates[rng02].ChangeOutputVar01(temp01);


            var (first, second) = temp01.CompareTo(temp02) <= 0
                ? (temp02, temp01)
                : (temp01, temp02);

            string swapPair = $"{first}-{second}";

            localSwapedGates.Add(temp01);
            localSwapedGates.Add(temp02);

            localSwapedGatesList.Add(swapPair);

        }

        SwapedGates = localSwapedGates;
        SwapedGatesList = localSwapedGatesList;
        swappedGates = new List<GateLogic>(gates);
        return swappedGates;

    }

    private long ConvertBitArrayToInt_String(int[] bits)
    {
        string bitString = string.Join("", bits.Reverse());
        // bits.Reverse(), damit bits[0] das am wenigsten signifikante Bit ist
        return Convert.ToInt64(bitString, 2);
    }
    private void CalculateGateLogic(GateLogic gate)
    {
        var solution = 0;
        if (gate.Operator == Operator.And)
        {
            if (Rule[gate.InputVar01] + Rule[gate.InputVar02] == 2) solution = 1;

        }
        if (gate.Operator == Operator.Xor)
        {
            if (Rule[gate.InputVar01] != Rule[gate.InputVar02]) solution = 1;
        }
        if (gate.Operator == Operator.Or)
        {
            if (Rule[gate.InputVar01] == 1 || Rule[gate.InputVar02] == 1) solution = 1;
        }

        Rule.Add(gate.OutputVar01, solution);
        Gates.Remove(gate);
    }

    private void ParseInput(ReadOnlySpan<string> input)
    {
        bool rules = true;
        for (int i = 0; i < input.Length; i++)
        {
            if (rules)
            {
                if (string.IsNullOrEmpty(input[i]))
                {
                    rules = false;
                    continue;
                }
                RuleExtractor(input[i]);
            }
            else
            {
                ExtractGateLogic(input[i]);
            }
        }
    }

    private void ExtractGateLogic(string line)
    {
        var inp = line.Split(' ');
        var op = GetOperator(inp[1]);
        Gates.Add(new GateLogic(inp[0], op, inp[2], inp[4]));
    }

    private Operator GetOperator(string operatorString)
    {
        return operatorString switch
        {
            "AND" => Operator.And,
            "XOR" => Operator.Xor,
            "OR" => Operator.Or,
            _ => throw new NotImplementedException()
        };
    }
    private void RuleExtractor(string line)
    {
        var test = line.IndexOf(':');
        Rule.Add(line.Substring(0, test), int.Parse(line.Substring(test + 1)));
    }


    private class GateLogic
    {
        public string InputVar01;
        public Operator Operator;
        public string InputVar02;
        public string OutputVar01 { get; set; }
        public int? Value01;


        public GateLogic(string inputVar01, Operator oper, string inputVar02, string outputVar01, int? value01 = null)
        {
            InputVar01 = inputVar01;
            Operator = oper;
            InputVar02 = inputVar02;
            OutputVar01 = outputVar01;
            Value01 = value01;
        }

        public void ChangeOutputVar01(string incomingString)
        {
            OutputVar01 = incomingString;
        }
    }


    private enum Operator
    {
        And,
        Or,
        Xor
    }
}

