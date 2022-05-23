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

// When a base class implements SafeHandle
class DerivedClassWithSafeHandle : BaseClassWithSafeHandle
{
    // To detect redundant calls
    private bool _disposedValue;

    // Instantiate a SafeHandle instance.
    private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

    // Protected implementation of Dispose pattern.
    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _safeHandle.Dispose();
            }

            _disposedValue = true;
        }

        // Call base class implementation, Idempotent!
        base.Dispose(disposing);
    }
}


// When a base class implements Finalizer pattern
class DerivedClassWithFinalizer : BaseClassWithFinalizer
{
    // To detect redundant calls
    private bool _disposedValue;

    ~DerivedClassWithFinalizer() => this.Dispose(false);

    // Protected implementation of Dispose pattern.
    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.
            _disposedValue = true;
        }

        // Call the base class implementation.
        base.Dispose(disposing);
    }
}
