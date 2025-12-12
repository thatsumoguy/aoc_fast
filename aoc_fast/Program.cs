//This is a C# port of https://github.com/maneatingape/advent-of-code-rust 
using System.Diagnostics;
using System.Net;
using BenchmarkDotNet.Running;

try
{
    var all = false;
    var years = new List<int>();
    var days = new List<int>();
    var cookiePath = AppDomain.CurrentDomain.BaseDirectory + @"\session\cookie.txt";
    if (!File.Exists(cookiePath))
    {
        File.Create(cookiePath);
        Console.WriteLine(@$"Please input a session cookie at {cookiePath} and save before running again");
        return;
    }
    var cookie = File.ReadAllText(cookiePath);
    if (cookie.Length == 0)
    {
        Console.WriteLine("The session cookie provided is empty pleaase enter a correct session cookie");
        return;
    }
    if (args.Length < 1)
    {
        Console.WriteLine("Enter a year (2015-2025) in a 4 digit or 2 digit format or enter -all for all years.");
        Console.WriteLine("You may also do individual years separated by a comma (2015,2017,2020).");
        Console.WriteLine("Use -h or --help for help");
        return;
    }
    if (args.Any(a => a.Equals("-all", StringComparison.CurrentCultureIgnoreCase)))
    {
        years.AddRange(Enumerable.Range(2015, 11));
        all = true;
    }
    else if (!args.Contains("-h") || !args.Contains("--help"))
    {
        try
        {
            if (args[0].Contains(','))
            {
                foreach (var year in args[0].Split(","))
                {
                    var curYear = int.Parse(year);
                    if (year.Length == 4)
                    {
                        if (curYear < 2015 || curYear > 2025)
                        {
                            Console.WriteLine("Must be between 2015 and 2023");
                            return;
                        }
                        years.Add(curYear);
                    }
                    else
                    {
                        if (curYear < 15 || curYear > 25)
                        {
                            Console.WriteLine("Must be between 2015 and 2023");
                            return;
                        }
                        years.Add(int.Parse("20" + year));
                    }
                }
            }
            else
            {
                var curYear = int.Parse(args[0]);
                if (args[0].Length == 4)
                {
                    if (curYear < 2015 || curYear > 2025)
                    {
                        Console.WriteLine("Must be between 2015 and 2023");
                        return;
                    }
                }
                else
                {
                    if (curYear < 15 || curYear > 25)
                    {
                        Console.WriteLine("Must be between 2015 and 2023");
                        return;
                    }
                    curYear = int.Parse("20" + curYear.ToString());
                }
                years.Add(curYear);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("The input is not in the correct format, you must but in a year in a format such as 2015 or 15.");
            return;
        }
    }
    if(args.Contains("--day") || args.Contains("-d"))
    {
        var index = Array.FindIndex(args, s => s == "--day" || s == "-d");
        if (args[index + 1].Contains(','))
        {
            foreach (var day in args[index + 1].Split(',')) days.Add(int.Parse(day));
        }
        else days.Add(int.Parse(args[index + 1]));
    }
    if (args.Contains("-b") || args.Contains("--benchmark"))
    {
        if (years.Count == 0)
        {
            Console.WriteLine("You must specify a year for benchmarking.");
            return;
        }
        foreach (var year in years)
        {
            Console.WriteLine($"Running benchmarks for {year}...");
            if(days.Count == 0)
            {
                foreach(var x in Enumerable.Range(1, 25))
                {
                    var projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
                    var inputPath = projectDir + $@"\Inputs\{year}\{x}.txt";
                    await GetInput(x, year, cookie, inputPath);
                }
                var fullClassName = $"aoc_fast.Benchmarks.Years._{year}.YearBenchmark";
                var benchmarkType = Type.GetType(fullClassName);
                if (benchmarkType != null)
                {
                    BenchmarkRunner.Run(benchmarkType);
                }
                return;
            }
            foreach (var x in days)
            {
                var projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
                var inputPath = projectDir + $@"\Inputs\{year}\{x}.txt";
                await GetInput(x, year, cookie, inputPath);
                Console.WriteLine($"\tRunning Day {x}");
                var dayClassName = $"Day{x}";
                var fullClassName = $"aoc_fast.Benchmarks.Years._{year}.{dayClassName}";
                var benchmarkType = Type.GetType(fullClassName);
                if (benchmarkType != null)
                {
                    BenchmarkRunner.Run(benchmarkType);
                }
                else
                {
                    Console.WriteLine($"Benchmark class for {dayClassName} not found.");
                }
            }
        }
        return;
    }
    else if(args.Contains("-h"))
    {
        Console.WriteLine("This is a C# fork of https://github.com/maneatingape/advent-of-code-rust that, just as that program, tries to complete AoC in a fast as possible (sub 1 second)");
        Console.WriteLine("The program has all days and years from 2015-2024. Right now I have it run all days for a given year but will add day selection. I will also add a benchmark switch, but I am slow on that.");
        Console.WriteLine("There is not much you can do right now, you can either input a year such as 2015 or 15, or you can input multiple years via comma separated list, example: 2015,2016,17");
        Console.WriteLine("You can also supply -all and it will go through all years.");
        return;
    }
    var allYearsSW = new Stopwatch();
    if (all) allYearsSW.Start();
    var totalSW = new Stopwatch();
    totalSW.Start();
    foreach (var year in years)
    {
        if (days.Count == 0)
        {
            if(year < 2025)
            {
                days.AddRange(Enumerable.Range(1, 25));
            }
            else
            {
                var curDate = DateTime.Now.Day;
                days.AddRange(Enumerable.Range(1, curDate < 13 ? curDate : 12));   
            }
        }
        var yearNamespace = $"aoc_fast.Years._{year}";
        Console.WriteLine($"Year {year}");
        foreach (var x in days)
        {
            var inputPath = AppDomain.CurrentDomain.BaseDirectory + $@"\Inputs\{year}\{x}.txt";
            await GetInput(x, year, cookie, inputPath);
            Console.WriteLine($"\tRunning Day {x}");
            var dayClassName = $"Day{x}";
            var fullClassName = $"{yearNamespace}.{dayClassName}";
            var sw = new Stopwatch();
            try
            {
                var type = Type.GetType(fullClassName);

                if (type != null)
                {
                    var dayInstance = Activator.CreateInstance(type);
                    var partOneMethod = type.GetMethod("PartOne");
                    var partTwoMethod = type.GetMethod("PartTwo");
                    var inputProp = type.GetProperty("input");
                    if ((partOneMethod != null || partTwoMethod != null) && (inputProp != null && inputProp.CanWrite))
                    {
                        inputProp.SetValue(null, File.ReadAllText(inputPath));
                        sw.Start();
                        var partOneOutput = partOneMethod.Invoke(dayInstance, null);
                        double ticks = sw.ElapsedTicks;
                        double seconds = ticks / Stopwatch.Frequency;
                        double milliseconds = (ticks / Stopwatch.Frequency) * 1000;
                        double nanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                        Console.WriteLine($"\t\tPart One: {partOneOutput}");
                        Console.WriteLine($"\t\t\tCompleted in {seconds}:{milliseconds}:{nanoseconds} s:ms:μs");
                        if (partTwoMethod != null)
                        {
                            var partTwoOutput = partTwoMethod.Invoke(dayInstance, null);
                            ticks = sw.ElapsedTicks;
                            seconds = ticks / Stopwatch.Frequency;
                            milliseconds = (ticks / Stopwatch.Frequency) * 1000;
                            nanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                            Console.WriteLine($"\t\tPart Two: {partTwoOutput}");
                            Console.WriteLine($"\t\t\tCompleted in {seconds}:{milliseconds}:{nanoseconds} s:ms:μs");
                        }
                        sw.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing {fullClassName}: {ex.Message}");
            }
        }
        days.Clear();
        double totalTicks = totalSW.ElapsedTicks;
        totalSW.Stop();
        double totalSeconds = totalTicks / Stopwatch.Frequency;
        double totalMili = (totalTicks / Stopwatch.Frequency) * 1000;
        double totalNano = (totalTicks / Stopwatch.Frequency) * 1000000000;
        Console.WriteLine($"All parts completed: {totalSeconds}:{totalMili}:{totalNano} s:ms:μs");
    }
    if (all)
    {
        double totalAllTicks = allYearsSW.ElapsedTicks;
        allYearsSW.Stop();
        double totalAllSeconds = totalAllTicks / Stopwatch.Frequency;
        double totalAllMili = (totalAllTicks / Stopwatch.Frequency) * 1000;
        double totalAllNano = (totalAllTicks / Stopwatch.Frequency) * 1000000000;
        Console.WriteLine($"All years completed: {totalAllSeconds}:{totalAllMili}:{totalAllNano} s:ms:μs");
    }
}
catch (Exception ex) { Console.WriteLine(ex.ToString()); }


static async Task GetInput(int day, int year, string cookie, string filename)
{
    if (!File.Exists(filename))
    {
        Console.WriteLine(@$"Downloading {filename}");
        var uri = new Uri("https://adventofcode.com");
        var cookies = new CookieContainer();
        cookies.Add(uri, new Cookie("session", cookie));
        using var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        using var handler = new HttpClientHandler() { CookieContainer = cookies };
        using var client = new HttpClient(handler) { BaseAddress = uri };
        using var response = await client.GetAsync($"/{year}/day/{day}/input");
        using var stream = await response.Content.ReadAsStreamAsync();
        await stream.CopyToAsync(file);
        //there is always an extra newline. I call trim() a lot but why not just do this...
        file.SetLength(file.Length - 1);
        file.Close();
    }
}


