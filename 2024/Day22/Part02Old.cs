namespace Day22;
public class Part02Old :IPart
{
    //TODO: Dic von array funktioniert nicht wie ich gedacht habe, Ref- vs valuetype !!!
    //    int[] array1 = { 0, 0, 0, -1 };
    //    int[] array2 = { 0, 0, 0, -1 };

    //    Console.WriteLine(array1 == array2); // False (Referenzvergleich)
    //  Console.WriteLine(array1.SequenceEqual(array2)); // True (Inhaltsvergleich)

    private Dictionary<string, int> Sequence=new Dictionary<string,int>();
    private int MaxSteps = 2000;

    public void ParseOnly(ReadOnlySpan<int> input)
    {
        //ParseInput(input);
    }
    public string Result(Input input)
    {
        long solution = 0;
        //ParseInput(input);
        foreach (var item in input.Lines)
        {
            HashSet<string> forbidenSequence = new();
            Queue<int> sequence = new Queue<int>();
            var temp = long.Parse(item);
            int newOneDigit = 0;
            int oneDigit = (int)temp%10;
            
            
            for (var i = 0; i < MaxSteps; i++)
            {

                temp = CalculateSecretNum(temp);
                newOneDigit =(int) temp % 10;
                if (sequence.Count >= 4)
                {
                    sequence.Enqueue(newOneDigit - oneDigit);
                    sequence.Dequeue();

                    var text = string.Join(',',sequence);
                    if (forbidenSequence.Add(text))
                    {
                        if(!Sequence.TryAdd(text, newOneDigit))
                        {
                            Sequence[text] += newOneDigit; 
                        }
                    }

                }
                else
                {
                    sequence.Enqueue(newOneDigit - oneDigit);
                }
                //GlobalLog.LogLine($"{temp%10}");

                oneDigit = newOneDigit;
                
            }

            
            
        }
        var banana = 0;
        foreach (var item in Sequence)
        {
            banana = Math.Max(banana, item.Value);
        }
        
        return banana.ToString();
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

