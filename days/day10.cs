namespace AOC.days;

internal class Day10 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "10_t1.txt"), 13140); // huh.. test result is invalid..
        AddRun("Part 1", () => RunPart(1, "10.txt"));
        AddRun("Demo 2", () => RunPart(2, "10_t1.txt"));
        AddRun("Part 2", () => RunPart(2, "10.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var commands = GetListOfLines(inputName).Select(l => l.Split());

        if (part == 1)
            return GetValues(commands).Select((x, i) => (i - 19) % 40 == 0 ? x * (i + 1) : 0).Sum();

        Console.WriteLine();
        Console.WriteLine(string.Join("\n", GetValues(commands)
            .Select((x, i) => ((x >= (i % 40) - 1) && (x <= (i % 40) + 1)) ? '█' : ' ')
            .Chunk(40).Select(x => string.Join("", x))));
        return 0;

        IEnumerable<int> GetValues(IEnumerable<string[]>commands)
        {
            var current = 1;
            foreach (var command in commands)
            {
                yield return current;
                if (command.Length > 1)
                {
                    yield return current;
                    current += int.Parse(command[1]);
                }
            }
        }
    }
}