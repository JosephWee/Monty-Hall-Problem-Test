using System;
using System.Drawing;

namespace Monty_Hall_Problem_Solution_Verification
{
    internal class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            DateTime dtSeed = DateTime.UtcNow;
            random = new Random(dtSeed.Hour + dtSeed.Minute + dtSeed.Second + dtSeed.Microsecond);

            int num_of_games = 100;
            int num_of_doors = 3;
            int prize = int.MinValue;
            int choice = int.MinValue;

            for (int i = 0; i < num_of_games; i++)
            {
                bool won = PlayLetsMakeADeal(num_of_doors, false, out prize, out choice);
                Console.WriteLine($"Game {i} | prize = {prize} | choice = {choice} | {(won ? "Win" : "Loose")}");
            }

            Console.WriteLine("Hit Enter to Continue...");
            Console.ReadKey();
        }

        static bool PlayLetsMakeADeal(int num_of_doors, bool change, out int prize, out int choice)
        {
            prize = random.Next(num_of_doors);

            var doors = InitDoors(num_of_doors, prize);

            bool play = true;
            choice = random.Next(num_of_doors);
            int doorOpened = int.MinValue;

            while (play)
            {
                doorOpened = OpenDoor(num_of_doors, prize, choice, doors);

                play = choice != doorOpened && doors.Values.Any(x => x == 0);

                if (!play && change)
                {
                    choice = ChangeChoice(num_of_doors, choice, doors);
                }
            }

            return choice == prize;
        }

        static Dictionary<int, int> InitDoors(int num_of_doors, int prize)
        {
            Dictionary<int, int> doors = new Dictionary<int, int>();

            for (int i = 0; i < num_of_doors; i++)
            {
                if (i == prize)
                {
                    doors.Add(i, 1);
                }
                else
                {
                    doors.Add(i, 0);
                }
            }

            return doors;
        }

        static int OpenDoor(int num_of_doors, int prize, int choice, Dictionary<int, int> doors)
        {
            var goatDoors = doors.Where((kvp, index) => kvp.Value == 0).ToList();

            if (goatDoors.Count() == 0)
            {
                return choice;
            }

            int indexGoat = random.Next(goatDoors.Count());
            var selectedDoor = goatDoors.ElementAt(indexGoat);

            doors[selectedDoor.Key] = 2;

            return selectedDoor.Key;
        }

        static int ChangeChoice(int num_of_doors, int currentChoice, Dictionary<int, int> doors)
        {
            var availiableDoors = doors.Where((kvp, index) => kvp.Value == 0).ToList();

            int index = random.Next(availiableDoors.Count());
            var selectedDoor = availiableDoors.ElementAt(index);

            return selectedDoor.Key;
        }
    }
}