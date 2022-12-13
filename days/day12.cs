using AOC.util;

namespace AOC.days;

internal class Day12 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "12_t1.txt"), 31);
        AddRun("Part 1", () => RunPart(1, "12.txt"));
        AddRun("Test 2", () => RunPart(2, "12_t1.txt"), 29);
        AddRun("Part 2", () => RunPart(2, "12.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var board = new Board2D<char>(GetListOfLines(inputName), x => x[0], false);

        var start = board.CoordinatesThatMatch(x => x == 'S').Single();
        var end = board.CoordinatesThatMatch(x => x == 'E').Single();
        board.Board[start] = 'a';
        board.Board[end] = 'z';

        var shortest = new Dictionary<Coordinate, long> {{end, 0}};

        long changes = 1;
        while (changes > 0)
        {
            changes = 0;
            foreach (var a in shortest.ToList())
                foreach (var neighbour in board.Neighbours(a.Key))
                    if (board.Board[a.Key] - board.Board[neighbour] < 2)
                        if (!shortest.ContainsKey(neighbour) || shortest[neighbour] > a.Value + 1)
                            changes = shortest[neighbour] = a.Value + 1;
        }

        if (part == 1) return shortest[start];
        
        return board.Board
            .Where(x => x.Value == 'a' && shortest.ContainsKey(x.Key))
            .Select(x => shortest[x.Key])
            .Min();
    }
}