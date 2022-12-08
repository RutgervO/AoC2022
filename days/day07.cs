namespace AOC.days;

internal class Day07 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "07_t1.txt"), 95437);
        AddRun("Part 1", () => RunPart(1, "07.txt"));
        AddRun("Test 2", () => RunPart(2, "07_t1.txt"), 24933642);
        AddRun("Part 2", () => RunPart(2, "07.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputValues = GetListOfLines(inputName);
        var files = new Dictionary<string, long>();  // full filename -> filesize
        var directories = new HashSet<string>();
        var currentDir = "/";
        foreach (var line in inputValues.Select(x => x.Split(" ")))
        {
            if (line[0] == "$")
            {
                switch (line[1])
                {
                    case "cd":
                        switch (line[2])
                        {
                            case "/":
                                currentDir = "/";
                                break;
                            case "..":
                                currentDir =
                                    string.Concat(
                                        string.Join("/", currentDir.Split('/').Reverse().Skip(2).Reverse())
                                        , "/");
                                break;
                            default:
                                currentDir = String.Concat(currentDir, line[2], "/");
                                break;
                        }
                        directories.Add(currentDir);
                        break;
                }
            }
            else
            {
                if (line[0] != "dir")
                {
                    files.Add(String.Concat(currentDir, line[1]), int.Parse(line[0]));
                }
            }
        }

        var sizes = directories
            .Select(d => files
                .Where(f => f.Key.StartsWith(d))
                .Select(f => f.Value)
                .Sum()
            ).OrderBy(x => x).ToList();
        
        if (part == 1)
        {
            return sizes.Where(s => s <= 100000).Sum();
        }

        var total = sizes.Last();
        return sizes.First(s => total - s <= (70000000 - 30000000));
    }
}