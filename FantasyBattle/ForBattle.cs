using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyBattle
{
    public static class ForBattle
    {
        private static Random randomizer = new Random();

        public static int Random(int lower, int higher)
        {
            return randomizer.Next(lower, higher + 1);
        }

        public static int Random(int higher)
        {
            return Random(0, higher);
        }

    }
}
