using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CWLibrary;

namespace Exam
{
    internal class Program
    {
        private const int MinWordLength = 4;
        private const int MaxWordLength = 11;

        private static readonly Random Rnd = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Get number.
        /// </summary>
        /// <typeparam name="T"> Type of number </typeparam>
        /// <param name="message"> Help message </param>
        /// <param name="conditions"> Conditions for number </param>
        /// <returns> Number </returns>
        private static T GetData<T>(string message, Predicate<T> conditions)
        {
            Console.Write(message);

            while (true)
            {
                try
                {
                    // Attempt to convert input string to required type.
                    var result = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));

                    // Check extra conditions.
                    if (conditions(result))
                        return result;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Number must be > 0: ");
                    Console.ResetColor();
                }
                catch
                {
                    // Print error message.
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong format of input data!");
                    Console.ResetColor();

                    Console.Write(message);
                }
            }
        }

        private static (string, string) GetPairWords()
        {
            var resEng = string.Empty;
            var resRus = string.Empty;

            var lengthEng = Rnd.Next(MinWordLength, MaxWordLength);
            var lengthRus = Rnd.Next(MinWordLength, MaxWordLength);

            for (var i = 0; i < lengthEng; i++)
            {
                resEng += (char)(Rnd.Next('a', 'b'+1));
            }

            for (var i = 0; i < lengthRus; i++)
            {
                resRus += (char)(Rnd.Next('а', 'в' + 1));
            }

            return (resEng, resRus);
        }

        private static void Main()
        {
            try
            {
                const string path = "dictionary.txt";

                var n = GetData<int>("Enter amount of lines: ", el => el > 0);

                using (var sw =
                    new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)))
                {
                    for (var i = 0; i < n; i++)
                    {
                        var (wordEng, wordRus) = GetPairWords();

                        sw.WriteLine($"{wordRus} {wordEng}");
                    }
                }

                var dictList = new List<Pair<string, string>>();

                using (var sr = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] temp = line.Split();
                        dictList.Add(new Pair<string, string>(temp[0], temp[1]));
                    }
                }

                var dict = new Dictionary(dictList);

                var path2 = "out.bin";

                dict.MySerialize(path2);

                var dict2 = Dictionary.MyDeserialize(path2);

                foreach (var pair in dict2)
                {
                    Console.WriteLine(pair);
                }

                Console.WriteLine();

                foreach (var pair in dict2.GetWords((char) Rnd.Next('а', 'в')))
                {
                    Console.WriteLine(pair);
                }

                Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected error!");
            }
        }
    }
}
