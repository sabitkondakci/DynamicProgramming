public class ExampleAsyncDisposable : IAsyncDisposable, IDisposable
{
	private Utf8JsonWriter _jsonWriter;

	public ExampleAsyncDisposable()
	{
		_jsonWriter = new(new MemoryStream());
	}
	
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		Dispose(false); // dispose unmanaged objects
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_jsonWriter?.Dispose();
		}

		_jsonWriter = null;
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (_jsonWriter is not null)
		{
			await _jsonWriter.DisposeAsync().ConfigureAwait(false);
		}

		_jsonWriter = null;
	}
}



// second approach

class ExampleConjunctiveDisposableusing : IDisposable, IAsyncDisposable
{
	IDisposable _disposableResource = new MemoryStream();
	IAsyncDisposable _asyncDisposableResource = new MemoryStream();

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_disposableResource?.Dispose();
			(_asyncDisposableResource as IDisposable)?.Dispose();
		}

		_disposableResource = null;
		_asyncDisposableResource = null;
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (_asyncDisposableResource is not null)
		{
			await _asyncDisposableResource.DisposeAsync().ConfigureAwait(false);
		}

		if (_disposableResource is IAsyncDisposable disposable)
		{
			await disposable.DisposeAsync().ConfigureAwait(false);
		}
		else
		{
			_disposableResource?.Dispose();
		}

		_asyncDisposableResource = null;
		_disposableResource = null;
	}
}

// using await in main

class ExampleConfigureAwaitProgram
{
    static async Task Main()
    {
        var exampleAsyncDisposable = new ExampleAsyncDisposable();
        await using (exampleAsyncDisposable.ConfigureAwait(false))
        {
            // Interact with the exampleAsyncDisposable instance.
        }

        Console.ReadLine();
    }
}
