

namespace Day21;
public class Part02
{
    private int NumOfControllerRobots = 25;

    private static Dictionary<(string, int), long> Cache;
    public long Result(ReadOnlySpan<string> input)
    {
        string inputString = "";
        long solution = 0;
        foreach (var line in input)
        {
            GlobalLog.LogLine($" ------------ {line} --------------");
            DoorController door = new DoorController();
            List<RobotController> robots = new List<RobotController>();


            long totalNumOfInputs = 0;
            foreach (char code in line)
            {
                GlobalLog.LogLine($"door: {code}");
                var inputList = door.ReadNextInput(code);

                totalNumOfInputs += GetNumberOfRoboInputs(inputList, 0);
                //foreach (var robot in robots)
                //{
                //    inputList = robot.ReadNextInput(inputList);
                //    GlobalLog.LogLine($"robot : {inputList.Count} ");
                //    GlobalLog.LogLine($"robot : {string.Join("", inputList)} ");
                //}

            GlobalLog.LogLine($"    {totalNumOfInputs}");
                //inputString += string.Join("", inputList);
            }
            solution += totalNumOfInputs * int.Parse(line.Substring(0, line.Length - 1));
            GlobalLog.LogLine($"totalNumOfInputs: {totalNumOfInputs}");
            GlobalLog.LogLine($"solution: {solution}");
            totalNumOfInputs = 0;
        }

        return solution;
    }

    private long GetNumberOfRoboInputs(List<char> inputList, int roboChain)
    {
        if (roboChain == NumOfControllerRobots)
        {
            return inputList.Count;
        }
        if (Cache.TryGetValue((inputList.ToString(), roboChain), out long cachedResult))
        {
            return cachedResult;
        }

        long result = 0;

        var chunks = ParseInputListIntoChunks(inputList);

        foreach (var chunk in chunks)
        {
            result += GetNumberOfRoboInputs(chunk, roboChain+1);
        }
        Cache[(inputList.ToString(), roboChain)] = result;
        return result;

    }

    private List<List<char>> ParseInputListIntoChunks(List<char> inputList)
    {
        List<List<char>> result = new List<List<char>>();
        List<char> currentChunk = new List<char>();

        foreach (char c in inputList)
        {
            currentChunk.Add(c);
             
            if (c == 'A')
            {
                result.Add(new List<char>(currentChunk));
                currentChunk.Clear(); 
            }
        }
        if (currentChunk.Count > 0)
        {
            result.Add(currentChunk);
        }

        return result;
    }

    private class RobotController
    {
        private (int Y, int X) Position;

        private Dictionary<char, (int Y, int X)> ControllerDic = new()
            {
                { '#', (0, 0) },{ '^', (0, 1) },{ 'A', (0, 2) },
                { '<', (1, 0) },{ 'v', (1, 1) },{ '>', (1, 2) }
            };
        public RobotController()
        {
            Position = ControllerDic['A']; //A
        }

        public List<char> ReadNextInput(List<char> inputs)
        {
            List<char> dirInputs = new List<char>();

            foreach (char dirInp in inputs)
            {
                var targetPos = ControllerDic[dirInp];
                var yDiff = targetPos.Y - Position.Y;
                var xDiff = targetPos.X - Position.X;
                var lrList = GetLeftRightInput(xDiff);
                var udList = GetUpDownInput(yDiff);

                if (Position.Y + yDiff == 0 && yDiff != 0 && Position.X == 0)
                {
                    dirInputs.AddRange(lrList);
                    dirInputs.AddRange(udList);
                }
                else if (Position.X + xDiff == 0 && xDiff != 0 && Position.Y == 0)
                {
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else
                {
                    dirInputs.AddRange(lrList);
                    dirInputs.AddRange(udList);
                }

                dirInputs.Add('A');
                Position = targetPos;
            }

            return dirInputs;
        }


        private List<char> GetUpDownInput(int yDiff)
        {
            List<char> dirInputs = new List<char>();

            for (var i = 0; i < Math.Abs(yDiff); i++)
            {
                if (yDiff == 0) break;
                if (yDiff < 0)
                {
                    dirInputs.Add('^');
                }
                if (yDiff > 0)
                {
                    dirInputs.Add('v');
                }
            }
            return dirInputs;
        }

        private List<char> GetLeftRightInput(int xDiff)
        {
            List<char> dirInputs = new List<char>();
            for (var i = 0; i < Math.Abs(xDiff); i++)
            {
                if (xDiff == 0) break;
                if (xDiff > 0)
                {
                    dirInputs.Add('>');
                }
                if (xDiff < 0)
                {
                    dirInputs.Add('<');
                }
            }
            return dirInputs;
        }

    }
    private class DoorController
    {
        private (int Y, int X) Position;

        private Dictionary<char, (int Y, int X)> ControllerDic = new()
            {
                { '7', (0, 0) },{ '8', (0, 1) },{ '9', (0, 2) },
                { '4', (1, 0) },{ '5', (1, 1) },{ '6', (1, 2) },
                { '1', (2, 0) },{ '2', (2, 1) },{ '3', (2, 2) },
                { '#', (3, 0) },{ '0', (3, 1) },{ 'A', (3, 2) }
            };
        public DoorController()
        {
            Position = ControllerDic['A']; //A
        }

        public List<char> ReadNextInput(char input)
        {
            List<char> dirInputs = new List<char>();
            var targetPos = ControllerDic[input];

            var yDiff = targetPos.Y - Position.Y;
            var xDiff = targetPos.X - Position.X;

            var lrList = GetLeftRightInput(xDiff);
            var udList = GetUpDownInput(yDiff);

            if (Position.Y + yDiff == 3 && yDiff != 0 && Position.X == 0)
            {
                dirInputs.AddRange(lrList);
                dirInputs.AddRange(udList);
            }
            else if (Position.X + xDiff == 0 && xDiff != 0 && Position.Y == 3)
            {
                dirInputs.AddRange(udList);
                dirInputs.AddRange(lrList);
            }
            else if (yDiff > 0 && xDiff > 0)
            {
                dirInputs.AddRange(udList);
                dirInputs.AddRange(lrList);
            }
            else
            {
                dirInputs.AddRange(lrList);
                dirInputs.AddRange(udList);
            }
            dirInputs.Add('A');

            Position = targetPos;

            return dirInputs;

        }

        private List<char> GetUpDownInput(int yDiff)
        {
            List<char> dirInputs = new List<char>();

            for (var i = 0; i < Math.Abs(yDiff); i++)
            {
                if (yDiff == 0) break;
                if (yDiff < 0)
                {
                    dirInputs.Add('^');
                }
                if (yDiff > 0)
                {
                    dirInputs.Add('v');
                }
            }
            return dirInputs;
        }

        private List<char> GetLeftRightInput(int xDiff)
        {
            List<char> dirInputs = new List<char>();
            for (var i = 0; i < Math.Abs(xDiff); i++)
            {
                if (xDiff == 0) break;
                if (xDiff > 0)
                {
                    dirInputs.Add('>');
                }
                if (xDiff < 0)
                {
                    dirInputs.Add('<');
                }
            }
            return dirInputs;
        }
    }


}