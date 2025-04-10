using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day7
    {
        public static string input
        {
            get;
            set;
        }
        private static Dictionary<byte, Step> Steps = [];

        class Step(uint remaining, List<byte> children)
        {
            public uint remaining { get; set; } = remaining;
            public List<byte> children { get; set; } = children;
        }

        private static void Parse()
        {
            var steps = new Dictionary<byte, Step>();

            foreach (var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Encoding.ASCII.GetBytes))
            {
                var from = line[5];
                var to = line[36];
                if (!steps.ContainsKey(from)) steps[from] = new Step(default, []);
                var step = steps[from];
                steps[from] = step;
                step.children.Add(to);
                if (!steps.ContainsKey(to)) steps[to] = new Step(default, []);
                step = steps[to];
                step.remaining++;
                steps[to] = step;
            }

            Steps = steps;
        }

        public static string PartOne()
        {
            Parse();
            var ready = new SortedDictionary<byte, Step>();
            var blocked = new Dictionary<byte, Step>();

            foreach(var (key, step) in Steps.ToList())
            {
                if(step.remaining == 0) ready.Add(key, step);
                else blocked.Add(key, step);
            }

            var done = new StringBuilder();

            while(ready.PopFront(out var s))
            {
                var key = s.Key;
                var step = s.Value;
                done.Append((char)key);

                foreach(var k in step.children)
                {
                    blocked.Remove(k, out var subStep);
                    subStep.remaining--;

                    if (subStep.remaining == 0) ready.Add(k, subStep);
                    else blocked[k] = subStep;
                }
            }

            return done.ToString();
        }

        public static uint PartTwo()
        {
            Parse();
            var ready = new SortedDictionary<byte, Step>();
            var blocked = new Dictionary<byte, Step>();

            foreach (var (key, step) in Steps.ToList())
            {
                if (step.remaining == 0) ready.Add(key, step);
                else blocked.Add(key, step);
            }

            var time = 0u;
            var workers = new List<(uint, Step)>();

            while (ready.Count > 0 || workers.Count > 0)
            {
                while (ready.Count > 0 && workers.Count < 5)
                {
                    ready.PopFront(out var r);
                    var finish = (uint)(time + 60 + (r.Key - 64));

                    workers.Add((finish, r.Value));
                    workers.Sort((a, b) => (uint.MaxValue - a.Item1).CompareTo(uint.MaxValue - b.Item1));
                }

                var (f, s) = workers[^1];
                workers.RemoveAt(workers.Count - 1);
                time = f;

                foreach (var key in s.children)
                {
                    blocked.Remove(key, out var st);
                    st.remaining--;

                    if (st.remaining == 0) ready[key] = st;
                    else blocked[key] = st;
                }
            }
            return time;
        }
    }
}
