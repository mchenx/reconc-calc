namespace Cargill.Reconc.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public string CounterpartyName { get; set; }
        public string MasterId { get; set; }
        public double Limit { get; set; }
        public double PDRate { get; set; }
        public double Rate { get; set; }
        public DateTime BizDate { get; set; }
    }
}