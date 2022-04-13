// LinqPad 6 evnironment
// MSDN : https://docs.microsoft.com/en-us/dotnet/csharp/write-safe-efficient-code#use-ref-readonly-return-statements

// create a global variable for SystemIOFlags, this will cut the need of "recreation of same struct on stack" on each call.
private static readonly SystemIOFlags originValue = SystemIOFlags.RefOriginFlags;

public void Main()
{
	// reusable reference on stack
	ref readonly var originReference = ref originValue;

	originValue.Dump();
	originReference.Dump();

}

public struct SystemIOFlags
{	
	private byte _cancelled;
	public byte Cancelled { readonly get => _cancelled; set => _cancelled = value; }
	
	private byte _aborted;
	public byte Aborted { readonly get => _aborted; set => _aborted = value; }
	
	private byte _reset;
	public byte Reset { readonly get => _reset; set => _reset = value; }
	
	private byte _terminated;
	public byte Terminated { readonly get => _terminated; set => _terminated = value; }
	
	private byte _halted;
	public byte Halted { readonly get => _halted; set => _halted = value; }
	
	private byte _continuing;
	public byte Continuing { readonly get => _continuing; set => _continuing = value; }
	
	// and more ....

	public SystemIOFlags(byte cancelled, byte aborted, byte reset
							, byte terminated, byte halted, byte continuing)
	{
		_cancelled = cancelled;
		_aborted = aborted;
		_reset = reset;
		_terminated = terminated;
		_halted = halted;
		_continuing = continuing;
	
	}
	
	// create a long-term instance of SystemIOFlags
	private static SystemIOFlags originFlags = 
		new SystemIOFlags(cancelled:1, aborted:2, reset:3
							,terminated:4, halted:5,continuing:7);
	
	// use the same reference on each call, this should reduce some overheads.
	public static ref readonly SystemIOFlags RefOriginFlags => ref originFlags;
}
