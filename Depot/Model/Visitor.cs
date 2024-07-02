namespace Depot;

public class Visitor : Model<Visitor>, IIdentifiable
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public DateTime LastLogin { get; set; }

    public void LoggedIn()
    {
        LastLogin = Program.World.Now;
    }

    public static Visitor WithBarcode(string barcode)
    {
        return Find(visitor => visitor.Barcode == barcode);
    }

    public override string ToString()
    {
        return $"bezoeker {Barcode}";
    }
}
