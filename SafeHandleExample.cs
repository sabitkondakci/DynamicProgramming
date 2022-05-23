class BaseClassWithSafeHandle : IDisposable
{
	// To detect redundant calls
	private bool _disposed;

	// Instantiate a SafeHandle instance.
	private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

	// Public implementation of Dispose pattern callable by consumers.
	public void Dispose() => Dispose(true);

	// Protected implementation of Dispose pattern.
	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_safeHandle.Dispose();
			}

			_disposed = true;
		}
	}
}
