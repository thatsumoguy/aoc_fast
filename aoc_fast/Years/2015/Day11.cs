using System.Data;
using System.Text;

namespace aoc_fast.Years._2015
{
    class Day11
    {
        public static string input
        {
            get;
            set;
        }

        private static byte[][] answer = [];

        private static void Parse()
        {
            var bytes = Encoding.ASCII.GetBytes(input.Trim());
            var password = Clean(bytes);
            var first = NextPass(password);
            var second = NextPass(first);
            answer = [first, second];
        }

        private static byte[] Clean(byte[] password)
        {
            var reset = false;


            foreach (var digit in password)
            {
                if(reset) password[digit] = (byte)'a';
                else if(digit == (byte)'i' ||  digit == (byte)'o' || digit == (byte)'l')
                {
                    password[digit]++;
                    reset = true;
                }
            }

            return password;
        }

        private static byte[] NextPass(byte[] password) 
        {
            var newPassword = new byte[password.Length];
            Array.Copy(password, newPassword, password.Length);
            if (newPassword[3] >= (byte)'g' && newPassword[3] <= (byte)'o') return Fill(newPassword, (byte)'p');

            if (newPassword[3] <= (byte)'x')
            {
                var candidate = Fill(newPassword, newPassword[3]);
                if (candidate.Zip(newPassword, (X, y) => new {X,y}).Where(z => z.X != z.y).Take(1).Where(z => z.X > z.y).Any())
                    return candidate;
                if (newPassword[3] == (byte)'x')
                {
                    newPassword[2] += (byte)((newPassword[2] == (byte)'h' || newPassword[2] == (byte)'n' || newPassword[2] == (byte)'k') ? 2 : 1);
                    return Fill(newPassword, (byte)'a');
                }
                else if (newPassword[3] == (byte)'f') return Fill(newPassword, (byte)'p');
                else return Fill(newPassword, (byte)(newPassword[3] + 1));
            }
            return newPassword;
        }

        private static byte[] Fill(byte[] password, byte start)
        {
            var newPassword = new byte[password.Length];
            Array.Copy(password, newPassword, password.Length);
            newPassword[3] = start;
            newPassword[4] = start;
            newPassword[5] = (byte)(start + 1);
            newPassword[6] = (byte)(start + 2);
            newPassword[7] = (byte)(start + 2);
            return newPassword;
        }

        public static string PartOne()
        {
            Parse();
            return Encoding.ASCII.GetString(answer[0]);
        }
        public static string PartTwo() => Encoding.ASCII.GetString(answer[1]);
    }
}
