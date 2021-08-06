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
                
                for (ushort j = 0; j < _chunkSize; j++)
                {
                    var tempPtr = IntPtr.Add(_chunkList[i], size * j);
                    var value = Marshal.ReadInt32(tempPtr);
                    _partialList[j] = value;
                }
                
                return _partialList;
            }
        }
    }
    
    private unsafe IntPtr[] Chunk(int[] largeArray,int chunkSize)
    {
        IntPtr[] list;
        var tempArr = largeArray;
        
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
