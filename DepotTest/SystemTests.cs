namespace Depot;

[TestClass]
public class SystemTests
{
    [TestMethod]
    public void SystemTestVisitorLoggedIn()
    {
        // Arrange
        FakeWorld fakeworld = new()
        {
            LinesToRead = new()
            {
                "1",
                "7890",
                "9"
            },
            LinesWritten = new(),
            Now = new(2002, 4, 1),
            Files = new()
            {
                ["Data/Visitors.json"] = "[{\"Id\":1,\"Barcode\":\"7890\"}]"
            }	
        };
        Program.World = fakeworld;

        // Act
        Program.Main();
        fakeworld.WriteDebugInfoToDebugConsole();
        fakeworld.WriteWarningsToDebugConsole();

        // Assert 1
        string expected = "Welkom bezoeker 7890";
        CollectionAssert.Contains(fakeworld.LinesWritten, expected);

        // Assert 2
        string actualJson = fakeworld.Files["Data/Visitors.json"];
        string expectedJson = "[{\"Id\":1,\"Barcode\":\"7890\",\"LastLogin\":\"2002-04-01T00:00:00\"}]";
        Assert.AreEqual(expectedJson, actualJson);
    }
}