using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SocialMedia_Dapper.Data
{
    class DataContext
    {
        private readonly IDbConnection _db;
        public DataContext(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T>(string sql, DynamicParameters parameters)
        {
            return await _db.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> LoadDataSingleAsync<T>(string sql, DynamicParameters parameters)
        {
            return await _db.QuerySingleAsync<T>(sql, parameters);
        }

        public async Task<bool> ExecuteSqlAsync(string sql, DynamicParameters parameters)
        {
            return await _db.ExecuteAsync(sql, parameters) > 0;
        }

    }
}
