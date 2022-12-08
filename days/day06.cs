namespace AOC.days;

internal class Day06 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "06_t1.txt"), 7);
        AddRun("Test 2", () => RunPart(1, "06_t2.txt"), 5);
        AddRun("Test 3", () => RunPart(1, "06_t3.txt"), 6);
        AddRun("Test 4", () => RunPart(1, "06_t4.txt"), 10);
        AddRun("Test 5", () => RunPart(1, "06_t5.txt"), 11);
        AddRun("Part 1", () => RunPart(1, "06.txt"));
        AddRun("Test 6", () => RunPart(2, "06_t6.txt"), 19);
        AddRun("Test 7", () => RunPart(2, "06_t7.txt"), 23);
        AddRun("Test 8", () => RunPart(2, "06_t8.txt"), 23);
        AddRun("Test 9", () => RunPart(2, "06_t9.txt"), 29);
        AddRun("Test 10", () => RunPart(2, "06_t10.txt"), 26);
        AddRun("Part 2", () => RunPart(2, "06.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var input = GetListOfLines(inputName).Single(_ => true);
        var marker = 4 + 10 * (part - 1);
        for (var c = marker; c < input.Length; c++)
            if (input.Substring(c - marker, marker).ToHashSet().Count() == marker)
                return c;
        return 0;
    }
}