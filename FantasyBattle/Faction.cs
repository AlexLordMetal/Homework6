using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyBattle
{
    public class Faction
    {
        public string Name { get; set; }
        public List<Squad> Squads { get; set; }

        public Faction()
        {
            Squads = new List<Squad>();
        }

        public Faction(string name)
        {
            Name = name;
            Squads = new List<Squad>();
        }

    }     
}
