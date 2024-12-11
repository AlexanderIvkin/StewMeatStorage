using System;
using System.Collections.Generic;
using System.Linq;

namespace StewMeatStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> names = new List<string>
            {
                "Говядина", "Телятина", "Нудятина", 
                "Свинятина","Курятина", "Индюшатина",
                "Простите, собачатина", "Аистятина", 
                "Кролятина", "Овощатина", "Грибятина"
            };
            int[] releaseYears = new int[]
            {
                2000, 2010
            };
            int[] expirationYears = new int[]
            {
                15, 25
            };
            StewMeatFactory stewMeatFactory = new StewMeatFactory(names, releaseYears, expirationYears);
            Storage storage = new Storage(stewMeatFactory.Create());

            storage.Execute();
        }
    }

    class Storage
    {
        private List<StewMeat> _stewMeats;

        public Storage(List<StewMeat> stewMeats)
        {
            _stewMeats = stewMeats;
        }

        public void Execute()
        {
            Console.Clear();
            Console.WriteLine("Вот вообще всё что мы заготовили:\n");
            ShowInfo(_stewMeats);
            Console.WriteLine("\nА вот то, что ещё годно в употребление(если вообще когда-то было):\n");
            ShowInfo(RequiredStewMeat());
        }

        private List<StewMeat> RequiredStewMeat()
        {
            return _stewMeats.Where(stewMeat => stewMeat.ReleaseYear + stewMeat.ExpirationYear > DateTime.Now.Year).ToList();
        }

        private void ShowInfo(List<StewMeat> stewMeats)
        {
            int count = 1;

            foreach(StewMeat stewMeat in stewMeats)
            {
                Console.Write($"{count++} ");
                stewMeat.ShowInfo();
            }
        }
    }

    class StewMeatFactory
    {
        private List<string> _names;
        private int[] _releaseYears;
        private int[] _expirationYears;

        public StewMeatFactory(List<string> names, int[] releaseYears, int[] expirationYears)
        {
            _names = names;
            _releaseYears = releaseYears;
            _expirationYears = expirationYears;
        }

        public List<StewMeat> Create()
        {
            List<StewMeat> stewMeats = new List<StewMeat>();
            
            for (int i =0; i<_names.Count; i++)
            {
                stewMeats.Add(new StewMeat(_names[i], 
                    UserUtills.GenerateNumberFromArrayLimits(_releaseYears), 
                    UserUtills.GenerateNumberFromArrayLimits(_expirationYears)));
            }

            return stewMeats;
        }
    }

    class StewMeat
    {
        private string _name;

        public StewMeat(string name, int releaseYear, int expirationYear)
        {
            _name = name;
            ReleaseYear = releaseYear;
            ExpirationYear = expirationYear;
        }

        public int ReleaseYear { get; }
        public int ExpirationYear { get; }

        public void ShowInfo()
        {
            Console.WriteLine($"{_name} изготовлена в году {ReleaseYear}. Срок годности {ExpirationYear} лет.");
        }
    }

    class UserUtills
    {
        private static Random s_random = new Random();

        public static int GenerateNumberFromArrayLimits(int[] limits)
        {
            Array.Sort(limits);

            return s_random.Next(limits[0], limits[1]);
        }
    }
}
