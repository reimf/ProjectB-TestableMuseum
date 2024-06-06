namespace Depot;

using System.Text.Json;

public class Model<T> where T : Model<T>, IIdentifiable
{
    private static string FileName
    {
        get => $"Data/{typeof(T).Name}s.json";
    }

    private static List<T> ReadAll()
    {
        string json = Program.World.ReadAllText(FileName);
        return JsonSerializer.Deserialize<List<T>>(json);
    }

    private static void WriteAll(List<T> items)
    {
        string json = JsonSerializer.Serialize(items);
        Program.World.WriteAllText(FileName, json);
    }

    public static T Load(Func<T, bool> predicate)
    {
        return ReadAll().Single(item => predicate(item));
    }

    public void Save()
    {
        List<T> items = ReadAll();
        T itemToSave = this as T;
        int index = items.FindIndex(item => item.Id == itemToSave.Id);
        if (index >= 0)
            items[index] = itemToSave;
        else
            items.Add(itemToSave);
        WriteAll(items);
    }
}