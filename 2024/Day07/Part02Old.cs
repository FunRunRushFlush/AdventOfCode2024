
namespace Day07;
public class Part02Old:IPart
{
    public string Result(Input rawInput)
    {
        long res = ParseInput(rawInput.Lines);
        return res.ToString();
    }
    private long ParseInput(string[] rawInput)
    {
        long counter = 0;

        foreach (string line in rawInput)
        {
            var res = line.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();


            BinaryTreePart02 tree = new BinaryTreePart02();
            tree.SetOperationValue(res);
            tree.Insert(res[1], out long result);
            counter += result;

            //Console.WriteLine("In-Order Traversierung:");
            //tree.PrintInOrder();
        }
        return counter;

    }
}