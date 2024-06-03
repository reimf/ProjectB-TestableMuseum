namespace Depot;

public class VisitorWelcome : View
{
    public static void Show(Visitor visitor)
    {
        WriteLine($"Welkom {visitor}");
    }
}