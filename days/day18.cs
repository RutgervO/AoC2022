using AOC.util;

namespace AOC.days;

internal class Day18 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "18_t1.txt"), 64);
        AddRun("Part 1", () => RunPart(1, "18.txt"));
        AddRun("Test 2", () => RunPart(2, "18_t1.txt"), 58);
        AddRun("Part 2", () => RunPart(2, "18.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var scan = GetListOfLines(inputName).Select(x => x.Split(',').Select(int.Parse).ToArray())
            .Select(x => new Coordinate3D(x[0], x[1], x[2])).ToHashSet();
        var directions = new[]
        {
            new Coordinate3D(-1, 0, 0),
            new Coordinate3D(1, 0, 0),
            new Coordinate3D(0, -1, 0),
            new Coordinate3D(0, 1, 0),
            new Coordinate3D(0, 0, -1),
            new Coordinate3D(0, 0, 1)
        };

        if (part == 1) return scan.Select(c => directions.Count(d => !scan.Contains(c.Add(d)))).Sum();
        
        var maxCoord = scan.Select(c => Math.Max(Math.Max(c.X, c.Y), c.Z)).Max() + 1;
        var negative = new HashSet<Coordinate3D>
        {
            new Coordinate3D(0, 0, 0)
        };
        var updates = 1;
        while (updates > 0)
        {
            updates = 0;
            foreach (var c in negative.ToList())
            {
                foreach (var d in directions)
                {
                    var n = c.Add(d);
                    if (n.X >= 0 && n.X <= maxCoord && n.Y >= 0 && n.Y <= maxCoord && n.Z >= 0 && n.Z <= maxCoord)
                    {
                        if (!negative.Contains(n) && !scan.Contains(n))
                        {
                            negative.Add(n);
                            updates++;
                        }
                    }
                }
            }
        }
        
        for (var x = 0; x <= maxCoord; x++) {
            for (var y = 0; y <= maxCoord; y++) {
                for (var z = 0; z <= maxCoord; z++)
                {
                    var n = new Coordinate3D(x, y, z);
                    if (!negative.Contains(n) && !scan.Contains(n)) scan.Add(n);
                }
            }
        }
        return scan.Select(c => directions.Count(d => !scan.Contains(c.Add(d)))).Sum();

    }
}