using BashSoft.Contracts.SortedListContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.DataStructures
{
    public class SimpleSortedList<T> : ISimpleOrderedBag<T> where T : IComparable<T>
    {
        private const string NEGATIVE_CAPACITY_MESSAGE = "Cappacity cannot be null";

        private const int DEFAULT_SIZE = 16;

        private T[] innerCollection;
        private int size;
        private IComparer<T> comparison;

        public SimpleSortedList()
            : this(DEFAULT_SIZE, Comparer<T>.Create((x, y) => x.CompareTo(y)))
        {

        }

        public SimpleSortedList(IComparer<T> comparer)
            : this(DEFAULT_SIZE, comparer)
        {

        }

        public SimpleSortedList(int capacity)
            : this(capacity, Comparer<T>.Create((x, y) => x.CompareTo(y)))
        {

        }

        public SimpleSortedList(int capacity, IComparer<T> comparer)
        {
            InitializeInnerCollection(capacity);
            this.comparison = comparer;
        }

        public int Size
        {
            get { return this.size; }
        }

        public int Capacity
        {
            get { return this.innerCollection.Length; }
        }

        public void Add(T element)
        {
            if (element == null)
            {
                throw new ArgumentException("Value cannot be null!");
            }

            if (this.innerCollection.Length <= this.Size)
            {
                this.Resize();
            }

            this.innerCollection[this.size] = element;
            this.size++;
            Array.Sort(this.innerCollection, 0, size, this.comparison);
        }

        public void AddAll(ICollection<T> collection)
        {
            if (collection == null || collection.Any(i => i == null))
            {
                throw new ArgumentException("Collection cannot be null and cannot contains null elements!");
            }

            if (this.Size + collection.Count >= this.innerCollection.Length)
            {
                this.MultiResize(collection);
            }

            foreach (var element in collection)
            {
                this.innerCollection[this.Size] = element;
                this.size++;
            }

            Array.Sort(this.innerCollection, 0, this.Size, this.comparison);
        }

        public string JoinWith(string joiner)
        {
            if (joiner == null)
            {
                throw new ArgumentException("Joiner value cannot be null!");
            }

            var sb = new StringBuilder();

            foreach (var element in this)
            {
                sb.Append(element);
                sb.Append(joiner);
            }

            sb.Remove(sb.Length - joiner.Length, joiner.Length);
            return sb.ToString();
        }

        public bool Remove(T element)
        {
            if (element == null)
            {
                throw new ArgumentException("Value to remove cannot be null!");
            }

            var hasBeenRemoved = false;
            int indexOfRemovedElement = 0;

            for (int i = 0; i < this.Size; i++)
            {
                if (this.innerCollection[i].Equals(element))
                {
                    indexOfRemovedElement = i;
                    this.innerCollection[i] = default(T);
                    hasBeenRemoved = true;
                    break;
                }
            }

            if (hasBeenRemoved)
            {
                for (int i = indexOfRemovedElement; i < this.Size; i++)
                {
                    this.innerCollection[i] = this.innerCollection[i + 1];
                }

                this.innerCollection[this.Size - 1] = default(T);
                this.size--;
            }

            return hasBeenRemoved;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Size; i++)
            {
                yield return this.innerCollection[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void MultiResize(ICollection<T> collection)
        {
            int newSize = this.Size == 0 ? (this.Size + 1) * 2 : this.Size * 2;

            while (this.Size + collection.Count >= newSize)
            {
                newSize *= 2;
            }

            T[] newCollection = new T[newSize];
            Array.Copy(this.innerCollection, newCollection, this.Size);
            this.innerCollection = newCollection;
        }

        private void Resize()
        {
            int newSize = this.Size == 0 ? (this.Size + 1) * 2 : this.Size * 2;
            T[] newCollection = new T[newSize];
            Array.Copy(this.innerCollection, newCollection, this.Size);
            innerCollection = newCollection;
        }

        private void InitializeInnerCollection(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException(NEGATIVE_CAPACITY_MESSAGE);
            }

            this.innerCollection = new T[capacity];
        }
    }
}
