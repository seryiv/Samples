﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

// Note that you have to install System.ValueTuple for this sample (see .csproj)

// Show IL code during demos using dnSpy.

// Learn more about C# tuples at https://docs.microsoft.com/en-us/dotnet/articles/csharp/tuples.

namespace ConsoleApplication
{
    public class Program
    {
        static readonly (int sum, int count) staticLiteral = (0, 1);

        public static void Main(string[] args)
        {
            var numbers = new[] { 1, 2, 3, 4, 5 };

            #region Old syntax
            void AnalyzeWithOutParams(out int sum, out int count)
            {
                sum = numbers.Sum();
                count = numbers.Count();
            }

            AnalyzeWithOutParams(out var localSum, out var localCount);
            Console.WriteLine($"Sum: {localSum}, Count: {localCount}");

            // Note that `Tuple` is a reference type
            Tuple<int, int> AnalyzeWithTuple()
            {
                return new Tuple<int, int>(numbers.Sum(), numbers.Count());
            }

            var oldTupleResult = AnalyzeWithTuple();
            Console.WriteLine($"Sum: {oldTupleResult.Item1}, Count: {oldTupleResult.Item2}");
            #endregion

            #region New syntax
            // Start Syntax Visualizer during demos and show how Roslyn can be used
            // to analzye tuple types.

            // Disassemble code using dnSpy and show compiler-generated 
            // `TupleElementNamesAttribute` with tuple element names.

            // Note that the following function returns a tuple 
            // with semantic names for each member.
            (int sum, int count) Analyze() => (numbers.Sum(), numbers.Count());
            var result = Analyze();
            Console.WriteLine($"Sum: {result.sum}, Count: {result.count}");     // New syntax with names
            Console.WriteLine($"Sum: {result.Item1}, Count: {result.Item2}");   // Old syntax

            // Note that you can cast result to `ValueTuple`. Note that `ValueTuple` 
            // is a value type (struct).
            var vtResult = (ValueTuple<int, int>)result;
            Console.WriteLine($"Sum: {vtResult.Item1}, Count: {vtResult.Item2}");

            // You can skip member names if you want (doesn't make much sense)
            (int, int) Analyze2() => (numbers.Sum(), numbers.Count());
            var result2 = Analyze2();
            Console.WriteLine($"Sum: {result2.Item1}, Count: {result2.Item2}");

            // Deconstruction
            (var mySum, var myCount) = Analyze();
            // Alternate syntaxes:
            // (int mySum, int myCount) = Analyze(numbers);
            // var (mySum, myCount) = Analyze(numbers);
            // int mySum, myCount; (mySum, myCount) = Analyze(numbers);
            Console.WriteLine($"Sum: {mySum}, Count: {myCount}");

            // Tuple literals
            (int sum, int count) literalExplicitType = (sum: 0, count: 1);
            var literal = (sum: 0, count: 1);
            literal = (1, 2);
            (int, int) literalWithoutElementNames = (2, 3);
            Console.WriteLine($"Sum: {literal.sum}, Count: {literal.count}");
            result = literal; // Note that you can assign tuples
            Console.WriteLine($"Sum: {result.sum}, Count: {result.count}");
            var literal2 = (3, 4);
            Console.WriteLine($"Sum: {literal2.Item1}, Count: {literal2.Item2}");

            // Note that tuples are mutable, you can make them readonly
            literal.sum = 42;
            Console.WriteLine($"Sum: {literal.sum}, Count: {literal.count}");
            // staticLiteral.sum = 42; -> would result in an error

            // Tuples in Lambdas
            Func<(int sum, int count)> f = () => (1, 2);
            Console.WriteLine(f().sum);

            // Tuples with Tasks
            Task<(int sum, int count)> t = Task.FromResult(f());
            Console.WriteLine(t.Result);

            // Tuples and generic collections
            IEnumerable<(int sum, int count)> collection = new[] { (0, 0), (1, 1) };
            foreach (var item in collection)
            {
                Console.Write($"{item}\t");
            }

            // Tuples and LINQ
            var squaresAndSquareRoots = numbers
                .Select(n => (Square: n * n, SquareRoot: Math.Sqrt((double)n)));
            foreach(var mathResult in squaresAndSquareRoots)
            {
                Console.WriteLine(mathResult.Square);
            }

            Console.WriteLine();
            #endregion

            // Deconstructors
            var p = new Point(1, 2);
            var (x, y) = p;
            Console.WriteLine($"p.x={x}, p.y={y}");
        }
    }

    class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) { X = x; Y = y; }

        // Note deconstructor function here
        public void Deconstruct(out int x, out int y) { x = X; y = Y; }
    }
}
