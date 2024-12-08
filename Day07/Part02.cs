#define LOGGING_ENABLED

namespace Day07;
public static class Part02
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


            BinaryTreePart02 tree = new BinaryTreePart02();
            tree.SetOperationValue(res);
            tree.Insert(res[1], out long result);
            counter += result;

            //Console.WriteLine("In-Order Traversierung:");
            //tree.PrintInOrder();
        }
        //Console.WriteLine($"FinalCounter : {counter}");
        //GlobalLog.Log($"FinalCounter : {counter}");

    }
}