public int FibonacciWithMemo(int n)
{
    if (n < 0)
        throw new ArgumentException("n >= 0", nameof(n));
    if (n <= 1)
        return n;

    var dictMemo = new Dictionary<int, int>() { { 0, 0 }, { 1, 1 } };

    int LocalMemoFibonacci(int key)
    {
        if (dictMemo.ContainsKey(key))
            return dictMemo[key];

        int value = LocalMemoFibonacci(key - 1) + LocalMemoFibonacci(key - 2);

        dictMemo[key] = value;

        return value;
    }

    return LocalMemoFibonacci(n);
}
