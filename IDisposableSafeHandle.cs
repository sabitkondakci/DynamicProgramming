void Main()
{
	string[] websites = { "http://www.olay53.com", "http://www.bilecikolay.com/"};
	DownloadWebsiteContent d = new(websites);
	foreach (var content in d.Download())
		content.Dump();
	
	d.Dispose();
}
// SafeHandle provides built-in finalizer, it's recommended to use this approach
// it's a abstratc wrapper class for operating system handles which must be inherited.
// SafeHandle class provides critical finalization of handle resources, 
// preventing handles from being reclaimed prematurely by garbage collection and from
// being recycled by Windows to reference unintended unmanaged objects.
public class SafeUnmanagedClass : SafeHandle
{
	public SafeUnmanagedClass() : base(IntPtr.Zero,false){	}
	public override bool IsInvalid => false;
	protected override bool ReleaseHandle() => base.ReleaseHandle();
}

public class DownloadWebsiteContent : IDisposable
{
	WebClient client;
	string[] urlList;
	bool disposed = false;
	SafeUnmanagedClass safeClass ;
	
	public DownloadWebsiteContent(string[] urls)
	{
		safeClass = new();
		urlList = urls;
		client = new();
	}
	
	public IEnumerable<string> Download()
	{
		if (urlList is not null)
		{
			for (int i = 0; i < urlList.Length; i++)
			{
				yield return client.DownloadString(urlList[i]);
			}
		}
	}
	
	public void Dispose()
	{
		Dispose(true); // true : release managed and unmanaged objects.
	}
	
	protected virtual void Dispose ( bool disposeManagedObjects)
	{
		if (!disposed)
		{
			if (disposeManagedObjects)
			{
				urlList = null; // GC will collect this array earlier.
				safeClass.Dispose(); // safeHandle is a safe unmanaged object wrapper.
			}

			client.Dispose(); // unmanaged objects got released
			disposed = true; 
		}
		
		// if there's a base class that also impements IDisposable
		// base.Dispose(disposeManagedObjects);
	}
	
	// SafeHandle is recommended!
	~DownloadWebsiteContent() { Dispose(false);} // false : release unmanaged objects
}
