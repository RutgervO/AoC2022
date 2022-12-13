namespace AOC.days;

internal class Day13 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "13_t1.txt"), 13);
        AddRun("Part 1", () => RunPart(1, "13.txt"));
        AddRun("Test 2", () => RunPart(2, "13_t1.txt"), 140);
        AddRun("Part 2", () => RunPart(2, "13.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        if (part == 1) return GetListOfLines(inputName)
            .Chunk(3)
            .Select((p, i) => new {pair = p.Take(2).ToArray(), value = i + 1})
            .Where(p => ListCompare(p.pair[0], p.pair[1]) < 0)
            .Select(p => p.value)
            .Sum();

        return GetListOfLines(inputName)
            .Where(l => l.Length > 0)
            .Concat(new List<string> { "[[2]]", "[[6]]"})
            .OrderBy(l => l, new ListComparer())
            .Select((x, i) => new {list = x, value = i + 1})
            .Where(x => x.list is "[[2]]" or "[[6]]")
            .Select(x => x.value)
            .Aggregate((x, y) => x * y);
    }

    private class ListComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            return ListCompare(x!, y!);
        }
    }

    private static int ListCompare(string left, string right)
    {
        if (left.Length == 0 || right.Length == 0) return left.Length.CompareTo(right.Length);
        if (left[0] != '[' && right[0] != '[') return int.Parse(left).CompareTo(int.Parse(right));
        var leftList = ParseList(left);
        var rightList = ParseList(right);
        for (var i = 0; i < Math.Min(leftList.Length, rightList.Length); i++)
        {
            var c = ListCompare(leftList[i], rightList[i]);
            if (c != 0) return c;
        }
        return leftList.Length.CompareTo(rightList.Length);
    }

    private static string[] ParseList(string list)
    {
        var result = new List<string>();
        var nesting = 0;
        var element = "";
        var start = list.Length > 0 && list[0] == '[' ? 1 : 0;
        foreach (var c in list.Substring(start, list.Length - 2 * start))
        {
            if (c == ',' && nesting == 0)
            {
                result.Add(element);
                element = "";
            }
            else
            {
                if (c == '[') nesting += 1;
                else if (c == ']') nesting -= 1;
                element += c;
            }
        }
        result.Add(element);
        return result.ToArray();
    }
}