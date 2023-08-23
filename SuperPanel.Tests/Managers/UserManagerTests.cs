using Moq;
using SuperApp.Core.Exceptions;
using SuperApp.Core.Models;
using SuperPanel.Tests.Data;
using SuperPanel.Tests.Fakes;
using SuperPanel.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperPanel.Tests.Managers
{
    public class UserManagerTests : IClassFixture<UserManagerTestFixture>
    {
        private readonly UserManagerTestFixture _fixture;

        public UserManagerTests(UserManagerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(1, 25)]
        [InlineData(3, 50)]
        [InlineData(2, 10)]
        public async Task GetUsersByPageAsync_WhenPaginationParametersAreValid_MustReturnPageData(int page, int pageSize)
        {
            //Arrange
            _fixture.Reset();
            int totalUsers = 1000;
            int totalPages = (int)Math.Ceiling((decimal)totalUsers / (decimal)pageSize);
            var users = FakeDataProvider.GenerateFakeUserDataModels(pageSize);
            BuildUserManager(HttpStatusCode.OK);
            _fixture.UserRepository
                .Setup(x => x.GetCountAsync())
                .Returns(Task.FromResult(totalUsers));
            _fixture.UserRepository
                .Setup(x => x.GetByPageAsync(It.Is<int>(c => c == page), It.Is<int>(c => c == pageSize)))
                .Returns(Task.FromResult(users));

            //Act

            var result = await _fixture.UserManager.GetUsersByPageAsync(page, pageSize);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(page, result.Page);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(totalUsers, result.TotalCount);
            Assert.Equal(totalPages, result.TotalPages);
        }

        [Theory]
        [InlineData(-1, 25)]
        [InlineData(-3, 50)]
        [InlineData(-2, 10)]
        [InlineData(1, -25)]
        [InlineData(3, -50)]
        [InlineData(2, -10)]
        public async Task GetUsersByPageAsync_WhePaginationParametersAreNegative_MustThrowException(int page, int pageSize)
        {
            //Arrange
            _fixture.Reset();
            int totalUsers = 1000;
            var users = FakeDataProvider.GenerateFakeUserDataModels(10);
            BuildUserManager(HttpStatusCode.OK);
            _fixture.UserRepository
                .Setup(x => x.GetCountAsync())
                .Returns(Task.FromResult(totalUsers));
            _fixture.UserRepository
                .Setup(x => x.GetByPageAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(users));

            //Act

            var result = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _fixture.UserManager.GetUsersByPageAsync(page, pageSize));

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserIdIsValid_MustReturnUser()
        {
            //Arrange
            _fixture.Reset();
            var users = FakeDataProvider.GenerateFakeUserDataModels(10);
            var user = users.First();
            BuildUserManager(HttpStatusCode.OK);
            _fixture.UserRepository
                .Setup(x => x.GetByIdAsync(It.Is<int>(c => c == user.Id)))
                .Returns(Task.FromResult(user));

            //Act
            var result = await _fixture.UserManager.GetUserByIdAsync(user.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1560)]
        [InlineData(-543)]
        public async Task GetUserByIdAsync_WhenUserIdIsNegative_MustThrowException(int userId)
        {
            //Arrange
            _fixture.Reset();
            var users = FakeDataProvider.GenerateFakeUserDataModels(10);
            var user = users.First();
            BuildUserManager(HttpStatusCode.OK);
            _fixture.UserRepository
                .Setup(x => x.GetByIdAsync(It.Is<int>(c => c == user.Id)))
                .Returns(Task.FromResult(user));

            //Act
            var result = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _fixture.UserManager.GetUserByIdAsync(userId));

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GdprDeleteAsync_WhenUserExistsAndExternalServiceReturnsOk_MustExecuteSuccessfully()
        {
            //Arrange
            _fixture.Reset();
            var user = FakeDataProvider.GenerateFakeUserDataModels(1).First();
            BuildUserManager(HttpStatusCode.OK);
            _fixture.UserRepository
                .Setup(x => x.GetByIdAsync(It.Is<int>(c => c == user.Id)))
                .Returns(Task.FromResult(user));
            _fixture.UserRepository
                .Setup(x => x.DeleteAsync(It.Is<int>(c => c == user.Id)))
                .Returns(Task.FromResult(true));

            //Act

            await _fixture.UserManager.GdprDeleteAsync(user.Id);

            //Assert
            _fixture.UserRepository
                .Verify(x => x.GetByIdAsync(It.Is<int>(c => c == user.Id)), Times.Once);
            _fixture.UserRepository
                .Verify(x => x.DeleteAsync(It.Is<int>(c => c == user.Id)), Times.Once);
        }

        [Theory]
        [InlineData(1034)]
        [InlineData(234)]
        [InlineData(145)]
        [InlineData(13344)]
        public async Task GdprDeleteAsync_WhenUserDoesNotExists_MustThrowException(int userId)
        {
            //Arrange
            _fixture.Reset();
            BuildUserManager(HttpStatusCode.OK);
            _fixture.UserRepository
                .Setup(x => x.GetByIdAsync(It.Is<int>(c => c == userId)))
                .Returns(Task.FromResult((UserDataModel)null));
            _fixture.UserRepository
                .Setup(x => x.DeleteAsync(It.Is<int>(c => c == userId)))
                .Returns(Task.FromResult(true));

            //Act

            var result = await Assert.ThrowsAsync<NotFoundException>(() => _fixture.UserManager.GdprDeleteAsync(userId));

            //Assert
            Assert.NotNull(result);
            _fixture.UserRepository
                .Verify(x => x.GetByIdAsync(It.Is<int>(c => c == userId)), Times.Once);
            _fixture.UserRepository
                .Verify(x => x.DeleteAsync(It.Is<int>(c => c == userId)), Times.Never);
        }

        [Fact]
        public async Task GdprDeleteAsync_WhenExternalServiceFails_MustThrowException()
        {
            //Arrange
            _fixture.Reset();
            var user = FakeDataProvider.GenerateFakeUserDataModels(1).First();
            BuildUserManager(HttpStatusCode.InternalServerError);
            _fixture.UserRepository
                .Setup(x => x.GetByIdAsync(It.Is<int>(c => c == user.Id)))
                .Returns(Task.FromResult(user));
            _fixture.UserRepository
                .Setup(x => x.DeleteAsync(It.Is<int>(c => c == user.Id)))
                .Returns(Task.FromResult(true));

            //Act

            var result = await Assert.ThrowsAsync<ExternalApiException>(() => _fixture.UserManager.GdprDeleteAsync(user.Id));

            //Assert
            Assert.NotNull(result);
            _fixture.UserRepository
                .Verify(x => x.GetByIdAsync(It.Is<int>(c => c == user.Id)), Times.Once);
            _fixture.UserRepository
                .Verify(x => x.DeleteAsync(It.Is<int>(c => c == user.Id)), Times.Never);
        }

        private void BuildUserManager(HttpStatusCode statusCode)
        {
            var messageHandler = new FakeHttpMessageHandler(() => Task.FromResult(new HttpResponseMessage(statusCode)));
            var httpClient = new HttpClient(messageHandler);
            _fixture.BuildUserManager(() => httpClient);
        }
    }
}
