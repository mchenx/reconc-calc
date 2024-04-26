namespace Cargill.Reconc.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int TradingId { get; set; }
        public double ExpectedLoss { get; set; }
        public double? InsuranceRate { get; set; }
        public double InsuranceLimit { get; set; }
        public double NetExposure { get; set; }
        public bool HasInsurance { get; set; }
        public double PDRate { get; set; }
    }
}