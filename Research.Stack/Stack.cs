using System;
using System.Collections.Generic;

namespace Research.Stack
{
    public class Stack<T>
    {
        private readonly List<T> _storage
            = new List<T>();

        private const int DefaultCapacity = 4;

        public Stack() : this(DefaultCapacity)
        { }

        public Stack(int capacity)
            => _storage = new List<T>(capacity);

        public bool IsEmpty => _storage.Count == 0;

        public void Push(T item) => _storage.Add(item);

        public T Peek()
        {
            ThrowIfStackIsEmpty();

            return _storage[_storage.Count - 1];
        }

        public T Pop()
        {
            ThrowIfStackIsEmpty();

            var index = _storage.Count - 1;
            var item = _storage[index];
            _storage.RemoveAt(index);
            return item;
        }

        private void ThrowIfStackIsEmpty()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Stack is empty");
            }
        }
    }
}
