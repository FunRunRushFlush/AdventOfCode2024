using System.Diagnostics;

namespace Day09;


public static class Part02
{
    public static void Result(ReadOnlySpan<char> input)
    {
        var lastFileIndex = 0;
        if ((input.Length - 1) % 2 == 0)
        {
            lastFileIndex = input.Length - 1;
        }
        else if ((input.Length - 2) % 2 == 0)
        {
            lastFileIndex = input.Length - 2;
        }
        var lastFileNum = CharToInt(input[lastFileIndex]);
        var fileIndex = 0;
        var spaceIndex = 0;
        var checksumIndex = 0;
        long checksum = 0;
        for (int i = lastFileIndex; i > 0; i--)
        {
                var num = CharToInt(input[i]);
           

        }
        

        GlobalLog.Log($"checksum: {checksum}");
    }

    private static int CharToInt(char c)
    {
        return (int)(c - '0');
    }
}
