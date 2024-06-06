namespace Depot;

public class Menu : View
{
    public static string Show()
    {
        WriteLine("1. Login");
        WriteLine("9. Exit");
        return ReadLine();
    }
}