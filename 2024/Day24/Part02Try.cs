
namespace Day24;
public class Part02Try
{
    private Dictionary<string, int> Rule = new Dictionary<string, int>();
    private Dictionary<string, int> RuleBackUp = new Dictionary<string, int>();
    private Dictionary<string, int> OriginalRule = new Dictionary<string, int>();
    private HashSet<string> BlackList = new HashSet<string>();
    private HashSet<string> WhiteList = new HashSet<string>();


    HashSet<string> AlreadyPicked = new();

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
        SettingBlackList();
        AllowedValues();

        for (int i = 0; i < Rule.Count; i++)
        {
            string xkey = $"x{i:D2}";
            string ykey = $"y{i:D2}";

            if (!Rule.TryGetValue(xkey, out int output)) break;

            if (Rule[ykey] + Rule[xkey] + MusterLoesung[i] >= 2)
            {
                MusterLoesung[i + 1] = 1;
                if (Rule[ykey] + Rule[xkey] + MusterLoesung[i] == 2)
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
                if (!Rule.TryGetValue(key, out int output))
                    //break;

                if (MusterLoesung[i] != output)
                {
                    if (i > bestNumOfMatches)
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
                for (int i = 0; i < SwapedGatesList.Count; i++)
                {
                    GlobalLog.LogLine($"    {SwapedGatesList[i]}");
                }

            }

            if (sim == 0)
            {
                OriginalRule = new Dictionary<string, int>(Rule);
            }
            if (sim % 100000 == 0)
            {
                GlobalLog.LogLine($"-------Sim {sim}-------------: ");
                GlobalLog.LogLine("Swapped Gates: ");
                for (int i = 0; i < SwapedGatesList.Count; i++)
                {
                    GlobalLog.LogLine($"    {SwapedGatesList[i]}");
                }
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

        var enTest = WhiteList.ToArray();
        var limit = enTest.Length;


        Random random = new Random();

        while (true)
        {
            if (localSwapedGates.Count >= 4)
            {
                if (AlreadyPicked.Add(string.Join("", localSwapedGatesList)))
                {
                    break;
                }
                else
                {
                    localSwapedGates.Clear();
                    localSwapedGatesList.Clear();
                }
            }
            
            var rng01 = random.Next(limit);
            var rng02 = random.Next(limit);
            string val1 = enTest[rng01];
            string val2 = enTest[rng02];
            if (rng01 == rng02) continue;
            string temp01 = gates.FirstOrDefault(x => x.OutputVar01 == val1)?.OutputVar01;
            string temp02 = gates.FirstOrDefault(x => x.OutputVar01 == val2)?.OutputVar01;
            if (temp01 == null || temp02 == null) continue;

            if (localSwapedGates.Contains(temp01)
                || localSwapedGates.Contains(temp02)) continue;

            if (BlackList.Contains(temp01)
            || BlackList.Contains(temp02)) continue;


            gates.FirstOrDefault(x => x.OutputVar01 == val1)?.ChangeOutputVar01(temp02);
            gates.FirstOrDefault(x => x.OutputVar01 == val2)?.ChangeOutputVar01(temp01);


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

    private void SettingBlackList()
    {
        string input = """
            fht
            dmk
            ntf
            nrs
            qdt
            pbb
            mkc
            bcm
            cjs
            dbv
            qlh
            qqv
            bvb
            jtk
            nqr
            bwb
            jgf
            pvt
            bdc
            smt
            chv
            mqt
            cjt
            sfm
            fmd
            jmq
            mms
            qsf
            cvm
            sbj
            knp
            swm
            wjb
            cgv
            hjc
            kjv
            dfj
            jkn
            whc
            hct
            hmc
            tsh
            wbt
            mfr
            dwn
            kng
            jqh
            fcd
            bnt
            kwb
            nmm
            fjv
            fgr
            gmh
            gbd
            cqb
            ckd
            hch
            dmm
            msb
            bhs
            cvg
            gts
            vmr
            mwp
            ptj
            srh
            fvk
            rjj
            kkg
            nwg
            qdp
            tfn
            qgq
            jgt
            rtt
            csj
            jgk
            rvk
            nsb
            vfv
            qpp
            ptw
            cpb
            pjk
            qpm
            gvt
            bbv
            rbf
            pww
            rkw
            rqf
            kdt
            cbd
            mrj
            tmh
            bjr
            bch
            cjf
            jsp
            pnh
            fjm
            ftc
            bkq
            fhk
            ghp
            mtn
            gnj
            bcv
            wdw
            hrf
            wtk
            thb
            nbk
            knk
            hrh
            chp
            cbh
            bgc
            qqd
            hfb
            wtb
            qmd
            wqn
            tmm
            wwt
            rjp
            bkk
            hvf
            qtd
            z00
            z01
            z02
            z03
            z04
            z05
            z06
            z07
            z08
            z09
            z10
            z11
            z12
            z13
            z14
            z15
            z16
            z17
            z18
            z19
            z20
            z21
            z22
            z23
            z24
            z25
            z26
            z27
            z28
            z29
            z30
            z31
            z32
            z33
            z34
            z35
            """;

        var test = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (string inp in test)
        {
            BlackList.Add(inp);
        }
    }

    private void AllowedValues()
    {

        string whiteList = """
        gts
        z00
        rrt
        tmh
        sfm
        wvm
        smt
        mvf
        nmh
        msb
        kqm
        wqn
        gvt
        rqf
        ndr
        qqj
        jtk
        wgw
        vmr
        tfn
        hjc
        qmn
        kjv
        pbb
        ptw
        wdq
        swm
        wtb
        sbj
        cgj
        cqb
        hch
        hnr
        fjv
        fcd
        hdk
        mpv
        nvr
        mfr
        bbb
        rjp
        nbk
        gqg
        hvf
        wrj
        kkg
        stn
        ghp
        dvj
        hrh
        fhk
        dbv
        pww
        hct
        rds
        thb
        bwb
        ftc
        qkp
        qsw
        njc
        vfv
        nqp
        qtd
        jsp
        pvt
        nmm
        cpg
        qkh
        tsh
        fgr
        ptj
        jmq
        cbh
        qdp
        mms
        qdt
        bvb
        cbd
        qqd
        gnj
        dmk
        fvk
        jkn
        fns
        bch
        rvk
        mqt
        kng
        fht
        ntf
        z01
        nrs
        z02
        mkc
        bcm
        z03
        qqv
        nqr
        z04
        jgf
        bdc
        z05
        chv
        cjt
        fmd
        z06
        qsf
        cvm
        z07
        knp
        z08
        wjb
        cgv
        z09
        dfj
        whc
        z10
        hmc
        wbt
        z11
        dwn
        jqh
        bnt
        kwb
        gmh
        z12
        z14
        gbd
        ckd
        z13
        dmm
        bhs
        z15
        cvg
        mwp
        srh
        z16
        z17
        rjj
        nwg
        jpt
        qgq
        z19
        z18
        jgt
        cjs
        z20
        nsb
        jgk
        z21
        qpp
        pjk
        z22
        cpb
        qpm
        z23
        rbf
        ssw
        rkw
        kdt
        mrj
        bjr
        z24
        z25
        z26
        cjf
        pnh
        fjm
        bkq
        z27
        z28
        mtn
        bcv
        hrf
        z29
        wtk
        z30
        knk
        chp
        bgc
        z31
        hfb
        z32
        qmd
        tmm
        wwt
        bkk
        gqf
        z33
        z34
        njp
        kqf
        z35
        ggh
        svm
        z36
        nsf
        z37
        jrg
        ngk
        qrh
        z38
        wtp
        kgw
        z39
        bjg
        djv
        qvh
        z41
        z40
        mmm
        rmc
        vwp
        pqg
        z42
        fcb
        z43
        jbp
        z44
        dhs
        z45
        """;


        var test = whiteList.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (string inp in test)
        {
            if (!BlackList.Contains(inp))
            {
                WhiteList.Add(inp);

            }
        }

    }
}

