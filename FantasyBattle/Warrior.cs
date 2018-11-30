using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyBattle
{
    public class Warrior
    {
        private int hp;

        public string Name { get; set; }
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                if (value >= 0) hp = value;
                else hp = 0;
            }
        }
        public int ATK { get; set; }
        public int STR { get; set; }
        public WarriorClass Class { get; set; }
        public int BetweenAbility { get; set; }
        public int AbilityRecoveryCounter { get; set; }

        public Warrior(string name)
        {
            Name = name;
            HP = 50 + ForBattle.Random(1, 30);
            ATK = 6 + ForBattle.Random(1, 6);
            STR = 1 + ForBattle.Random(4);
            Class = WarriorClass.Normal;
        }

        public Warrior()
        {
            HP = 50 + ForBattle.Random(1, 30);
            ATK = 6 + ForBattle.Random(1, 6);
            STR = 1 + ForBattle.Random(4);
            Class = WarriorClass.Normal;
        }

    }
}
