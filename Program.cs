using AOC.days;

namespace AOC;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("AOC 2022 runner");
        
        var skip = 0;
        var limit = 25;
        if (DateTime.Now.Year == 2022 && DateTime.Now.Month == 12 && DateTime.Now.Day <= 25)
        {
            skip = DateTime.Now.Day - 1;
            limit = 1;
        }

        var baseName = new string(typeof(Day<>).FullName!.TakeWhile(c => c != '`').ToArray());
        var days = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.FullName!.StartsWith(baseName))
            .Where(p => p.Name.Length == 5 && p.Name[3] >= '0' && p.Name[3] < '3')
            .Select(p => new Tuple<System.Type,int> ( p, int.Parse(p.Name[3..])))
            .ToArray();
        skip = Math.Min(days.Length - 1, skip); // Don't skip past the last day
        Console.WriteLine($"Running the last {limit} day(s) out of {days.Length} ");
        foreach (var (classToRun, dayNumber) in days.Skip(skip).Take(limit))
        {
            switch (Activator.CreateInstance(classToRun))
            {
                case Day<long> day:
                    day.DayNumber = dayNumber;
                    day.Run();
                    break;
                case Day<string> day:
                    day.DayNumber = dayNumber;
                    day.Run();
                    break;
            }
        }
    }
}