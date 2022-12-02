namespace AOC.days;

internal class Day02 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "02_t1.txt"), 15);
        AddRun("Part 1", () => RunPart(1, "02.txt"));
        AddRun("Test 2", () => RunPart(2, "02_t1.txt"), 12);
        AddRun("Part 2", () => RunPart(2, "02.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        if (part == 1)
        {
            var scores = new Dictionary<string, int>()
            {
                {"A X", 1 + 3},
                {"A Y", 2 + 6},
                {"A Z", 3 + 0},
                {"B X", 1 + 0},
                {"B Y", 2 + 3},
                {"B Z", 3 + 6},
                {"C X", 1 + 6},
                {"C Y", 2 + 0},
                {"C Z", 3 + 3},
            };
            return lines.Select(x => scores[x]).Sum();
        }
        else
        {
            var scores = new Dictionary<string, int>()
            {
                {"A X", 3 + 0},
                {"A Y", 1 + 3},
                {"A Z", 2 + 6},
                {"B X", 1 + 0},
                {"B Y", 2 + 3},
                {"B Z", 3 + 6},
                {"C X", 2 + 0},
                {"C Y", 3 + 3},
                {"C Z", 1 + 6},
            };
            return lines.Select(x => scores[x]).Sum();
        }
    }
}
