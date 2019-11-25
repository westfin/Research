using Research.Stack;
using System;

namespace Research.Queue.Stacks
{
    public class Queue<T>
    {
        private readonly Stack<T> _head
            = new Stack<T>();

        private readonly Stack<T> _tail
            = new Stack<T>();

        public bool IsEmpty => _head.IsEmpty && _tail.IsEmpty;

        public void Enqueue(T item) => _tail.Push(item);

        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty");

            if (_head.IsEmpty)
            {
                while (!_tail.IsEmpty)
                {
                    _head.Push(_tail.Pop());
                }
            }

            return _head.Pop();
        }
    }
}
