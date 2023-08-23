using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using SuperApp.Core.Interfaces.Data;
using SuperPanel.App.Controllers;
using SuperPanel.App.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPanel.Tests.Fixtures
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    public class UserControllerTestFixture : IDisposable
    {
        public UsersController Controller { get; set; }
        public Mock<IUserManager> UserManager { get; set; }
        public Mock<ILogger<UsersController>> Logger { get; set; }

        public UserControllerTestFixture()
        {
            UserManager = new Mock<IUserManager>();
            Logger = new Mock<ILogger<UsersController>>();
            Controller = new UsersController(Logger.Object, UserManager.Object);
        }

        public void Dispose()
        {
        }
    }
}
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
