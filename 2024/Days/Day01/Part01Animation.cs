using Spectre.Console;
using System.Media;

namespace Year_2024.Days.Day01
{
    public class Part01Animation : IPart
    {
        public string Result(Input input)
        {
            List<int> list01 = new List<int>();
            List<int> list02 = new List<int>();

            foreach (var item in input.SpanLines)
            {
                var res = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                list01.Add(int.Parse(res[0]));
                list02.Add(int.Parse(res[1]));
            }

            string FormatText(string text, int width) =>
    text.PadLeft((width + text.Length) / 2).PadRight(width);

            string CreateScrollingText(List<int> list, int currentIndex) =>
                string.Join("\n", list.Skip(currentIndex).Take(10)) + (list.Count > currentIndex + 10 ? "\n..." : "");

            var panel1 = new Panel(FormatText("Empty", 20)).Header("List01", Justify.Center);

            var panel2 = new Panel(FormatText("Empty", 20)).Header("List02", Justify.Center);
            var panel3 = new Panel(FormatText("0", 20)).Header("Distance", Justify.Center);
            var panel4 = new Panel(FormatText("0", 20)).Header("Total Distance", Justify.Center);
            var textBox = new Panel("Steps to solve Day01_Part01: \n[Blink]Press any key to start...[/]")
                .Header("Description", Justify.Center)
                .Expand();

            var layout = new Rows(
                new Columns(new[] { panel1, panel2, panel3, panel4 }),
                textBox
            );

            int distance = 0;
            int delay = 250;
            int minDelay = 1;
            float acceleration = 0.9f;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(basePath, "Days\\Day01\\Yu-Gi-Oh_Life_Points_Loop.wav");
            string fileFinalPath = Path.Combine(basePath, "Days\\Day01\\Yu-Gi-Oh_Life_Points_FinalPart.wav");

            using var soundPlayer = new SoundPlayer(filePath);
            using var soundFinalPlayer = new SoundPlayer(fileFinalPath);


            AnsiConsole.Live(layout)
                .Start(ctx =>
                {
                    ctx.Refresh();
                    Console.ReadKey(true);

                    textBox = new Panel("Parsing the input in to two lists...\n[Blink]Press any key...[/]")
                                .Header("Description", Justify.Center)
                                .Expand();

                    ctx.UpdateTarget(new Rows(
               new Columns(new[] { panel1, panel2, panel3, panel4 }),
               textBox
           ));
                    Console.ReadKey(true);

                    panel1 = new Panel(FormatText(CreateScrollingText(list01, 0), 20)).Header("List01", Justify.Center);
                    panel2 = new Panel(FormatText(CreateScrollingText(list02, 0), 20)).Header("List02", Justify.Center);
                    panel3 = new Panel(FormatText("Calculating...", 20)).Header("Distance", Justify.Center);
                    panel4 = new Panel(FormatText("0", 20)).Header("Total Distance", Justify.Center);


                    textBox = new Panel("Ordering the lists...\n[Blink]Press any key...[/]")
                             .Header("Description", Justify.Center)
                             .Expand();
                    ctx.UpdateTarget(new Rows(
                        new Columns(new[] { panel1, panel2, panel3, panel4 }),
                        textBox
                    ));

                    list01.Sort();
                    list02.Sort();

                    Console.ReadKey(true);

                    panel1 = new Panel(FormatText(CreateScrollingText(list01, 0), 20)).Header("List01", Justify.Center);
                    panel2 = new Panel(FormatText(CreateScrollingText(list02, 0), 20)).Header("List02", Justify.Center);
                    panel3 = new Panel(FormatText("Calculating...", 20)).Header("Distance", Justify.Center);
                    panel4 = new Panel(FormatText("0", 20)).Header("Total Distance", Justify.Center);
                    ctx.UpdateTarget(new Rows(
                         new Columns(new[] { panel1, panel2, panel3, panel4 }),
                         textBox
                     ));

                    textBox = new Panel("Start calculating distance...\n[Blink]Press any key...[/]")
                             .Header("Description", Justify.Center)
                            .Expand();

                    ctx.UpdateTarget(new Rows(
                             new Columns(new[] { panel1, panel2, panel3, panel4 }),
                             textBox
                         ));

                    Console.ReadKey(true);
                    soundPlayer.PlayLooping();

                    for (int i = 0; i < list01.Count; i++)
                    {
                        var dist = Math.Abs(list01[i] - list02[i]);
                        distance += dist;

                        panel1 = new Panel(FormatText(CreateScrollingText(list01, i), 20)).Header("List01", Justify.Center);
                        panel2 = new Panel(FormatText(CreateScrollingText(list02, i), 20)).Header("List02", Justify.Center);
                        panel3 = new Panel(FormatText($"+{dist}", 20)).Header("Distance", Justify.Center);
                        panel4 = new Panel(FormatText($"{distance}", 20)).Header("Total Distance", Justify.Center);
                        textBox = new Panel($"Processing item {i + 1}/{list01.Count}...")
                            .Header("Description", Justify.Center)
                            .Expand();

                        ctx.UpdateTarget(new Rows(
                            new Columns(new[] { panel1, panel2, panel3, panel4 }),
                            textBox
                        ));

                        Thread.Sleep(delay);
                        delay = Math.Max(minDelay, (int)(delay * acceleration));
                    }
                    soundFinalPlayer.Play();

                    textBox = new Panel($"Calculation complete! \n \nResult: [underline][Bold]Total Distance = {distance}[/][/]")
                        .Header("Description", Justify.Center)
                        .Expand();

                    ctx.UpdateTarget(new Rows(
                        new Columns(new[] { panel1, panel2, panel3, panel4 }),
                        textBox
                    ));
                });

            return distance.ToString();
        }
    }
}
