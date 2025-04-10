namespace aoc_fast.Years._2016
{
    class Day19
    {
        public static string input
        {
            get;
            set;
        }

        public static uint PartOne()
        {
            var elf = uint.Parse(input);

            elf *= 2;

            elf -= (uint)1 << (int)Math.Log2(elf);
            elf++;

            return elf;
        }

        public static uint PartTwo()
        {
            var target= uint.Parse(input);
            var elf = 0u;
            var size = 1u;

             while(size < target)
             {
                var remaining = target - size;

                if(elf > size /2)
                {
                    var possible = 2 * elf - size;
                    size += Math.Min(possible, remaining);
                }
                else
                {
                    if(elf >= remaining)
                    {
                        elf -= remaining;
                        size += remaining;
                    }
                    else
                    {
                        elf += size;
                        size = elf + 1;
                    }
                }
             }

            return target - elf;
        }
    }
}
