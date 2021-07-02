static (int min, int max)  FindMinMax(params int[] input)
{
      if (input is null || input.Length == 0)
      {
          throw new ArgumentException("Cannot find minimum and maximum of a null or empty array.");
      }

      var min = int.MaxValue;
      var max = int.MinValue;

      foreach(var i in input)
      {
           if(i < min)
              min = i;
                
          if(i > max)
             max = i;
      }

     return (min, max)
}
