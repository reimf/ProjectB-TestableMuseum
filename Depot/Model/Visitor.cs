namespace Depot;

public class Visitor : Model<Visitor>, IBarcodable
{
    public string Barcode { get; set; }
    public DateTime LastLogin { get; set; }

    public void LoggedIn()
    {
        LastLogin = Now;
    }

    public override string ToString()
    {
        return $"bezoeker {Barcode}";
    }
}
