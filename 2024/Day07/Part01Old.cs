
using System.Data;



namespace Day07;

public class Part01Old:IPart
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


            BinaryTree tree = new BinaryTree();
            tree.SetOperationValue(res);
            tree.Insert(res[0], out long result);
            counter += result;

            //Console.WriteLine("In-Order Traversierung:");
            //tree.PrintInOrder();
        }
        return counter;
       
    }
}