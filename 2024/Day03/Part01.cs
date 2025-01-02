

using System.Text.RegularExpressions;

namespace Day03;

public class Part01 : IPart
{
    public string Result(Input input)
    {
        // https://learn.microsoft.com/de-de/dotnet/api/system.text.regularexpressions.regex.match?view=net-8.0
        string pattern = @"mul\(\d{1,3},\d{1,3}\)";
        Regex reg = new Regex(pattern);
        int result = 0;
        foreach(Match match in reg.Matches(input.Text))
        {
            var value = match.Value.Split(new[] { '(', ')', ',', }, StringSplitOptions.RemoveEmptyEntries);

            int num1 = int.Parse(value[1]);
            int num2 = int.Parse(value[2]);

            int multRes = num1 * num2;
            result += multRes;

        }
        //Console.WriteLine(result);
        return $"{ result}";
    }


    public int Result_Improved(ReadOnlySpan<char> input)
    {
        bool[] mul = new bool[4];
        char[] num1 = new char[3];
        char[] num2 = new char[3];
        ushort num1Length = 0;
        ushort num2Length = 0;

        bool seperator = false;
        int result = 0;
        for (int i=0; i<input.Length; i++)
        {
            char current = input[i];


            if (!mul.Contains(false))
            {
                if (CheckIfCharIsNum(current))
                {
                    if (!seperator && num1Length < num1.Length)
                    {
                        num1[num1Length++] = current;
                    }
                    else if (seperator && num2Length < num2.Length)
                    {
                        num2[num2Length++] = current;
                    }
                    else
                    {
                        seperator = false;

                        num1Length = 0;
                        num2Length = 0;
                        mul = [false, false, false, false];
                    }
                }
                else if(current == ',' && num1.Length > 0)
                {
                    seperator = true;
                }
                else if((num1Length > 0|| num1Length < 4) && (num2Length > 0|| num2Length < 4) && seperator && current== ')')
                {
                    result += ParseNumber(num1,num1Length) * ParseNumber(num2, num2Length);
                    seperator = false;

                    num1Length = 0;
                    num2Length = 0;
                    mul = [false, false, false, false];
                }
                else
                {
                    seperator = false;

                    num1Length = 0;
                    num2Length = 0;
                    mul = [false, false, false, false];
                }

                continue;
            }

            if(current == 'm')
            {
                mul[0]=true;
                
            }
            else if(current == 'u' && mul[0])
            {
                mul[1]=true;
            }
            else if (current == 'l' && mul[0] && mul[1])
            {
                mul[2] = true;
            }
            else if (current == '(' && mul[0] && mul[1] && mul[2])
            {
                mul[3] = true;
            }
            else
            {
                mul = [ false, false, false, false];
            }

        }

        return result;
    }

private bool CheckIfCharIsNum(char input) => ((input - '0') >= 0 && (input - '0') <= 9)? true : false;


    // Simpler IntParser der perfomanter ist als Int.Parse()
    // WARNUNG: Aufkosten von Robustheit(kein edgecases etc...)
    // https://youtu.be/EWmufbVF2A4?feature=shared&t=880 
    private int IntParser(ReadOnlySpan<char> span)
    {
        int temp = 0;
        for (int i = 0; i < span.Length; i++)
        {
            // Der ASCII-Wert des Zeichens (z. B. '3' → ASCII 51) wird von dem ASCII-Wert von '0' (ASCII 48) subtrahiert.
            // Dadurch wird der numerische Wert des Zeichens erhalten (z. B. '3' → 3).
            temp = temp * 10 + (span[i] - '0');
        }
        return temp;
    }


    public int Result_Improved_02(ReadOnlySpan<char> input)
    {
        bool[] mul = new bool[4];
        char[] num1 = new char[3];
        char[] num2 = new char[3];

        ushort num1Length = 0;
        ushort num2Length = 0;

        MulState state = MulState.None;
        bool separator = false;
        int result = 0;
        for (int i = 0; i < input.Length; i++)
        {
            char current = input[i];
            if (state != MulState.OpenParen)
            {

                switch (state)
                {
                
                case MulState.None when current == 'm':
                    state = MulState.M;
                    break;
                case MulState.M when current == 'u':
                    state = MulState.U;
                    break;
                case MulState.U when current == 'l':
                    state = MulState.L;
                    break;
                case MulState.L when current == '(':
                    state = MulState.OpenParen;
                    break;
                default:
                    if (state != MulState.OpenParen)
                    {
                        state = MulState.None;
                    }
                    break;
                }
            }
            else if (state == MulState.OpenParen)
            {
                if (char.IsDigit(current))
                {
                    if (!separator)
                    {
                        if (num1Length < 3)
                        {
                            num1[num1Length++] = current;
                        }
                    }
                    else
                    {
                        if (num2Length < 3)
                        {
                            num2[num2Length++] = current;
                        }
                    }
                }
                else if (current == ',' && num1Length > 0)
                {
                    separator = true;
                }
                else if (current == ')' && num1Length > 0 && num2Length > 0)
                {

                    result += ParseNumber(num1, num1Length) * ParseNumber(num2, num2Length);

                                        separator = false;
                    num1Length = 0;
                    num2Length = 0;
                    state = MulState.None;
                }
                else
                {

                    separator = false;
                    num1Length = 0;
                    num2Length = 0;
                    state = MulState.None;
                }
            }
        }
        return result;
    }

    [Flags]
    enum MulState
    {
        None = 0,
        M = 1,
        U = 2,
        L = 4,
        OpenParen = 8
    }

    private int ParseNumber(char[] chars, int length)
    {
        int number = 0;
        for (int i = 0; i < length; i++)
        {
            number = number * 10 + (chars[i] - '0');
        }
        return number;
    }

    public int Result_Improved_03(ReadOnlySpan<char> input)
    {

        char[] num1 = new char[3];
        char[] num2 = new char[3];
        ushort num1Length = 0;
        ushort num2Length = 0;
        MulState state = MulState.None;
        bool seperator = false;
        int result = 0;
        for (int i = 0; i < input.Length; i++)
        {
            char current = input[i];
            if (state != MulState.OpenParen)
            {

                switch (state)
                {
                    case MulState.None when current == 'm':
                        state = MulState.M;
                        break;
                    case MulState.M when current == 'u':
                        state = MulState.U;
                        break;
                    case MulState.U when current == 'l':
                        state = MulState.L;
                        break;
                    case MulState.L when current == '(':
                        state = MulState.OpenParen;
                        break;
                    default:
                        if (state != MulState.OpenParen)
                        {
                            state = MulState.None;
                        }
                        break;
                }
            }
            else if (state == MulState.OpenParen)
            {

                if (CheckIfCharIsNum(current))
                {
                    if (!seperator && num1Length < num1.Length)
                    {
                        num1[num1Length++] = current;
                    }
                    else if (seperator && num2Length < num2.Length)
                    {
                        num2[num2Length++] = current;
                    }
                    else
                    {
                        seperator = false;

                        num1Length = 0;
                        num2Length = 0;
                        state = MulState.None;
                    }
                }
                else if (current == ',' && num1.Length > 0)
                {
                    seperator = true;
                }
                else if ((num1Length > 0 || num1Length < 4) && (num2Length > 0 || num2Length < 4) && seperator && current == ')')
                {
                    result += ParseNumber(num1, num1Length) * ParseNumber(num2, num2Length);
                    seperator = false;

                    num1Length = 0;
                    num2Length = 0;
                    state = MulState.None;
                }
                else
                {
                    seperator = false;

                    num1Length = 0;
                    num2Length = 0;
                    state = MulState.None;
                }

            }
        }
            return result;
    }


}
