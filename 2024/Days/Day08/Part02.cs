using System.Diagnostics;

namespace Year_2024.Days.Day08;


public class Part02 : IPart
{
    public string Result(Input input)
    {
        var inputHeightY = input.Lines.Length;
        var inputWidthX = input.Lines[0].Length;

        var dicAntenna = new Dictionary<char, List<(int Y, int X)>>();
        HashSet<(int x, int y)> dicAntinodes = new HashSet<(int, int)>();


        for (int y = 0; y < input.Lines.Length; y++)
        {
            for (int x = 0; x < input.Lines[0].Length; x++)
            {
                char charCheck = input.Lines[y][x];
                if (charCheck == '.' || charCheck == '#') continue;

                if (!dicAntenna.ContainsKey(charCheck))
                {
                    dicAntenna[charCheck] = new List<(int Y, int X)>();
                }
                dicAntenna[charCheck].Add((y, x));

            }
        }

        foreach (var element in dicAntenna)
        {
            var list = element.Value;

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    GlobalLog.LogLine($"##### New Calc I:{i} j: {j} #########");
                    GlobalLog.LogLine($"tuple1 = list[i]: Y:{list[i].Y} X:{list[i].X}");
                    GlobalLog.LogLine($"tuple2 = list[j]: Y:{list[j].Y} X:{list[j].X}");
                    var tuple1 = list[i];
                    var tuple2 = list[j];
                    dicAntinodes.Add(tuple1);
                    dicAntinodes.Add(tuple2);

                    bool tuple1Posible = true;
                    bool tuple2Posible = true;
                    var diffY = tuple2.Y - tuple1.Y;
                    var diffX = tuple2.X - tuple1.X;
                    int mult = 1;
                    while (tuple1Posible || tuple2Posible)
                    {
                        GlobalLog.LogLine($"tuple1.Y: {tuple1.Y}");
                        GlobalLog.LogLine($"tuple2.Y: {tuple2.Y}");
                        var antinode01 = (tuple1.Y - diffY * mult, tuple1.X - diffX * mult);
                        var antinode02 = (tuple2.Y + diffY * mult, tuple2.X + diffX * mult);
                        if (antinode01.Item1 >= 0 && antinode01.Item2 >= 0
                            && antinode01.Item1 < inputHeightY && antinode01.Item2 < inputWidthX)
                        {
                            GlobalLog.LogLine($"antinode01: Y:{antinode01.Item1},X:{antinode01.Item2} ");
                            dicAntinodes.Add(antinode01);
                        }
                        else
                        {
                            tuple1Posible = false;
                        }

                        if (antinode02.Item1 >= 0 && antinode02.Item2 >= 0
                            && antinode02.Item1 < inputHeightY && antinode02.Item2 < inputWidthX)
                        {
                            GlobalLog.LogLine($"antinode02: Y:{antinode02.Item1},X:{antinode02.Item2} ");
                            dicAntinodes.Add(antinode02);
                        }
                        else
                        {
                            tuple2Posible = false;
                        }
                        mult++;
                    }
                }

            }
        }
        int antinodesCounter = dicAntinodes.Count;

        return antinodesCounter.ToString();
    }
}
