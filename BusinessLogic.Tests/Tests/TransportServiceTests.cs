using BusinessLogic.Contracts;
using BusinessLogic.Services;
using FakeItEasy;
using Moq;
using Serialization.Data;
using System.Text.Json;

namespace BusinessLogic.Tests;

// record ist eine immutable Daten-Klasse
public record OrderTestDataArgs(string Name, int Days);

[TestClass]
public class TransportServiceTests
{
    // Dieses Property wird von MSTest automatisch injiziert
    // Ex muss exakt TestContext heissen und von diesem Typ sein
    public TestContext TestContext { get; set; }

    #region CreateOrder Tests
    private static IEnumerable<object[]> CreateOrderParameters()
    {
        yield return new object[] { new OrderTestDataArgs("Max Mustermann", 10) };
        yield return new object[] { new OrderTestDataArgs("Jane Doe", 14) };
    }

    [TestMethod]
    [DynamicData(nameof(CreateOrderParameters), DynamicDataSourceType.Method)]
    public void CreateOrder_ValidData_ReturnsValidOrder(OrderTestDataArgs args)
    {
        // Arrange
        var expectedDate = DateTime.Now.AddDays(args.Days);
        var mockService = Mock.Of<IVehicleService>();
        // uut = Unit Under Test
        var uut = new TransportService(mockService);

        // Act
        var result = uut.CreateOrder(args.Name, args.Days);

        // Assert
        Assert.IsNotNull(result, "Order should not be null");
        Assert.AreEqual(args.Name, result.CustomerName);
        Assert.AreEqual(expectedDate.Day, result.DueDate?.Day, "Due date should be 7 days from now");
    }

    [TestMethod]
    public void CreateOrder_NullCustomerName_ReturnsNull()
    {
        // Arrange
        var mockService = Mock.Of<IVehicleService>();
        var uut = new TransportService(mockService);

        // Act
        var result = uut.CreateOrder(null);

        // Assert
        Assert.IsNull(result, "Order should be null");
    }
    #endregion

    #region CreateVehicleFleet

    [TestMethod]
    public void CreateVehicleFleet_MoqCreateCar_Returns3Vehicles()
    {
        // Arrange
        var expectedModelName = "TestModel";
        var mockService = new Mock<IVehicleService>();

        mockService.Setup(m => m.CreateCar(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new Car
            {
                Model = expectedModelName,
                Manufacturer = "TestManufacturer"
            });

        var service = new TransportService(mockService.Object);

        // Act
        var result = service.CreateVehicleFleet();

        // Assert
        Assert.IsTrue(result.Any(), "Expected at any vehicles");
        Assert.AreEqual(3, result.Count(), "Expected 3 vehicles");
        Assert.IsTrue(result.All(v => v.Model == expectedModelName), "Expected all vehicles to have the same model");

        ExportTestResults(result);
    }

    [TestMethod]
    public void CreateVehicleFleet_FakeItEasyCreateCar_Returns3Vehicles()
    {
        // Arrange
        var expectedModelName = "TestModel";
        var expectedModelNames = new[] { expectedModelName, expectedModelName, expectedModelName };
        var fakeService = A.Fake<IVehicleService>();

        A.CallTo(() => fakeService.CreateCar(A<string>.Ignored, A<string>.Ignored))
            .Returns(new Car
            {
                Model = expectedModelName,
                Manufacturer = "TestManufacturer"
            });

        var service = new TransportService(fakeService);

        // Act
        var result = service.CreateVehicleFleet();

        // Assert
        Assert.IsTrue(result.Any(), "Expected at any vehicles");
        Assert.AreEqual(3, result.Count(), "Expected 3 vehicles");
        CollectionAssert.AreEqual(expectedModelNames, result.Select(v => v.Model).ToArray());

        ExportTestResults(result);
    }

    #endregion

    #region ExportTestResults
    private static readonly string? _testId = $"{DateTime.Now.Ticks}";

    private string TestResultsFilePath
    {
        get
        {
            var resultsDir = new DirectoryInfo(TestContext.TestRunDirectory!).Parent;
            var path = Path.Combine(resultsDir!.FullName, _testId!, TestContext.FullyQualifiedTestClassName!);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, TestContext.TestName!);
        }
    }

    private void ExportTestResults<T>(T result)
    {
        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        // doesn't work for any unknown reason anymore
        //TestContext.AddResultFile(TestResultFilePath);

        File.WriteAllText(TestResultsFilePath + ".json", json);
    } 
    #endregion
}
