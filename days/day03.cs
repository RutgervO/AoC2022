using System.Collections.Immutable;

namespace AOC.days;

internal class Day03 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "03_t1.txt"), 157);
        AddRun("Part 1", () => RunPart(1, "03.txt"));
        AddRun("Test 2", () => RunPart(2, "03_t2.txt"), 70);
        AddRun("Part 2", () => RunPart(2, "03.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputValues = GetListOfLines(inputName);
        
        if (part == 1) return inputValues
            .Select(bag =>
                Priority(bag.Take(bag.Length / 2).ToHashSet()
                    .Intersect(bag.Skip(bag.Length / 2).ToHashSet()).First()
                )
            ).Sum();
        
        return inputValues
            .Chunk(3).Select(group =>
                Priority(group.First().ToHashSet()
                    .Intersect(group.Skip(1).First().ToHashSet())
                    .Intersect(group.Skip(2).First().ToHashSet()).First()
                )
            ).Sum();

        int Priority(char item)
        {
            return item >= 'a' ? item - 'a' + 1 : item - ('A' - 27);
        }
    }
}
