using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

class BestPractices
{
    public static void Main()
    {
        
         var a = BenchmarkRunner.Run<ChunkBenchmark>();

    }
    
}

public static class ArrayExtensions
{
    public static IEnumerable<int[]> Chunk(this int[] array, int size)
    {
        while (array.Any())
        {
            yield return array.Take(size).ToArray();
            array = array.Skip(size).ToArray();
        }   
    }
}

[MemoryDiagnoser]
public class ChunkBenchmark
{
    public int[] array;
    public Random random;
    [GlobalSetup]
    public void InitializeArray()
    {
        array = Enumerable.Range(1, 10_000_000).Where(x => x % 2 == 0).ToArray();
        random = new Random();
    }

    [Benchmark]
    public void PointerChunkTest()
    { 
        var pointerChunk = new PointerChunk(array, 1_000);
        for (int i = 0; i < 3; i++)
        {
            var a = pointerChunk[random.Next(1,100)];
        }
    }

    [Benchmark]
    public void LinqChunkTest()
    {
        var chunk = array.Chunk(1_000);
        for (int i = 0; i < 3; i++)
        {
            var a = chunk.ElementAt(random.Next(1,100));
        }
    }
}

[SkipLocalsInit]
// int64, int32, int16, byte, IntPtr
public sealed class PointerChunk 
{ 
    private IntPtr[] _chunkList;
    private int[] _partialList;
    private int _chunkSize;
    private int _lastLoopSize;
    private int  _chunkListSize { get; }
    private int _sizeofInt = sizeof(int);
   
    public PointerChunk(int[] largeArray,int chunkSize)
    {
        _chunkSize = chunkSize;
        _chunkList = Chunk(largeArray, chunkSize); // Chunk method handles exceptions.
        _chunkListSize = _chunkList.Length; // so that _chunkList won't be null.
    }
    
    public int Length => _chunkListSize;

    public int[] this[int i]
    {
        get
        {
            if (i == _chunkListSize - 1)
                _chunkSize = _lastLoopSize;
            
            if (i > _chunkListSize - 1)
                throw new IndexOutOfRangeException();
            else
            {
                _partialList = new int[_chunkSize];
                var tempPtr = _chunkList[i];
                
                for (int j = 0; j < _chunkSize; j++)
                {
                    var value = Marshal.ReadInt32(tempPtr, _sizeofInt * j);
                    _partialList[j] = value;
                }

                return _partialList;
            }
        }
    }

    public IntPtr[] Chunk(int[] largeArray,int chunkSize)
    {
        if (largeArray is null)
            throw new NullReferenceException("Array can't be null");
        if (largeArray.Length == 0)
            throw new ArgumentException("You can't chunk an empty array");
                
        IntPtr[] list; // thread safe IntPtr list.
        int[] tempArr = largeArray;
        int arrLength = tempArr.Length;
        
        int remainder = arrLength % chunkSize;
        int noReminderSize = arrLength / chunkSize;
        int remainderSize = arrLength / chunkSize + 1;

        int loopSize = remainder == 0 ? noReminderSize : remainderSize;
        _lastLoopSize = remainder == 0 ? chunkSize : remainder;
                
        list = new IntPtr[loopSize];

        unsafe
        {
            fixed (int* ptr = tempArr)
            {
                IntPtr iPtr = new IntPtr(ptr);
                for (int i = 0; i < loopSize; i++)
                {
                    IntPtr tempPtr = iPtr + i * _sizeofInt * chunkSize;
                    list[i] = tempPtr;
                }
            }
        }

        return list;
    }
}

