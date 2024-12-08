

public class TreeNode
{
    public long Value;
    public TreeNode? Add;
    public TreeNode? Mult;


    public TreeNode(long value)
    {
        Value = value;
        Add = null;
        Mult = null;
    }
}

public class BinaryTree
{
    public TreeNode? Root;
    public int OperationIndex = 0;
    private long[] OperationValue= Array.Empty<long>();
    private long SearchTarget = 0;
    private int NumberOfOperations = 0;
    bool Found = false;
    public BinaryTree()
    {
        Root = null;
        Found = false;
    }


    public void SetOperationValue(long[] value)
    { 
        OperationValue = value;
        SearchTarget = value[1];
        NumberOfOperations = value.Length > 2 ? value.Length - 2 : 0;
    }

    public void Insert(long value, out long result)
    {
        if (Root == null)
        {
            Root = new TreeNode(value);
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

    private void InsertRecursive(TreeNode node, int operationNum)
    {
        if (operationNum <= NumberOfOperations)
        {

            long value = OperationValue[^operationNum];
            if (node.Value % value == 0 && (node.Value / value) >= SearchTarget)
            {
                if (node.Mult == null)
                {
                    node.Mult = new TreeNode(node.Value / value);
                }
                InsertRecursive(node.Mult, operationNum + 1);
            }

            if (node.Value - value >= SearchTarget)
            {
                if (node.Add == null)
                {
                    node.Add = new TreeNode(node.Value - value);
                }

                InsertRecursive(node.Add, operationNum + 1);
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

    private void PrintInOrderRecursive(TreeNode? node)
    {
        if (node != null)
        {
            PrintInOrderRecursive(node.Add);
            Console.WriteLine(node.Value);
            PrintInOrderRecursive(node.Mult);
        }
    }
}



