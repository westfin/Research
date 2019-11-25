using System;
using System.Collections.Generic;
using System.Linq;

namespace Research.Heap
{
    /// <summary>
    /// Binary min heap
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryHeap<T>
    {
        private readonly IComparer<T> _comparer;

        private readonly List<T> _storage;

        public int HeapSize => _storage.Count;

        public bool IsEmpty => _storage.Count == 0;

        public BinaryHeap()
            : this(Comparer<T>.Default)
        { }

        public BinaryHeap(IComparer<T> comparer)
        {
            _comparer = comparer;
            _storage = new List<T>();
        }

        /// <summary>
        /// Get and remove minimum O(log(n))
        /// </summary>
        /// <returns>Minimum</returns>
        public T ExtractMin()
        {
            T min = GetMin();

            var index = 0;
            var lstIndex = HeapSize - 1;

            _storage[index] = _storage[lstIndex];
            _storage.RemoveAt(lstIndex);

            while (true)
            {
                var minIndex = index;
                var lftIndex = GetLftIndex(index);
                var rhtIndex = GetRhtIndex(index);

                if (rhtIndex < HeapSize && _comparer.Compare(_storage[rhtIndex], _storage[minIndex]) < 0)
                {
                    minIndex = rhtIndex;
                }

                if (lftIndex < HeapSize && _comparer.Compare(_storage[lftIndex], _storage[minIndex]) < 0)
                {
                    minIndex = lftIndex;
                }

                if (minIndex == index)
                {
                    break;
                }

                T tmp = _storage[index];
                _storage[index] = _storage[minIndex];
                _storage[minIndex] = tmp;
                index = minIndex;
            }

            return min;
        }

        /// <summary>
        ///  Get minimum O(1)
        /// </summary>
        /// <returns>Minimum</returns>
        public T GetMin() => _storage.Count > 0 ?
            _storage[0] : throw new InvalidOperationException("Binary heap is empty");

        /// <summary>
        /// O(log(n))
        /// </summary>
        public void Insert(T item)
        {
            _storage.Add(item);

            int index = _storage.Count - 1;

            while (index != 0)
            {
                var parentIndex = GetParentIndex(index);
                T parent = _storage[parentIndex];

                if (_comparer.Compare(item, parent) > 0)
                {
                    break;
                }

                _storage[index] = parent;
                _storage[parentIndex] = item;
                index = parentIndex;
            }
        }

        private static int GetLftIndex(int index) => 2 * index + 1;

        private static int GetRhtIndex(int index) => 2 * index + 2;

        private static int GetParentIndex(int index) => (index - 1) / 2;
    }
}
