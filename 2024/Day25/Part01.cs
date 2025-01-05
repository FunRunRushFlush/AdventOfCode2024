
namespace Day25;
public class Part01:IPart
{
    private HashSet<int[]> KeyDic = new HashSet<int[]>();
    private HashSet<int[]> LockDic = new HashSet<int[]>();
    private int Counter;
    public void ParseOnly(ReadOnlySpan<string> input)
    {
        ParseInput(input);
    }

    public string Result(Input input)
    {
        ParseInput(input.Lines);

        foreach (var item in LockDic)
        {
            foreach (var key in KeyDic)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    var temp = 0;
                    temp += item[i];
                    temp += key[i];
                    if (temp > 5) break;

                    if (i == item.Length - 1) Counter++;
                }
                
            }
        }
        return Counter.ToString();
    }

    private void ParseInput(ReadOnlySpan<string> input)
    {
        for(int i=0; i<input.Length;i+=8)
        {
            var sliceInput = input.Slice(i, 7);
            if (input[i][0] == '#')
            {
                ParseLock(sliceInput);
            }
            else
            {
                ParseKey(sliceInput);
            }

        }

    }

    private void ParseLock(ReadOnlySpan<string> sliceInput)
    {
        int[] Lock = new int[5];
        for (int y = 1; y < sliceInput.Length-1; y++)
        {
            for(var x = 0; x < sliceInput[y].Length; x++)
            {
                if (sliceInput[y][x] == '#') Lock[x]++;
            }
        }
        LockDic.Add(Lock);
    }

    private void ParseKey(ReadOnlySpan<string> sliceInput)
    {
        int[] Key = new int[5];
        for (int y = 1; y < sliceInput.Length-1; y++)
        {
            for (var x = 0; x < sliceInput[y].Length; x++)
            {
                if (sliceInput[y][x] == '#') Key[x]++;
            }
        }
        KeyDic.Add(Key);
    }
}