using Bogus;
using SuperApp.Core.Models;
using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SuperPanel.Tests.Data
{
    public static class FakeDataProvider
    {
        public static IEnumerable<UserDataModel> GenerateFakeUserDataModels(int quantity)
        {
            var userIds = 10000;
            var faker = new Faker<UserDataModel>()
                .CustomInstantiator(f => new UserDataModel(userIds++))
                .RuleFor(u => u.Login, (f, u) => f.Internet.UserName())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.CreatedAt, (f, u) => f.Date.Past(3));

            var users = faker.Generate(quantity)
                    .OrderBy(_ => Randomizer.Seed.Next())
                    .ToList();

            return users;
        }

        public static IEnumerable<UserViewModel> GenerateFakeUserViewModels(int quantity)
        {
            var userIds = 10000;
            var faker = new Faker<UserViewModel>()
                .CustomInstantiator(f => new UserViewModel())
                .RuleFor(u => u.Id, (f, u) => userIds++)
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.CreatedAt, (f, u) => f.Date.Past(3));

            var users = faker.Generate(quantity)
                    .OrderBy(_ => Randomizer.Seed.Next())
                    .ToList();

            return users;
        }
    }
}
