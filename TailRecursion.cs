
// tail recursion, function calls itself as the last action
// so that it reuses the same stack frame, no-overflow
[MethodImpl(MethodImplOptions.AggressiveOptimization)]
long Fact(long n, long a)
{
	if(n==0)
		return a;
	
	// first Fact() call doesn't wait for the second Fact() call.
	// once upon termination, the previously pushed recursive call is popped and 
	// this stack space is replaced by a brand new recursive call being pushed.
	// the tail-recursive function can execute in constant stack space
	// it’s just efficient as an equivalent iterative process. 
	return Fact(n-1,a*n);
}

// recursive call, fills up the stack frame till it overflows
long Factorial(int n)
{
	if (n < 2)
		return 1;
	
	// first Factorial() call has to wait for other 
	// n * Factorial() calls, it'll be stacked up 
	// till the overflow.
	return n * Factorial(n - 1);
}

// tail recursion for fibonacci series
[MethodImpl(MethodImplOptions.AggressiveOptimization)]
long Fibo(int term, long prev = 0L, long curr = 1L)
{
	if(term == 0) 
		return prev;
		
	return Fibo(term - 1, curr, curr + prev);
}

// implementation with Tuple
long Fibonacci(int n)
{
	if(n == 0) return 0;
	if(n == 1) return 1;
	
	var (prev, curr) = (0L, 1L);
	for (int i = 2; i <= n; i++)
		(prev, curr) = (curr, curr + prev);

	return curr;
}

