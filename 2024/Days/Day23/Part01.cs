namespace Year_2024.Days.Day23;
public class Part01 : IPart
{
    private Dictionary<string, HashSet<string>> LanDic = new Dictionary<string, HashSet<string>>();
    HashSet<string> SetOf3 = new();


    private int StartIndex;
    public void ParseOnly(ReadOnlySpan<string> input)
    {
        ParseInput(input);
    }

    public string Result(Input input)
    {
        ParseInput(input.Lines);
        var setOfT = LanDic.Where(x => x.Key.StartsWith('t')).ToArray();

        foreach (var startNode in setOfT)
        {
            foreach (var secNode in startNode.Value)
            {
                foreach (var thirdNode in LanDic[secNode])
                {
                    foreach (var loopCheck in LanDic[thirdNode])
                    {
                        if (loopCheck == startNode.Key)
                        {
                            var sortedTextArray = new[] { startNode.Key, secNode, thirdNode }.OrderBy(x => x).ToArray();
                            string sortedText = string.Join(",", sortedTextArray);

                            SetOf3.Add(sortedText);
                        }
                    }
                }
            }
        }

        foreach (var ele in SetOf3)
        {
            GlobalLog.LogLine(ele);
        }

        return SetOf3.Count.ToString();
    }


    private void ParseInput(ReadOnlySpan<string> input)
    {

        for (int i = 0; i < input.Length; i++)
        {
            var pc1 = input[i].Substring(0, 2);
            var pc2 = input[i].Substring(3, 2);

            if (!LanDic.TryAdd(pc1, new HashSet<string>() { pc2 }))
            {
                LanDic[pc1].Add(pc2);
            }
            if (!LanDic.TryAdd(pc2, new HashSet<string>() { pc1 }))
            {
                LanDic[pc2].Add(pc1);
            }
        }
    }

}

