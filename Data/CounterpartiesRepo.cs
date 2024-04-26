using Cargill.Reconc.Models;
using Dapper;

namespace Cargill.Reconc.Data
{
    public class CounterpartiesRepo : BaseRepo
    {
        public CounterpartiesRepo(IConfiguration config, ILogger<CounterpartiesRepo> logger) : base(config, logger) { }

        public async Task<IEnumerable<Counterparty>> GetCounterparties()
        {
            using (var conn = GetConnection())
            {
                return await conn.QueryAsync<Counterparty>("SELECT * FROM dbo.Counterparties WITH (NOLOCK)");
            }
        }

        public async Task<Counterparty> GetByCode(int code)
        {
            using (var conn = GetConnection())
            {
                return await conn.QuerySingleAsync<Counterparty>("SELECT * FROM dbo.Counterparties WITH (NOLOCK) WHERE Code=@code", new { code });
            }
        }

        public async Task<Dictionary<int, double>> GetPDRates()
        {
            using (var conn = GetConnection())
            {
                return (await conn.QueryAsync<Counterparty>("SELECT Code, PDRate FROM dbo.Counterparties WITH (NOLOCK)"))
                .ToDictionary(c => c.Code, c => c.PDRate);
            }
        }

        public async Task<double?> GetPDRate(int supplierCode)
        {
            var sql = @"
                SELECT  PDRate
                FROM    dbo.Counterparties WITH (NOLOCK)
                WHERE   Code=@supplierCode;";

            using (var conn = GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<double?>(sql, new { supplierCode });
            }
        }
    }
}