using Cargill.Reconc.Models;

namespace Cargill.Reconc.Business
{
    public interface IReconcCalculator
    {
        public Task<Report[]> GetBatchReports(Trading[] tradings, Counterparty[] counterparties, Insurance[] insurances);
        public Report GetReportByTrading(TradingAggregation t, Insurance? insurance, double pdRate);
    }

    public class ReconcCalculator : IReconcCalculator
    {
        public Task<Report[]> GetBatchReports(Trading[] tradings, Counterparty[] counterparties, Insurance[] insurances)
        {
            return Task.Run(() =>
            {
                var pdRates = counterparties.ToDictionary(c => c.Code, c => c.PDRate);
                var insurancesLookup = insurances.ToDictionary(i => i.CounterpartyName, i => i);
                var counts = tradings.GroupBy(t => t.SupplierName).ToDictionary(g => g.Key, g => g.Count());

                return tradings.Select(t => GetReportByTrading(t, insurancesLookup, pdRates, counts)).ToArray();
            });
        }

        public Report GetReportByTrading(Trading t, Dictionary<string, Insurance> insurances, Dictionary<int, double> pdRates, Dictionary<string, int> counts)
        {
            // assume if the counterparty has insurance, then there must be a insurance limit
            // insurance limit per trading = insurance limit (if the counterparties has) / # of tradings with this counterparties
            var insurance = insurances.ContainsKey(t.SupplierName) ? insurances[t.SupplierName] : null;
            var insuranceLimit = (insurance?.Limit ?? 0.0) / counts[t.SupplierName];
            var pdRate = pdRates[t.SupplierCode];

            return new Report
            {
                TradingId = t.Id,
                HasInsurance = insurance != null,
                PDRate = pdRate,
                InsuranceRate = insurance?.Rate?? 0,
                InsuranceLimit = insuranceLimit,
                ExpectedLoss = t.AmountInJDE * pdRate,
                NetExposure = CalcNetExposure(insurance, t.AmountInJDE, t.AmountInCTRM, insuranceLimit)
            };
        }

        public Report GetReportByTrading(TradingAggregation t, Insurance? insurance, double pdRate)
        {
            // assume if the counterparty has insurance, then there must be a insurance limit
            // insurance limit per trading = insurance limit (if the counterparties has) / # of tradings with this counterparties
            var insuranceLimit = (insurance?.Limit ?? 0.0) / t.TradingCount;

            return new Report
            {
                TradingId = t.Id,
                HasInsurance = insurance != null,
                PDRate = pdRate,
                InsuranceRate = insurance?.Rate,
                InsuranceLimit = insuranceLimit,
                ExpectedLoss = t.AmountInJDE * pdRate,
                NetExposure = CalcNetExposure(insurance, t.AmountInJDE, t.AmountInCTRM, insuranceLimit)
            };
        }

        // /// <summary>
        // /// Calculate Expected Loss according to amount in JDE, supplier code and PD Rates
        // /// 
        // ///     expected loss = PD rate * amount (in JDE)
        // /// 
        // /// </summary>
        // /// <param name="pdRates"></param>
        // /// <param name="supplierCode"></param>
        // /// <param name="amountInJDE"></param>
        // /// <returns></returns>
        public double CalcExpectedLoss(Dictionary<int, double>? pdRates, int supplierCode, double amountInJDE)
        {
            return amountInJDE * (pdRates?.ContainsKey(supplierCode) == true ? pdRates[supplierCode] : 0.0);
        }

        /// <summary>
        /// Calculate Net Exposure according to insurance, amount in JDE and amount in CTRM for a trading.
        /// </summary>
        /// <param name="insurance">Insurance data that contains insurance rate, PD rate and insurance limit.</param>
        /// <param name="amountInJDE"></param>
        /// <param name="amountInCTRM"></param>
        /// <param name="insuranceLimitPerTrading"></param>
        /// <returns></returns>
        public double CalcNetExposure(Insurance? insurance, double amountInJDE, double? amountInCTRM, double insuranceLimitPerTrading)
        {
            // If no insurance, use amount (in JDE)
            // 
            // otherwise,
            // 
            //      if amount in CTRM * insurance rate < insurance limit 
            //          use amount (in JDE) * (1- insurance rate)
            //      else 
            //          use insurance limit

            return insurance == null ? amountInJDE :
                amountInCTRM * insurance.Rate < insuranceLimitPerTrading ? amountInJDE * (1 - insurance.Rate) : insuranceLimitPerTrading;
        }
    }
}