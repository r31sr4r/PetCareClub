using Bogus;
using PetCareClub.Domain.Entity;
using PetCareClub.Domain.Validation;
using PetCareClub.UnitTests.Common;

namespace PetCareClub.UnitTests.Domain.Entity;

[CollectionDefinition(nameof(UserTestFixture))]
public class UserTestFixtureCollection : ICollectionFixture<UserTestFixture> { }

public class UserTestFixture : BaseFixture
{

    public string GenerateValidPassword()
    {
        string password;
        do
        {
            password = Faker.Internet.Password(10, false, "[a-zA-Z0-9!@#$%^&*()]");
        }
        while (!ValidationHelper.IsValidPassword(password));

        return password;
    }

    public User CreateValidUser(string? password = "ValidPassword123!")
    {
        return new User(
            name: Faker.Name.FullName(),
            email: Faker.Internet.Email(),
            password: password,
            isActive: true,
            group: Faker.Commerce.Department(),
            role: "Admin"
        );
    }
}
