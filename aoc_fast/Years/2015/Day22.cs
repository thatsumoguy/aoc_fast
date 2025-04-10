using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day22
    {
        public static string input
        {
            get;
            set;
        }
        private static short[] startingInfo = [];
        class State : IEquatable<State>
        {
            public short bossHp { get; set; }
            public short playerHp { get; set; }
            public short playerMp { get; set; }
            public byte shieldEffect { get; set; }
            public byte poisonEffect { get; set; }
            public byte rechargeEffect { get; set; }

            public State() { }

            public bool Equals(State? other)
            {
                return (other.bossHp == bossHp && other.playerMp == playerMp && other.playerHp == playerHp
                    && other.poisonEffect == poisonEffect && other.shieldEffect == shieldEffect
                    && other.rechargeEffect == rechargeEffect);
            }
            public override int GetHashCode() => bossHp.GetHashCode() + playerHp.GetHashCode() + playerMp.GetHashCode() + shieldEffect.GetHashCode() + poisonEffect.GetHashCode() + rechargeEffect.GetHashCode();
        }

        private static bool BossTurn(State state, short attack)
        {
            if (state.shieldEffect > 0) attack = (short)Math.Max(attack - 7, 1);

            state.playerHp -= attack;
            return state.playerHp > 0 && state.playerMp >= 53;
        }

        private static bool ApplySpellEffects(State state)
        {
            if(state.shieldEffect > 0) state.shieldEffect--;
            if(state.poisonEffect >0)
            {
                state.poisonEffect--;
                state.bossHp -= 3;
            }
            if(state.rechargeEffect > 0)
            {
                state.rechargeEffect--;
                state.playerMp += 101;
            }

            return state.bossHp <= 0;
        }

        private static short Play(short[] input, bool hardMode = false)
        {
            var (bossHp, bossDamage) = input switch { var a => (a[0], a[1]) };
            var start = new State
            {
                bossHp = bossHp,
                playerHp = 50,
                playerMp = 500,
                shieldEffect = 0,
                poisonEffect = 0,
                rechargeEffect = 0
            };

            var todo = new PriorityQueue<State, short>();
            var cache = new HashSet<State>(5000);

            todo.Enqueue(start, 0);
            cache.Add(start);

            while (todo.TryDequeue(out var state, out var spent))
            {
                if (ApplySpellEffects(state)) return spent;
                if (hardMode)
                {
                    if (state.playerHp > 1) state.playerHp--;
                    else continue;
                }

                if (state.playerMp >= 53)
                {
                    var next = new State
                    {
                        bossHp = (short)(state.bossHp - 4),
                        playerMp = (short)(state.playerMp - 53),
                        playerHp = state.playerHp,
                        poisonEffect = state.poisonEffect,
                        rechargeEffect = state.rechargeEffect,
                        shieldEffect = state.shieldEffect
                    };
                    if (ApplySpellEffects(next)) return (short)(spent + 53);
                    if (BossTurn(next, bossDamage) && cache.Add(next)) todo.Enqueue(next, (short)(spent + 53));
                }

                if (state.playerMp >= 73)
                {
                    var next = new State
                    {
                        bossHp = (short)(state.bossHp - 2),
                        playerHp = (short)(state.playerHp + 2),
                        playerMp = (short)(state.playerMp - 73),
                        poisonEffect = state.poisonEffect,
                        rechargeEffect = state.rechargeEffect,
                        shieldEffect = state.shieldEffect
                    };

                    if (ApplySpellEffects(next)) return (short)(spent + 73);

                    if (BossTurn(next, bossDamage) && cache.Add(next)) todo.Enqueue(next, (short)(spent + 73));
                }

                if (state.playerMp >= 113 && state.shieldEffect == 0)
                {
                    var next = new State
                    {
                        playerMp = (short)(state.playerMp - 113),
                        shieldEffect = 6,
                        playerHp = state.playerHp,
                        bossHp = state.bossHp,
                        poisonEffect = state.poisonEffect,
                        rechargeEffect = state.rechargeEffect,
                    };

                    if (ApplySpellEffects(next)) return (short)(spent + 113);

                    if (BossTurn(next, bossDamage) && cache.Add(next)) todo.Enqueue(next, (short)(spent + 113));
                }

                if (state.playerMp >= 173 && state.poisonEffect == 0)
                {
                    var next = new State
                    {
                        bossHp = state.bossHp,
                        playerHp = state.playerHp,
                        playerMp = (short)(state.playerMp - 173),
                        shieldEffect = state.shieldEffect,
                        poisonEffect = 6,
                        rechargeEffect = state.rechargeEffect,
                    };

                    if (ApplySpellEffects(next)) return (short)(spent + 173);
                    if (BossTurn(next, bossDamage) && cache.Add(next)) todo.Enqueue(next, (short)(spent + 173));
                }

                if (state.playerMp >= 229 && state.rechargeEffect == 0)
                {
                    var next = new State
                    {
                        bossHp = state.bossHp,
                        playerHp = state.playerHp,
                        playerMp = (short)(state.playerMp - 229),
                        shieldEffect = state.shieldEffect,
                        poisonEffect = state.poisonEffect,
                        rechargeEffect = 5,
                    };

                    if (ApplySpellEffects(next)) return (short)(spent + 229);
                    if (BossTurn(next, bossDamage) && cache.Add(next)) todo.Enqueue(next, (short)(spent + 229));
                }
            }
            throw new Exception();
        }

        public static short PartOne()
        {
            startingInfo = [.. input.ExtractNumbers<short>()];
            return Play(startingInfo);
        }
        public static short PartTwo()
        {
            startingInfo = [.. input.ExtractNumbers<short>()];
            return Play(startingInfo, true);
        }
    }
}
