using System;
using ACE.Client.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Client.Test.Client.Infrastructure
{
    [TestClass]
    public class CircularListTests
    {
        [TestMethod]
        public void InstantiationTest()
        {
            var target = new CircularList<char>(10);
            Assert.IsNotNull(target);
        }

        private char[] letters = new char[] {'A', 'B', 'C', 'D'};
        [TestMethod]
        public void AddPeek1Test()
        {
            var target = new CircularList<char>(4);
            target.Add(letters[0]);
            var actual = target.Peek();
            Assert.AreEqual(letters[0], actual);
        }
        [TestMethod]
        public void AddPeek2Test()
        {
            var target = new CircularList<char>(4);
            target.Add(letters[0]);
            target.Add(letters[1]);
            var actual = target.Peek();
            Assert.AreEqual(letters[1], actual);
        }
        [TestMethod]
        public void AddPeek3Test()
        {
            var target = new CircularList<char>(4);
            target.Add(letters[0]);
            target.Add(letters[1]);
            var actual = target.Peek(-1);
            Assert.AreEqual(letters[0], actual);
        }
        [TestMethod]
        public void AddPeek4Test()
        {
            var target = new CircularList<char>(2);
            target.Add(letters[0]);
            target.Add(letters[1]);
            target.Add(letters[2]);
            var actual = target.Peek(0);
            Assert.AreEqual(letters[2], actual);
            actual = target.Peek(-1);
            Assert.AreEqual(letters[1], actual);
        }

        [TestMethod]
        public void AddRemove1Test()
        {
            var target = new CircularList<char>(2);
            target.Add(letters[0]);
            target.Remove(letters[0]);
            target.Add(letters[1]);
            var actual = target.Peek();
            Assert.AreEqual(letters[1], actual);
        }
        [TestMethod]
        public void AddRemove2Test()
        {
            var target = new CircularList<char>(2);
            target.Add(letters[0]);
            target.Add(letters[1]);
            target.Remove(letters[0]);
            var actual = target.Peek();
            Assert.AreEqual(letters[1], actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRemove3Test()
        {
            var target = new CircularList<char>(2);
            target.Add(letters[0]);
            target.Add(letters[1]);
            target.Add(letters[2]);
            target.Remove(letters[0]);
        }
        [TestMethod]
        public void AddRemove4Test()
        {
            var target = new CircularList<char>(3);
            target.Add(letters[0]);
            target.Add(letters[1]);
            target.Add(letters[2]);
            target.Remove(letters[2]);
            var actual = target.Peek();
            Assert.AreEqual(letters[1], actual);
            actual = target.Peek(-1);
            Assert.AreEqual(letters[0], actual);
        }

        [TestMethod]
        public void AddRemove5Test()
        {
            var target = new CircularList<char>(3);
            target.Add(letters[0]);
            target.Add(letters[1]);
            target.Add(letters[2]);
            target.Remove(letters[0]);
            var actual = target.Peek();
            Assert.AreEqual(letters[2], actual);
            actual = target.Peek(-1);
            Assert.AreEqual(letters[1], actual);
            target.Add(letters[3]);
            actual = target.Peek();
            Assert.AreEqual(letters[3], actual);
        }


        [TestMethod]
        public void AddRemove6Test()
        {
            var target = new CircularList<char>(3);
            target.Add(letters[0]);
            target.Add(letters[1]);
            target.Add(letters[2]);
            target.Remove(letters[1]);
            var actual = target.Peek();
            Assert.AreEqual(letters[2], actual);
            actual = target.Peek(-1);
            Assert.AreEqual(letters[0], actual);
            target.Add(letters[3]);
            actual = target.Peek();
            Assert.AreEqual(letters[3], actual);
        }


    }
}
