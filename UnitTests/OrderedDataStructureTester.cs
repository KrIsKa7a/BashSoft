using BashSoft.Contracts.SortedListContracts;
using BashSoft.DataStructures;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnitTests
{
    [TestFixture]
    public class OrderedDataStructureTester
    {
        private ISimpleOrderedBag<string> names;

        [SetUp]
        public void Initialize()
        {
            this.names = new SimpleSortedList<string>();
        }

        [Test]
        public void TestEmptyCtor()
        {
            this.names = new SimpleSortedList<string>();

            Assert.That(this.names.Capacity, Is.EqualTo(16));
            Assert.That(this.names.Size, Is.EqualTo(0));
        }

        [Test]
        public void TestCtorWithInitialCapacity()
        {
            this.names = new SimpleSortedList<string>(20);

            Assert.That(this.names.Capacity, Is.EqualTo(20));
            Assert.That(this.names.Size, Is.EqualTo(0));
        }

        [Test]
        public void TestCtorWithAllParameters()
        {
            this.names = new SimpleSortedList<string>(20, StringComparer.OrdinalIgnoreCase);

            var innerComparerField = typeof(SimpleSortedList<string>)
                .GetField("comparison", BindingFlags.NonPublic | BindingFlags.Instance);

            var innerFieldValue = innerComparerField
                .GetValue(this.names);

            Assert.That(this.names.Capacity, Is.EqualTo(20));
            Assert.That(this.names.Size, Is.EqualTo(0));
            Assert.That(innerFieldValue, Is.EqualTo(StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void TestCtorWithComparison()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase);

            var innerComparerField = typeof(SimpleSortedList<string>)
                .GetField("comparison", BindingFlags.NonPublic | BindingFlags.Instance);

            var innerFieldValue = innerComparerField
                .GetValue(this.names);

            Assert.That(this.names.Capacity, Is.EqualTo(16));
            Assert.That(this.names.Size, Is.EqualTo(0));
            Assert.That(innerFieldValue, Is.EqualTo(StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void TestAddIncreasesSize()
        {
            this.names.Add("Pesho");

            Assert.That(this.names.Size, Is.EqualTo(1));
        }

        [Test]
        public void TestAddingNullValue()
        {
            this.names.Add("Pesho");

            Assert.Throws<ArgumentException>(() => this.names.Add(null));
        }

        [Test]
        public void TestAddUnsortedDataIsHeldSorted()
        {
            this.names.Add("Rosen");
            this.names.Add("Georgi");
            this.names.Add("Balkan");

            var expectedValues = new string[] { "Balkan", "Georgi", "Rosen" };

            var actualValues = new List<string>();

            foreach (var value in this.names)
            {
                actualValues.Add(value);
            }

            var actualValuesAsArray = actualValues.ToArray();

            Assert.That(actualValuesAsArray, Is.EquivalentTo(expectedValues));
        }

        [Test]
        public void TestAddingMoreThanInitialCapacity()
        {
            for (int i = 0; i < 17; i++)
            {
                this.names.Add(i.ToString());
            }

            Assert.That(this.names.Size, Is.EqualTo(17));
            Assert.That(this.names.Capacity, Is.GreaterThan(16));
        }

        [Test]
        public void TestAddingAllFromCollectionIncreasesSize()
        {
            var names = new List<string>() { "Pesho", "Gosho" };

            this.names.AddAll(names);

            Assert.That(this.names.Size, Is.EqualTo(2));
        }

        [Test]
        public void TestAddingAllFromNullThrowsException()
        {
            Assert.Throws<ArgumentException>(() => this.names.AddAll(null));
        }

        [Test]
        public void TestAddAllKeepsSorted()
        {
            var passedValues = new List<string>() { "Rosen", "Georgi", "Balkan" };
            var expectedValues = new List<string>() { "Balkan", "Georgi", "Rosen" };

            var actualValues = new List<string>();

            this.names.AddAll(passedValues);

            foreach (var value in this.names)
            {
                actualValues.Add(value);
            }

            Assert.That(actualValues, Is.EquivalentTo(expectedValues));
        }

        [Test]
        public void TestRemoveValidElementDecreasesSize()
        {
            this.names.Add("Pesho");
            this.names.Add("Gosho");

            bool removed = this.names.Remove("Pesho");

            Assert.That(removed, Is.EqualTo(true));
            Assert.That(this.names.Size, Is.EqualTo(1));
        }

        [Test]
        public void TestRemoveValidElementRemovesSelectedOne()
        {
            this.names.Add("Ivan");
            this.names.Add("Nasko");

            bool removed = this.names.Remove("Ivan");

            Assert.That(removed, Is.EqualTo(true));
            Assert.That(() => this.names.Contains("Ivan") == false);
            Assert.That(this.names.Size, Is.EqualTo(1));
        }

        [Test]
        public void TestRemovingNullThrowsException()
        {
            this.names.Add("Pesho");

            Assert.Throws<ArgumentException>(() => this.names.Remove(null));
        }

        [Test]
        public void TestJoinWithNull()
        {
            this.names.Add("Pesho");
            this.names.Add("Gosho");

            Assert.Throws<ArgumentException>(() => this.names.JoinWith(null));
        }

        [Test]
        public void TestJoinWorksFine()
        {
            this.names.Add("Pesho");
            this.names.Add("Gosho");

            string expected = "Gosho, Pesho";

            string result = this.names.JoinWith(", ");

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
