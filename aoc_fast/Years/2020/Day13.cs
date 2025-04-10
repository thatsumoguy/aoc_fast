namespace aoc_fast.Years._2020
{
    internal class Day13
    {
        public static string input { get; set; }
        private static (ulong timeStamp, List<(ulong, ulong)> bus) Buses = (0, []);
        private static void Parse()
        {
            var lines = input.Trim().Split("\n");
            var timeStamp =ulong.Parse(lines[0]);
            var bus = lines[1].Split(",").ToList().Index().Where(i => i.Item != "x").Select(i => ((ulong)i.Index, ulong.Parse(i.Item))).ToList();
            Buses = (timeStamp, bus);
        }
        public static ulong PartOne()
        {
            Parse();
            var (id, next) = Buses.bus.Select(i => (i.Item2, i.Item2 - Buses.timeStamp % i.Item2)).MinBy(i => i.Item2);
            return id * next;
        }
        public static ulong PartTwo()
        {
            var (time, step) = Buses.bus[0];
            foreach(var (offset, id) in Buses.bus[1..])
            {
                var remainder = id - offset % id;
                while (time % id != remainder) time += step;
                step *= id;
            }
            return time;
        }
    }
}
