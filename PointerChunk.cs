using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Iced.Intel;
using Encoder = System.Text.Encoder;

class BestPractices
{
    public static void Main()
    {
         // var chunk = new PointerChunk(Enumerable.Range(1, 1000).ToArray(), 200);
         // for (int i = 0; i < chunk.Length; i++)
         // {
         //     var temp = chunk[i];
         // }
         //
         // Console.Read();

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
        _chunkList = Chunk(largeArray, chunkSize);
        _chunkListSize = _chunkList.Length;
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
        IntPtr[] list;
        int[] tempArr = largeArray;
        int arrLength = largeArray.Length;
        
        int noReminderSize = arrLength / chunkSize;
        int reminderSize = arrLength / chunkSize + 1;
        int remainder = arrLength % chunkSize;
                
        int loopSize = remainder == 0 ? noReminderSize : reminderSize;
        _lastLoopSize = remainder == 0 ? chunkSize : remainder;
                
        list = new IntPtr[loopSize];

        unsafe
        {
            fixed (int* ptr = tempArr)
            {
                IntPtr iPtr = new IntPtr(ptr);
                for (int i = 0; i < loopSize; i++)
                {
                    IntPtr tempPtr = IntPtr.Add(iPtr, i * _sizeofInt * chunkSize);
                    list[i] = tempPtr;
                }
            }
        }

        return list;
    }
}

