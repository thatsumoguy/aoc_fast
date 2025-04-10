using System.Text;
using System.Text.RegularExpressions;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal partial class Day25
    {
        public static string input { get; set; }
        private static uint GrayCode(uint n) => n ^ (n >> 1);
        private static void DrainOutput(Computer comp)
        {
            while (comp.Run(out var _) == State.Output) { }
        }
        private static void DropItem(Computer comp, string item)
        {
            comp.InputAscii($"drop {item}\n");
            DrainOutput(comp);
        }
        private static void TakeItem(Computer comp, string item)
        {
            comp.InputAscii($"take {item}\n");
            DrainOutput(comp);
        }
        private static void MoveSlient(Computer comp, string dir)
        {
            if(dir != "none")
            {
                comp.InputAscii($"{dir}\n");
                DrainOutput(comp);
            }
        }
        private static State MoveNoisy(Computer comp, string dir, StringBuilder output)
        {
            if (dir != "none") comp.InputAscii($"{dir}\n");
            while(true)
            {
                switch(comp.Run(out var val))
                {
                    case State.Output:
                        var ascii = (char)((byte)val);
                        output.Append(ascii);
                        break;
                    case State.Input: return State.Input;
                    case State.Halted: return State.Halted;
                }
            }
        }
        private static bool Dangerous(string item) => MyRegex().IsMatch(item);
        private static string Opposite(string dir) => dir switch
        {
            "north" => "south",
            "south" => "north",
            "east" => "west",
            "west" => "east",
            _ => "none"
        };
        private static void Explore(Computer comp, List<string> stack, List<string> path, List<string> inventory)
        {
            var dir = stack.Count == 0 ? "none" : stack[^1];
            var reverse = Opposite(dir);
            
            var output = new StringBuilder();
            MoveNoisy(comp, dir, output);
            foreach(var line in output.ToString().Split("\n"))
            {
                if(line.StartsWith("== Pressure-Sensitive Floor =="))
                {
                    path.Clear();
                    path.AddRange(stack);
                    return;
                }
                else if(line.StartsWith("- "))
                {
                    var suffix = line.Substring(2);
                    if(Opposite(suffix) == "none")
                    {
                        var item = suffix;
                        if(!Dangerous(item))
                        {
                            TakeItem(comp, item);
                            inventory.Add(item);
                        }
                        
                    }
                    else
                    {
                        var direction = suffix;
                        if (direction != reverse)
                        {
                            stack.Add(direction);
                            Explore(comp, stack, path, inventory);
                            stack.Pop();
                        }
                    }
                }
            }
            MoveSlient(comp, reverse);
        }

        private static string Play(long[] input)
        {
            var comp = new Computer(input);
            var stack = new List<string>();
            var path = new List<string>();  
            var inventory = new List<string>();
            Explore(comp, stack, path, inventory);

            var last = path.Pop();
            foreach(var dir in path) MoveSlient(comp, dir);
            var combinations = 1u << inventory.Count;
            var output = new StringBuilder();

            var tooLight = new HashSet<uint>();
            var tooHeavy = new HashSet<uint>();

            for(var i = 1u; i < combinations; i++)
            {
                var current = GrayCode(i);
                var prev = GrayCode(i - 1);
                var changed = current ^ prev;
                var index = (int)uint.TrailingZeroCount(changed);

                if((current & changed) == 0)
                {
                    TakeItem(comp, inventory[index]);
                    if(tooHeavy.Contains(prev))
                    {
                        tooHeavy.Add(current);
                        continue;
                    }
                }
                else
                {
                    DropItem(comp, inventory[index]);
                    if(tooLight.Contains(prev))
                    {
                        tooLight.Add(current);
                        continue;
                    }
                }
                var state = MoveNoisy(comp, last, output);
                if (state == State.Halted)
                {
                    output = output.Retain(c => char.IsAsciiDigit(c));
                    break;
                }
                else if (output.ToString().Contains("heavier")) tooLight.Add(current);
                else tooHeavy.Add(current);
                output.Clear();
            }
            return output.ToString();
        }
        public static string PartOne()
        {
            var code = input.ExtractNumbers<long>().ToArray();
            return Play(code);
        }
        public static string PartTwo() => "Merry Christmas";
        [GeneratedRegex("escape pod|giant electromagnet|infinite loop|molten lava|photons")]
        private static partial Regex MyRegex();
    }
}
