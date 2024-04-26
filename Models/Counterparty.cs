namespace Cargill.Reconc.Models
{
    public class Counterparty
    {
        public int Code { get; set; }
        public string NameInJDE { get; set; }
        public string NameInSalesforce { get; set; }
        public double PDRate { get; set; }
    }
}