using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyBattle
{
    public class Squad
    {
        public string Name { get; set; }
        public List<Warrior> Warriors { get; set; }

        public Squad()
        {
            Warriors = new List<Warrior>();
        }

        public Squad(string name)
        {
            Name = name;
            Warriors = new List<Warrior>();
        }

    }
}
