namespace Depot;

using System.Text.Json;

[TestClass]
public class SystemTests
{
    [TestMethod]
    public void SystemTestVisitorLoggedIn()
    {
        // Arrange
        FakeWorld fakeworld = new()
        {
            LinesToRead = new() { "1", "1234", "9" },
            Now = new DateTime(2002, 4, 1),
            Files = new()
            {
                ["Data/Visitors_20020401.json"] = "[{\"Barcode\": \"1234\"}]"
            }	
        };
        Program.World = fakeworld;

        // Act
        Program.Main();
        fakeworld.WriteDebugInfoToDebugConsole();

        // Assert
        string expected = "Welkom bezoeker 1234";
        CollectionAssert.Contains(fakeworld.LinesWritten, expected);

        string json = fakeworld.Files["Data/Visitors_20020401.json"];
        List<Visitor> visitors = JsonSerializer.Deserialize<List<Visitor>>(json);
        Assert.AreEqual(1, visitors.Count);
        Visitor visitor = visitors[0];
        Assert.AreEqual(fakeworld.Now, visitor.LastLogin);
    }
}