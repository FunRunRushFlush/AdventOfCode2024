#define LOGGING_ENABLED

using System.Data;



namespace Day07;

public static class Part01
{
    public static int Result(string[] rawInput)
    {
         ParseInput(rawInput);
        return 0;
    }

    private static void ParseInput(string[] rawInput)
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
        //Console.WriteLine($"FinalCounter : {counter}");
        //GlobalLog.Log($"FinalCounter : {counter}");
       
    }
}