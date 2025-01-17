namespace Year_2024.Days.Day01;
public class Part01 : IPart
{
    public string Result(Input input)
    {
        List<int> list01 = new List<int>();
        List<int> list02 = new List<int>();
        foreach (var item in input.SpanLines)
        {
            var res = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            list01.Add(int.Parse(res[0]));
            list02.Add(int.Parse(res[1]));
        }
        list01.Sort();
        list02.Sort();
        int distnace = 0;
        for (int i = 0; i < list01.Count; i++)
        {
            distnace += Math.Abs(list01[i] - list02[i]);
        }

        return distnace.ToString();
    }
}
