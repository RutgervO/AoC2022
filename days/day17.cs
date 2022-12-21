using AOC.util;

namespace AOC.days;

internal class Day17 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "17_t1.txt"), 3068);
        AddRun("Part 1", () => RunPart(1, "17.txt"));
        AddRun("Test 2", () => RunPart(2, "17_t1.txt"), 1514285714288);
        AddRun("Part 2", () => RunPart(2, "17.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var moves = GetListOfLines(inputName).Single().SelectMany(x => new[] {x, 'v'}).ToArray();
        var moveLength = moves.Length;
        var shapes = new HashSet<Coordinate>[]
        {
            new() // ____
            {
                new Coordinate(0, 0),
                new Coordinate(1, 0),
                new Coordinate(2, 0),
                new Coordinate(3, 0)
            },
            new() // +
            {
                new Coordinate(1, 0),
                new Coordinate(0, 1),
                new Coordinate(1, 1),
                new Coordinate(2, 1),
                new Coordinate(1, 2)
            },
            new() // angle
            {
                new Coordinate(0, 0),
                new Coordinate(1, 0),
                new Coordinate(2, 0),
                new Coordinate(2, 1),
                new Coordinate(2, 2)
            },
            new() // |
            {
                new Coordinate(0, 0),
                new Coordinate(0, 1),
                new Coordinate(0, 2),
                new Coordinate(0, 3)
            },
            new() // square
            {
                new Coordinate(0, 0),
                new Coordinate(1, 0),
                new Coordinate(0, 1),
                new Coordinate(1, 1)
            }
        };
        var directions = new Dictionary<char, Coordinate>()
        {
            {'<', new Coordinate(-1, 0)},
            {'>', new Coordinate(1, 0)},
            {'v', new Coordinate(0, -1)}
        };
        var width = 7;
        var maxRocks = part == 1 ? 2022 : 1000000000000;
        var cave = Enumerable.Range(0, width).Select(x => new Coordinate(x, 0)).ToHashSet();
        var patterns = new Dictionary<string, Tuple<long, long, long>>();

        long move = 0;
        long shiftedHeight = 0;

        for (long rockNumber = 0; rockNumber < maxRocks; rockNumber++)
        {
            var maxHeight = cave.Select(c => c.Y).Max();

            var pattern = string.Join("",
                Enumerable.Range(maxHeight - 9, 10).SelectMany(y =>
                    Enumerable.Range(0, width).Select(x => cave.Contains(new Coordinate(x, y)) ? '#' : ',')));
            if (patterns.ContainsKey(pattern))
            {
                var oldValues = patterns[pattern];
                var heightDiff = maxHeight + shiftedHeight - oldValues.Item1;
                var moveDiff = move - oldValues.Item2;
                var rockDiff = rockNumber - oldValues.Item3;

                if (moveDiff % moveLength == 0)
                {
                    // found a pattern! Do this until we are almost done....
                    if (rockNumber + rockDiff < maxRocks)
                    {
                        var multiplier = (maxRocks - rockNumber) / rockDiff;
                        rockNumber += multiplier * rockDiff;
                        shiftedHeight += multiplier * heightDiff;
                        move += multiplier * moveDiff;
                    }
                }
            }
            else
            {
                patterns.Add(pattern, new Tuple<long, long, long>(maxHeight + shiftedHeight, move, rockNumber));
            }
            
            var start = new Coordinate(2, maxHeight + 4);
            var rock = shapes[rockNumber % shapes.Length].Select(x => x.Add(start)).ToHashSet();

            while (true)
            {
                //PrintCave(rock);
                var m = moves[move++ % moves.Length];
                var direction = directions[m];
                var newRock = rock.Select(x => x.Add(direction)).ToHashSet();
                var intersects = newRock.Intersect(cave).Any();
                if (m != 'v')
                {
                    if (intersects || newRock.Any(x => x.X < 0 || x.X >= width)) continue;
                } else if (intersects) {
                    cave.UnionWith(rock);
                    break;
                }
                rock = newRock;
            }

            if (maxHeight > 40)
            {
                // Keep the cave data small by shifting and discarding the bottom
                var shift = maxHeight - 40;
                shiftedHeight += shift;
                cave = cave.Where(c => c.Y >= shift).Select(c => new Coordinate(c.X, c.Y - shift)).ToHashSet();
            }
        }
        return shiftedHeight + cave.Select(c => c.Y).Max();

        void PrintCave(HashSet<Coordinate> moving)
        {
            Console.WriteLine();
            for (var y = 30; y > 0; y--)
            {
                Console.WriteLine("|" + String.Join("", Enumerable.Range(0, width).Select(x => cave.Contains(new Coordinate(x, y)) ? '#' :
                    moving.Contains(new Coordinate(x, y)) ? '@' : '.')) + '|');
            }
            Console.WriteLine("+" + String.Join("", Enumerable.Repeat('-', width)) + "+");
        }
    }
}