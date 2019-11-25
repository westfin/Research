using System;

namespace Research.Queue.Circular
{
    public class Queue<T>
    {
        private const int DefaultCapacity = 2;

        private T[] _storage = new T[DefaultCapacity];

        private int _headIndex = 0;

        private int _tailIndex = 0;

        private int _count = 0;

        private int Head => GetIndex(_headIndex);

        private int Tail => GetIndex(_tailIndex);

        public bool IsEmpty => _count == 0;

        public void Enqueue(T item)
        {
            if (_count >= _storage.Length)
            {
                Resize();
            }

            _storage[Tail] = item;
            _tailIndex++;
            _count++;
        }

        private void Resize()
        {
            T[] temp = new T[_storage.Length * 2];
            int current = Head;

            for (int i = 0; i < _count; i++)
            {
                temp[i] = _storage[current];
                current = GetIndex(current + 1);
            }

            _headIndex = 0;
            _tailIndex = _count;
            _storage = temp;
        }

        public T Dequeue()
        {
            ThrowIfQueueIsEmpty();

            int head = Head;
            T item = _storage[head];
            _storage[head] = default;
            _headIndex++;
            _count--;
            return item;
        }

        public T Peek()
        {
            ThrowIfQueueIsEmpty();

            return _storage[Head];
        }

        private int GetIndex(int index) => index % _storage.Length;

        private void ThrowIfQueueIsEmpty()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty");
            }
        }
    }
}
