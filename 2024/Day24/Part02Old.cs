

namespace Day24;
public class Part02Old:IPart
{
    private Dictionary<string, int> Rule = new Dictionary<string, int>();
    private Dictionary<string, int> ZAddRule = new Dictionary<string, int>();
    private Dictionary<string, int> failedZ = new Dictionary<string, int>();
    private List<GateLogic> Gates = new List<GateLogic>();
    private List<GateLogic> GatesBackUp = new List<GateLogic>();




    public string Result(Input input)
    {
        ParseInput(input.Lines);

        CalculateSupposedZBit();
        bool loop = true;
        while (loop)
        {
            for (int i = Gates.Count - 1; i >= 0; i--)
            {
                var gate = Gates[i];
                if (Rule.ContainsKey(gate.InputVar01) && Rule.ContainsKey(gate.InputVar02))
                {
                    CalculateGateLogic(gate);
                }
            }
            if (Gates.Count == 0) break;
        }
        //CalculateGateList(GatesBackUp);
        foreach(var gate in GatesBackUp)
        {

        }

        var test = GatesBackUp.Where(x => x.OutputVar01.StartsWith('z')).ToList();

        foreach(var ele in ZAddRule)
        {
            if (ele.Value != Rule[ele.Key])
            {
                failedZ.Add(ele.Key,ele.Value);
            }
        }

        int[] bitArray = new int[64];
        for (int i = 0; i < bitArray.Length; i++)
        {
            string key = $"z{i:D2}";
            if (!Rule.TryGetValue(key, out int output)) break;

            bitArray[i] = output;
        }
        return ConvertBitArrayToInt_String(bitArray).ToString();
    }

    private void CalculateGateList(List<GateLogic> gatesBackUp)
    {
        foreach (var gate in gatesBackUp)
        {

        }
    }

    private void CalculateSupposedZBit()
    {
        for (int i = 0; i < Rule.Count; i++)
        {
            string xkey = $"x{i:D2}";
            string ykey = $"y{i:D2}";
            if (!Rule.TryGetValue(xkey, out int xOut)) break;
            if (!Rule.TryGetValue(ykey, out int yOut)) break;

            if (xOut != yOut)
            {
                ZAddRule.Add($"z{i:D2}", 1);
            }
            else
            {
                ZAddRule.Add($"z{i:D2}", 0);
            }

        }
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
        gate.Value01 = solution;
        GatesBackUp.Add(gate);
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


    private struct GateLogic
    {
        public string InputVar01;
        public Operator Operator;
        public string InputVar02;
        public string OutputVar01;
        public int? Value01;


        public GateLogic(string inputVar01, Operator oper, string inputVar02, string outputVar01, int? value01 = null)
        {
            InputVar01 = inputVar01;
            Operator = oper;
            InputVar02 = inputVar02;
            OutputVar01 = outputVar01;
            Value01 = value01;
        }
    }


    private enum Operator
    {
        And,
        Or,
        Xor
    }
}

