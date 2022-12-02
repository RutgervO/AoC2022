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
        var lines = GetListOfLines(inputName)
                .Select(x => new {L = Translate(x[0]), R = Translate(x[2])});
        
        if (part == 1) return lines.Select(x => x.R + 1 + ((1 - x.L + x.R) % 3) * 3).Sum();
        
        return lines.Select(x => (x.L + x.R + 2) % 3 + 1 + x.R * 3).Sum();

        int Translate(char i)
        {
            return (i - 'A') % ('X' - 'A');
        }
    }
}
