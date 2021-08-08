class UnmanagedHeap
{
	static unsafe void Main()
	{
		byte* buffer = null;
		try
		{
			const int Size = 256;
			buffer = (byte*)Memory.Alloc(Size);
			for (int i = 0; i < Size; i++) buffer[i] = (byte)i;
			byte[] array = new byte[Size];
			fixed (byte* p = array) Memory.Copy(buffer, p, Size);
			for (int i = 0; i < Size; i++) Console.WriteLine(array[i]);
		}
		finally
		{
			if (buffer != null) Memory.Free(buffer);
		}
	}
}

public static unsafe class Memory
{
	// Handle for the process heap. This handle is used in all calls to the
	// HeapXXX APIs in the methods below.
	private static readonly IntPtr s_heap = GetProcessHeap();

	// Allocates a memory block of the given size. The allocated memory is
	// automatically initialized to zero.
	public static void* Alloc(int size)
	{
		void* result = HeapAlloc(s_heap, HEAP_ZERO_MEMORY, (UIntPtr)size);
		if (result == null) throw new OutOfMemoryException();
		return result;
	}

	// Copies count bytes from src to dst. The source and destination
	// blocks are permitted to overlap.
	public static void Copy(void* src, void* dst, int count)
	{
		byte* ps = (byte*)src;
		byte* pd = (byte*)dst;

		Console.WriteLine((long)ps);
		Console.WriteLine((long)pd);
		if (ps > pd)
		{
			for (; count != 0; count--) *pd++ = *ps++;
		}
		else if (ps < pd)
		{
			for (ps += count, pd += count; count != 0; count--) *--pd = *--ps;
		}
	}

	// Frees a memory block.
	public static void Free(void* block)
	{
		if (!HeapFree(s_heap, 0, block)) throw new InvalidOperationException();
	}

	// Re-allocates a memory block. If the reallocation request is for a
	// larger size, the additional region of memory is automatically
	// initialized to zero.
	public static void* ReAlloc(void* block, int size)
	{
		void* result = HeapReAlloc(s_heap, HEAP_ZERO_MEMORY, block, (UIntPtr)size);
		if (result == null) throw new OutOfMemoryException();
		return result;
	}

	// Returns the size of a memory block.
	public static int SizeOf(void* block)
	{
		int result = (int)HeapSize(s_heap, 0, block);
		if (result == -1) throw new InvalidOperationException();
		return result;
	}

	// Heap API flags
	private const int HEAP_ZERO_MEMORY = 0x00000008;

	// Heap API functions
	[DllImport("kernel32")]
	private static extern IntPtr GetProcessHeap();

	[DllImport("kernel32")]
	private static extern void* HeapAlloc(IntPtr hHeap, int flags, UIntPtr size);

	[DllImport("kernel32")]
	private static extern bool HeapFree(IntPtr hHeap, int flags, void* block);

	[DllImport("kernel32")]
	private static extern void* HeapReAlloc(IntPtr hHeap, int flags, void* block, UIntPtr size);

	[DllImport("kernel32")]
	private static extern UIntPtr HeapSize(IntPtr hHeap, int flags, void* block);
}
