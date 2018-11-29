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
            var factions = CreateMiddleEarth();
            FactionsRandomizer(factions);
            //ConsoleBattleOneOnOne(factions);
            ConsoleBattleSquadOnSquad(factions);
            ConsoleReport(factions);
            Console.ReadKey();
        }

        public List<Faction> CreateMiddleEarth()
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

            return factions;
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

        public void ConsoleReport(List<Faction> factions)
        {
            Console.WriteLine("Отчет по фракциям.");
            foreach (var faction in factions)
            {
                Console.WriteLine($"\nВ фракции \"{faction.Name}\" {faction.Squads.Count} отряда:");
                foreach (var squad in faction.Squads)
                {
                    Console.WriteLine($"\n   В отряде \"{squad.Name}\" {squad.Warriors.Count} воинов");
                    SquadConsoleReport(squad);
                }
            }
        }

        public void SquadConsoleReport(Squad squad)
        {
            foreach (var warrior in squad.Warriors)
            {
                Console.Write("\t");
                WarriorConsoleReport(warrior);
                Console.WriteLine();
            }
        }

        public void WarriorConsoleReport(Warrior warrior)
        {
            Console.Write($"{warrior.Name} (HP: {warrior.HP}, ATK: {warrior.ATK}, STR: {warrior.STR})");
        }

        public void ConsoleBattleOneOnOne(List<Faction> factions)
        {
            var firstWarriorIndexes = SelectRandomWarrior(factions, 0);
            var firstWarrior = factions[firstWarriorIndexes[0]].Squads[firstWarriorIndexes[1]].Warriors[firstWarriorIndexes[2]];
            var secondWarriorIndexes = SelectRandomWarrior(factions, 1);
            var secondWarrior = factions[secondWarriorIndexes[0]].Squads[secondWarriorIndexes[1]].Warriors[secondWarriorIndexes[2]];

            Console.WriteLine($"Сражаются:");
            Console.Write("   ");
            WarriorConsoleReport(firstWarrior);
            Console.WriteLine($" из отряда \"{factions[firstWarriorIndexes[0]].Squads[firstWarriorIndexes[1]].Name}\" из фракции \"{factions[firstWarriorIndexes[0]].Name}\"");
            Console.Write("   ");
            WarriorConsoleReport(secondWarrior);
            Console.WriteLine($" из отряда \"{factions[secondWarriorIndexes[0]].Squads[secondWarriorIndexes[1]].Name}\" из фракции \"{factions[secondWarriorIndexes[0]].Name}\"");
            Console.WriteLine();

            var winnerWarrior = BattleOneOnOne(factions, firstWarrior, secondWarrior);

            Console.WriteLine($"\nПобедил {winnerWarrior.Name} (осталось HP: {winnerWarrior.HP})\n");
        }

        public int[] SelectRandomWarrior(List<Faction> factions, int factionIndex, int squadIndex)
        {
            var warriorIndex = ForBattle.Random(factions[factionIndex].Squads[squadIndex].Warriors.Count - 1);
            return new int[] { factionIndex, squadIndex, warriorIndex };
        }

        public int[] SelectRandomWarrior(List<Faction> factions, int factionIndex)
        {
            var squadIndex = ForBattle.Random(factions[factionIndex].Squads.Count - 1);
            return SelectRandomWarrior(factions, factionIndex, squadIndex);
        }

        public int[] SelectRandomWarrior(List<Faction> factions)
        {
            var factionIndex = ForBattle.Random(factions.Count - 1);
            return SelectRandomWarrior(factions, factionIndex);
        }

        public Warrior BattleOneOnOne(List<Faction> factions, Warrior firstWarrior, Warrior secondWarrior)
        {
            while (firstWarrior.HP != 0 && secondWarrior.HP != 0)
            {
                ConsoleAttack(firstWarrior, secondWarrior);
                if (secondWarrior.HP > 0)
                {
                    ConsoleAttack(secondWarrior, firstWarrior);
                }
            }
            CorpseCollector(factions);
            return firstWarrior.HP == 0 ? secondWarrior : firstWarrior;
        }

        private int AttackPower(Warrior warrior)
        {
            return ForBattle.Random(1, warrior.ATK) + warrior.STR;
        }

        private void ConsoleAttack(Warrior attacker, Warrior defender)
        {
            int damage = AttackPower(attacker);
            defender.HP -= damage;

            Console.Write($"\t{attacker.Name} атаковал {defender.Name}, нанеся {damage} урона. ");
            if (defender.HP > 0) Console.WriteLine($"У {defender.Name} осталось {defender.HP} HP.");
            else Console.WriteLine($"{defender.Name} погиб.");
            
        }

        private void CorpseCollector(List<Faction> factions)
        {
            foreach (var faction in factions)
            {
                foreach (var squad in faction.Squads)
                {
                    CorpseCollector(squad);
                }
            }
        }

        public void ConsoleBattleSquadOnSquad(List<Faction> factions)
        {
            var firstSquadIndexes = SelectRandomSquad(factions, 0);
            var firstSquad = factions[firstSquadIndexes[0]].Squads[firstSquadIndexes[1]];
            var secondSquadIndexes = SelectRandomSquad(factions, 1);
            var secondSquad = factions[secondSquadIndexes[0]].Squads[secondSquadIndexes[1]];

            Console.WriteLine($"Сражались:");
            Console.WriteLine($"   Отряд \"{factions[firstSquadIndexes[0]].Squads[firstSquadIndexes[1]].Name}\" из фракции \"{factions[firstSquadIndexes[0]].Name}\"");
            SquadConsoleReport(firstSquad);
            Console.WriteLine($"\n   Отряд \"{factions[secondSquadIndexes[0]].Squads[secondSquadIndexes[1]].Name}\" из фракции \"{factions[secondSquadIndexes[0]].Name}\"");
            SquadConsoleReport(secondSquad);
            Console.WriteLine();

            var winnerSquad = BattleSquadOnSquad(factions, firstSquad, secondSquad);

            Console.WriteLine($"\nПобедил отряд \"{winnerSquad.Name}\". Герои:");
            SquadConsoleReport(winnerSquad);
            Console.WriteLine();
        }

        public int[] SelectRandomSquad(List<Faction> factions, int factionIndex)
        {
            var squadIndex = ForBattle.Random(factions[factionIndex].Squads.Count - 1);
            return new int[] { factionIndex, squadIndex };
        }

        public int[] SelectRandomSquad(List<Faction> factions)
        {
            var factionIndex = ForBattle.Random(factions.Count - 1);
            return SelectRandomSquad(factions, factionIndex);
        }

        public Squad BattleSquadOnSquad(List<Faction> factions, Squad firstSquad, Squad secondSquad)
        {
            var firstSquadWarriorIndex = 0;
            var firstWarriorStepCounter = 0;
            var firstSquadWarrior = firstSquad.Warriors[firstSquadWarriorIndex];

            var secondWarriorStepCounter = 0;
            var secondSquadWarriorIndex = 0;
            var secondSquadWarrior = secondSquad.Warriors[secondSquadWarriorIndex];

            while (firstSquad.Warriors.Count != 0 && secondSquad.Warriors.Count != 0)
            {
                ConsoleAttack(firstSquadWarrior, secondSquadWarrior);
                firstWarriorStepCounter++;
                secondWarriorStepCounter++;

                firstSquadWarrior = IfEndTurn(firstSquad, ref firstWarriorStepCounter, ref firstSquadWarriorIndex);
                secondSquadWarrior = IfDied(secondSquad, ref secondWarriorStepCounter, ref secondSquadWarriorIndex);
                if (secondSquadWarrior == null) break;

                ConsoleAttack(secondSquadWarrior, firstSquadWarrior);
                firstWarriorStepCounter++;
                secondWarriorStepCounter++;

                firstSquadWarrior = IfDied(firstSquad, ref firstWarriorStepCounter, ref firstSquadWarriorIndex);
                secondSquadWarrior = IfEndTurn(secondSquad, ref secondWarriorStepCounter, ref secondSquadWarriorIndex);
            }
            return firstSquad.Warriors.Count == 0 ? secondSquad : firstSquad;
        }

        /// <summary>
        /// Faster version of CorpseCollector. Collects corps only in current squad.
        /// </summary>
        /// <param name="factions"></param>
        private void CorpseCollector(Squad squad)
        {
            for (int warriorIndex = 0; warriorIndex < squad.Warriors.Count; warriorIndex++)
            {
                if (squad.Warriors[warriorIndex].HP == 0)
                {
                    squad.Warriors.RemoveAt(warriorIndex);
                }
            }
        }

        private Warrior IfEndTurn(Squad squad, ref int warriorStepCounter, ref int squadWarriorIndex)
        {
            if (warriorStepCounter == 2)
            {
                warriorStepCounter = 0;
                IncreaseSquadWarriorIndex(squad, ref squadWarriorIndex);
            }
            return squad.Warriors[squadWarriorIndex];
        }

        private Warrior IfDied(Squad squad, ref int warriorStepCounter, ref int squadWarriorIndex)
        {
            if (squad.Warriors[squadWarriorIndex].HP > 0)
            {
                return IfEndTurn(squad, ref warriorStepCounter, ref squadWarriorIndex);
            }
            else
            {
                CorpseCollector(squad);
                if (squad.Warriors.Count > 0)
                {
                    warriorStepCounter = 0;
                    VerifySquadWarriorIndex(squad, ref squadWarriorIndex);
                    return squad.Warriors[squadWarriorIndex];
                }
                else return null;
            }
        }

        private void IncreaseSquadWarriorIndex(Squad squad, ref int warriorIndex)
        {
            if (warriorIndex < squad.Warriors.Count - 1) warriorIndex++;
            else warriorIndex = 0;
        }

        private void VerifySquadWarriorIndex(Squad squad, ref int warriorIndex)
        {
            if (warriorIndex > squad.Warriors.Count - 1) warriorIndex = 0;
        }

    }
}