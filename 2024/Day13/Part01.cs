using System;

namespace Day13;
public static class Part01
{
    private static List<ClawInstruction> _instructions;


    public static long Result(ReadOnlySpan<string> input)
    {


        long A_ButtonCounter = 0;
        long B_ButtonCounter = 0;
        long specialCase = 0;
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
        var token = (A_ButtonCounter*3)+B_ButtonCounter;
        GlobalLog.LogLine($"token: {token}");
        return token;
    }

    //private static (int APresses, int BPresses) CalculateButtonPresses(ClawInstruction ins)
    //{
    //    int limitButtonPress = 100;


    //    for (var a = 0; a < limitButtonPress+1; a++)
    //    {
            
    //        for (var b = 0; b < limitButtonPress+1; b++)
    //        {
    //            if(ins.Price.Y == (ins.A.Y * a +ins.B.Y*b) 
    //                && ins.Price.X == (ins.A.X * a + ins.B.X * b))
    //            {
    //                GlobalLog.LogLine($"Found Combi = A:{a} B:{b}");
    //                return (a, b);
    //            }
    //        }
    //    }
    //    return (0, 0);
    //}




    private static (int APresses, int BPresses) CalculateButtonPresses(ClawInstruction ins)
    {
        double Z_y = ins.Price.Y;double Z_x = ins.Price.X;
        double A_y = ins.A.Y; double A_x = ins.A.X;
        double B_y = ins.B.Y; double B_x = ins.B.X;

        double b = (( Z_x - (Z_y *A_x  / A_y))
                    /
                    (B_x - (B_y*A_x/A_y)));
        double a = (Z_y - b * B_y) / A_y;

        const double epsilon = 1e-3; 

     
        if (Math.Abs(a - Math.Round(a)) > epsilon || Math.Abs(b - Math.Round(b)) > epsilon)
        {
            return (0, 0);
        }
        a = Math.Round(a);
        b = Math.Round(b);
        if (a > 100 || b > 100)
        {
            return (0, 0);
        }

        return ((int)(a), (int)b);
    }
    private static (bool IsPos, int EdgeCase) CheckIfPriceIsPossible(ClawInstruction ins)
    {
        
        var priceAngle = Math.Atan((double)ins.Price.Y / (double)ins.Price.X);
        var aAngle = Math.Atan((double)ins.A.Y / (double)ins.A.X);
        var bAngle = Math.Atan((double)ins.B.Y / (double)ins.B.X);

        const double epsilon = 1e-5; 
        
        if (Math.Abs(priceAngle - aAngle) < epsilon || Math.Abs(priceAngle - bAngle) < epsilon)
        {
            return (true, 1);
        }
        else if (priceAngle < aAngle && priceAngle > bAngle)
        {
            return (true, 0);
        }
        else if (priceAngle > aAngle && priceAngle < bAngle)
        {
            return (true, 0);
        }
        else
        {
            return (false, 0);
        }


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

            GlobalLog.LogLine($"ParseLine: prize {prize.Y} ,{prize.X}");
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

    private static (int Y, int X) ParseFixedPrize(string input)
    {

        var cleanInput = input.Substring(9);
        var parts = cleanInput.Split(new[] { ',', '=' }, StringSplitOptions.RemoveEmptyEntries);


        return (int.Parse(parts[^1]), int.Parse(parts[0]));
    }



    private struct ClawInstruction
    {
        public (int Y, int X) Price { get; set; }
        public (int Y, int X) A { get; set; }
        public (int Y, int X) B { get; set; }
        public ClawInstruction((int Y, int X) price, (int Y, int X) a, (int Y, int X) b)
        {
            Price = price;
            A = a;
            B = b;
        }
    }


}

