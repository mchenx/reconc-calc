namespace Unit.Tests;

public class ReconcCalculatorTests
{
    [Theory]
    [InlineData(null, 12051.35, 1205113.35, 0, 12051.35)]   // No insurance
    [InlineData(0.9, 142088.19, null, 13303.75, 13303.75)]  // insurance rate = 0.9, insurance limit = 53215 / 4
    [InlineData(0.9, 30596.33, null, 13303.75, 13303.75)]   // insurance rate = 0.9, insurance limit = 53215 / 4
    [InlineData(0.9, 26015.09, null, 13303.75, 13303.75)]   // insurance rate = 0.9, insurance limit = 53215 / 4
    [InlineData(null, 10369.01, 1032969.01, 0, 10369.01)]   // No insurance
    public void Test_CalcNetExposure(double? insuranceRate, double amountInJDE, double? amountInCTRM, double insuranceLimitPerTrading, double expectedNetExposure)
    {
        // Arrange
        Insurance? insurance = insuranceRate.HasValue ? new Insurance { Rate = insuranceRate.Value } : null;

        var calculator = new ReconcCalculator();

        // Act
        double netExposure = calculator.CalcNetExposure(insurance, amountInJDE, amountInCTRM, insuranceLimitPerTrading);

        // Assert
        Assert.Equal(expectedNetExposure, netExposure);
    }

    public static IEnumerable<object[]> ExpectedLosssTestData => new List<object[]>
    {
        new object[] { new Dictionary<int, double>{ [123]=0.257}, 123, 9469.18, 2433.57926 },   // Expected loss with matching supplier code, PD rate 0.257
        new object[] { new Dictionary<int, double>{ [456]=0.36}, 456, 30596.33, 11014.6788 },     // Expected loss with matching supplier code, PD rate 0.36
        new object[] { new Dictionary<int, double>{ [123]=0.257}, 456, 170030.86, 0.0 },        // No expected loss for unmatched supplier code
        new object[] { null, 123, 1000, 0 },                                                    // No expected loss with null pdRates
        new object[] { new Dictionary<int, double>(), 456, 2000, 0}                             // No expected loss with empty pdRates
    };

    [Theory]
    [MemberData(nameof(ExpectedLosssTestData))]
    public void Test_CaclExpectedLoss(Dictionary<int, double> pdRates, int supplierCode, double amountInJDE, double expectedLoss)
    {
        var calculator = new ReconcCalculator();

        // Act
        double actualLoss  = calculator.CalcExpectedLoss(pdRates, supplierCode, amountInJDE);

        // Assert
        Assert.Equal(expectedLoss, actualLoss);
    }
}