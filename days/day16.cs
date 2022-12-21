namespace AOC.days;

internal class Day16 : Day<long>
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "16_t1.txt"), 1651);
        AddRun("Part 1", () => RunPart(1, "16.txt"));
        AddRun("Test 2", () => RunPart(2, "16_t1.txt"), 1707);
        AddRun("Part 2", () => RunPart(2, "16.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var input = GetListOfLines(inputName)
            .Select(l => l.Split(new[] {' ', '=', ';', ','}, StringSplitOptions.RemoveEmptyEntries)
                .Where((_, i) => i == 1 || i == 5 || i > 9).ToList()).ToList();

        var flowRates = input.ToDictionary(i => i[0], i => int.Parse(i[1]));
        var usefulFlows = flowRates.Where(i => i.Value > 0).Select(i => i.Key).ToHashSet();
        var usefulCount = usefulFlows.Count();
        var allMoves = input.ToDictionary(i => i[0], i => i.Skip(2).ToArray());
        var usefulAndStart = usefulFlows.Select(x => x).Append("AA").ToHashSet();
        var usefulMoves = new Dictionary<Tuple<string, string>, int>();
        foreach (var valve1 in usefulAndStart)
        {
            foreach (var valve2 in usefulFlows)
            {
                if (valve1 == valve2) continue;
                var reachable = new HashSet<string>(){valve1};
                var visited = new HashSet<string>();
                var distance = 0;
                while (!reachable.Contains(valve2))
                {
                    var newReachable = new HashSet<string>();
                    foreach (var source in reachable)
                    {
                        foreach (var target in allMoves[source])
                        {
                            if (visited.Add(target))
                            {
                                newReachable.Add(target);
                            }
                        }
                    }
                    distance += 1;
                    reachable = newReachable;
                }

                usefulMoves.Add(new Tuple<string, string>(valve1, valve2), distance);
            }
        }

        var maxTime = part == 1 ? 30 : 26;

        if (part == 1) return GetMorePressure("AA", new SortedSet<string>(), maxTime);
        return GetMorePressure2("AA", "AA", new SortedSet<string>(), maxTime, 0, 0);
        
        long GetMorePressure(string current, IReadOnlyCollection<string> opened, int timeLeft)
        {
            long result = 0;
            if (timeLeft < 3) return result;
            foreach (var move in usefulMoves.Where(x => x.Key.Item1 == current && x.Value < timeLeft - 2))
            {
                var dest = move.Key.Item2;
                var distance = move.Value + 1; // one extra because opening takes 1 minute
                var newOpened = new SortedSet<string>(opened);
                if (newOpened.Add(dest))
                    result = Math.Max(result,
                        (timeLeft - distance) * flowRates[dest] +
                        GetMorePressure(dest, newOpened, timeLeft - distance));
            }

            return result;
        }
        
        long GetMorePressure2(string current1, string current2, IReadOnlyCollection<string> visited, int timeLeft, long delay1, long delay2)
        {
            if (visited.Count == usefulCount) return 0;
            long result = 0;
            foreach (var move1 in usefulMoves.Where(x => x.Key.Item1 == current1 && x.Value < timeLeft - 2 - delay1 && !visited.Contains(x.Key.Item2)))
            {
                var dest1 = move1.Key.Item2;
                var distance1 = move1.Value + 1 + delay1; // one extra because opening takes 1 minute
                
                // move both
                foreach (var move2 in usefulMoves.Where(x =>
                             x.Key.Item1 == current2 && x.Value < timeLeft - 2 - delay2 && !visited.Contains(x.Key.Item2)))
                {
                    var dest2 = move2.Key.Item2;
                    if (dest1 == dest2) continue;
                    var distance2 = move2.Value + 1 + delay2; // one extra because opening takes 1 minute
                    var minDistance = Math.Min(distance1, distance2);
                    if (timeLeft <= minDistance) continue;
                    var newDelay1 = distance1 - minDistance;
                    var newDelay2 = distance2 - minDistance;
                    var newVisited = new SortedSet<string>(visited);
                    if (newVisited.Add(dest1) && newVisited.Add(dest2))
                        result = Math.Max(result,
                            Math.Max(0, (timeLeft - distance1) * flowRates[dest1])
                            + Math.Max(0, (timeLeft - distance2) * flowRates[dest2])
                            + GetMorePressure2(dest1, dest2, newVisited, (int) (timeLeft - minDistance), newDelay1,
                                newDelay2)
                        );
                }

                // move only one
                {
                    var distance2 = delay2;
                    var minDistance = Math.Min(distance1, distance2);
                    if (timeLeft <= minDistance) continue;
                    var newVisited = new SortedSet<string>(visited);
                    var newDelay1 = distance1 - minDistance;
                    var newDelay2 = distance2 - minDistance;
                    if (newVisited.Add(dest1))
                        result = Math.Max(result,
                            Math.Max(0, (timeLeft - distance1) * flowRates[dest1])
                            + GetMorePressure2(dest1, current2, newVisited, (int) (timeLeft - distance1), newDelay1, newDelay2)
                        );
                }
            }

            return result;
        }
    }

}