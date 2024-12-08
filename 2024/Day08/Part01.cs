using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
namespace Day08;
public static class Part01
{
    public static void Result(ReadOnlySpan<string> input)
    {


        var dic = new Dictionary<char, List<(int Y, int X)>>();

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {   
                char charCheck = input[y][x];
                if (charCheck == '.') continue;

                if(!dic.ContainsKey(charCheck))
                {
                    dic[charCheck] = new List<(int Y, int X)>(); 
                }
                dic[charCheck].Add((y, x));

            }
        }

        //foreach(var element in dic)
        //{
        //    GlobalLog.Log($"element.Key : {element.Key}");
        //}
        //GlobalLog.Log("TestDay08");
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
