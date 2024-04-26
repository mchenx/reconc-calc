using Cargill.Reconc.Data;
using Cargill.Reconc.Models;

namespace Cargill.Reconc.Business
{
    public class InsuranceBusinessLogic
    {
        private readonly InsuranceRepo _repo;

        public InsuranceBusinessLogic(InsuranceRepo repo)
        {
            _repo = repo;
        }

        public async Task<Insurance[]> GetAll()
        {
            return (await _repo.GetAll()).ToArray();

        }

        public Task<Insurance> GetById(int insuranceId)
        {
            return _repo.GetById(insuranceId);

        }
    }
}