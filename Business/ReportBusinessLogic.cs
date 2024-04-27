using Cargill.Reconc.Data;
using Cargill.Reconc.Models;

namespace Cargill.Reconc.Business
{
    public class ReportBusinessLogic
    {
        private readonly ReportsRepo _repo;
        private readonly IReconcCalculator _calculator;

        public ReportBusinessLogic(ReportsRepo repo, IReconcCalculator calculator)
        {
            _repo = repo;
            _calculator = calculator;
        }

        public async Task<Report[]> GetAll(int[] tradingIds)
        {
            return (await _repo.GetByTradingId(tradingIds)).ToArray();

        }

        public Task<Report> GetByTradingId(int tradingId)
        {
            return _repo.GetByTradingId(tradingId);
        }

        public Task<int> AddOrUpdateReports(Report[] reports)
        {
            return _repo.AddOrUpdateReports(reports);
        }

        public Task<int> AddOrUpdateReport(Report report)
        {
            return _repo.AddOrUpdateReport(report);
        }
    }
}