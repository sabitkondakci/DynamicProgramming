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
