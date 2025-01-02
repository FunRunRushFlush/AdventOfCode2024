namespace Day01;
public class Part02 : IPart
{
    public string Result(Input input)
    {
        List<int> list01 = new List<int>();
        Dictionary<int, int> dic02 = new Dictionary<int, int>();
        foreach (var item in input.Lines)
        {
            var res = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            list01.Add(int.Parse((res.First())));
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
    //public string Result(Input input)
    //{
    //    int[] numbers = input.Text
    //        .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
    //        .Select(int.Parse)
    //        .ToArray();

    //    int[] number01 = new int[numbers.Length / 2];
    //    int[] number02 = new int[numbers.Length / 2];

    //    int ind = 0;
    //    for (int i = 0; i < numbers.Length; i += 2)
    //    {

    //        number01[ind] = numbers[i];
    //        number02[ind] = numbers[i + 1];
    //        ind++;
    //    }

    //    int similarityScore = 0;
    //    for (int i = 0; i < number01.Length; i++)
    //    {
    //        int checkingInt = number01[i];
    //        int localSimilarityScore = checkingInt * number02.Count(x => x == checkingInt);

    //        similarityScore += localSimilarityScore;
    //    }

    //    return $"{similarityScore}";
    //}
}
