using AOC.util;

namespace AOC.days;

internal class Day14 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "14_t1.txt"), 24);
        AddRun("Part 1", () => RunPart(1, "14.txt"));
        AddRun("Test 2", () => RunPart(2, "14_t1.txt"), 93); 
        AddRun("Part 2", () => RunPart(2, "14.txt"));
    }

    public override long RunPart(int part, string inputName)
    {

        var board = new Dictionary<Coordinate, char>();
        foreach (var lines in GetListOfLines(inputName)
                     .Select(x => x.Split(new char[] {',', ' ', '-', '>'}, StringSplitOptions.RemoveEmptyEntries)
                         .Select(int.Parse).Chunk(2).ToArray().Select(x => new Coordinate(x[0], x[1]))))
        {
            var current = lines.First();
            board[current] = '#';
            foreach (var next in lines.Skip(1))
            {
                var direction = next.Subtract(current).AbsMax(1);
                while (current != next)
                {
                    current = current.Add(direction);
                    board[current] = '#';
                }
            }
        }

        var maxY = board.Keys.Select(x => x.Y).Max();
        var sandCount = 0;
        var start = new Coordinate(500, 0);
        var sand = start;
        while (true)
        {
            if (part == 1 && sand.Y > maxY) return sandCount;
            if (part == 2 && board.ContainsKey(start)) return sandCount;
            sand = sand.Add(new Coordinate(0, 1));
            if (!board.ContainsKey(sand) && (part == 1 || sand.Y < maxY + 2)) continue;
            sand = sand.Add(new Coordinate(-1, 0));
            if (!board.ContainsKey(sand) && (part == 1 || sand.Y < maxY + 2)) continue;
            sand = sand.Add(new Coordinate(2, 0));
            if (!board.ContainsKey(sand) && (part == 1 || sand.Y < maxY + 2)) continue;
            board[sand.Add(new Coordinate(-1, -1))] = 'o';
            sandCount += 1;
            sand = start;
        }
    }
}