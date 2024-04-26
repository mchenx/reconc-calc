using Cargill.Reconc.Models;
using Dapper;

namespace Cargill.Reconc.Data
{
    public class TradingsRepo: BaseRepo
    {

        public TradingsRepo(IConfiguration config, ILogger<TradingsRepo> logger) :base(config, logger){}
        
        public async Task<IEnumerable<Trading>> GetTradings()
        {
            var sql = @"
                SELECT  t.*,
                        c.NameInJDE AS SfAccountTitle
                FROM    dbo.Tradings t WITH (NOLOCK)
                LEFT JOIN dbo.Counterparties c WITH (NOLOCK) ON t.SupplierCode = c.Code";
            using (var conn = GetConnection())
            {
                return await conn.QueryAsync<Trading>(sql);
            }
        }

        public async Task<Trading?> GetById(int tradingId)
        {
            var sql = @"
                SELECT  *
                FROM    dbo.Tradings WITH (NOLOCK)
                WHERE Id=@tradingId";

            using(var conn = GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<Trading>(sql, new { tradingId });
            }
        }

        public async Task<TradingAggregation?> GetAggregationById(int tradingId)
        {
            var sql = @"
                SELECT		t1.Id,
                            t1.SupplierCode,
                            t1.AmountInCTRM,
                            t1.AmountInJDE,
                            COUNT (*) [TradingCount]
                FROM		dbo.Tradings t1 WITH (NOLOCK)
                INNER JOIN	dbo.Tradings t2 WITH (NOLOCK) ON t1.SupplierCode = t2.SupplierCode
                WHERE		t1.Id = @tradingId
                GROUP BY	t1.Id,
                            t1.SupplierCode,
                            t1.AmountInCTRM,
                            t1.AmountInJDE;";

            using(var conn = GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<TradingAggregation?>(sql, new { tradingId });
            }
        }

        public async Task<int> Add(Trading trade)
        {
            var sql = @"
                INSERT INTO dbo.Tradings(Code,Description,SupplierCode,SupplierName,ContractNo,DueDate,AmountInCTRM,AmountInJDE)
                VALUES(@Code,@Description,@SupplierCode,@SupplierName,@ContractNo,@DueDate,@AmountInCTRM,@AmountInJDE)";

            using(var conn = GetConnection())
            {
                var rowsAffected = await conn.ExecuteAsync(sql, trade);

                _logger.LogDebug($"{rowsAffected} rows added to Tradings table");

                return rowsAffected;
            }
        }

    }
}