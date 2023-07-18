// See https://aka.ms/new-console-template for more information
using System.ComponentModel.Design;
using System.Drawing;
using System.Net.Http.Headers;
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
    byte[] asciiValues;
    try
    {
        asciiValues = Encoding.ASCII.GetBytes(data);
    }
    catch (Exception ex) 
    { 
        Console.WriteLine(ex.Message);
        Console.WriteLine("There is an unsupported character in your prompt");
        return 0;
    }
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
        while (binary.Length < 8)
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
        }
        BarCode.Save(Directory.GetCurrentDirectory()+"\\barcode.bmp");
    }
    Console.WriteLine("Barcode saved.");
    return 1;
}

int DecodeBarcode(string filename)
{
    var bitmap = new Bitmap(Directory.GetCurrentDirectory() + "\\" + filename);
    List<byte> decoded = new List<byte>();
    int current = 0;
    for (int i = 0; i < bitmap.Width; i++)
    {
        if (bitmap.GetPixel(i, 0).ToArgb() == Color.Black.ToArgb())
        {
            switch (i%8)
            {
                case 1: current += 64; break;
                case 2: current += 32; break;
                case 3: current += 16; break;
                case 4: current += 8; break;
                case 5: current += 4; break;
                case 6: current += 2; break;
                case 7: current += 1; break;
            }
        }
        
        if (i%8 == 0 && i != 0 || i == bitmap.Width-1)
        {
            decoded.Add((byte)current);
            current = 0;
        }
    }
    Console.WriteLine($"Decoded text: {Encoding.ASCII.GetString(decoded.ToArray())}");
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