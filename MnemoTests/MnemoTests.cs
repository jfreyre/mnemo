using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MnemoTests
{
    [TestClass]
    public class MnemoTests
    {
        [TestMethod]
        public void TestFirstMillion()
        {

            var current = Mnemo.Mnemo.Current;

            for (int i = 1; i < 100000; i++)
            {
                var _Word = current.FromInteger(i);
                var _Result = current.ToInteger(_Word);

                Assert.AreEqual(i, _Result);
            }
        }

        [TestMethod]
        public void TestBasicConversion()
        {
            Dictionary<string, int> _DataSet = new Dictionary<string, int>()
            {
                 {"nada", 2455},
                 {"haruka", 76955},
                 {"karasu", 125704},
                 {"kazuma", 127010},
                 {"kotoba", 141260},
                 {"takeshimaya", 1329724967},
                 {"winamote", -173866},
                 {"wina", -35},
            };

            foreach (var _Set in _DataSet)
            {
                Assert.AreEqual(_Set.Key, Mnemo.Mnemo.Current.FromInteger(_Set.Value));
                Assert.AreEqual(_Set.Value, Mnemo.Mnemo.Current.ToInteger(_Set.Key));
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void TestInvalid()
        {
            Mnemo.Mnemo.Current.ToInteger("lorem");
        }

        [TestMethod]
        public void TestIfIsMnemoWord()
        {
            Dictionary<string, bool> _DataSet = new Dictionary<string, bool>()
            {
                 {"fugu", true},
                 {"kazuma", true},
                 {"toriyamanobashi", true},
                 {"George", false},
                 {"abscdef", false},
                 {"ijklmnopq", false},
                 {"_a_", false}
            };

            foreach (var _Set in _DataSet)
            {
                Assert.AreEqual(_Set.Value, Mnemo.Mnemo.Current.IsAMnemoWord(_Set.Key));
            }
        }

        [TestMethod]
        public void TestZeroAndEmpty()
        {
            Assert.AreEqual(0, Mnemo.Mnemo.Current.ToInteger(""));
            Assert.AreEqual(0, Mnemo.Mnemo.Current.ToInteger("  "));

        }

    }

   
}
