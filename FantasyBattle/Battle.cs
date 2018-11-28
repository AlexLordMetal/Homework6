using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyBattle
{
    public class Battle
    {
        public void Start()
        {
            var factions = new List<Faction>();

            var orcs = new Faction("Орки");
            orcs.Squads.Add(new Squad("Черные Уруки"));
            orcs.Squads.Add(new Squad("Минас Моргул"));
            factions.Add(orcs);

            var elfs = new Faction("Эльфы");
            elfs.Squads.Add(new Squad("Нолдор"));
            elfs.Squads.Add(new Squad("Телери"));
            factions.Add(elfs);
                                 
            FactionsRandomizer(factions);

            var winner = BattleOneOnOne(SelectRandomWarrior(factions, 0, out var firstWarriorIndexes), SelectRandomWarrior(factions, 1, out var secondWarriorIndexes));
            Console.Write(BattleOneOnOneReport(factions, firstWarriorIndexes, secondWarriorIndexes, winner));
                        
            Console.Write(Report(factions));
            Console.ReadKey();
        }

        public void FactionsRandomizer(List<Faction> factions)
        {
            var elfNames = ReadFromFile("ElfNames.txt");
            var orcNames = ReadFromFile("OrcNames.txt");
            var defaultNames = ReadFromFile("DefaultNames.txt");

            foreach (var faction in factions)
            {
                for (int squadIndex = 0; squadIndex < faction.Squads.Count; squadIndex++)
                {
                    for (int warriorIndex = 0; warriorIndex < 10; warriorIndex++)
                    {
                        switch (faction.Name.ToLower())
                        {
                            case "эльфы":
                                faction.Squads[squadIndex].Warriors.Add(WarriorRandomizer(elfNames));
                                break;
                            case "орки":
                                faction.Squads[squadIndex].Warriors.Add(WarriorRandomizer(orcNames));
                                break;
                            default:
                                faction.Squads[squadIndex].Warriors.Add(WarriorRandomizer(defaultNames));
                                break;
                        }
                    }
                }

            }
        }

        public Warrior WarriorRandomizer(List<string> names)
        {
            return new Warrior(names[ForBattle.Random(0, names.Count - 1)]);
        }

        private List<string> ReadFromFile(string fileName)
        {
            var names = new List<string>();
            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    names.Add(reader.ReadLine());
                }
            }
            return names;
        }

        public string Report(List<Faction> factions)
        {
            var output = "Отчет по фракциям.\n";
            foreach (var faction in factions)
            {
                output += $"\nВ фракции \"{faction.Name}\" {faction.Squads.Count} отряда:\n";
                foreach (var squad in faction.Squads)
                {
                    output += $"\n   В отряде \"{squad.Name}\" {squad.Warriors.Count} воинов:\n";
                    foreach (var warrior in squad.Warriors)
                    {
                        output += $"\t{WarriorReport(warrior)}\n";
                    }                    
                }
            }
            return output;
        }

        public string WarriorReport(Warrior warrior)
        {
            return $"{warrior.Name} (HP: {warrior.HP}, ATK: {warrior.ATK}, STR: {warrior.STR})";
        }

        public string BattleOneOnOneReport(List<Faction> factions, int[] firstWarriorIndexes, int[] secondWarriorIndexes, Warrior winnerWarrior)
        {
            return $"Сражались воин {WarriorReport(factions[firstWarriorIndexes[0]].Squads[firstWarriorIndexes[1]].Warriors[firstWarriorIndexes[2]])} " +
                $"из отряда \"{factions[firstWarriorIndexes[0]].Squads[firstWarriorIndexes[1]].Name}\" из фракции \"{factions[firstWarriorIndexes[0]].Name}\" " +
                $"и воин {WarriorReport(factions[secondWarriorIndexes[0]].Squads[secondWarriorIndexes[1]].Warriors[secondWarriorIndexes[2]])} " +
                $"из отряда \"{factions[secondWarriorIndexes[0]].Squads[secondWarriorIndexes[1]].Name}\" из фракции \"{factions[secondWarriorIndexes[0]].Name}\".\n" +
                $"Победил воин {winnerWarrior.Name} (осталось HP: {winnerWarrior.HP})!\n\n";
        }

        public Warrior SelectRandomWarrior(List<Faction> factions, int factionIndex, out int[] warriorIndexes)
        {
            var squadIndex = ForBattle.Random(factions[factionIndex].Squads.Count - 1);
            var warriorIndex = ForBattle.Random(factions[factionIndex].Squads[squadIndex].Warriors.Count - 1);
            warriorIndexes = new int[3] {factionIndex, squadIndex, warriorIndex};
            return factions[factionIndex].Squads[squadIndex].Warriors[warriorIndex];
        }

        public Warrior SelectRandomWarrior(List<Faction> factions, int factionIndex)
        {
            var squadIndex = ForBattle.Random(factions[factionIndex].Squads.Count - 1);
            var warriorIndex = ForBattle.Random(factions[factionIndex].Squads[squadIndex].Warriors.Count - 1);
            return factions[factionIndex].Squads[squadIndex].Warriors[warriorIndex];
        }

        public Warrior BattleOneOnOne(Warrior firstWarrior, Warrior secondWarrior)
        {
            while (firstWarrior.HP != 0 && secondWarrior.HP != 0)
            {
                secondWarrior.HP -= Attack(firstWarrior);
                if (secondWarrior.HP > 0)
                {
                    firstWarrior.HP -= Attack(secondWarrior);
                }
            }
            if (firstWarrior.HP == 0)
            {
                
            }

            return firstWarrior.HP == 0 ? secondWarrior : firstWarrior;
        }

        private int Attack(Warrior warrior)
        {
            return ForBattle.Random(1, warrior.ATK) + warrior.STR;
        }
    }
}
