using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace SuperApp.Core.Interfaces.Data
{
    public interface IDatabase
    {
        public IEnumerable<T> Get<T>(string sqlCommand, DbParameter[] parameters, CommandType commandType) where T : class;
        Task<IEnumerable<T>> GetAsync<T>(string sqlCommand, DbParameter[] parameters, CommandType commandType) where T : class;
        Task<T> GetScalarAsync<T>(string sqlCommand, DbParameter[] parameters, CommandType commandType);
        Task<T> SingleAsync<T>(string sqlCommand, DbParameter[] parameters, CommandType commandType) where T : class;
        Task<int> ExecuteAsync(string sqlCommand, DbParameter[] parameters, CommandType commandType);
    }
}
