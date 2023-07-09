// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Text;

string input = Console.ReadLine();
//string command = "encode ";
int option = input switch
{
    "help" => Help(),
    ['e','n','c','o','d','e',' ', .. string data] => GenerateCode(data),
};

int GenerateCode(string data)
{
    Console.WriteLine(data);
    var BarCode = new Bitmap(data.Length*8, 100);
    byte[] asciiValues = Encoding.ASCII.GetBytes(data);
    foreach (var value in asciiValues)
    {
        //TODO: translate value to binary

        //TODO: encode it onto bitmap - black = 1 white = 0
    }
    return 1;
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