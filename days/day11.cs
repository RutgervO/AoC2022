namespace AOC.days;

internal class Day11 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "11_t1.txt"), 10605);
        AddRun("Part 1", () => RunPart(1, "11.txt"));
        AddRun("Test 2", () => RunPart(2, "11_t1.txt"), 2713310158);
        AddRun("Part 2", () => RunPart(2, "11.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var numRounds = part == 1 ? 20 : 10000;
        var monkeys = GetListOfLines(inputName)
            .Select(x => x.Split(new[] { ' ', ':', ',' }, StringSplitOptions.RemoveEmptyEntries))
                .Chunk(7).Select(x => new Monkey(x)).ToArray();
        var limit = monkeys.Select(m => m.Division).Aggregate((x, y) => x * y);

        foreach (var _ in Enumerable.Range(1, numRounds))
        {
            foreach (var m in monkeys)
            {
                var todo = m.Items.ToList();
                m.Items.Clear();
                foreach (var oldItem in todo)
                {
                    var item = oldItem;
                    m.Count += 1;
                    if (m.DoSquare) item *= item;
                    else item = (item * m.Multiplier) + m.Addition;

                    if (part == 1) item /= 3;
                    item %= limit;

                    monkeys[m.NextMonkey[item % m.Division == 0]].Items.Enqueue(item);
                }
            }
        }

        return monkeys.Select(m => m.Count).OrderByDescending(x => x).Take(2).Aggregate((x, y) => x * y);
    }

    private class Monkey
    {
        public Queue<long> Items { get; } = new();
        public int Multiplier { get; set; } = 1;
        public int Addition { get; set; }
        public bool DoSquare { get; set; }
        public int Division { get; set; }
        public Dictionary<bool, int> NextMonkey { get; set; } = new();
        public long Count { get; set; }

        public Monkey(string[][] line)
        {
            foreach (var item in line[1].Skip(2)) Items.Enqueue(int.Parse(item));
            if (!(DoSquare = line[2][5] == "old"))
            {
                var value = int.Parse(line[2][5]);
                switch (line[2][4])
                {
                    case "+":
                        Addition = value;
                        break;
                    case "*":
                        Multiplier = value;
                        break;
                }
            }
            Division = int.Parse(line[3][3]);
            NextMonkey[true] = int.Parse(line[4][5]);
            NextMonkey[false] = int.Parse(line[5][5]);
        }
    }
}
