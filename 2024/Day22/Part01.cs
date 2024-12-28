namespace Day22;
public class Part01
{
    private int MaxSteps = 2000;

    public void ParseOnly(ReadOnlySpan<int> input)
    {
        //ParseInput(input);
    }

    public long Result(ReadOnlySpan<string> input)
    {
        long solution = 0;
        //ParseInput(input);
        foreach (var item in input)
        {
            var temp = long.Parse(item);
            for (var i = 0; i < MaxSteps; i++)
            {
                temp = CalculateSecretNum(temp);
                if (i == MaxSteps - 1)
                {
                    GlobalLog.LogLine($"{temp}");
                    solution += temp;
                }
            }
        }

        return solution;
    }

    private long CalculateSecretNum(long secretNum)
    {
        secretNum = Mult(secretNum, 64);
        secretNum = Div(secretNum, 32);
        secretNum = Mult(secretNum, 2048);
        return secretNum;
    }

    private long Mix(long secretNum, long value)
    {
        return (secretNum ^ value);
    }
    private long Prune(long secretNum, long modulo = 16777216)
    {
        return (secretNum % modulo);
    }
    private long Div(long secretNum, int div)
    {
        long value = secretNum / div;
        secretNum = Mix(secretNum, value);
        secretNum = Prune(secretNum);
        return secretNum;
    }

    private long Mult(long secretNum, int mult)
    {
        long value = secretNum * mult;
        secretNum = Mix(secretNum, value);
        secretNum = Prune(secretNum);
        return secretNum;
    }


}

