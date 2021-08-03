using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

class NotTooSafeStringReverse
{
    
    public static void Main()
    {
        var a = BenchmarkRunner.Run<ReverseBench>();
    }

    public static string GetLargeString()
    {
        string insanOlmak = "Bu  konularda  kafa  yoran  ve  bir  türlü  taşı  yerine  oturtamayan  arkadaşlarla  zaman,  zaman  “neden”  diye  tartışıp,  fikir  teatisinde  bulunuruz.. Ne yazık ki,  sonuçta  konu hüsranla kapanır  çoğunlukla..  Çünkü  toplumun  büyük  bir  kesiminde  dostlukları, iyilikleri, güzellikleri,  hizmetleri  unutma  ya  hastalık  olmuş,  ya da  işlerine  geliyor,  kararına  varırız..     Maneviyata  dönmek  isteriz.. Ama  nerde.. Aramak  faydasız.. Manevi  değerler  dünyamıza  gözümüzü  kapattığımız  ölçüde  vefadan, minnet  duygusundan,  kadirbilir  olmaktan  kopup  uzaklaşılmış  olduğunu  görürüz.. Anlarız ki,  insanlık  için  önce  insan  olmak  gerekir..              Aslında  bize  şöyle  öğretmişti  atalarımız.. “Aman  haaa, bir  fincan  kahvenin  kırk  yıl  hatırı  vardır”.. “Komşu  komşunun  külüne  muhtaçtır,  iyi  geçinin  çevrenizdekilerle, sayın, sevin” derlerdi..Hatta  bizlerin  beyinlerine  kazınacağına inandıkları  hikayelerle  nasihatte  bulunurlardı.. Demek  gerçekten  başarılı  olmuşlar,  nasıl  hayatımda  beklentisiz  hizmet  aşkı  coşkusu  hiç  eksilmediyse, her  şeye  rağmen  anlatılan  hikayeler de  unutulmadı  şükürler  olsun.. Hem de  tüm  etkisiyle  beynimde  koruyor yerini.. Mesela  bakın  bir  tanesi  şöyleydi  ve  bu  hikayenin  insan  üstüne  hassasiyetini  şimdi  çok  daha  iyi  anlıyorum.Küçük bir  kız çocuğu  çimenlerin üzerinde  yürürken bir  kelebeğin dikene  takılıp kalmış  olduğunu görür..Hemen  koşar ve  çok dikkatli  bir şekilde  kelebeği dikenden  kurtarır..Kelebek  sevinçle uçmaya  başlar..Ve  az sonra  çok güzel  ve iyi  kalpli bir  peri olarak  geri döner..Küçük  kıza “Güzel kız, bu  iyiliğine karşı  bende sana  en çok  istediğin şeyi  vermek istiyorum” der..Küçük  kız bir  an düşünür  ve cevap  verir “Hayat boyu  mutlu olmak  istiyorum” Onun üzerine  peri ona  doğru eğilir  ve kulağına  bir şeyler  fısıldar..Ardından  birden bire  gözden kaybolur..Kız  büyür..Çevresindeki  hiç kimse  ondan daha  mutlu değildir..Bu  mutluluğun sırrını  ona her  sorduklarında yalnızca  gülümser ve  “iyi bir  perinin sözünü  dinledim”  der..Seneler  geçer ve  bu mutlu  yaşlının sırrının  onunla birlikte  öleceğinden korkan  komşuları “Lütfen bu  sırrı artık  bize de  söyle”  diye yalvarırlar. “Perinin sana  ne ";
        string largeString = string.Empty;      

        for (int i = 0; i < 15; i++)
        {
            largeString += insanOlmak;
        }
        
        return largeString;
    }
}

[MemoryDiagnoser]
public class ReverseBench
{
    string largeString;

    [GlobalSetup]
    public void CreateLargeString()
    {
        largeString = NotTooSafeStringReverse.GetLargeString();
    }

    [Benchmark]
    public void CustomReverseTest()
    {
        var a = largeString.CustomReverse();
    }
    [Benchmark]
    public void LinqReverseTest()
    {
        var b = largeString.LinqReverse();
    }
    [Benchmark]
    public void SpanReverseTest()
    {
        var c = largeString.SpanReverse();
    }
    [Benchmark]
    public void MemoryReverseTest()
    {
        var d = largeString.MemoryReverse();
    }

    [Benchmark]
    public void StringCreateReverseTest()
    {
        var d = largeString.StringCreateReverse();      
    }

}

public static class ExtensionString
{
    public static string CustomReverse(this string script)
    {
        string result = null;
        string strValue = script;
        int strLength = strValue.Length;


        // Allocate HGlobal memory for source and destination strings
        IntPtr sourcePtr = Marshal.StringToHGlobalUni(strValue);
        IntPtr destPtr = Marshal.StringToHGlobalUni(strValue);

        // The unsafe section where byte pointers are used.
        try
        {
            if (sourcePtr != IntPtr.Zero && destPtr != IntPtr.Zero)
            {
                unsafe
                {
                    ushort* src = (ushort*)sourcePtr.ToPointer();
                    ushort* dst = (ushort*)destPtr.ToPointer();

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

            return result;
        }
        finally
        {
            Marshal.FreeHGlobal(sourcePtr);
            Marshal.FreeHGlobal(destPtr);    
        }

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

    public static string SpanReverse(this string script)
    {
        Span<char> spnStr = new Span<char>(script.ToCharArray());
        spnStr.Reverse<char>();
        return spnStr.ToString();
    }

    public static string MemoryReverse(this string script)
    {
        Memory<char> spnStr = new Memory<char>(script.ToCharArray());
        spnStr.Span.Reverse<char>();
        return spnStr.ToString();
    }

    public static string StringCreateReverse(this string script)
    {
        var text = script;

        if (text == null)
            throw new NullReferenceException();

        if (text == string.Empty)
            return text;

        return string.Create(text.Length, text, (output, context) =>
        {
            int length = context.Length - 1;
            for (int i = length; i >= 0; i--)
            {
                output[length - i] = context[i];
            }
        });

    }
}
