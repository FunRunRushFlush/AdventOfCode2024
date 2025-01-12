namespace Year_2024.Days.Day01;
public class Part02 : IPart
{
    public string Result(Input input)
    {
        List<int> list01 = new List<int>();
        Dictionary<int, int> dic02 = new Dictionary<int, int>();
        foreach (var item in input.Lines)
        {
            var res = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            list01.Add(int.Parse(res.First()));
            if (!dic02.TryAdd(int.Parse(res.Last()), 1))
                dic02[int.Parse(res.Last())] += 1;
        }
        int similarity = 0;
        for (int i = 0; i < list01.Count; i++)
        {
            dic02.TryGetValue(list01[i], out int count);
            similarity += list01[i] * count;
        }

        return similarity.ToString();
    }

}
