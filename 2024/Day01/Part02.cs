namespace Day01;
public class Part02 : IPart
{
    public string Result(Input input)
    {
        int[] numbers = input.Text
            .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        int[] number01 = new int[numbers.Length / 2];
        int[] number02 = new int[numbers.Length / 2];

        int ind = 0;
        for (int i = 0; i < numbers.Length; i += 2)
        {

            number01[ind] = numbers[i];
            number02[ind] = numbers[i + 1];
            ind++;
        }

        int similarityScore = 0;
        for (int i = 0; i < number01.Length; i++)
        {
            int checkingInt = number01[i];
            int localSimilarityScore = checkingInt * number02.Count(x => x == checkingInt);

            similarityScore += localSimilarityScore;
        }

        return $"{similarityScore}";
    }
}
