using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        //var chunk = new PointerChunk(Enumerable.Range(1, 1000).ToArray(), 200);
        // for (int i = 0; i < chunk.Length; i++)
        // {
        //     var temp = chunk[i];
        // }
       
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

    [GlobalSetup]
    public void InitializeArray()
    {
        array = Enumerable.Range(1, 10_000_000).Where(x => x % 2 == 0).ToArray();
    }

    [Benchmark]
    public void PointerChunkTest()
    { 
        var pointerChunk = new PointerChunk(array, 1_000);
        for (int i = 0; i < 100; i++)
        {
            var a = pointerChunk[3];
        }
    }

    [Benchmark]
    public void LinqChunkTest()
    {
        var chunk = array.Chunk(1_000);
        for (int i = 0; i < 100; i++)
        {
            var a = chunk[3];
        }
    }
}

public sealed class PointerChunk : IDisposable
{ 
    private IntPtr[] _chunkList;
    private int[] _partialList;
    private int _chunkSize;
    private int _lastLoopSize;
    private int  _chunkListSize { get; }
   
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
            var size = sizeof(int);

            if (i == _chunkListSize - 1)
                _chunkSize = _lastLoopSize;

            if (i > _chunkListSize - 1)
                return null;
            else
            {
                _partialList = new int[_chunkSize];
                var tempPtr = _chunkList[i];
                for (int j = 0; j < _chunkSize; j++)
                {
                    var value = Marshal.ReadInt32(tempPtr, size * j);
                    _partialList[j] = value;
                }

                return _partialList;
            }
        }
    }

    public IntPtr[] Chunk(int[] largeArray,int chunkSize)
    {
        IntPtr[] list;
        var tempArr = largeArray;

        unsafe
        {
            fixed (int* ptr = tempArr)
            {
                IntPtr iPtr = new IntPtr(ptr);
                int arrLength = largeArray.Length;
            
                int noReminderSize = arrLength / chunkSize;
                int reminderSize = arrLength / chunkSize + 1;
                int remainder = arrLength % chunkSize;
            
                int bounce = sizeof(int);
                int loopSize = remainder == 0 ? noReminderSize : reminderSize;
                _lastLoopSize = remainder == 0 ? chunkSize : remainder;

                list = new IntPtr[loopSize];
                for (int i = 0; i < loopSize; i++)
                {
                    IntPtr tempPtr = IntPtr.Add(iPtr, i * bounce * chunkSize);
                    list[i] = tempPtr;
                }
            }
        }

        return list;
    }

    public void Dispose()
    {
        for (int i = 0; i < _chunkListSize; i++)
        {
            Marshal.FreeHGlobal(_chunkList[i]);
        }
    }
}

