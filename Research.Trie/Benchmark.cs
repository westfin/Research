using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Research.Trie
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private Trie<int> _trie;
        private Dictionary<string, int> _dictionary;

        [GlobalSetup]
        public void Setup()
        {
            _trie = new Trie<int>();
            _dictionary = new Dictionary<string, int>();

            _trie.GetOrAdd("Alene", () => 1);
            DictionaryGetOrAdd("Alene", () => 1);
        }

        [Benchmark]
        public int AddTrie() => _trie.GetOrAdd("Daniella", () => 1);

        [Benchmark]
        public int AddDictionary() => DictionaryGetOrAdd("Daniella", () => 1);

        [Benchmark]
        public int GetTrie() => _trie.GetOrAdd("Alene", () => 1);

        [Benchmark]
        public int GetDictionary() => DictionaryGetOrAdd("Alene", () => 1);

        [Benchmark]
        public bool ContainsTrueTrie() => _trie.Contains("Alene");

        [Benchmark]
        public bool ContainsTrueDictionary() => _dictionary.ContainsKey("Alene");

        [Benchmark]
        public bool ContainsFalseTrie() => _trie.Contains("Daniella");

        [Benchmark]
        public bool ContainsFalseDictionary() => _dictionary.ContainsKey("Daniella");

        [Benchmark]
        public List<ReadOnlyMemory<char>> GetKeysTrie() => _trie.Keys.ToList();

        [Benchmark]
        public List<string> GetKeysDictionary() => _dictionary.Keys.ToList();

        public int DictionaryGetOrAdd(string key, Func<int> factory)
        {
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary[key] = factory.Invoke();
            }

            return _dictionary[key];
        }
    }

    class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run<Benchmark>();
    }
}
