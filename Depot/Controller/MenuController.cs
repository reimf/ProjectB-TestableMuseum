namespace Depot;

public static class MenuController
{
    public static void Start()
    {
        while (true)
        {
            string option = Menu.Show();
            switch (option)
            {
                case "1":
                    VisitorController.Login();
                    break;
                case "9":
                    return;
            }
        }
    }
}