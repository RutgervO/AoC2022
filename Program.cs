﻿using AOC.days;

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

        var rootType = typeof(Day);
        var days = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.BaseType == rootType)
            .ToList();
        var dayNumbers = days.Select(d => int.Parse(d.Name[3..])).ToArray().Skip(skip).Take(limit);
        foreach (var dayNumber in dayNumbers)
        {
            var classToRun = days.Single(d => d.Name[3..].EndsWith(dayNumber.ToString("D2")));
            if (Activator.CreateInstance(classToRun) is not Day day) return;
            day.DayNumber = dayNumber;
            day.Run();
        }
    }
}