namespace Cargill.Reconc.Models
{
    public class TradingAggregation
    {
        public int Id { get; set; }
        public int SupplierCode { get; set; }
        public int TradingCount { get; set; }
        public double? AmountInCTRM { get; set; }
        public double AmountInJDE   { get; set; }
    }
}