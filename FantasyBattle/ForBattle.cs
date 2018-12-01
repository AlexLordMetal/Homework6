using System;

namespace FantasyBattle
{
    public static class ForBattle
    {
        private static Random randomizer = new Random();

        /// <summary>
        /// Returns random int number.
        /// </summary>
        /// <param name="lower">The inclusive lower bound of the random number</param>
        /// <param name="higher">The inclusive upper bound of the random number</param>
        /// <returns></returns>
        public static int Random(int lower, int higher)
        {
            return randomizer.Next(lower, higher + 1);
        }

        /// <summary>
        /// Returns non-negative random int number.
        /// </summary>
        /// <param name="higher">The inclusive upper bound of the random number</param>
        /// <returns></returns>
        public static int Random(int higher)
        {
            return Random(0, higher);
        }

        /// <summary>
        /// Reads string from console and converts it to number with conditions - greater than 0 and less or equal than Condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int ConditionParse(int condition = 2147483647)
        {
            var isCorrect = false;
            int number = 0;
            while (isCorrect != true)
            {
                isCorrect = Int32.TryParse(Console.ReadLine(), out number);
                if (number <= 0 || number > condition)
                {
                    isCorrect = false;
                }
                if (isCorrect == false)
                {
                    Console.Write("Ввод некорректен! Еще раз: ");
                }
            }
            return number;
        }

    }
}
