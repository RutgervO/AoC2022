using AOC.util;

namespace AOC.days;

internal class Day09 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "09_t1.txt"), 13);
        AddRun("Part 1", () => RunPart(1, "09.txt"));
        AddRun("Test 2", () => RunPart(2, "09_t1.txt"), 1);
        AddRun("Test 2", () => RunPart(2, "09_t2.txt"), 36);
        AddRun("Part 2", () => RunPart(2, "09.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var moves = GetListOfLines(inputName)
            .Select(line => line.Split(" "))
            .Select(parts => new { direction = new Direction(parts[0]).ToCoordinate(), distance = int.Parse(parts[1])});

        var head = new Coordinate(0, 0);
        var numKnots = 8 * part - 7;
        var knots = Enumerable.Range(0, numKnots).Select(_ => new Coordinate(0, 0)).ToArray();
        var visited = new HashSet<Coordinate> { head };
        foreach (var move in moves)
        {
            foreach (var _ in Enumerable.Range(0, move.distance))
            {
                var prev = head = head.Add(move.direction);
                foreach (var tailNumber in Enumerable.Range(0, numKnots))
                {
                    var knot = knots[tailNumber];
                    var d = prev.Subtract(knot);
                    prev = Math.Abs(d.X) >= 2 || Math.Abs(d.Y) >= 2 ? knots[tailNumber] = knot.Add(d.AbsMax(1)) : knot;
                }
                visited.Add(prev);
            }
        }
        return visited.Count;
    }
}