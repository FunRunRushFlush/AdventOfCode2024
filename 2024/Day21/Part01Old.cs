

using System;
using System.Linq;

namespace Day21;
public class Part01Old :IPart
{
    public void ParseOnly(ReadOnlySpan<string> input)
    {
        ParseInput(input);
    }

    private static List<(int yDiff, int xDiff, bool lrFirst)> _rngDecisionsThisSim = new();
    private Dictionary<(int yDiff, int xDiff), DecisionStats> _decisionStats = new();

    //TODO: um zusehen welche coinflips die minimalen werte erzeugt hat
    private class DecisionStats
    {
        public int LrFirstCount { get; set; }
        public int UdFirstCount { get; set; }
        public double SumSolutionsLrFirst { get; set; }
        public double SumSolutionsUdFirst { get; set; }
       
    }
    public string Result(Input input)
    {
        int sim = 0;
        int minSolution = int.MaxValue;
        string minInputString = string.Empty;
        while (sim < 1_0)
        {
            string inputString = "";
            int solution = 0;
            //ParseInput(input);
            foreach (var line in input.Lines)
            {
                GlobalLog.LogLine($" ------------ {line} --------------");
                DoorController door = new DoorController();
                List<RobotController> robots = new List<RobotController>();


                for (int i = 0; i < 10; i++)
                {
                    robots.Add(new RobotController());
                }


                foreach (char code in line)
                {
                    GlobalLog.LogLine($"code: {code}");
                    var inputList = door.ReadNextInput(code);

                    foreach (var robot in robots)
                    {
                       
                        inputList = robot.ReadNextInput(inputList);
                        GlobalLog.LogLine($"robot : {inputList.Count} ");
                        GlobalLog.LogLine($"robot : {string.Join("", inputList)} ");
                    }

                    inputString += string.Join("", inputList);
                }
                solution += inputString.Length * int.Parse(line.Substring(0, line.Length - 1));

                GlobalLog.LogLine($"{inputString}");
                GlobalLog.LogLine($"{inputString.Length}");
                GlobalLog.LogLine($"solution: {solution}");
                if (string.IsNullOrEmpty(minInputString))
                {
                      minInputString = inputString;
                }
                if (minInputString.Length > inputString.Length)
                {
                    minInputString = inputString;
                }

            foreach (var dec in _rngDecisionsThisSim)
            {
                var key = (dec.yDiff, dec.xDiff);
                if (!_decisionStats.TryGetValue(key, out var stats))
                {
                    stats = new DecisionStats();
                    _decisionStats[key] = stats;
                }

                if (dec.lrFirst)
                {
                    stats.LrFirstCount++;
                    stats.SumSolutionsLrFirst += inputString.Length;
                }
                else
                {
                    stats.UdFirstCount++;
                    stats.SumSolutionsUdFirst += inputString.Length; ;
                }
            }
            _rngDecisionsThisSim.Clear();
                inputString = string.Empty;
            }

            if (Math.Min(minSolution, solution) < minSolution)
            {
                GlobalLog.LogLine($"{sim}");
                minSolution = Math.Min(minSolution, solution);
                GlobalLog.LogLine($"{minSolution}");
                GlobalLog.LogLine($"{minInputString.Length}");
                GlobalLog.LogLine($"{minInputString}");
                Console.WriteLine($"{minInputString.Length}");
            }


            sim++;
            
            GlobalLog.LogLine($"----------------Next Sim---------------");
            //if (sim%10 ==0) GlobalLog.LogLine($"solution: {sim}");
        }

        foreach (var kvp in _decisionStats)
        {
            var key = kvp.Key; // (yDiff, xDiff)
            var stats = kvp.Value;
            double avgLr = stats.LrFirstCount > 0
               ? stats.SumSolutionsLrFirst / stats.LrFirstCount
               : 0;
            double avgUd = stats.UdFirstCount > 0
               ? stats.SumSolutionsUdFirst / stats.UdFirstCount
               : 0;

            Console.WriteLine(
                $"(yDiff={key.yDiff}, xDiff={key.xDiff}) => " +
                $"LR-first: avg={avgLr:0.##}, count={stats.LrFirstCount} || " +
                $"UD-first: avg={avgUd:0.##}, count={stats.UdFirstCount}");
        }

        GlobalLog.LogLine($"{minInputString.Length}");
        GlobalLog.LogLine($"{minInputString}");
        return minInputString.Length.ToString();
    }

    private void ParseInput(ReadOnlySpan<string> input)
    {


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
                    GlobalLog.LogLine($"Robo: Lr-First: yDiff:{yDiff}, xDiff:{xDiff}");

                    dirInputs.AddRange(lrList);
                    dirInputs.AddRange(udList);
                }
                else if (Position.X + xDiff == 0 && xDiff != 0 && Position.Y == 0)
                {
                        GlobalLog.LogLine($"Robo: Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                        dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else if (yDiff == 1 && xDiff == 1)
                {
                    GlobalLog.LogLine($"Robo: Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else if (yDiff == -1 && xDiff == -1)
                {
                    GlobalLog.LogLine($"Robo: Lr-First: yDiff:{yDiff}, xDiff:{xDiff}");

                    dirInputs.AddRange(lrList);
                    dirInputs.AddRange(udList);
                }
                else if (yDiff == 0 && xDiff == -1)
                {
                    GlobalLog.LogLine($"Robo: Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else if (Math.Abs(yDiff) == 1 && xDiff == 0)
                {
                    GlobalLog.LogLine($"Robo: Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else if (Math.Abs(yDiff) == 0 && xDiff == 0)
                {
                    GlobalLog.LogLine($"Robo: Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else if (Math.Abs(yDiff) == 0 && xDiff == 1)
                {
                    GlobalLog.LogLine($"Robo: Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else if (yDiff == -1 && xDiff == 1)
                {
                    GlobalLog.LogLine($"Robo: Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else if (yDiff == 1 && xDiff == -1)
                {
                    GlobalLog.LogLine($"Robo: Lr-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(lrList);
                    dirInputs.AddRange(udList);
                }
                else if (yDiff == -3 && xDiff == -1)
                {
                    GlobalLog.LogLine($"Robo: Lr-First: yDiff:{yDiff}, xDiff:{xDiff}");

                    dirInputs.AddRange(lrList);
                    dirInputs.AddRange(udList);
                }
                else
                {
                    Random test = new Random();
                    bool lrFirst = test.Next(2) == 0;
                    _rngDecisionsThisSim.Add((yDiff, xDiff, lrFirst));
                    if (lrFirst)
                    {
                        GlobalLog.LogLine($"RNG Robo: Lr-First: yDiff:{yDiff}, xDiff:{xDiff}");
                        dirInputs.AddRange(lrList);
                        dirInputs.AddRange(udList);
                    }
                    else
                    {
                        GlobalLog.LogLine($"RNG Robo: Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                        dirInputs.AddRange(udList);
                        dirInputs.AddRange(lrList);
                    }
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
        //private int ySteps;
        //private int xSteps;

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
            //ySteps = 0;
            //xSteps = 0;
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
            else if (yDiff == 1 && xDiff == 1)
            {
                dirInputs.AddRange(udList);
                dirInputs.AddRange(lrList);
            }
            else if (yDiff == -1 && xDiff == -1)
            {
                dirInputs.AddRange(lrList);
                dirInputs.AddRange(udList);
            }
            else if (yDiff == -2 && xDiff == 1)
            {
                dirInputs.AddRange(udList);
                dirInputs.AddRange(lrList);
            }
            else if (yDiff == -3 && xDiff == -1)
            {
                dirInputs.AddRange(lrList);
                dirInputs.AddRange(udList);
            }
            else
            {
                Random test = new Random();
                bool lrFirst = test.Next(2) == 0;
                if (lrFirst)
                {
                    GlobalLog.LogLine($"Lr-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(lrList);
                    dirInputs.AddRange(udList);
                }
                else
                {
                    GlobalLog.LogLine($"Ud-First: yDiff:{yDiff}, xDiff:{xDiff}");
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }

            }
            dirInputs.Add('A');


            CheckIfPathIsAllowed(dirInputs);

            Position = targetPos;

            return dirInputs;

        }

        private void CheckIfPathIsAllowed(List<char> dirInputs)
        {
            var test = dirInputs.Where(x => x == '<');
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

