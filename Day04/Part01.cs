
using CommunityToolkit.HighPerformance;
using Microsoft.Diagnostics.Runtime.Utilities;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Day04;

public static class Part01
{
    public static int Result(ReadOnlySpan<string> input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
        XmasState state = XmasState.None;
        XmasBackwardsState stateBackwards = XmasBackwardsState.None;

        Span2D<char> input2D = new char[rows, cols];

        int xmasCounter= 0;

        for ( int i=0; i<rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                input2D[i, j] = input[i][j];
            }
        }


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                char currentCol = input2D[i, j];
                state = UpdateXmasState(state, currentCol);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, currentCol);


                if ( stateBackwards == XmasBackwardsState.X || state == XmasState.S )
                {
                    xmasCounter++;
                }

            }

        }

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                char currentCol = input2D[j, i];
                state = UpdateXmasState(state, currentCol);
                stateBackwards = UpdateXmasBackwardsState(stateBackwards, currentCol);


                if (stateBackwards == XmasBackwardsState.X || state == XmasState.S)
                {
                    xmasCounter++;
                }

            }

        }
        return xmasCounter;
    }

    private static void CheckForXmasHorizontal()
    {
        throw new NotImplementedException();
    }
    private static XmasState UpdateXmasState(XmasState currentState, char currentChar)
    {
        return currentState switch
        {
            XmasState.None when currentChar == 'X' => XmasState.X,
            XmasState.X when currentChar == 'M' => XmasState.M,
            XmasState.M when currentChar == 'A' => XmasState.A,
            XmasState.A when currentChar == 'S' => XmasState.S,
            _ => currentState = XmasState.None
        };
    }

    private static XmasBackwardsState UpdateXmasBackwardsState(XmasBackwardsState currentState, char currentChar)
    {
        return currentState switch
        {
            XmasBackwardsState.None when currentChar == 'S' => XmasBackwardsState.S,
            XmasBackwardsState.S when currentChar == 'A' => XmasBackwardsState.A,
            XmasBackwardsState.A when currentChar == 'M' => XmasBackwardsState.M,
            XmasBackwardsState.M when currentChar == 'X' => XmasBackwardsState.X,
            _ => currentState = XmasBackwardsState.None
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
