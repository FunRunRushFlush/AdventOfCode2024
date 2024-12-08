
using CommunityToolkit.HighPerformance;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Text.RegularExpressions;

namespace Day04;
public static class Part02
{
    public static int Result(ReadOnlySpan<string> input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
        XmasState state = XmasState.None;
        XmasBackwardsState stateBackwards = XmasBackwardsState.None;

        Span2D<char> input2D = new char[rows, cols];

        Span2D<int> inputLog2D = new int[rows, cols];



        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                input2D[i, j] = input[i][j];
            }
        }


      

        int diaRow = 0, diaCol = 0;
        for (int i = 0; i < cols; i++)
        {
            diaCol = i;

            while (diaCol < cols && diaRow < rows)
            {
                var current = input2D[diaRow, diaCol];
                inputLog2D[diaRow, diaCol]++;
                state = UpdateXmasState(state, current);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, current);
                if (stateBackwards == XmasBackwardsState.M)
                {
                    //inputLog2D[diaRow - 2, diaCol - 2]++;
                    inputLog2D[diaRow - 1, diaCol - 1]++;
                    //inputLog2D[diaRow, diaCol]++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    //inputLog2D[diaRow - 2, diaCol - 2]++;
                    inputLog2D[diaRow - 1, diaCol - 1]++;
                    //inputLog2D[diaRow, diaCol]++;
                    state = XmasState.None;
                }
                diaRow++;
                diaCol++;
            PrintCheckedState(inputLog2D, input2D);
            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            diaRow = 0;

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
                if (stateBackwards == XmasBackwardsState.M)
                {
                    //inputLog2D[diaRow - 2, diaCol - 2]++;
                    inputLog2D[diaRow - 1, diaCol - 1]++;
                    //inputLog2D[diaRow, diaCol]++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    //inputLog2D[diaRow - 2, diaCol - 2]++;
                    inputLog2D[diaRow - 1, diaCol - 1]++;
                    //inputLog2D[diaRow, diaCol]++;
                    state = XmasState.None;
                }
                diaRow++;
                diaCol++;
            PrintCheckedState(inputLog2D, input2D);
            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            diaCol = 0;

        }

        diaRow = rows; diaCol = 0;
        for (int i = rows - 1; i > 0; i--)
        {
            diaRow = i;
            while (diaCol < cols && diaRow >= 0)
            {
                var current = input2D[diaRow, diaCol];
                inputLog2D[diaRow, diaCol]++;
                state = UpdateXmasState(state, current);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, current);
                if (stateBackwards == XmasBackwardsState.M)
                {
                    //inputLog2D[diaRow + 2, diaCol - 2]++;
                    inputLog2D[diaRow + 1, diaCol - 1]++;
                    //inputLog2D[diaRow, diaCol]++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    //inputLog2D[diaRow + 2, diaCol - 2]++;
                    inputLog2D[diaRow + 1, diaCol - 1]++;
                    //inputLog2D[diaRow, diaCol]++;
                    state = XmasState.None;
                }
                diaRow--;
                diaCol++;
            PrintCheckedState(inputLog2D, input2D);
            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            diaCol = 0;

        }

        diaRow = rows - 1; diaCol = 0;
        for (int i = 1; i < cols; i++)
        {
            diaCol = i;
            while (diaCol < cols && diaRow >= 0)
            {
                var current = input2D[diaRow, diaCol];
                inputLog2D[diaRow, diaCol] += 1;
                state = UpdateXmasState(state, current);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, current);
                if (stateBackwards == XmasBackwardsState.M)
                {
                    //inputLog2D[diaRow + 2, diaCol - 2]++;
                    inputLog2D[diaRow + 1, diaCol - 1]++;
                    //inputLog2D[diaRow, diaCol]++;
                    stateBackwards = XmasBackwardsState.None;
                }
                else if (state == XmasState.S)
                {
                    //inputLog2D[diaRow + 2, diaCol - 2]++;
                    inputLog2D[diaRow + 1, diaCol - 1]++;
                    //inputLog2D[diaRow, diaCol]++;
                    state = XmasState.None;
                }
                diaRow--;
                diaCol++;
            PrintCheckedState(inputLog2D, input2D);
            }
            state = XmasState.None;
            stateBackwards = XmasBackwardsState.None;
            diaRow = rows - 1;

        }



        PrintCheckedState(inputLog2D, input2D);


        return CountXCheckedState(inputLog2D); 
    }

    private static void PrintCheckedState(Span2D<int> span,Span2D<char> input)
    {
        //int rows = span.Height;
        //int cols = span.Width;
        //Console.Write("#######################");
        //Console.WriteLine();
        //for (int i = 0; i < rows; i++)
        //{
        //    for (int j = 0; j < cols; j++)
        //    {
        //        var value = span[i, j];
        //        var charValue = input[i, j];
        //        SetConsoleColor(value);
        //        Console.Write(value);
        //        Console.ResetColor();
        //        Console.Write(" ");
        //    }
        //    Console.WriteLine();
        //}
        //Console.Write("#######################");

    }
    private static int CountXCheckedState(Span2D<int> span)
    {
        var xCounter = 0;
        int rows = span.Height;
        int cols = span.Width;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var value = span[i, j];
                if (value >= 4) xCounter++;


            }

        }
        return xCounter;
    }
    static void SetConsoleColor(int value)
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


    private static XmasState UpdateXmasState(XmasState currentState, char currentChar)
    {
        var nextState = currentState switch
        {
            XmasState.None when currentChar == 'M' => XmasState.M,
            XmasState.M when currentChar == 'A' => XmasState.A,
            XmasState.A when currentChar == 'S' => XmasState.S,
            _ when currentChar == 'M' => XmasState.M,
            _ => XmasState.None
        };
        //Console.WriteLine($"Char: {currentChar}, CurrentState: {currentState}, NextState: {nextState}");
        return nextState;
    }

    private static XmasBackwardsState UpdateXmasBackwardsState(XmasBackwardsState currentState, char currentChar)
    {
        return currentState switch
        {
            XmasBackwardsState.None when currentChar == 'S' => XmasBackwardsState.S,
            XmasBackwardsState.S when currentChar == 'A' => XmasBackwardsState.A,
            XmasBackwardsState.A when currentChar == 'M' => XmasBackwardsState.M,
            _ when currentChar == 'S' => XmasBackwardsState.S,
            _ => XmasBackwardsState.None
        };
    }

    [Flags]
    enum XmasState
    {
        None = 0,
        M = 1,
        A = 2,
        S = 4
    }
    [Flags]
    enum XmasBackwardsState
    {
        None = 0,
        S = 1,
        A = 2,
        M = 4,

    }


}
