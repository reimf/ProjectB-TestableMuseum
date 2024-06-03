namespace Depot;

[TestClass]
public class UnitTests
{
    [TestMethod]
    public void ModelTestVisitorToString()
    {
        // Arrange
        Visitor visitor = new() { Barcode = "1234" };

        // Act
        string actual = visitor.ToString();

        // Assert
        string expected = "bezoeker 1234";
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ViewTestVisitorDenied()
    {
        // Arrange
        FakeWorld fakeworld = new();  
        Program.World = fakeworld;

        // Act
        VisitorDenied.Show();

        // Assert
        string actual = fakeworld.LinesWritten.Last();
        string expected = "Dit is een ongeldig entreebewijs";
        Assert.AreEqual(expected, actual);
    }
}