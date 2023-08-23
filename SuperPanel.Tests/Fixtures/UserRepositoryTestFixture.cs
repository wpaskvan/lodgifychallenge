using Moq;
using SuperApp.Core.Implementations.Data;
using SuperApp.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPanel.Tests.Fixtures
{
    public class UserRepositoryTestFixture : IDisposable
    {
        public Mock<IDatabase> Database { get; set; }
        public IUserRepository UserRepository { get; set; }
        public UserRepositoryTestFixture() 
        {
            Database = new Mock<IDatabase>();
            UserRepository = new UserRepository(Database.Object);
        }

        public void Dispose()
        {
        }

        public void Reset()
        {
            Database.Reset();
        }
    }
}
