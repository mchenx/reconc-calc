namespace Cargill.Reconc.Models
{
    public class Trading
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string ContractNo { get; set; }
        public DateTime DueDate { get; set; }
        public double? AmountInCTRM { get; set; }
        public double AmountInJDE   { get; set; }

        // same as Name in JDE
        public string SfAccountTitle { get; set; }
    }
}