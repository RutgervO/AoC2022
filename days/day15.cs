using System.Xml;
using AOC.util;

namespace AOC.days;

internal class Day15 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "15_t1.txt"), 26);
        AddRun("Part 1", () => RunPart(2, "15.txt"));
        AddRun("Test 2", () => RunPart(3, "15_t1.txt"), 56000011);
        AddRun("Part 2", () => RunPart(4, "15.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var startCoordinate = part == 1 ? 10 : part == 2 ? 2000000 : 0;
        var endCoordinate = part == 1 ? 11 : part == 2 ? 2000001 : part == 3 ? 20 : 4000000;
        var inputValues = GetListOfLines(inputName)
            .Select(l => l.Split(new char[] { '=', ',', ':' }))
            .Select(l => new
            {
                sensor = new Coordinate(int.Parse(l[1]), int.Parse(l[3])),
                beacon = new Coordinate(int.Parse(l[5]), int.Parse(l[7])),
            })
            .Select(l => new
            {
                sensor = l.sensor,
                beacon = l.beacon,
                distance = l.sensor.Manhattan(l.beacon)
            }).ToList();
        var beacons = inputValues.Select(l => l.beacon).ToHashSet();
        for (var targetLine = startCoordinate; targetLine < endCoordinate; targetLine++)
        {
            var ranges = new HashSet<Tuple<int, int>>();
            foreach (var l in inputValues)
            {
                var dy = Math.Abs(targetLine - l.sensor.Y);
                if (dy <= l.distance)
                {
                    var dx = l.distance - dy;
                    var sx = l.sensor.X - dx;
                    var ex = l.sensor.X + dx;
                    foreach (var range in ranges.ToList())
                    {
                        if (ex >= range.Item1 && ex <= range.Item2) ex = range.Item2;
                        if (sx >= range.Item1 && sx <= range.Item2) sx = range.Item1;
                        if (sx <= range.Item1 && ex >= range.Item2) ranges.Remove(range);
                    }
                    ranges.Add(new Tuple<int, int>(sx, ex));
                }
            }

            if (part <= 2) return ranges.Select(x => x.Item2 - x.Item1 + 1).Sum() - beacons.Count(b => b.Y == targetLine);

            var finder = 0;
            var count = 0;
            long hit = 0;
            while (finder < endCoordinate)
            {
                var inRange = ranges.Where(x => x.Item1 <= finder && x.Item2 >= finder).ToList();
                if (inRange.Count == 0)
                {
                    count += 1;
                    hit = finder;
                    if (count > 1) break;
                    finder++;
                }
                else
                {
                    finder = inRange.First().Item2 + 1;
                }
            }

            if (count == 1) return (long)(hit * 4000000 + targetLine);
        }

        return 0;
    }
}