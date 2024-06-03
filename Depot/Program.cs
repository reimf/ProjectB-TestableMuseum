namespace Depot;

public static class Program
{
    public static IWorld World = new RealWorld();

    public static void Main()
    {
        MenuController.Start();
        Visitor.WriteAll();
    }
}