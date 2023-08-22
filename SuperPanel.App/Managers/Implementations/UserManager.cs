using AutoMapper;
using SuperApp.Core.Interfaces.Data;
using SuperPanel.App.Managers.Interfaces;
using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperPanel.App.Managers.Implementations
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserManager(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaginationViewModel<UserViewModel>> GetUsersByPageAsync(int page, int pageSize)
        {
            int skip = pageSize * (page - 1);
            var count = await _userRepository.GetCountAsync();
            var totalPages = (int)Math.Ceiling((decimal)count / (decimal)pageSize);
            var users = await _userRepository.GetByPageAsync(skip, pageSize);

            var viewModels = _mapper.Map<IEnumerable<UserViewModel>>(users);

            return new PaginationViewModel<UserViewModel>()
            {
                Items = viewModels,
                Page = page,
                TotalCount = count,
                TotalPages = totalPages,
                PageSize = pageSize
            };
        }
    }
}
