using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperApp.Core.Interfaces.Data;
using SuperApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperApp.Core.Implementations.Data
{
    public class SqliteDatabase : IDatabase
    {
        private readonly ILogger<SqliteDatabase> _logger;
        private readonly SqliteConnection _connection;

        public SqliteDatabase(IOptions<DatabaseConfiguration> databaseOptions, ILogger<SqliteDatabase> logger)
        {
            _logger = logger;
            _connection = new SqliteConnection($"Data Source={databaseOptions.Value.ConnectionString}");
        }

        #region Public Methods

        public IEnumerable<T> Get<T>(string sqlCommand, DbParameter[] parameters, CommandType commandType) where T : class
        {
            _logger.LogTrace("Method SqliteDatabase.Get Started: Executing {command} on Database.", sqlCommand);
            try
            {
                var queryParams = ConvertToDapperParameter(parameters);
                var result = _connection.Query<T>(sqlCommand, queryParams, commandType: commandType);

                _logger.LogTrace("Method SqliteDatabase.Get finished.");
                return result;
            }catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred trying to access to database.");
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAsync<T>(string sqlCommand, DbParameter[] parameters, CommandType commandType) where T : class
        {
            _logger.LogTrace("Method SqliteDatabase.GetAsync Started: Executing {command} on Database.", sqlCommand);
            try
            {
                var queryParams = ConvertToDapperParameter(parameters);
                var result = await _connection.QueryAsync<T>(sqlCommand, queryParams, commandType: commandType);

                _logger.LogTrace("Method SqliteDatabase.GetAsync finished.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred trying to access to database.");
                throw;
            }
        }

        public async Task<T> SingleAsync<T>(string sqlCommand, DbParameter[] parameters, CommandType commandType) where T : class
        {
            _logger.LogTrace("Method SqliteDatabase.GetAsync Started: Executing {command} on Database.", sqlCommand);
            try
            {
                var queryParams = ConvertToDapperParameter(parameters);
                var result = await _connection.QueryAsync<T>(sqlCommand, queryParams, commandType: commandType);

                _logger.LogTrace("Method SqliteDatabase.GetAsync finished.");
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred trying to access to database.");
                throw;
            }
        }

        public async Task<T> GetScalarAsync<T>(string sqlCommand, DbParameter[] parameters, CommandType commandType)
        {
            _logger.LogTrace("Method SqliteDatabase.GetScalarAsync Started: Executing {command} on Database.", sqlCommand);
            try
            {
                var queryParams = ConvertToDapperParameter(parameters);
                var result = await _connection.ExecuteScalarAsync<T>(sqlCommand, queryParams, commandType: commandType);

                _logger.LogTrace("Method SqliteDatabase.GetScalarAsync finished.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred trying to access to database.");
                throw;
            }
        }

        #endregion

        #region Private Methods

        private DynamicParameters ConvertToDapperParameter(DbParameter[] parameters)
        {
            var dynamicParameters = new DynamicParameters();
            foreach(var p in parameters)
            {
                dynamicParameters.Add(p.ParameterName, p.Value, p.DbType);
            }

            return dynamicParameters;
        }

        #endregion
    }
}
