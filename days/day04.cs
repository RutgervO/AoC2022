namespace AOC.days;

internal class Day04 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "04_t1.txt"), 2);
        AddRun("Part 1", () => RunPart(1, "04.txt"));
        AddRun("Test 2", () => RunPart(2, "04_t1.txt"), 4);
        AddRun("Part 2", () => RunPart(2, "04.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputValues = GetListOfLines(inputName)
            .Select(line => line.Split(',', '-')
            .Select(int.Parse)
            .ToArray());
        
        if (part == 1)
            return inputValues
                .Count(x => (x[0] >= x[2] && x[1] <= x[3]) || (x[0] <= x[2] && x[1] >= x[3]));

        return inputValues
            .Count(x => (x[0] <= x[2] && x[1] >= x[2]) || (x[0] <= x[3] && x[1] >= x[3])
                            || (x[2] <= x[0] && x[3] >= x[1]) || (x[0] <= x[2] && x[1] >= x[3]));
    }
}
