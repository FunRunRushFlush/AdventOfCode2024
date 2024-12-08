using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
namespace Day08;
public static class Part01
{
    public static void Result(ReadOnlySpan<string> input)
    {
        var inputHeightY = input.Length;
        var inputWidthX = input[0].Length;

        var dicAntenna = new Dictionary<char, List<(int Y, int X)>>();
        HashSet<(int x, int y)> dicAntinodes = new HashSet<(int, int)>();
   

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {   
                char charCheck = input[y][x];
                if (charCheck == '.') continue;

                if(!dicAntenna.ContainsKey(charCheck))
                {
                    dicAntenna[charCheck] = new List<(int Y, int X)>(); 
                }
                dicAntenna[charCheck].Add((y, x));

            }
        }

        foreach (var element in dicAntenna)
        {
            var list = element.Value;

            for (int i = 0; i < list.Count-1; i++)
            {
                for(int j = i+1; j<list.Count;j++)
                {
                    var tuple1 = list[i];
                    var tuple2 = list[j];


                    var diffY = tuple2.Y - tuple1.Y;
                    var diffX = tuple2.X - tuple1.X;

                    var antinode01 = (tuple1.Y - diffY, tuple1.X - diffX);
                    var antinode02 = (tuple2.Y + diffY, tuple2.X + diffX);        

        
                    if ((antinode01.Item1 >= 0 && antinode01.Item2 >= 0) 
                        && (antinode01.Item1 < inputHeightY && antinode01.Item2 < inputWidthX))
                    {
                        dicAntinodes.Add(antinode01);
                    }

                    if ((antinode02.Item1 >= 0 && antinode02.Item2 >= 0) 
                        && (antinode02.Item1 < inputHeightY && antinode02.Item2 < inputWidthX))
                    {
                        dicAntinodes.Add(antinode02);
                    }

                    
                }
            }
            
        }
        int antinodesCounter = dicAntinodes.Count;
        

        //GlobalLog.Log($"antinodesCounter: {antinodesCounter}");
    }

    public static void Result_Char(ReadOnlySpan<string> input)
    {


        var dic = new Dictionary<char, List<(int Y, int X)>>();

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                if (input[y][x] == '.') continue;

                if (!dic.ContainsKey(input[y][x]))
                {
                    dic[input[y][x]] = new List<(int Y, int X)>();
                }
                dic[input[y][x]].Add((y, x));

            }
        }

        //foreach(var element in dic)
        //{
        //    GlobalLog.Log($"element.Key : {element.Key}");
        //}
        //GlobalLog.Log("TestDay08");
    }
}

