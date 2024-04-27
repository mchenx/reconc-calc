using Cargill.Reconc.Data;
using Cargill.Reconc.Models;

namespace Cargill.Reconc.Business
{
    public interface ICounterpartyBusinessLogic
    {
        public Task<Counterparty[]> GetAll();
        public Task<double?> GetPDRate(int supplierCode);
    }

    public class CounterpartyBusinessLogic: ICounterpartyBusinessLogic
    {
        private readonly CounterpartiesRepo _repo;

        public CounterpartyBusinessLogic(CounterpartiesRepo repo)
        {
            _repo = repo;
        }

        public async Task<Counterparty[]> GetAll()
        {
            return (await _repo.GetCounterparties()).ToArray();

        }

        public Task<Counterparty> GetById(int code)
        {
            return _repo.GetByCode(code);

        }

        public Task<Dictionary<int, double>> GetPDRates()
        {
            return _repo.GetPDRates();
        }

        public Task<double?> GetPDRate(int supplierCode)
        {
            return _repo.GetPDRate(supplierCode);
        }
    }
}