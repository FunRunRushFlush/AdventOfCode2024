using Day01;
using System.Diagnostics;

var day01Input = new Input();
#region Part01

Stopwatch sw = Stopwatch.StartNew();
int[] numbers = day01Input.InputString
    .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();


int[] number01 = new int[numbers.Length / 2];
int[] number02 = new int[numbers.Length / 2];

int ind = 0;
for (int i = 0; i < numbers.Length; i+=2)
{
  
    number01[ind] = numbers[i];
    number02[ind] = numbers[i+1];
    ind++;
}

Array.Sort(number01);
Array.Sort(number02);
int distance = 0;
for (int i = 0;i < number01.Length;i++)
{
    int localDistance = Math.Abs(number01[i] - number02[i]);
    distance += localDistance;
}

sw.Stop();
Console.WriteLine($"Distance {distance}, Time {sw.ElapsedMilliseconds}");
#endregion

#region Part02

#endregion