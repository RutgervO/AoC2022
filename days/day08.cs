using AOC.util;

namespace AOC.days;

internal class Day08 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "08_t1.txt"), 21);
        AddRun("Part 1", () => RunPart(1, "08.txt"));
        AddRun("Test 2", () => RunPart(2, "08_t1.txt"), 8);
        AddRun("Part 2", () => RunPart(2, "08.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputValues = GetListOfLines(inputName);
        var board = new Board2D<int>(inputValues, int.Parse, false);
        if (part == 1)
            return board
                .AllCoordinates()
                .Count(c => Direction.AllDirections()
                    .Select(d => board.FindLargerOrEqualValue(c, d)).Any(lev => lev == false));
        return board
            .AllCoordinates()
            .Max(c => Direction.AllDirections()
                .Select(d => board.GetDistanceToLargerOrEqualValue(c, d))
                .Aggregate((x, y) => x * y));
    }
}