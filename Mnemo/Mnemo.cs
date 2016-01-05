using System;
using System.Collections.Generic;

namespace Mnemo
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Mnemo
    {
        // Singleton
        private static Mnemo current;
        public static Mnemo Current => current ?? (current = new Mnemo());

        private readonly List<string> pConsonants = new List<string> {"b","d","g","h","j","k","m","n","p","r","s","t","z"};
        private readonly List<string> pVowels = new List<string> {"a", "e", "i", "o", "u"};
        private readonly List<string> pAdditional = new List<string> {"wa", "wo", "ya", "yo", "yu"};

        private readonly Dictionary<string, string> pSpecials = new Dictionary<string, string>()
        {
            {"hu", "fu"},
            {"si", "shi"},
            {"ti", "chi"},
            {"tu", "tsu"},
            {"zi", "tzu"}
        };

        private const string NEGATIVE_SYMBOL = "wi";

        private readonly List<string> pAvailableSyllables = new List<string>();

        private Mnemo()
        {
            // Builind syllables
            foreach (var _Consonant in pConsonants)
            {
                foreach (var _Vowel in pVowels)
                {
                    pAvailableSyllables.Add($"{_Consonant}{_Vowel}");
                }
            }
            pAvailableSyllables.AddRange(pAdditional);
        }

        public string FromInteger(int aInteger)
        {
            var _Prefix = aInteger < 0 ? NEGATIVE_SYMBOL : "";
            return $"{_Prefix}{this.ToSpecial(this.FromIntegerInner(Math.Abs(aInteger)))}";
        }

        public int ToInteger(string aWord)
        {
            return this.ToIntegerInner(this.FromSpecial(aWord));
        }

        public bool IsAMnemoWord(string aMnemo)
        {
            var _IsAMnemoWord = true;

            try
            {
                this.ToInteger(aMnemo);
            }
            catch (Exception)
            {
                _IsAMnemoWord = false;
            }

            return _IsAMnemoWord;

        }


        //public string toString(int aInteger)
        //{
        //    return this.FromIntegerInner(aInteger);
        //}

        private int FModOf(int a, int b)
        {
            if (b < 0)
            {
                return (int) (a - b*Math.Ceiling((double) (a/b)));
            }
            else
            {
                return (int) (a - b*Math.Floor((double) (a/b)));
            }
        }

        private string FromIntegerInner(int aInteger)
        {
            if (aInteger == 0)
            {
                return "";
            }

            var _Modulo = this.FModOf(aInteger, pAvailableSyllables.Count);
            int _Rest = (int) Math.Floor((double) (aInteger/pAvailableSyllables.Count));

            return $"{this.FromIntegerInner(_Rest)}{pAvailableSyllables[_Modulo]}";
        }

        

        private List<string> Split(string aWord)
        {
            var _Syllables = new List<string>();

            if (!string.IsNullOrWhiteSpace(aWord))
            {
                aWord = this.FromSpecial(aWord);

                for (var i = 0; i < aWord.Length; i++)
                {
                    var _Syllable = aWord.Substring(i, 2);

                    if (this.pAvailableSyllables.Contains(_Syllable))
                    {
                        _Syllables.Add(this.ToSpecial(_Syllable));

                    } else throw new ArgumentException($"The syllable {_Syllable} was not found");
                }
            }
            return _Syllables;
        }

        

        private int FromString(string aWord)
        {
            return this.ToInteger(aWord);
        }

        private int ToIntegerInner(string aWord)
        {
            if (string.IsNullOrWhiteSpace(aWord)) return 0;

            if (aWord.Substring(0, 2).Equals(Mnemo.NEGATIVE_SYMBOL))
            {
                return -1 * this.ToInteger(aWord.Substring(2));
            }

            return this.pAvailableSyllables.Count*
                   this.ToIntegerInner(aWord.Substring(0, aWord.Length - 2)) +
                   this.ToNumber(aWord.Substring(aWord.Length - 2));


        }

        private int ToNumber(string aSyllable)
        {
            var _Index = this.pAvailableSyllables.IndexOf(aSyllable);

            if (_Index == -1) throw new ArgumentException($"The syllable {aSyllable} was not found");

            return _Index;
        }

        private string ToSpecial(string aWord)
        {
            foreach (var _KeyValuePair in this.pSpecials)
            {
                aWord = aWord.Replace(_KeyValuePair.Key, _KeyValuePair.Value);
            }
            return aWord;
        }

        private string FromSpecial(string aWord)
        {
            foreach (var _KeyValuePair in this.pSpecials)
            {
                aWord = aWord.Replace(_KeyValuePair.Value, _KeyValuePair.Key);
            }
            return aWord;
        }

    }

}
