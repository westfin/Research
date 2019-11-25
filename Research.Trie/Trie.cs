using System;
using System.Collections.Generic;

namespace Research.Trie
{
    /// <summary>
    /// Префиксное дерево
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Trie<TValue>
    {
        private static int R = 256;

        private int _maxKeyLength = 0;

        private readonly Node<TValue> _root = new Node<TValue>();

        public int Count { get; private set; }

        public TValue GetOrAdd(string key, Func<TValue> factory) => GetOrAdd(key.AsSpan(), factory);

        public TValue GetOrAdd(ReadOnlyMemory<char> key, Func<TValue> factory) => GetOrAdd(key.Span, factory);

        public TValue GetOrAdd(ReadOnlySpan<char> key, Func<TValue> factory)
        {
            _maxKeyLength = key.Length > _maxKeyLength ? key.Length : _maxKeyLength;

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var node = _root;

            for (int i = 0; i < key.Length; i++)
            {
                if (node._childs == null)
                {
                    node._childs = new Node<TValue>[R];
                }

                var index = key[i];

                if (node._childs[index] == null)
                {
                    var n = new Node<TValue>
                    {
                        _parent = node,
                        _symbol = index
                    };
                    node._childs[index] = n;
                }

                node = node._childs[index];
            }

            if (!node._leaf)
            {
                node._value = factory.Invoke();
                Count++;
            }

            node._leaf = true;

            return node._value;
        }

        public bool Contains(string key) => Contains(key.AsSpan());

        public bool Contains(ReadOnlyMemory<char> key) => Contains(key.Span);

        public bool Contains(ReadOnlySpan<char> key)
        {
            var node = _root;

            for (int i = 0; i < key.Length; i++)
            {
                var index = key[i];

                if (node._childs == null || node._childs[index] == null)
                {
                    return false;
                }

                node = node._childs[index];
            }

            return node._leaf;
        }

        public void Clear()
        {
            Count = 0;
            Clear(_root);
        }

        private static void Clear(Node<TValue> node)
        {
            if (node == null)
            {
                return;
            }

            node._leaf = false;
            node._value = default;

            if (node._childs == null)
            {
                return;
            }

            foreach (var child in node._childs)
            {
                Clear(child);
            }
        }

        public TValue this[string key]
        {
            get => this[key.AsSpan()];
            set => this[key.AsSpan()] = value;
        }

        public TValue this[ReadOnlyMemory<char> key]
        {
            get => this[key.Span];
            set => this[key.Span] = value;
        }

        public TValue this[ReadOnlySpan<char> key]
        {
            get
            {
                var node = GetLeafByKey(_root, key);

                return node._value;
            }
            set
            {
                var node = GetLeafByKey(_root, key);

                node._value = value;
            }
        }

        private static Node<TValue> GetLeafByKey(Node<TValue> node, ReadOnlySpan<char> key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                var index = key[i];

                if (node._childs == null || node._childs[index] == null)
                {
                    throw new KeyNotFoundException($"Key '{key.ToString()}' not found!");
                }

                node = node._childs[index];
            }

            return node;
        }

        public IEnumerable<ReadOnlyMemory<char>> Keys => EnumerateKeys();

        private IEnumerable<ReadOnlyMemory<char>> EnumerateKeys()
        {
            var c = 0;
            var node = _root;

            Memory<char> buffer = new char[_maxKeyLength];

            while (node != null)
            {
                for (; c < R; c++)
                {
                    var n = node._childs[c];
                    if (n != null)
                    {
                        node = n;
                        break;
                    }
                }

                if (node._leaf && c < R)
                {
                    yield return YieldKey(buffer, node);
                }

                if (node._childs == null || c >= R)
                {
                    c = node._symbol + 1;
                    node = node._parent;
                }
                else c = 0;
            }
        }

        /// <summary>
        /// Строит ключ, начиная с листа и двигаясь к корню
        /// </summary>
        /// <param name="buffer">Буфер для ключа</param>
        /// <param name="leaf">Лист</param>
        /// <returns>Ключ</returns>
        private ReadOnlyMemory<char> YieldKey(Memory<char> buffer, Node<TValue> leaf)
        {
            var i = _maxKeyLength;
            while (leaf._parent != null)
            {
                buffer.Span[--i] = leaf._symbol;
                leaf = leaf._parent;
            }

            return buffer.Slice(i, _maxKeyLength - i);
        }

        class Node<T>
        {
            public Node<T> _parent;

            public char _symbol;

            public bool _leaf = false;

            public Node<T>[] _childs;

            public T _value;
        }
    }
}
