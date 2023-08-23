using Moq;
using SuperApp.Core.Models;
using SuperPanel.Tests.Data;
using SuperPanel.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperPanel.Tests.Repositories
{
    public class UserRepositoryTests : IClassFixture<UserRepositoryTestFixture>
    {
        private readonly UserRepositoryTestFixture _fixture;

        public UserRepositoryTests(UserRepositoryTestFixture fixture) 
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task QueryAllAsync_WhenDatabaseOk_MustReturnData()
        {
            //Arrange
            _fixture.Reset();
            var users = FakeDataProvider.GenerateFakeUserDataModels(10);
            _fixture.Database
                .Setup(x => x.GetAsync<UserDataModel>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()))
                .Returns(Task.FromResult(users));

            //Act
            var result = await _fixture.UserRepository.QueryAllAsync();

            //Assert
            Assert.Equal(users.Count(), result.Count());
            _fixture.Database.Verify(x => x.GetAsync<UserDataModel>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()), Times.Once);
        }

        [Fact]
        public async Task GetCountAsync_WhenDatabaseOk_MustReturnUserCount()
        {
            //Arrange
            _fixture.Reset();
            _fixture.Database
                .Setup(x => x.GetScalarAsync<int>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()))
                .Returns(Task.FromResult(10));

            //Act
            var result = await _fixture.UserRepository.GetCountAsync();

            //Assert
            Assert.Equal(10, result);
            _fixture.Database.Verify(x => x.GetScalarAsync<int>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()), Times.Once);
        }

        [Theory]
        [InlineData(1, 25)]
        [InlineData(2, 10)]
        [InlineData(1, 50)]
        public async Task GetByPageAsync_WhenParameterAreValid_MustReturnData(int page, int pageSize)
        {
            //Arrange
            _fixture.Reset();
            var users = FakeDataProvider.GenerateFakeUserDataModels(pageSize);
            _fixture.Database
                .Setup(x => x.GetAsync<UserDataModel>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()))
                .Returns(Task.FromResult(users));

            //Act
            var result = await _fixture.UserRepository.GetByPageAsync(page, pageSize);

            //Assert
            Assert.Equal(users.Count(), result.Count());
            _fixture.Database.Verify(x => x.GetAsync<UserDataModel>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()), Times.Once);
        }

        [Theory]
        [InlineData(10001)]
        [InlineData(10002)]
        [InlineData(10003)]
        public async Task GetByIdAsync_WhenUserExists_MustReturnUser(int userId)
        {
            //Arrange
            _fixture.Reset();
            var users = FakeDataProvider.GenerateFakeUserDataModels(10);
            var user = users.First(x => x.Id == userId);
            _fixture.Database
                .Setup(x => x.SingleAsync<UserDataModel>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()))
                .Returns(Task.FromResult(user));

            //Act
            var result = await _fixture.UserRepository.GetByIdAsync(userId);

            //Assert
            Assert.Equal(user.Id, result.Id);
            _fixture.Database.Verify(x => x.SingleAsync<UserDataModel>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()), Times.Once);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(102)]
        [InlineData(103)]
        public async Task GetByIdAsync_WhenUserDoesNotExists_MustReturnNull(int userId)
        {
            //Arrange
            _fixture.Reset();
            var users = FakeDataProvider.GenerateFakeUserDataModels(10);
            var user = users.FirstOrDefault(x => x.Id == userId);
            _fixture.Database
                .Setup(x => x.SingleAsync<UserDataModel>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()))
                .Returns(Task.FromResult(user));

            //Act
            var result = await _fixture.UserRepository.GetByIdAsync(userId);

            //Assert
            Assert.Null(result);
            _fixture.Database.Verify(x => x.SingleAsync<UserDataModel>(It.IsAny<string>(), It.IsAny<DbParameter[]>(), It.IsAny<CommandType>()), Times.Once);
        }
    }
}
