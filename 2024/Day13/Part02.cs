namespace Day13;

public static class Part02
{

    private static List<ClawInstruction> _instructions;


    public static long Result(ReadOnlySpan<string> input)
    {
        long A_ButtonCounter = 0;
        long B_ButtonCounter = 0;

        _instructions = ParseInputToClawInstructions(input);
        GlobalLog.LogLine($"instructionsNumbers: {_instructions.Count}");
        foreach (var instruction in _instructions)
        {
            GlobalLog.LogLine($"instruction: {instruction}");

            var buttons = CalculateButtonPresses(instruction);

            A_ButtonCounter += buttons.APresses;
            B_ButtonCounter += buttons.BPresses;

        }

        GlobalLog.LogLine($"A_ButtonCounter: {A_ButtonCounter}");
        GlobalLog.LogLine($"B_ButtonCounter: {B_ButtonCounter}");
        var token = (A_ButtonCounter * 3) + B_ButtonCounter;
        GlobalLog.LogLine($"token: {token}");
        return token;
    }




    private static (long APresses, long BPresses) CalculateButtonPresses(ClawInstruction ins)
    {
        decimal Z_y = ins.Price.Y; decimal Z_x = ins.Price.X;
        decimal A_y = ins.A.Y; decimal A_x = ins.A.X;
        decimal B_y = ins.B.Y; decimal B_x = ins.B.X;



        decimal b = ((Z_x * A_y - (Z_y * A_x))
                    /
                    (B_x * A_y - (B_y * A_x)));
        decimal a = (Z_y - b * B_y) / A_y;


        const decimal epsilon =(decimal)0.0001;


        if (Math.Abs(a - Math.Round(a)) > epsilon || Math.Abs(b - Math.Round(b)) > epsilon)
        {
            return (0, 0);
        }

        a = Math.Round(a);
        b = Math.Round(b);

        return ((long)(a), (long)b);
    }
 

    private static List<ClawInstruction> ParseInputToClawInstructions(ReadOnlySpan<string> input)
    {
        var instructions = new List<ClawInstruction>();

        for (var i = 0; i < input.Length; i += 4)
        {
            GlobalLog.LogLine($"ParseLine: {i}");
            var a = ParseFixedCoordinates(input[i]);
            var b = ParseFixedCoordinates(input[i + 1]);
            var prize = ParseFixedPrize(input[i + 2]);

            GlobalLog.LogLine($"ParseLine: prize {prize.X} ,{prize.Y}");
            instructions.Add(new ClawInstruction(prize, a, b));
        }

        return instructions;
    }

    private static (int Y, int X) ParseFixedCoordinates(string input)
    {

        var cleanInput = input.Substring(12);
        var parts = cleanInput.Split(new[] { ',', '+' }, StringSplitOptions.RemoveEmptyEntries);


        return (int.Parse(parts[^1]), int.Parse(parts[0]));
    }

    private static (long Y, long X) ParseFixedPrize(string input)
    {
        long CostNum = 10000000000000;

        var cleanInput = input.Substring(9);
        var parts = cleanInput.Split(new[] { ',', '=' }, StringSplitOptions.RemoveEmptyEntries);

        long Y = long.Parse(parts[^1]) + CostNum;
        long X = long.Parse(parts[0]) + CostNum;
        return (Y, X);
    }



    private struct ClawInstruction
    {
        public (long Y, long X) Price { get; set; }
        public (int Y, int X) A { get; set; }
        public (int Y, int X) B { get; set; }
        public ClawInstruction((long Y, long X) price, (int Y, int X) a, (int Y, int X) b)
        {
            Price = price;
            A = a;
            B = b;
        }
    }
}