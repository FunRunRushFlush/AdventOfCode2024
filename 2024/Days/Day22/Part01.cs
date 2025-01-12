using System.Runtime.CompilerServices;

namespace Year_2024.Days.Day22;
public class Part01 : IPart
{
    private int MaxSteps = 2000;


    public string Result(Input input)
    {
        long solution = 0;
        //ParseInput(input);
        foreach (var item in input.Lines)
        {
            var temp = long.Parse(item);
            for (var i = 0; i < MaxSteps; i++)
            {
                temp = CalculateSecretNum(temp);
            }
            solution += temp;
        }

        return solution.ToString();
    }



    private long CalculateSecretNum(long secretNum)
    {

        secretNum = Mult(secretNum, 6);//64 = 2^6 => 6 für bitshift
        secretNum = Div(secretNum, 5); //32 = 2^5 => 5 für bitshift
        secretNum = Mult(secretNum, 11);//2048 = 2^11 => 11 für bitshift

        return secretNum;
    }

    //hat fast die gleiche performance
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //private long CalculateSecretNum(long secretNum)
    //{
    //    // Mult(secretNum, 6)
    //    long temp = secretNum << 6;
    //    secretNum = (secretNum ^ temp) & 0xFFFFFF;

    //    // Div(secretNum, 5)
    //    temp = secretNum >> 5;
    //    secretNum = (secretNum ^ temp) & 0xFFFFFF;

    //    // Mult(secretNum, 11)
    //    temp = secretNum << 11;
    //    secretNum = (secretNum ^ temp) & 0xFFFFFF;

    //    return secretNum;
    //}


    private long Mix(long secretNum, long value)
    {
        return secretNum ^ value;
    }

    private long Prune(long secretNum, long modulo = 16777216)
    {
        //modulo = 16777216 => & = 16777216 -1
        return secretNum & 16777215;
    }

    private long Div(long secretNum, int div)
    {
        long value = secretNum >> div;
        secretNum = Mix(secretNum, value);
        secretNum = Prune(secretNum);
        return secretNum;
    }

    private long Mult(long secretNum, int mult)
    {
        long value = secretNum << mult;
        secretNum = Mix(secretNum, value);
        secretNum = Prune(secretNum);
        return secretNum;
    }


}

