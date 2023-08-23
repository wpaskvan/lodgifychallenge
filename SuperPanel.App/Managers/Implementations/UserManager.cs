using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperApp.Core.Constants;
using SuperApp.Core.Exceptions;
using SuperApp.Core.Extensions;
using SuperApp.Core.Interfaces.Data;
using SuperApp.Core.Models;
using SuperPanel.App.Managers.Interfaces;
using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperPanel.App.Managers.Implementations
{
    public class UserManager : IUserManager
    {
        private readonly IOptions<ExternalApiConfiguration> _externalApiConfiguration;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UserManager> _logger;

        public UserManager(IOptions<ExternalApiConfiguration> externalApiConfiguration,
            IUserRepository userRepository,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ILogger<UserManager> logger)
        {
            _externalApiConfiguration = externalApiConfiguration;
            _httpClientFactory = httpClientFactory;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginationViewModel<UserViewModel>> GetUsersByPageAsync(int page, int pageSize)
        {
            _logger.LogTrace("Method UserManager:GetUsersByPageAsync started");
            page.MustBePossitive("Page");
            pageSize.MustBePossitive("PageSize");

            var count = await _userRepository.GetCountAsync();
            var totalPages = (int)Math.Ceiling((decimal)count / (decimal)pageSize);

            int skip = pageSize * (page <= totalPages ? page - 1 : 1);
            var users = await _userRepository.GetByPageAsync(skip, pageSize);

            var viewModels = _mapper.Map<IEnumerable<UserViewModel>>(users);

            _logger.LogTrace("Method UserManager:GetUsersByPageAsync finished");
            return new PaginationViewModel<UserViewModel>()
            {
                Items = viewModels,
                Page = page,
                TotalCount = count,
                TotalPages = totalPages,
                PageSize = pageSize
            };
        }

        public async Task<UserViewModel> GetUserByIdAsync(int userId)
        {
            _logger.LogTrace("Method UserManager:GetUserByIdAsync started");
            userId.MustBePossitive("UserId");

            var user = await _userRepository.GetByIdAsync(userId);

            _logger.LogTrace("Method UserManager:GetUserByIdAsync finished");
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task GdprDeleteAsync(int userId)
        {
            _logger.LogTrace("Method UserManager:GdprDeleteAsync started");
            userId.MustBePossitive("UserId");

            var user = await _userRepository.GetByIdAsync(userId);
            if(user == null)
            {
                _logger.LogTrace("Method UserManager:GdprDeleteAsync: User Not Found");
                throw new NotFoundException(userId, "User does not exists");
            }
            var url = BuildExternalApiUrl(string.Format(ExternalApiConstants.GDPR_ENDPOINT, userId));

            _logger.LogTrace("Method UserManager:GdprDeleteAsync: Sending request to API");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
            using (var client = _httpClientFactory.CreateClient())
            {
                try
                {
                    var result = await client.SendAsync(request);
                    _logger.LogTrace($"Method UserManager:GdprDeleteAsync: API Result: {result.StatusCode}");
                    if (result.IsSuccessStatusCode)
                    {
                        _ = await _userRepository.DeleteAsync(userId);
                    }
                    else
                    {
                        throw new ExternalApiException((int)result.StatusCode, result.ReasonPhrase, result.ReasonPhrase);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "An error occurred processing the request");
                    throw new ExternalApiException(500, "Service Unavailable. Try again later", ex.Message);
                }
                finally
                {
                    _logger.LogTrace("Method UserManager:GdprDeleteAsync finished");
                }
            }
        }

        private string BuildExternalApiUrl(string endpoint)
        {
            var result = $"{_externalApiConfiguration.Value.Url.TrimEnd('/')}{endpoint}";

            return result;
        }
    }
}
