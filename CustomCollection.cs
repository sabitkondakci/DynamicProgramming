        interface IReadOnlyArray<out T> : IEnumerable<T>, IEnumerable ,IEnumerator<T>,IEnumerator
        {
            // your custom implementations
            T this[int index] { get; }
        }
        
        public struct ReadOnlyArray<T> : IReadOnlyArray<T>
        {
            private readonly T[] _array;

            public int Length => _array.Length;

            // Enumerators are positioned before the first element
            // until the first MoveNext() call
            int position; // Enumerator

            public ReadOnlyArray(T[]  array)
            {          
                position = -1;
                _array = array;
            }

            // such a nice pointer shot! 
            public ref readonly T IndexReference(int index)
            {
                return ref this._array[index];
            }
                                  
            
            public static implicit operator  ReadOnlyArray<T>(T[]? array) => 
             new ReadOnlyArray<T>(array ?? new T[1]);
        

            // IEnumerable<T>
            public IEnumerator<T> GetEnumerator()
            {
                return new ReadOnlyArray<T>(_array);
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }


            // IEnumerator<T>
            
            public bool MoveNext()
            {
                position++;
                return (position < _array.Length);
            }

            public void Reset()
            {
                position = -1;
            }

            public void Dispose()
            {     
                // extra disposing!
            }

            object IEnumerator.Current => Current;

            public T Current
            {
                get
                {
                    try
                    {
                        return _array[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            
            public readonly T this[int index] =>  _array[index];
         }
        
         LoopInForEach list = new();
         foreach (var item in list)
         {
             Console.WriteLine(item); // valid
         }
        
         var refCheck = new int[] { 1, 2, 3 };
         IReadOnlyArray<int> readOnlyArray = (ReadOnlyArray<int>) refCheck; // valid
         ReadOnlyArray<int> roArray = refCheck; // valid   
        
        public class LoopInForEach
        {    
            private  readonly List<string> internalList = new List<string>() { "hello", "world" };
            public List<string>.Enumerator GetEnumerator() => internalList.GetEnumerator();   
        }
            
            
