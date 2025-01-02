
using CommunityToolkit.HighPerformance;


namespace Day04;

public class Part01 : IPart
{

    public string Result(Input input)
    {
 
        int rows = input.Lines.Length;
        int cols = input.Lines[0].Length;
        XmasState state = XmasState.None;
        XmasBackwardsState stateBackwards = XmasBackwardsState.None;

        Span2D<char> input2D = new char[rows, cols];

        Span2D<int> inputLog2D = new int[rows, cols];

        int xmasCounter= 0;

        for ( int i=0; i<rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                input2D[i, j] = input.Lines[i][j];
            }
        }


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                char currentCol = input2D[i, j];
                inputLog2D[i, j]++;
                state = UpdateXmasState(state, currentCol);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, currentCol);

                if (stateBackwards == XmasBackwardsState.X)
                {
                    xmasCounter++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    xmasCounter++;
                    state = XmasState.None;
                }

            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            PrintCheckedState(inputLog2D);
        }

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                char currentCol = input2D[j, i];
                inputLog2D[j, i]++;
                state = UpdateXmasState(state, currentCol);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, currentCol);


                if (stateBackwards == XmasBackwardsState.X)
                {
                    xmasCounter++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    xmasCounter++;
                    state = XmasState.None;
                }

            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            PrintCheckedState(inputLog2D);
        }

        int xmasDiagCounter = 0;
        int diaRow=0, diaCol=0;
        for (int i = 0; i < cols; i++)
        {
            diaCol = i;

            while (diaCol<cols && diaRow<rows)
            {
                var current = input2D[diaRow, diaCol];
                inputLog2D[diaRow, diaCol]++;
                state = UpdateXmasState(state, current);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, current);
                if (stateBackwards == XmasBackwardsState.X)
                {
                    xmasCounter++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    xmasCounter++;
                    state = XmasState.None;
                }
                diaRow++;
                diaCol++;
            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            diaRow = 0;
            PrintCheckedState(inputLog2D);

        }


        diaRow = 0; diaCol = 0;
        for (int i = 1; i < rows; i++)
        {
            diaRow = i;
            while (diaCol < cols && diaRow < rows)
            {
                var current = input2D[diaRow, diaCol];
                inputLog2D[diaRow, diaCol]++;
                state = UpdateXmasState(state, current);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, current);
                if (stateBackwards == XmasBackwardsState.X)
                {
                    xmasCounter++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    xmasCounter++;
                    state = XmasState.None;
                }
                diaRow++;
                diaCol++;
            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            diaCol = 0;
            PrintCheckedState(inputLog2D);

        }

        diaRow = rows; diaCol = 0;
        for (int i = rows-1; i > 0; i--)
        {
            diaRow = i;
            while (diaCol < cols && diaRow >= 0)
            {
                var current = input2D[diaRow, diaCol];
                inputLog2D[diaRow, diaCol]++;
                state = UpdateXmasState(state, current);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, current);
                if (stateBackwards == XmasBackwardsState.X)
                {
                    xmasCounter++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    xmasCounter++;
                    state = XmasState.None;
                }
                diaRow--;
                diaCol++;
            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            diaCol = 0;
            PrintCheckedState(inputLog2D);

        }

        diaRow = rows-1; diaCol = 0;
        for (int i = 1; i< cols; i++)
        {
            diaCol = i;
            while (diaCol < cols && diaRow >= 0)
            {
                var current = input2D[diaRow, diaCol];
                inputLog2D[diaRow, diaCol] +=1;
                state = UpdateXmasState(state, current);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, current);
                if (stateBackwards == XmasBackwardsState.X)
                {
                    xmasCounter++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if(state == XmasState.S)
                {
                    xmasCounter++;
                    state = XmasState.None;
                }
                diaRow--;
                diaCol++;
            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            diaRow = rows - 1;
            PrintCheckedState(inputLog2D);

        }



        PrintCheckedState(inputLog2D);


                return $"{xmasCounter}";
    }

    private void PrintCheckedState(Span2D<int> span)
    {
        //int rows = span.Height;
        //int cols = span.Width;
        //Console.Write("#######################");
        //Console.WriteLine();
        //for (int i = 0; i < rows; i++)
        //{
        //    for (int j = 0; j < cols; j++)
        //    {
        //        SetConsoleColor(span[i, j]);
        //        Console.Write(span[i, j]);
        //        Console.ResetColor(); 
        //        Console.Write(" ");
        //    }
        //    Console.WriteLine();
        //}
        //Console.Write("#######################");
    }
    void SetConsoleColor(int value)
    {
        // Mapping von Zahlen auf Farben
        switch (value)
        {
            case 1: Console.ForegroundColor = ConsoleColor.Blue; break;
            case 2: Console.ForegroundColor = ConsoleColor.Green; break;
            case 3: Console.ForegroundColor = ConsoleColor.Red; break;
            case 4: Console.ForegroundColor = ConsoleColor.Yellow; break;
            case 5: Console.ForegroundColor = ConsoleColor.Magenta; break;
            case 6: Console.ForegroundColor = ConsoleColor.Red; break;
            default: Console.ForegroundColor = ConsoleColor.White; break;
        }
    }

    private void CheckForXmasHorizontal()
    {
        throw new NotImplementedException();
    }
    private XmasState UpdateXmasState(XmasState currentState, char currentChar)
    {
        var nextState = currentState switch
        {
            XmasState.None when currentChar == 'X' => XmasState.X,
            XmasState.X when currentChar == 'M' => XmasState.M,
            XmasState.M when currentChar == 'A' => XmasState.A,
            XmasState.A when currentChar == 'S' => XmasState.S,
            XmasState.S when currentChar == 'X' => XmasState.X,
            _ when currentChar == 'X' => XmasState.X,
            _ => XmasState.None
        };
        //Console.WriteLine($"Char: {currentChar}, CurrentState: {currentState}, NextState: {nextState}");
        return nextState;
    }

    private XmasBackwardsState UpdateXmasBackwardsState(XmasBackwardsState currentState, char currentChar)
    {
        return currentState switch
        {
            XmasBackwardsState.None when currentChar == 'S' => XmasBackwardsState.S,
            XmasBackwardsState.S when currentChar == 'A' => XmasBackwardsState.A,
            XmasBackwardsState.A when currentChar == 'M' => XmasBackwardsState.M,
            XmasBackwardsState.M when currentChar == 'X' => XmasBackwardsState.X,
            _ when currentChar == 'S' => XmasBackwardsState.S,
            _ => XmasBackwardsState.None
        };
    }

    [Flags]
    enum XmasState
    {
        None = 0,
        X = 1,
        M = 2,
        A = 4,
        S = 8
    }
    [Flags]
    enum XmasBackwardsState
    {
        None = 0,
        S = 1,
        A = 2,
        M = 4,
        X = 8
    }


}
