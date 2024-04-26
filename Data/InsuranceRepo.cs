using Cargill.Reconc.Models;
using Dapper;

namespace Cargill.Reconc.Data
{
    public class InsuranceRepo : BaseRepo
    {
        public InsuranceRepo(IConfiguration config, ILogger<InsuranceRepo> logger) : base(config, logger) { }

        public async Task<IEnumerable<Insurance>> GetAll()
        {
            using (var conn = GetConnection())
            {
                return await conn.QueryAsync<Insurance>("SELECT * FROM dbo.Insurance WITH (NOLOCK)");
            }
        }

        public async Task<Insurance> GetById(int insuranceId)
        {
            using (var conn = GetConnection())
            {
                return await conn.QuerySingleAsync<Insurance>("SELECT * FROM dbo.Insurance WITH (NOLOCK) WHERE Id=@insuranceId", new { insuranceId });
            }
        }
    }
}