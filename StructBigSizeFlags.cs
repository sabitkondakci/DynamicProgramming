// LinqPad 6 evnironment

private static readonly SystemIOFlags originValue = SystemIOFlags.RefOriginFlags;

public void Main()
{
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

	private static SystemIOFlags originFlags = 
		new SystemIOFlags(cancelled:1, aborted:2, reset:3
							,terminated:4, halted:5,continuing:7);
							
	public static ref readonly SystemIOFlags RefOriginFlags => ref originFlags;
}
