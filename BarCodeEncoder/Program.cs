// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Text;


while(true)
{
    string input = Console.ReadLine();
    //string command = "encode ";
    int option = input switch
    {
        "help" => Help(),
        ['e', 'n', 'c', 'o', 'd', 'e', ' ', .. string data] => GenerateCode(data),
        ['d', 'e', 'c', 'o', 'd', 'e', ' ', .. string data] => DecodeBarcode(data),
        "exit" => Exit(),
        _ => Unknown()
    };
}


int GenerateCode(string data)
{
    const int BarCodeHeight = 100;
    var BarCode = new Bitmap(data.Length*8, BarCodeHeight);
    byte[] asciiValues = Encoding.ASCII.GetBytes(data);
    foreach (var v in asciiValues) Console.WriteLine(v);
    for (int i = 0; i < asciiValues.Length; i++)
    {
        var value = asciiValues[i];
        string binary = new("");
        while (true)
        {
            if (value % 2 == 1) binary = binary.Insert(0, "1");
            else binary = binary.Insert(0, "0");
            value /= 2;
            if (value == 1)
            {
                binary = binary.Insert(0, "1");
                break;
            }
            else if (value == 0)
            {
                break;
            }
        }
        while (binary.Length <= 8)
        {
            binary = binary.Insert(0, "0");
        }
        // encode the binary
        for (int a=8*i; a < 8*i+8; a++)
        {
            for (int b=0; b< BarCodeHeight; b++)
            {
                BarCode.SetPixel(a, b,
                    binary[a%8] == '1' ? Color.Black : Color.White
                    );
            }
            Console.WriteLine($"Encoding stripe: {a}");
        }
        BarCode.Save(Directory.GetCurrentDirectory()+"\\barcode.bmp");
    }
    return 1;
}

int DecodeBarcode(string filename)
{
    var bitmap = new Bitmap(Directory.GetCurrentDirectory() + "\\" + filename);
    List<byte> decoded = new List<byte>();
    for (int i=0; i<bitmap.Width/8; i++) 
    {
        bool[] stripes = new bool[8];
        for (int a = 0+i*8; a < 8+i*8; a++)
        {
            if (bitmap.GetPixel(a, 0).ToArgb() == Color.Black.ToArgb()) stripes[a % 8] = true;
            else stripes[a % 8] = false;
        }
        int asciiValue = 0;
        int addent = 128;
        for (int s=0; s < stripes.Length; s++)
        {
            if (stripes[s]) asciiValue += addent;
            addent /= 2;
        }
        decoded.Add((byte)asciiValue);
    }
    foreach (byte b in decoded) Console.WriteLine(b);

    Console.WriteLine($"Decoded string: {Encoding.ASCII.GetString(decoded.ToArray())}");
    return 2;
}


int Help()
{
    Console.WriteLine("Help list placeholder");
    return 0;
}
int Unknown()
{
    Console.WriteLine("Unknown command. Type 'help' to display command list");
    return 0;
}

int Exit()
{
    Environment.Exit(0);
    return 0;
}