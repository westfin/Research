using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace Research.Rope
{
    public class Benchmark
    {
        string source = "Buffer";

        [Benchmark]
        public void SpanCopy()
        {
            var buffer = new char[16];
            source.ToCharArray().AsSpan().CopyTo(buffer);
        }

        [Benchmark]
        public void ArrayCopy()
        {
            var buffer = new char[16];
            Array.Copy(source.ToCharArray(), 0, buffer, 0, source.Length);
        }

        [Benchmark]
        public void BlockCopy()
        {
            var buffer = new char[16];
            Buffer.BlockCopy(source.ToCharArray(), 0, buffer, 0, sizeof(char) * source.Length);
        }

        class Program
        {
            static void Main(string[] args) => BenchmarkRunner.Run<Benchmark>();
        }
    }
}
