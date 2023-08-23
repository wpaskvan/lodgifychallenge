using SuperApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperApp.Core.Interfaces.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDataModel>> QueryAllAsync();
        Task<IEnumerable<UserDataModel>> GetByPageAsync(int skip, int pageSize);
        Task<int> GetCountAsync();
        Task<UserDataModel> GetByIdAsync(int userId);
        Task<bool> DeleteAsync(int userId);
    }
}
