namespace AOC.days;

internal class Day01 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "01_t1.txt"), 24000); // ToDo
        AddRun("Part 1", () => RunPart(1, "01.txt"));
        AddRun("Test 2", () => RunPart(2, "01_t1.txt"), 45000); // ToDo
        AddRun("Part 2", () => RunPart(2, "01.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputValues = GetListOfLines(inputName);
        var i = 0;
        var totals = inputValues
            .GroupBy(x => x == "" ? i++ : i, x => x == "" ? 0 : int.Parse(x))
            .Select(x => x.Sum());

        if (part == 1)
            return totals.Max();

        return totals.OrderByDescending(x => x).Take(3).Sum();
    }
}