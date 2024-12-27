using System.Xml;

namespace Day23;
public class Part02
{
    private Dictionary<string, HashSet<string>> LanDic = new Dictionary<string, HashSet<string>>();
    HashSet<string> SetOf3 = new();
    HashSet<string> UniquePC = new();
    private int MaxCounter = 0;


    private int StartIndex;
    public void ParseOnly(ReadOnlySpan<string> input)
    {
        ParseInput(input);
    }

    public string Result(ReadOnlySpan<string> input)
    {
        ParseInput(input);


        foreach (var startNode in LanDic)
        {
            var counter = 0;
            HashSet<string> temp = new(); 
            foreach (var secNode in startNode.Value)
            {
                foreach (var thirdNode in LanDic[secNode])
                {
       
                        if (startNode.Value.Contains(thirdNode))
                        {
                            var sortedTextArray = new[] { startNode.Key, secNode, thirdNode }.OrderBy(x => x).ToArray();
                            string sortedText = string.Join(",", sortedTextArray);
                            
                            temp.Add(sortedText);
                            counter++;
                        }
                    
                }
            }
            if(counter> MaxCounter)
            {
                MaxCounter = counter;
                SetOf3 = new(temp);                
            }
        }

        foreach (var ele in SetOf3)
        {
            var test = ele.Split(',').ToArray();
            foreach (var testNode in test)
            {
                UniquePC.Add(testNode);
            }
            GlobalLog.LogLine(ele);
        }

        string solution = string.Join(",", UniquePC.OrderBy(x => x));
        return solution;
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

