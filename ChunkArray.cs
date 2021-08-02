public  IEnumerable<int[]> Chunk(int[] array, int size)
    {
        while (array.Any())
        {
            yield return array.Take(size).ToArray();
            array = array.Skip(size).ToArray();
        }
        
    }
