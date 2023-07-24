using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;

class n
{
    public int age;
    public string name;

}

class a
{
    static void Main()
    {
        //FileStream 파스 = new FileStream("a.txt", FileMode.OpenOrCreate);
        //byte[] data = Encoding.Default.GetBytes("아아 님은 갔읍니다");
        //파스.Write(data);
        //파스.Close();


        FileStream 파스 = new FileStream("a.txt", FileMode.OpenOrCreate);
        byte[] a=new byte[1000];
        string 문자열 = "";
        Decoder d = Encoding.Default.GetDecoder();
        while (0!=파스.Read(a, 0, 40))
        {
            Span<char> tt=new Span<char>();
             d.GetChars(a,tt,false);
            문자열 += tt.ToString();
            
            //foreach(char dd in tt)
            //{

            //}
        }
        Console.WriteLine(문자열);
        파스.Close();
        
    }
}
