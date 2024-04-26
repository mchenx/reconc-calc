using Cargill.Reconc.Data;
using Cargill.Reconc.Models;

namespace Cargill.Reconc.Business
{
    public class TradingBusinessLogic
    {
        private readonly TradingsRepo _repo;
        
        public TradingBusinessLogic(TradingsRepo repo)
        {
            _repo = repo;
        }

        public async Task<Trading[]> GetAll()
        {
            return (await _repo.GetTradings()).ToArray();
        }

        public Task<Trading?> GetById(int tradingId)
        {
            return _repo.GetById(tradingId);
        }

        public Task<TradingAggregation?> AggregateBySupplier(int tradingId)
        {
            return _repo.GetAggregationById(tradingId);
        }

        public async Task<bool> AddTrading(Trading trade)
        {
            return (await _repo.Add(trade)) > 0;
        }
    }
}