using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CWLibrary
{
    [Serializable]
    public class Dictionary
    {
        private static readonly Random Rnd = new Random(DateTime.Now.Millisecond);

        private int locate; //0 - русс - англ, 1 - англ - рус
        private List<Pair<string, string>> words = new List<Pair<string, string>>();

        internal Dictionary()
        {
            locate = Rnd.Next(2);
        }

        public Dictionary(List<Pair<string, string>> words)
        {
            locate = Rnd.Next(2);
            this.words = words;
        }

        internal void Add(Pair<string, string> pair)
        {
            words.Add(pair);
        }

        internal void Add(string word1, string word2)
        {
            words.Add(new Pair<string, string>(word1, word2));
        }

        public IEnumerator GetEnumerator()
        {
            return locate == 0
                         ? words.GetRange(0, words.Count)
                             .OrderBy(w => w.item1)
                             .Select(w => w)
                             .GetEnumerator()

                         : words.GetRange(0, words.Count)
                             .OrderBy(w => w.item2)
                             .Select(w => w)
                             .GetEnumerator();
        }

        public IEnumerable GetWords(char letter)
        {
            return words
                    .GetRange(0, words.Count)
                    .OrderBy(w => w.item1)
                    .Where(w => w.item1.StartsWith(letter.ToString()))
                    .ToList();
        }

        public void MySerialize(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = new BinaryFormatter();

                formatter.Serialize(fs, this);
            }
        }

        public static Dictionary MyDeserialize(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var formatter = new BinaryFormatter();

                var dict = (Dictionary)formatter.Deserialize(fs);

                return dict;
            }
        }
    }
}
