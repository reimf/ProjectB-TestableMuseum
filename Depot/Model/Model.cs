namespace Depot;

using System.Text.Json;

public class Model<T> where T : IBarcodable
{
    protected DateTime Now { get => Program.World.Now; }
    private static readonly string FileName = GetFileName();
    protected static readonly List<T> _items = ReadAll();

    public static T WithBarcode(string barcode)
    {
        return _items.Find(item => item.Barcode == barcode);
    }

    public static List<T> ReadAll()
    {
        string json = Program.World.ReadAllText(FileName);
        return JsonSerializer.Deserialize<List<T>>(json);
    }

    public static void WriteAll()
    {
        string json = JsonSerializer.Serialize(_items);
        Program.World.WriteAllText(FileName, json);
    }

    public static string GetFileName()
    {
        string name = typeof(T).Name;
        string today = Program.World.Now.ToString("yyyyMMdd");
        return $"Data/{name}s_{today}.json";
    }
}