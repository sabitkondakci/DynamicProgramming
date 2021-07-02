public static Main(string[] args)
{
      //int32 minimum , int32 maximum
      var (minimum,maximum) = FindMinMax(-1,2,33,4,-10,6);
      System.Console.WriteLine($"Min:{minimum} Max:{maximum}");
}


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
