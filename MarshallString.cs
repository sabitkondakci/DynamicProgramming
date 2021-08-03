public static class ExtensionString
{
    public static string CustomReverse(this string script)
    {
        string result = null;
        string strValue = script;
        int strLength = strValue.Length;

        // Allocate HGlobal memory for source and destination strings
        IntPtr sourcePtr = Marshal.StringToHGlobalAnsi(strValue);
        IntPtr destPtr = Marshal.AllocHGlobal(strLength + 1);

        // The unsafe section where byte pointers are used.
        if (sourcePtr != IntPtr.Zero && destPtr != IntPtr.Zero)
        {
            unsafe
            {
                byte *src = (byte *)sourcePtr.ToPointer();
                byte *dst = (byte *)destPtr.ToPointer();

                if (strLength > 0)
                {
                    // set the source pointer to the end of the string
                    // to do a reverse copy.
                    src += strLength - 1;
                    while (strLength-- > 0)
                    {
                        *dst++ = *src--;
                    }
                
                    *dst = 0;
                }
            }
            
            result = Marshal.PtrToStringAnsi(destPtr);
        }
        
       
        Marshal.FreeHGlobal(sourcePtr);
        Marshal.FreeHGlobal(destPtr);

        return result;
    }
    
    public static string LinqReverse(this string script)
    {
        var tmpString = new StringBuilder();
        var reversedStr = script.Reverse();
        foreach (var c in reversedStr)
        {
            tmpString.Append(c);
        }

        return tmpString.ToString();
    }
}



// Unicode 
public static class ExtensionString
{
    public static string CustomReverse(this string script)
    {
       
        string result = null;
        string strValue = script;
        int strLength = strValue.Length;
        
        
        // Allocate HGlobal memory for source and destination strings
        IntPtr sourcePtr = Marshal.StringToHGlobalUni(strValue);
        IntPtr destPtr = Marshal.AllocHGlobal(strLength + 1);

        // The unsafe section where byte pointers are used.
        if (sourcePtr != IntPtr.Zero && destPtr != IntPtr.Zero)
        {
            unsafe
            {
                ushort *src = (ushort *)sourcePtr.ToPointer();
                ushort *dst = (ushort *)destPtr.ToPointer();

                if (strLength > 0)
                {
                    // set the source pointer to the end of the string
                    // to do a reverse copy.
                    src += strLength - 1;
                    while (strLength-- > 0)
                    {
                        *dst++ = *src--;
                    }
                
                    *dst = 0;
                }
            }
            
            result = Marshal.PtrToStringUni(destPtr);
        }
        
        Marshal.FreeHGlobal(sourcePtr);
        Marshal.FreeHGlobal(destPtr);

        return result;
    }
    
    public static string LinqReverse(this string script)
    {
        var tmpString = new StringBuilder();
        var reversedStr = script.Reverse();
        foreach (var c in reversedStr)
        {
            tmpString.Append(c);
        }

        return tmpString.ToString();
    }
}
