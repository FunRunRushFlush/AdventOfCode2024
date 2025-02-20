namespace Year_2024.Days.Day09;


public class Part02 : IPart
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
        DiscSpace[] discSpace = new DiscSpace[input.Text.Length];
        Dictionary<int, char> skipIndex = new Dictionary<int, char>();
        var fileIndex = 0;
        var checksumIndex = 0;

        long checksum = 0;

        for (int j = 1; j < input.Text.Length; j += 2)
        {
            var temp = CharToInt(input.Text[j]);
            discSpace[j].SpaceLeft = temp;
            discSpace[j].Data = new int[temp];
        }

        for (int i = lastFileIndex; i > 0; i -= 2)
        {
            var num = CharToInt(input.Text[i]);
            GlobalLog.LogLine($"num = {num}; lastFileIndex = {i}");
            for (int j = 1; j < discSpace.Length; j += 2)
            {
                if (j > i) break;
                //{
                //    i = 0;
                //    break;
                //}
                var discSpaceSpaceLeft = discSpace[j].SpaceLeft;

                if (discSpaceSpaceLeft == 0) continue;

                if (discSpaceSpaceLeft >= num)
                {
                    skipIndex.Add(i, input.Text[i]);
                    var temp = discSpaceSpaceLeft - num;
                    for (int k = discSpace[j].SpaceLeft; k > temp; k--)
                    {
                        GlobalLog.LogLine($"discSpace[{j}].Data[^{k}] = {i / 2};");
                        discSpace[j].Data[^k] = i / 2;
                    }
                    discSpace[j].SpaceLeft -= num;
                    break;
                }

            }
        }

        for (int i = 0; i < input.Text.Length; i++)
        {

            if (i % 2 == 0)
            {
                if (skipIndex.ContainsKey(i) == true)
                {

                    for (int j = 0; j < CharToInt(skipIndex[i]); j++)
                    {
                        checksumIndex++;
                    }
                    fileIndex++;

                    if (checksumIndex == 10)
                    {

                    }

                    continue;
                }
                var num = CharToInt(input.Text[i]);
                GlobalLog.LogLine($"### num: {num} x fileIndex:{fileIndex} ### ");
                for (int j = 0; j < num; j++)
                {
                    if (checksumIndex == 10)
                    {

                    }
                    var addingChecksum = checksumIndex * fileIndex;
                    GlobalLog.LogLine($"addingChecksum: {addingChecksum} for checksumIndex:{checksumIndex} and fileIndex I:{fileIndex}");
                    checksum += addingChecksum;
                    checksumIndex++;
                }
                fileIndex++;
            }
            else
            {
                var dSpace = discSpace[i];
                GlobalLog.LogLine($"### dSpace.Data.Length: {dSpace.Data.Length} ###");

                for (int j = 0; j < dSpace.Data.Length; j++)
                {
                    if (checksumIndex == 10)
                    {

                    }

                    var addingChecksum = checksumIndex * dSpace.Data[j];
                    GlobalLog.LogLine($"addingChecksum: {addingChecksum} for checksumIndex:{checksumIndex} and dSpace.Data[j]:{dSpace.Data[j]}");
                    checksum += addingChecksum;
                    checksumIndex++;

                }
            }
        }

        GlobalLog.LogLine($"checksum: {checksum}");
        return checksum.ToString();
    }
    public struct DiscSpace
    {
        public int SpaceLeft { get; set; }
        public int[] Data { get; set; }

        public DiscSpace(int value, int[] data)
        {
            SpaceLeft = value;
            Data = data;
        }
    }
    public int CharToInt(char c)
    {
        return c - '0';
    }
}
