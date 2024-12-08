

namespace Day07;

public class TreeNodePart02
{
    public long Value;
    public TreeNodePart02? Add;
    public TreeNodePart02? Mult;
    public TreeNodePart02? Concat;


    public TreeNodePart02(long value)
    {
        Value = value;
        Add = null;
        Mult = null;
        Concat = null;
    }
}

public class BinaryTreePart02
{
    public TreeNodePart02? Root;
    public int OperationIndex = 0;
    private long[] OperationValue= Array.Empty<long>();
    private long SearchTarget = 0;
    private int NumberOfOperations = 0;
    bool Found = false;
    public BinaryTreePart02()
    {
        Root = null;
        Found = false;
    }


    public void SetOperationValue(long[] value)
    { 
        OperationValue = value;
        SearchTarget = value[0];
        NumberOfOperations = value.Length > 2 ? value.Length - 2 : 0;
    }

    public void Insert(long value, out long result)
    {
        if (Root == null)
        {
            Root = new TreeNodePart02(value);
        }        
            InsertRecursive(Root, 1);

        if (Found)
        {
            result = OperationValue[0];
            Found = false;
        }
        else 
        {
            result = 0;
        }
    }

    private void InsertRecursive(TreeNodePart02 node, int operationNum)
    {
        if (operationNum <= NumberOfOperations)
        {

            long value = OperationValue[operationNum+1];
            if (node.Value * value <= SearchTarget)
            {
                if (node.Mult == null)
                {
                    node.Mult = new TreeNodePart02(node.Value * value);
                }
                InsertRecursive(node.Mult, operationNum + 1);
            }

            if (node.Value + value <= SearchTarget)
            {
                if (node.Add == null)
                {
                    node.Add = new TreeNodePart02(node.Value + value);
                }

                InsertRecursive(node.Add, operationNum + 1);
            }

            if (long.Parse(node.Value.ToString() + value.ToString()) <= SearchTarget)
            {
                if (node.Concat == null)
                {
                    node.Concat = new TreeNodePart02(long.Parse(node.Value.ToString() + value.ToString()));
                }

                InsertRecursive(node.Concat, operationNum + 1);
            }
        }
        else if (node.Value == SearchTarget)
        {
            Found = true;
        }
    }


    public void PrintInOrder()
    {
        PrintInOrderRecursive(Root); 
    }

    private void PrintInOrderRecursive(TreeNodePart02? node)
    {
        if (node != null)
        {
            PrintInOrderRecursive(node.Add);
            Console.WriteLine(node.Value);
            PrintInOrderRecursive(node.Mult);
        }
    }
}



