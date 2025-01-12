namespace Year_2024.Days.Day09;
public class Part01 : IPart
{
    public string Result(Input input)
    {
        var lastFileIndex = 0;
        if ((input.Text.Length - 1) % 2 == 0)
        {
            lastFileIndex = input.Text.Length - 1;
        }
        else if ((input.Text.Length - 2) % 2 == 0)
        {
            lastFileIndex = input.Text.Length - 2;
        }
        var lastFileNum = CharToInt(input.Text[lastFileIndex]);
        var fileIndex = 0;
        var checksumIndex = 0;
        long checksum = 0;
        for (int i = 0; i < lastFileIndex; i++)
        {
            if (i % 2 == 0)
            {
                var num = CharToInt(input.Text[i]);
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
                var spaceNum = CharToInt(input.Text[i]);
                GlobalLog.LogLine($"### spaceNum: {spaceNum} x lastFileIndex/2:{lastFileIndex / 2} ###");

                for (int j = 0; j < spaceNum; j++)
                {
                    while (lastFileNum == 0)
                    {
                        lastFileIndex -= 2;
                        if (lastFileIndex < i) break;
                        lastFileNum = CharToInt(input.Text[lastFileIndex]);
                    }
                    if (lastFileIndex < i) break;
                    var addingChecksum = checksumIndex * lastFileIndex / 2;
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
                var addingChecksum = checksumIndex * lastFileIndex / 2;
                GlobalLog.LogLine($"addingChecksum: {addingChecksum} for checksumIndex:{checksumIndex} and Index I:{lastFileIndex}");
                checksum += addingChecksum;
                checksumIndex++;
            }
        }
        GlobalLog.LogLine($"checksum: {checksum}");
        return checksum.ToString();
    }

    private int CharToInt(char c)
    {
        return c - '0';
    }

}

