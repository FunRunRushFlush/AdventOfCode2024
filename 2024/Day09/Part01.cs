
namespace Day09;
public static class Part01
{
    public static long Result(ReadOnlySpan<char> input)
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
        var checksumIndex = 0;
        long checksum = 0;
        for (int i = 0; i < lastFileIndex; i++)
        {
            if(i%2==0)
            {
                var num = CharToInt(input[i]);
                GlobalLog.LogLine($"### num: {num} x fileIndex:{fileIndex} ### ");
                for (int j = 0; j < num; j++)
                {
                    var addingChecksum = checksumIndex * fileIndex;
                    GlobalLog.LogLine($"addingChecksum: {addingChecksum} for checksumIndex:{checksumIndex} and fileIndex I:{fileIndex}");
                    checksum += addingChecksum;
                    checksumIndex++;
                }
                fileIndex++;
            }
            else
            {
                var spaceNum = CharToInt(input[i]);
                GlobalLog.LogLine($"### spaceNum: {spaceNum} x lastFileIndex/2:{lastFileIndex/2} ###");

                for (int j = 0; j < spaceNum; j++)
                {
                    while(lastFileNum == 0)
                    {
                        lastFileIndex -= 2;
                        if (lastFileIndex < i) break;
                        lastFileNum = CharToInt(input[lastFileIndex]);
                    }
                        if (lastFileIndex < i) break;
                    var addingChecksum = checksumIndex * lastFileIndex/2;
                    GlobalLog.LogLine($"addingChecksum: {addingChecksum} for checksumIndex:{checksumIndex} and lastFileIndex/2:{lastFileIndex / 2}");
                    checksum += addingChecksum;
                    checksumIndex++;
                    lastFileNum--;
                }
            }
            
        }
        GlobalLog.LogLine($"checksum: {checksum}");
        GlobalLog.LogLine($"lastFileNum: {lastFileNum}");
        if (lastFileNum != 0)
        {
            for (int j = 0; j < lastFileNum; j++)
            {
                var addingChecksum = checksumIndex * lastFileIndex/2;
                GlobalLog.LogLine($"addingChecksum: {addingChecksum} for checksumIndex:{checksumIndex} and Index I:{lastFileIndex}");
                checksum += addingChecksum;
                checksumIndex++;
            }
        }
        GlobalLog.LogLine($"checksum: {checksum}");
        return checksum;
    }

    private static int CharToInt(char c)
    {
        return (int)(c - '0');
    }

}

