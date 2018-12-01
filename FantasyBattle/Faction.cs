using System.Collections.Generic;

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
