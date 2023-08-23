using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SuperApp.Core.Interfaces.Data;
using SuperApp.Core.Models;
using SuperPanel.App.Managers.Implementations;
using SuperPanel.App.Managers.Interfaces;
using SuperPanel.App.Mapping;
using SuperPanel.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SuperPanel.Tests.Fixtures
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    public class UserManagerTestFixture : IDisposable
    {
        public IOptions<ExternalApiConfiguration> ExternalApiConfiguration { get; set; }
        public Mock<IUserRepository> UserRepository { get; set; }
        public IMapper Mapper { get; set; }
        public IHttpClientFactory HttpClientFactory { get; set; }
        public Mock<ILogger<UserManager>> Logger { get; set; }
        public IUserManager UserManager { get; set; }

        public UserManagerTestFixture() 
        {
            ExternalApiConfiguration = Options.Create(new ExternalApiConfiguration() {Url = "http://faketest.com/" });
            UserRepository = new Mock<IUserRepository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
            });

            Mapper = new Mapper(configuration);
            Logger = new Mock<ILogger<UserManager>>();
        }

        public void Dispose()
        {
        }

        public void BuildUserManager(Func<HttpClient> httpClientBuilder)
        {
            HttpClientFactory = new FakeHttpClientFactory(httpClientBuilder);
            UserManager = new UserManager(ExternalApiConfiguration,
                UserRepository.Object, HttpClientFactory, Mapper, Logger.Object);
        }

        public void Reset()
        {
            UserRepository.Reset();
            Logger.Reset();
        }
    }
}
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
