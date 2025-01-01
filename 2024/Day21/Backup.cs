

using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Day21;
public class Backup
{
    private static Dictionary<List<char>, List<char>> OptimalPath = new();
    private static Dictionary<char, List<char>> OptimalStartPath = new();

    public void ParseOnly(ReadOnlySpan<string> input)
    {
        ParseInput(input);
    }

    public long Result(ReadOnlySpan<string> input)
    {
        int sim = 0;
            int minSolution =int.MaxValue;
        //while (sim<1_000) 
        //{
        string inputString="";
        int solution = 0;

            foreach (var line in input)
            {
                //GlobalLog.LogLine($" ------------ {line} --------------");
                DoorController door = new DoorController();
                List<RobotController> robots = new List<RobotController>();

                
                for (int i = 0; i < 2; i++)
                {
                    robots.Add(new RobotController());
                }


                foreach (char code in line)
                {
                    var inputList = door.ReadNextInput(code);
                    
                    foreach (var robot in robots)
                    {
                        inputList = robot.ReadNextInput(inputList);
                        //GlobalLog.LogLine($"robot : {inputList.Count} ");
                        //GlobalLog.LogLine($"robot : {string.Join("", inputList)} ");
                    }

                    inputString += string.Join("", inputList);
                }
                solution += inputString.Length * int.Parse(line.Substring(0, line.Length - 1));

                //GlobalLog.LogLine($"{inputString}");
                //GlobalLog.LogLine($"{inputString.Length}");
                //GlobalLog.LogLine($"solution: {solution}");
                inputString = string.Empty;
            }

            if(Math.Min(minSolution, solution) < minSolution)
            {
                GlobalLog.LogLine($"{sim}");
                minSolution = Math.Min( minSolution, solution);
                GlobalLog.LogLine($"{minSolution}");
            }
            sim++;
            GlobalLog.LogLine($"----------------Next Sim---------------");
            //if (sim%10 ==0) GlobalLog.LogLine($"solution: {sim}");
        //}
        return minSolution;
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
            if(OptimalPath.TryGetValue(inputs, out var result))
            {
                return result;
            }

            var posReset=Position;
            List<char> minDirInputs = new List<char>();
            int bestSolu = int.MaxValue;
            int sim = 0;
            while (sim < 1000)
            {
                List<char> dirInputs = new List<char>();
                Position = posReset;
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

                        Random test = new Random();
                        var yeah = test.NextDouble();
                        //GlobalLog.LogLine($"rng: {yeah}");
                        if (yeah >= 0.5)
                        {
                            dirInputs.AddRange(lrList);
                            dirInputs.AddRange(udList);
                        }
                        else
                        {
                            dirInputs.AddRange(udList);
                            dirInputs.AddRange(lrList);
                        }

                    }
                    dirInputs.Add('A');

                    Position = targetPos;
                }
                //228442
                var solu00 = CheckBestNextInput(dirInputs, Position);
                //var solu01 = CheckBestNextInput(solu00.list, solu00.po);
                if (solu00.list.Count < bestSolu)
                {
                    bestSolu = solu00.list.Count;
                    minDirInputs = dirInputs;
                }
                sim++;
            }

            if(!OptimalPath.TryAdd(inputs, minDirInputs))
            {
                GlobalLog.LogLine($"OptimalPath.TryAdd: Error");

            }


            return minDirInputs;
        }

        private (List<char> list, (int Y,int X)po) CheckBestNextInput(List<char> inputs, (int Y, int X) pos)
        {
            var position = pos;
            List<char> dirInputs = new List<char>();

            foreach (char dirInp in inputs)
            {
                var targetPos = ControllerDic[dirInp];
                var yDiff = targetPos.Y - position.Y;
                var xDiff = targetPos.X - position.X;
                var lrList = GetLeftRightInput(xDiff);
                var udList = GetUpDownInput(yDiff);

                if (position.Y + yDiff == 0 && yDiff != 0 && position.X == 0)
                {
                    dirInputs.AddRange(lrList);
                    dirInputs.AddRange(udList);
                }
                else if (position.X + xDiff == 0 && xDiff != 0 && position.Y == 0)
                {
                    dirInputs.AddRange(udList);
                    dirInputs.AddRange(lrList);
                }
                else
                {
                    Random test = new Random();
                    var yeah = test.NextDouble();
                    //GlobalLog.LogLine($"rng: {yeah}");
                    if (yeah >= 0.5)
                    {
                        dirInputs.AddRange(lrList);
                        dirInputs.AddRange(udList);
                    }
                    else
                    {
                        dirInputs.AddRange(udList);
                        dirInputs.AddRange(lrList);
                    }

                }
                dirInputs.Add('A');

                position = targetPos;
            }

            return (dirInputs, position);
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
            if (OptimalPath.TryGetValue( new List<char>{ input} , out var result))
            {
                return result;
            }

            var posReset = Position;
            List<char> minDirInputs = new List<char>();
            int bestSolu = int.MaxValue;
            int sim = 0;
            while (sim < 100)
            {
                List<char> dirInputs = new List<char>();
                Position = posReset;
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
                else
                {
                    Random test = new Random();
                    var yeah = test.NextDouble();
                    GlobalLog.LogLine($"rng: {yeah}");
                    if (yeah >= 0.5)
                    {
                        dirInputs.AddRange(lrList);
                        dirInputs.AddRange(udList);
                    }
                    else
                    {
                        dirInputs.AddRange(udList);
                        dirInputs.AddRange(lrList);
                    }

                }
                dirInputs.Add('A');

                Position = targetPos;
                var solu00 = CheckBestNextInput(dirInputs);
                //var solu01 = CheckBestNextInput(solu00.list, solu00.po);
                if (solu00 < bestSolu)
                {
                    bestSolu = solu00;
                    minDirInputs = dirInputs;
                }
                sim++;

            }
            if (!OptimalPath.TryAdd(new List<char> { input }, minDirInputs))
            {
                GlobalLog.LogLine($"OptimalPath.TryAdd: Error");

            }

            return minDirInputs;

        }
        private int CheckBestNextInput(List<char> inputs)
        {
            RobotController robo = new RobotController();
            var result = robo.ReadNextInput(inputs);

            return result.Count;
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

