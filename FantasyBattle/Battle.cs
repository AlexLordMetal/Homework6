using System;
using System.Collections.Generic;
using System.IO;

namespace FantasyBattle
{
    public class Battle
    {
        public int FactionAmount { get; set; }
        public int SquadAmount { get; set; }

        public void Start()
        {
            var factions = CreateMiddleEarth();
            FactionsRandomizer(factions);
            AddFactionFeatures(factions);
            //ConsoleReport(factions);
            //ConsoleBattleOneOnOne(factions);
            ConsoleFactionsBattle(factions);
            ConsoleFactionsBattle(factions);
            ConsoleReport(factions);
            Console.ReadKey();
        }

        public List<Faction> CreateMiddleEarth()
        {
            Console.Write("Введите количество отрядов в каждой фракции: ");
            FactionAmount = ForBattle.ConditionParse();
            Console.Write("Введите базовое количество бойцов в каждом отряде: ");
            SquadAmount = ForBattle.ConditionParse();
            Console.Clear();

            var factions = new List<Faction>();
            factions.Add(new Faction("Орки"));
            factions.Add(new Faction("Эльфы"));
            return factions;
        }

        public void FactionsRandomizer(List<Faction> factions)
        {
            foreach (var faction in factions)
            {
                var factionNames = ReadFromFile(faction.Name);
                for (int squadIndex = 0; squadIndex < FactionAmount; squadIndex++)
                {
                    switch (faction.Name.ToLower())
                    {
                        case "эльфы":
                            switch (squadIndex)
                            {
                                case 0:
                                    faction.Squads.Add(new Squad("Нолдор"));
                                    break;
                                case 1:
                                    faction.Squads.Add(new Squad("Телери"));
                                    break;
                                case 2:
                                    faction.Squads.Add(new Squad("Ваниар"));
                                    break;
                                default:
                                    faction.Squads.Add(new Squad($"{squadIndex + 1} отряд эльфов"));
                                    break;
                            }
                            break;
                        case "орки":
                            switch (squadIndex)
                            {
                                case 0:
                                    faction.Squads.Add(new Squad("Черные Уруки"));
                                    break;
                                case 1:
                                    faction.Squads.Add(new Squad("Минас Моргул"));
                                    break;
                                case 2:
                                    faction.Squads.Add(new Squad("Урук-Хай"));
                                    break;
                                default:
                                    faction.Squads.Add(new Squad($"{squadIndex + 1} отряд орков"));
                                    break;
                            }
                            break;
                        default:
                            faction.Squads.Add(new Squad($"{squadIndex + 1} отряд {faction.Name.ToLower()}"));
                            break;
                    }

                    for (int warriorIndex = 0; warriorIndex < SquadAmount; warriorIndex++)
                    {
                        faction.Squads[squadIndex].Warriors.Add(WarriorRandomizer(factionNames));
                    }
                    ClassRandomizer(faction.Squads[squadIndex]);
                }
            }
        }

        public Warrior WarriorRandomizer(List<string> names)
        {
            return new Warrior(names[ForBattle.Random(0, names.Count - 1)]);
        }

        private void AddFactionFeatures(List<Faction> factions)
        {
            int firstFactionIndex = ForBattle.Random(1);
            int secondFactionIndex = firstFactionIndex == 0 ? 1 : 0;

            AddFactionWarriors(factions[firstFactionIndex]);
            AddFactionClasses(factions[secondFactionIndex]);
        }

        private void AddFactionWarriors(Faction faction)
        {
            var factionNames = ReadFromFile(faction.Name);
            if ((Enum.GetNames(typeof(WarriorClass)).Length - 1) < faction.Squads[0].Warriors.Count)
            {
                foreach (var squad in faction.Squads)
                {
                    squad.Warriors.Add(WarriorRandomizer(factionNames));
                }
            }
        }

        private void AddFactionClasses(Faction faction)
        {
            foreach (var squad in faction.Squads)
            {
                var classesAmount = Enum.GetNames(typeof(WarriorClass)).Length - 1;
                WarriorClass randomClass = (WarriorClass)ForBattle.Random(1, classesAmount);
                var classAdded = false;
                while (!classAdded && classesAmount < squad.Warriors.Count)
                {
                    var warrior = squad.Warriors[ForBattle.Random(squad.Warriors.Count - 1)];
                    if (warrior.Class == WarriorClass.Normal)
                    {
                        AddClassAbility(warrior, randomClass);
                        classAdded = true;
                    }
                }
            }
        }

        public void ClassRandomizer(Squad squad)
        {
            var counter = 1;
            while (counter < Enum.GetNames(typeof(WarriorClass)).Length && counter <= squad.Warriors.Count)
            {
                var warrior = squad.Warriors[ForBattle.Random(squad.Warriors.Count - 1)];
                if (warrior.Class == WarriorClass.Normal)
                {
                    AddClassAbility(warrior, (WarriorClass)counter);
                    counter++;
                }
            }
        }

        public void AddClassAbility(Warrior warrior, WarriorClass warriorClass)
        {
            switch (warriorClass)
            {
                case WarriorClass.Berserk:
                    AddBerserkAbility(warrior);
                    break;
                case WarriorClass.Priest:
                    AddPriestAbility(warrior);
                    break;
                case WarriorClass.Mage:
                    AddMageAbility(warrior);
                    break;
                case WarriorClass.Warrior:
                    AddWarriorAbility(warrior);
                    break;
            }
        }

        private void AddBerserkAbility(Warrior warrior)
        {
            warrior.Class = WarriorClass.Berserk;
            warrior.BetweenAbility = 4;
            warrior.AbilityRecoveryCounter = warrior.BetweenAbility;
            warrior.Name = "Берсерк " + warrior.Name;
        }

        private void AddPriestAbility(Warrior warrior)
        {
            warrior.Class = WarriorClass.Priest;
            warrior.BetweenAbility = 4;
            warrior.AbilityRecoveryCounter = warrior.BetweenAbility;
            warrior.Name = "Прист " + warrior.Name;
        }

        private void AddMageAbility(Warrior warrior)
        {
            warrior.Class = WarriorClass.Mage;
            warrior.BetweenAbility = 4;
            warrior.AbilityRecoveryCounter = warrior.BetweenAbility;
            warrior.Name = "Маг " + warrior.Name;
        }

        private void AddWarriorAbility(Warrior warrior)
        {
            warrior.Class = WarriorClass.Warrior;
            warrior.BetweenAbility = -1;
            warrior.AbilityRecoveryCounter = warrior.BetweenAbility;
            warrior.Name = "Воин " + warrior.Name;
        }

        private List<string> ReadFromFile(string factionName)
        {
            var names = new List<string>();
            var fileName = "";
            switch (factionName.ToLower())
            {
                case "эльфы":
                    fileName = "ElfNames.txt";
                    break;
                case "орки":
                    fileName = "OrcNames.txt";
                    break;
                default:
                    fileName = "DefaultNames.txt";
                    break;
            }
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
                    Console.WriteLine($"\n   В отряде \"{squad.Name}\" {squad.Warriors.Count} бойцов");
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

            Console.WriteLine("ЛЕТОПИСЬ СРАЖЕНИЯ:");
            var winnerWarrior = BattleOneOnOne(firstWarrior, secondWarrior);
            ChargeClassAbilities(factions);
            CorpseCollector(factions);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"\nПобедил {winnerWarrior.Name} (осталось HP: {winnerWarrior.HP})\n");
            Console.ResetColor();            
        }

        public void ConsoleBattleSquadOnSquad(List<Faction> factions, int firstFactionSquadIndex, int secondFactionSquadIndex)
        {
            var firstSquad = factions[0].Squads[firstFactionSquadIndex];
            var secondSquad = factions[1].Squads[secondFactionSquadIndex];

            Console.WriteLine($"Сражаются отряды:");
            Console.WriteLine($"   Отряд \"{firstSquad.Name}\" из фракции \"{factions[0].Name}\"");
            SquadConsoleReport(firstSquad);
            Console.WriteLine($"\n   Отряд \"{secondSquad.Name}\" из фракции \"{factions[1].Name}\"");
            SquadConsoleReport(secondSquad);
            Console.WriteLine();

            Console.WriteLine("ЛЕТОПИСЬ СРАЖЕНИЯ:");
            var winnerSquad = BattleSquadOnSquad(firstSquad, secondSquad);
            ChargeClassAbilities(factions);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"\nПобедил отряд \"{winnerSquad.Name}\". Герои:");
            SquadConsoleReport(winnerSquad);
            Console.ResetColor();
            Console.WriteLine();
        }

        public void ConsoleFactionsBattle(List<Faction> factions)
        {
            var firstFactionSquadIndex = FactionNotEmpty(factions[0]);
            var secondFactionSquadIndex = FactionNotEmpty(factions[1]);

            if (firstFactionSquadIndex == -1 || secondFactionSquadIndex == -1)
            {
                Console.WriteLine("Не с кем сражаться. Одна из фракций (или обе) пуста.");
            }
            else
            {
                Console.WriteLine($"Фракция \"{factions[0].Name}\" сражается с фракцией \"{factions[1].Name}\":\n");
                while (firstFactionSquadIndex != -1 && secondFactionSquadIndex != -1)
                {
                    ConsoleBattleSquadOnSquad(factions, firstFactionSquadIndex, secondFactionSquadIndex);
                    firstFactionSquadIndex = FactionNotEmpty(factions[0]);
                    secondFactionSquadIndex = FactionNotEmpty(factions[1]);
                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                if (FactionNotEmpty(factions[0]) != -1) Console.WriteLine($"Победила фракция \"{factions[0].Name}\"!\n");
                else Console.WriteLine($"Победила фракция \"{factions[1].Name}\"!\n");
                Console.ResetColor();
            }
        }

        public Warrior BattleOneOnOne(Warrior firstWarrior, Warrior secondWarrior)
        {
            var firstSquad = new Squad();
            firstSquad.Warriors.Add(firstWarrior);

            var secondSquad = new Squad();
            secondSquad.Warriors.Add(secondWarrior);

            while (firstWarrior.HP != 0 && secondWarrior.HP != 0)
            {
                ClassAttack(firstSquad, 0, secondSquad, 0);
                if (secondWarrior.HP > 0)
                {
                    ClassAttack(secondSquad, 0, firstSquad, 0);
                }

                if (firstWarrior.AbilityRecoveryCounter < firstWarrior.BetweenAbility)
                {
                    firstWarrior.AbilityRecoveryCounter++;
                }
                if (secondWarrior.AbilityRecoveryCounter < secondWarrior.BetweenAbility)
                {
                    secondWarrior.AbilityRecoveryCounter++;
                }

            }
            return firstWarrior.HP == 0 ? secondWarrior : firstWarrior;
        }

        public Squad BattleSquadOnSquad(Squad firstSquad, Squad secondSquad)
        {
            var firstSquadWarriorIndex = 0;
            var firstWarriorStepCounter = 0;
            var firstSquadWarrior = firstSquad.Warriors[firstSquadWarriorIndex];

            var secondSquadWarriorIndex = 0;
            var secondWarriorStepCounter = 0;
            var secondSquadWarrior = secondSquad.Warriors[secondSquadWarriorIndex];

            while (firstSquad.Warriors.Count != 0 && secondSquad.Warriors.Count != 0)
            {
                ClassAttack(firstSquad, firstSquadWarriorIndex, secondSquad, secondSquadWarriorIndex);
                firstWarriorStepCounter++;
                secondWarriorStepCounter++;

                firstSquadWarrior = IfEndTurn(firstSquad, ref firstWarriorStepCounter, ref firstSquadWarriorIndex);
                secondSquadWarrior = IfSomeoneDied(secondSquad, ref secondWarriorStepCounter, ref secondSquadWarriorIndex);
                if (secondSquadWarrior == null) break;

                ClassAttack(secondSquad, secondSquadWarriorIndex, firstSquad, firstSquadWarriorIndex);
                firstWarriorStepCounter++;
                secondWarriorStepCounter++;

                firstSquadWarrior = IfSomeoneDied(firstSquad, ref firstWarriorStepCounter, ref firstSquadWarriorIndex);
                secondSquadWarrior = IfEndTurn(secondSquad, ref secondWarriorStepCounter, ref secondSquadWarriorIndex);
            }
            return firstSquad.Warriors.Count == 0 ? secondSquad : firstSquad;
        }

        private void ClassAttack(Squad attackerSquad, int attackerIndex, Squad defenderSquad, int defenderIndex)
        {
            switch (attackerSquad.Warriors[attackerIndex].Class)
            {
                case WarriorClass.Berserk:
                    ConsoleBerserkAttack(attackerSquad, attackerIndex, defenderSquad, defenderIndex);
                    break;
                case WarriorClass.Priest:
                    ConsolePriestAttack(attackerSquad, attackerIndex, defenderSquad, defenderIndex);
                    break;
                case WarriorClass.Mage:
                    ConsoleMageAttack(attackerSquad, attackerIndex, defenderSquad, defenderIndex);
                    break;
                default:
                    ConsoleNormalAttack(attackerSquad.Warriors[attackerIndex], defenderSquad.Warriors[defenderIndex]);
                    break;
            }
        }

        private void ConsoleBerserkAttack(Squad attackerSquad, int attackerIndex, Squad defenderSquad, int defenderIndex)
        {
            var attacker = attackerSquad.Warriors[attackerIndex];

            ConsoleNormalAttack(attacker, defenderSquad.Warriors[defenderIndex]);

            if (attacker.BetweenAbility == attacker.AbilityRecoveryCounter)
            {
                var defendersLess10HP = new List<Warrior>();
                foreach (var defender in defenderSquad.Warriors)
                {
                    if (defender.HP < 10 && defender.HP > 0) defendersLess10HP.Add(defender);
                }
                if (defendersLess10HP.Count > 0)
                {
                    var defender = defendersLess10HP[ForBattle.Random(defendersLess10HP.Count - 1)];
                    var damage = AttackPower(attacker);
                    attacker.AbilityRecoveryCounter = 0;

                    HighlightTextSpecial();
                    Console.Write($"{attacker.Name} дополнительно атакует противника с малым здоровьем (меньше 10) {defender.Name}, нанося {damage} урона. ");

                    ConsoleDefend(defender, damage);
                }
            }
        }

        private void ConsolePriestAttack(Squad attackerSquad, int attackerIndex, Squad defenderSquad, int defenderIndex)
        {
            var attacker = attackerSquad.Warriors[attackerIndex];

            if (attacker.BetweenAbility == attacker.AbilityRecoveryCounter)
            {
                var warrior = FindMinHPWarrior(attackerSquad);
                if (warrior.HP < 20)
                {
                    var heal = AttackPower(attacker);
                    warrior.HP += heal;
                    attacker.AbilityRecoveryCounter = 0;

                    HighlightTextSpecial();
                    Console.Write($"{attacker.Name} отхилил союзника {warrior.Name} на {heal} HP. ");
                    Console.WriteLine($"У {warrior.Name} стало {warrior.HP} HP.");
                }
                else ConsoleNormalAttack(attacker, defenderSquad.Warriors[defenderIndex]);
            }
            else ConsoleNormalAttack(attacker, defenderSquad.Warriors[defenderIndex]);
        }

        private void ConsoleMageAttack(Squad attackerSquad, int attackerIndex, Squad defenderSquad, int defenderIndex)
        {
            var attacker = attackerSquad.Warriors[attackerIndex];

            if (attacker.BetweenAbility == attacker.AbilityRecoveryCounter)
            {
                var damage = AttackPower(attacker);
                attacker.AbilityRecoveryCounter = 0;

                HighlightTextSpecial();
                Console.WriteLine($"{attacker.Name} атакует файерболом следующих противников, нанося им по {damage} урона:");

                if (defenderSquad.Warriors.Count > 2)
                {
                    DecreaseSquadWarriorIndex(defenderSquad, ref defenderIndex);
                    for (int counter = 0; counter < 3; counter++)
                    {
                        var defender = defenderSquad.Warriors[defenderIndex];

                        Console.Write($"\t    {defender.Name}. ");
                        ConsoleDefend(defender, damage);

                        IncreaseSquadWarriorIndex(defenderSquad, ref defenderIndex);
                    }
                }
                else
                {
                    foreach (var defender in defenderSquad.Warriors)
                    {
                        Console.Write($"\t    {defender.Name}. ");
                        ConsoleDefend(defender, damage);
                    }
                }
            }
            else
            {
                var defender = FindMinHPWarrior(defenderSquad);
                var damage = AttackPower(attacker);

                Console.Write($"\t{attacker.Name} атакует противника с наименьшим здоровьем {defender.Name}, нанося {damage} урона. ");

                ConsoleDefend(defender, damage);
            }
        }

        private void ConsoleNormalAttack(Warrior attacker, Warrior defender)
        {
            int damage = AttackPower(attacker);

            Console.Write($"\t{attacker.Name} атакует {defender.Name}, нанося {damage} урона. ");

            ConsoleDefend(defender, damage);
        }

        private int AttackPower(Warrior warrior)
        {
            return ForBattle.Random(1, warrior.ATK) + warrior.STR;
        }

        private void ConsoleDefend(Warrior defender, int damage)
        {
            if (defender.Class == WarriorClass.Warrior && defender.AbilityRecoveryCounter == defender.BetweenAbility)
            {
                defender.AbilityRecoveryCounter = 0;

                HighlightTextSpecial();
                Console.WriteLine($"{defender.Name} уворачивается от атаки.");
            }
            else
            {
                defender.HP -= damage;

                if (defender.HP > 0) Console.WriteLine($"У {defender.Name} остается {defender.HP} HP.");
                else HighlightTextDied(defender.Name);
            }
        }

        private void CorpseCollector(List<Faction> factions)
        {
            foreach (var faction in factions)
            {
                foreach (var squad in faction.Squads)
                {
                    for (int warriorIndex = 0; warriorIndex < squad.Warriors.Count; warriorIndex++)
                    {
                        if (squad.Warriors[warriorIndex].HP == 0)
                        {
                            squad.Warriors.RemoveAt(warriorIndex);
                        }
                    }
                }
            }
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

        private void ChargeClassAbilities(List<Faction> factions)
        {
            foreach (var faction in factions)
            {
                foreach (var squad in faction.Squads)
                {
                    foreach (var warrior in squad.Warriors)
                    {
                        warrior.AbilityRecoveryCounter = warrior.BetweenAbility;
                    }
                }
            }
        }

        private Warrior IfEndTurn(Squad squad, ref int warriorStepCounter, ref int squadWarriorIndex)
        {
            if (warriorStepCounter == 2)
            {
                warriorStepCounter = 0;
                if (squad.Warriors[squadWarriorIndex].AbilityRecoveryCounter < squad.Warriors[squadWarriorIndex].BetweenAbility)
                {
                    squad.Warriors[squadWarriorIndex].AbilityRecoveryCounter++;
                }
                IncreaseSquadWarriorIndex(squad, ref squadWarriorIndex);
            }
            return squad.Warriors[squadWarriorIndex];
        }

        private Warrior IfSomeoneDied(Squad squad, ref int warriorStepCounter, ref int squadWarriorIndex)
        {
            for (int index = 0; index < squad.Warriors.Count; index++)
            {
                if (squad.Warriors[index].HP == 0)
                {
                    if (index < squadWarriorIndex)
                    {
                        squadWarriorIndex--;
                    }
                    else if (index == squadWarriorIndex)
                    {
                        warriorStepCounter = 0;
                    }
                    squad.Warriors.RemoveAt(index);
                    index--;
                }
            }
            if (squad.Warriors.Count > 0)
            {
                VerifySquadWarriorIndex(squad, ref squadWarriorIndex);
                return IfEndTurn(squad, ref warriorStepCounter, ref squadWarriorIndex);
            }
            else return null;
        }

        private Warrior FindMinHPWarrior(Squad squad)
        {
            var warriorWithMinHP = squad.Warriors[0];
            foreach (var warrior in squad.Warriors)
            {
                if (warrior.HP < warriorWithMinHP.HP)
                {
                    warriorWithMinHP = warrior;
                }
            }
            return warriorWithMinHP;
        }

        private int FactionNotEmpty(Faction faction)
        {
            for (int squadIndex = 0; squadIndex < faction.Squads.Count; squadIndex++)
            {
                if (faction.Squads[squadIndex].Warriors.Count > 0) return squadIndex;
            }
            return -1;
        }

        private void VerifySquadWarriorIndex(Squad squad, ref int warriorIndex)
        {
            if (warriorIndex >= squad.Warriors.Count) warriorIndex = 0;
        }

        private void IncreaseSquadWarriorIndex(Squad squad, ref int warriorIndex)
        {
            if (warriorIndex < squad.Warriors.Count - 1) warriorIndex++;
            else warriorIndex = 0;
        }

        private void DecreaseSquadWarriorIndex(Squad squad, ref int warriorIndex)
        {
            if (warriorIndex > 0) warriorIndex--;
            else warriorIndex = squad.Warriors.Count - 1;
        }

        private void HighlightTextSpecial()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\tСПЕЦ УМЕНИЕ: ");
            Console.ResetColor();
        }

        private void HighlightTextDied(string name)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{name} погиб.");
            Console.ResetColor();
        }
                
    }
}