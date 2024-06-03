namespace Depot;

public static class VisitorController
{
    public static void Login()
    {
        string barcode = VisitorLogin.Show();
        Visitor visitor = Visitor.WithBarcode(barcode);
        if (visitor is null)
        {
            VisitorDenied.Show();
        }
        else
        {
            visitor.LoggedIn();
            VisitorWelcome.Show(visitor);
        }
    }
}