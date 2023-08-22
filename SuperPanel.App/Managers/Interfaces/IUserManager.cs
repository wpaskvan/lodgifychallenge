using SuperPanel.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperPanel.App.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<PaginationViewModel<UserViewModel>> GetUsersByPageAsync(int page, int pageSize);
        Task<UserViewModel> GetUserByIdAsync(int userId);
    }
}
