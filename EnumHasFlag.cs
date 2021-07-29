using System;


class Example
{


    [Flags]
     enum Color 
    {
        None = 1,
        Blue = 2,
        Green = 4,
        Yellow = 8,
        Purple = 16,
        Red = 32,
        Orange = 64,
        Pink = 128,
        Cyan = 256,
        Magenta = 512
    }


    private static readonly Color[] colors = Enum.GetValues<Color>();    
    
    static void Main()
    {

        var randomColors = Color.Red | Color.Magenta | Color.Cyan | Color.Blue | Color.Orange | Color.Purple;
        var (primaryColors, secondaryColors, otherColors) = Filter(randomColors);

        Console.WriteLine();


    }

    static (Color[] primary, Color[] secondary, Color[] other) Filter(Color color)
    {      
        var primaryList = new Color[3];
        var secondaryList = new Color[3];
        var mixList = new Color[3];
        int i = 0, j = 0, k = 0;
        var primaryColors = Color.Yellow | Color.Red | Color.Blue;
        var secondaryColors = Color.Purple | Color.Orange | Color.Green;
        foreach (var item in colors)
        {
            if (color.HasFlag(item))
            {
                if (primaryColors.HasFlag(item))
                    primaryList[i++] = item;
                else if (secondaryColors.HasFlag(item))
                    secondaryList[j++] = item;
                else
                    mixList[k++] = item;
            }
        }
        return (primaryList, secondaryList, mixList);
    }

}
