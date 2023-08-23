using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SuperPanel.App.Models;
using SuperPanel.Tests.Data;
using SuperPanel.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperPanel.Tests.Controllers
{
    public class UsersControllerTests : IClassFixture<UserControllerTestFixture>
    {
        private readonly UserControllerTestFixture _fixture;

        public UsersControllerTests(UserControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(1, 25)]
        [InlineData(5, 10)]
        [InlineData(3, 50)]
        public async Task Index_WhenValidPaginationParameters_MustReturnCorrectPageData(int page,  int pageSize)
        {
            //Arrange
            var users = FakeDataProvider.GenerateFakeUserViewModels(pageSize);
            _fixture.UserManager
                .Setup(x => x.GetUsersByPageAsync(It.Is<int>(c => c == page), It.Is<int>(c => c == pageSize)))
                .Returns(Task.FromResult(new PaginationViewModel<UserViewModel>()
                {
                    Items = users,
                    Page = page,
                    PageSize = pageSize
                }));


            //Act

            var result = (await _fixture.Controller.Index(page, pageSize)) as ViewResult;
            var model = result.Model as PaginationViewModel<UserViewModel>;

            //Assert

            Assert.NotNull(result);

            Assert.Equal(page, model.Page);
            Assert.Equal(pageSize, model.PageSize);
            Assert.Equal(pageSize, model.Items.Count());
        }

        [Theory]
        [InlineData(1, 25)]
        [InlineData(5, 10)]
        [InlineData(3, 50)]
        public async Task Index_WhenException_MustReturn500(int page, int pageSize)
        {
            //Arrange
            _fixture.UserManager
                .Setup(x => x.GetUsersByPageAsync(It.Is<int>(c => c == page), It.Is<int>(c => c == pageSize)))
                .ThrowsAsync(new Exception());

            //Act

            var result = (await _fixture.Controller.Index(page, pageSize)) as StatusCodeResult;

            //Assert

            Assert.NotNull(result);

            Assert.Equal(500, result.StatusCode);
        }
    }
}
