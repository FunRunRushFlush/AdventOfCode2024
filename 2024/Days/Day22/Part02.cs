using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
namespace Year_2024.Days.Day22;
public class Part02 : IPart
{
    //TODO: Dic von array funktioniert nicht wie ich gedacht habe, Ref- vs valuetype !!!
    //    int[] array1 = { 0, 0, 0, -1 };
    //    int[] array2 = { 0, 0, 0, -1 };

    //    Console.WriteLine(array1 == array2); // False (Referenzvergleich)
    //  Console.WriteLine(array1.SequenceEqual(array2)); // True (Inhaltsvergleich)


    private Dictionary<int, int> Sequence = new();
    private int MaxSteps = 2000;

    public string Result(Input input)
    {
        var banana = 0;
        HashSet<int> forbidenSequence = new();
        Queue<int> sequence = new Queue<int>();
        foreach (var item in input.Lines)
        {
            forbidenSequence.Clear();
            sequence.Clear();
            var temp = long.Parse(item);
            int newOneDigit = 0;
            int oneDigit = (int)temp % 10;

            for (var i = 0; i < MaxSteps; i++)
            {
                temp = CalculateSecretNum(temp);
                newOneDigit = (int)temp % 10;
                if (sequence.Count >= 4)
                {
                    sequence.Enqueue(newOneDigit - oneDigit);
                    sequence.Dequeue();

                    //var text = string.Join(',', sequence);
                    int hash = GenerateHashFromQueue(sequence);

                    if (forbidenSequence.Add(hash))
                    {
                        if (!Sequence.TryAdd(hash, newOneDigit))
                        {
                            Sequence[hash] += newOneDigit;
                            banana = Math.Max(banana, Sequence[hash]);
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

        return banana.ToString();
    }


    //TODO: irgendwie keine andere Art gefundem, wie ich den Inhalt einer Queue simple hashen kann --> ohne extra einen string zu erstellen
    int GenerateHashFromQueue(Queue<int> queue)
    {
        int hash = 17; // Startwert für den Hash
        int basePrime = 31; // Basis (Primzahl) für die Gewichtung

        foreach (var item in queue)
        {
            hash = hash * basePrime + item.GetHashCode();
        }

        return hash;
    }



    private long CalculateSecretNum(long secretNum)
    {

        secretNum = Mult(secretNum, 6);//64 = 2^6 => 6 für bitshift
        secretNum = Div(secretNum, 5); //32 = 2^5 => 5 für bitshift
        secretNum = Mult(secretNum, 11);//2048 = 2^11 => 11 für bitshift

        return secretNum;
    }


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

