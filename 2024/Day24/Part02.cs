
namespace Day24;
public class Part02 : IPart
{
    private Dictionary<string, (GateLogic? AsOutGate,List<GateLogic> CalcNodes)> WireChain = new();
    private HashSet<string> BadNodes = new();

    public string Result(Input input)
    {
        ParseInput(input.Lines);


        foreach (var chain in WireChain)
        {
            if (chain.Key.StartsWith('y') || chain.Key.StartsWith('x')) continue;

            if (!CheckForValidChain(chain))
            {
                BadNodes.Add(chain.Key);
            }
        }

        var sortedList = BadNodes.OrderBy(x => x).ToList();

        return string.Join(',', sortedList);
    }

    private bool CheckForValidChain(KeyValuePair<string, (GateLogic? AsOutGate, List<GateLogic> CalcNodes)> chain)
    {
        bool valid = true;
        var asOutGate = chain.Value.AsOutGate.Value;
        var firstOperator = asOutGate.GateOp;
        Operator secondOperator;
        foreach (var ele in chain.Value.CalcNodes)
        {
            if (!ele.OutputVar01.StartsWith('z') && !BadNodes.Contains(ele.OutputVar01) 
                && !asOutGate.InputVar01.Contains("x00") )
            {
                secondOperator = ele.GateOp;
                return CheckForValidChainOpCombo(firstOperator, secondOperator);
            } 
        }


        return valid;
    }

    private bool CheckForValidChainOpCombo(Operator firstOperator, Operator secondOperator)
    {
        if (firstOperator == secondOperator)
        {
            return false;
        }

        if (firstOperator == Operator.Or && secondOperator == Operator.Xor)
        {
            return false;
        }
        if (secondOperator == Operator.Or && firstOperator == Operator.Xor)
        {
            return false;
        }

        return true;
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
        var gate = new GateLogic(inp[0], op, inp[2], inp[4]);
        if(!gate.CheckIfGateIsValid())
        {
            BadNodes.Add(gate.OutputVar01);
        }
        if(!gate.OutputVar01.StartsWith('z')
            || gate.InputVar01 != "y00"
            || gate.InputVar01 != "x00")
        {
            if(!WireChain.TryAdd(gate.OutputVar01, (gate, new List<GateLogic>())))
            {
                WireChain[gate.OutputVar01] = (gate, WireChain[gate.OutputVar01].CalcNodes);
            }
            if (!WireChain.TryAdd(gate.InputVar01, (null, new List<GateLogic>() { gate})))
            {
                WireChain[gate.InputVar01] = (WireChain[gate.InputVar01].AsOutGate, new List<GateLogic>(WireChain[gate.InputVar01].CalcNodes) { gate });
            }
            if (!WireChain.TryAdd(gate.InputVar02, (null, new List<GateLogic>() { gate })))
            {
                WireChain[gate.InputVar02] = (WireChain[gate.InputVar02].AsOutGate, new List<GateLogic>(WireChain[gate.InputVar02].CalcNodes) { gate });
            }
        }       
    }

    private Operator GetOperator(string operatorString)
    {
        return operatorString switch
        {
            "AND" => Operator.And,
            "XOR" => Operator.Xor,
            "OR" => Operator.Or,
            _ => Operator.None
        };
    }

    private struct GateLogic
    {
        public string InputVar01;
        public Operator GateOp;
        public string InputVar02;
        public string OutputVar01;
        public int? Value01;

        public GateLogic(string inputVar01, Operator oper, string inputVar02, string outputVar01, int? value01 = null)
        {
            InputVar01 = inputVar01;
            GateOp = oper;
            InputVar02 = inputVar02;
            OutputVar01 = outputVar01;
            Value01 = value01;
        }

        public bool CheckIfGateIsValid()
        { 
            bool valid = true;
            if(InputVar01.StartsWith('y') || InputVar01.StartsWith('x'))
            {
                if(GateOp == Operator.Or) valid = false;

                if (GateOp != Operator.Xor && OutputVar01.StartsWith('z')) valid = false;

            }
            else if (OutputVar01.StartsWith('z') )
            {                
                if (GateOp != Operator.Xor && OutputVar01 != "z45") //edgecase
                    valid = false;
            }
            if (!(InputVar01.StartsWith('x') || InputVar01.StartsWith('y')
               || InputVar02.StartsWith('x') || InputVar02.StartsWith('y')))
            {
                if (GateOp == Operator.Xor && !OutputVar01.StartsWith('z')) 
                    valid = false;
            }
            return valid;
        }
    }


    private enum Operator
    {
        None,
        And,
        Or,
        Xor
    }
}

