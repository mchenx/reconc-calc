using Cargill.Reconc.Models;
using Dapper;

namespace Cargill.Reconc.Data
{
    public class ReportsRepo : BaseRepo
    {
        public ReportsRepo(IConfiguration config, ILogger<ReportsRepo> logger) : base(config, logger) { }

        public async Task<IEnumerable<Report>> GetAll()
        {
            using (var conn = GetConnection())
            {
                return await conn.QueryAsync<Report>("SELECT * FROM dbo.Reports");
            }
        }

        public async Task<IEnumerable<Report>> GetByTradingId(int[] tradingIds)
        {
            using (var conn = GetConnection())
            {
                return await conn.QueryAsync<Report>("SELECT * FROM dbo.Reports WITH (NOLOCK) WHERE TradingId IN @tradingIds", new { tradingIds });
            }
        }

        public async Task<Report> GetByTradingId(int tradingId)
        {
            using (var conn = GetConnection())
            {
                return await conn.QuerySingleAsync<Report>("SELECT * FROM dbo.Reports WITH (NOLOCK) WHERE TradingId=@tradingId", new { tradingId });
            }
        }

        public async Task<int> AddOrUpdateReport(Report report)
        {
            var sql = @"MEREGE";

            using(var conn = GetConnection())
            {
                return await conn.ExecuteAsync(sql, report);
            }
        }

        public async Task<int> AddOrUpdateReports(Report[] reports)
        {
            var sql = @"MEREGE";

            using(var conn = GetConnection())
            {
                return await conn.ExecuteAsync(sql, reports);
            }
        }
    }
}