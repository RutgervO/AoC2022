namespace AOC.days;

internal class Day05 : Day<string>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "05_t1.txt"), "CMZ"); 
        AddRun("Part 1", () => RunPart(1, "05.txt"));
        AddRun("Test 2", () => RunPart(2, "05_t1.txt"), "MCD");
        AddRun("Part 2", () => RunPart(2, "05.txt"));
    }

    public override string RunPart(int part, string inputName)
    {
        var input= GetListOfLines(inputName);
        var numColumns = (input.First().Length + 1) / 4;
        var columns = Enumerable.Range(0, numColumns).Select(x => new Stack<char>()).ToArray();
        foreach (var line in input.TakeWhile(x => !char.IsDigit(x[1])).Reverse())
        {
            foreach (var col in Enumerable.Range(0, numColumns))
            {
                var c = line[col * 4 + 1];
                if (c != ' ')
                {
                    columns[col].Push(c);
                }
            }
        }

        foreach (var (count, from, to) in input
                     .Where(x => x.StartsWith("move"))
                     .Select(x => x.Split(' ').Chunk(2).Select(x => int.Parse(x[1])).ToList())
                     .Select(x => (x[0], x[1] - 1, x[2] - 1))
                )
        {
            if (part == 1)
            {
                foreach (var _ in Enumerable.Range(0, count))
                {
                    columns[to].Push(columns[from].Pop());
                }
            }
            else
            {
                var crane = new Stack<char>();
                foreach (var _ in Enumerable.Range(0, count))
                {
                    crane.Push(columns[from].Pop());
                }

                foreach (var _ in Enumerable.Range(0, count))
                {
                    columns[to].Push(crane.Pop());
                }
            }
        }

        return new string(columns.Select(x => x.Peek()).ToArray());
    }
}