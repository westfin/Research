using System;

namespace Research.Rope
{
    public class Rope
    {
        private readonly Node _root;

        private const int NodeCapacity = 256;

        public int Length => _root._length;

        public Rope() : this(new Node())
        { }

        private Rope(Node root) => _root = root;

        public void Insert(int index, string text) => Insert(index, text.AsSpan());

        public void Insert(int index, string text, int count) => Insert(index, text.AsSpan(0, count));

        public void Insert(int index, ReadOnlySpan<char> text)
        {
            if (index > _root._length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        private static Node Insert(Node node, int index, ReadOnlySpan<char> text)
        {
            throw new NotImplementedException();
        }

        public char this[int index] => GetChar(_root, index);

        private static char GetChar(Node node, int index)
        {
            if (index >= node._length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (node.IsLeaf)
            {
                return node._content[index];
            }

            return index < node._left._length ? GetChar(node._left, index) : GetChar(node._right, index - node._left._length - 1);
        }

        private static Node CreateLeaf() => CreateNode(true);

        private static Node CreateNode(bool leaf = false)
        {
            var n = new Node();

            if (leaf)
            {
                n._content = new char[NodeCapacity];
            }

            return n;
        }

        class Node
        {
            public Node _left;

            public Node _right;

            public int _length;

            public char[] _content;

            public bool IsLeaf => _content != null;
        }
    }
}
