using Bogus.Extensions.Brazil;
using PetCareClub.Domain.Entity;
using PetCareClub.IntegrationTests.Base;

namespace PetCareClub.IntegrationTests.Infra.Data.EF.UnitOfWork;

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection
    : ICollectionFixture<UnitOfWorkTestFixture>
{ }

public class UnitOfWorkTestFixture
    : BaseFixture
{
    public string GetValidUserName()
    => Faker.Internet.UserName();


    public string GetValidEmail()
        => Faker.Internet.Email();

    public string GetValidPhone()
    {
        var phoneNumber = Faker.Random.Bool()
            ? Faker.Phone.PhoneNumber("(##) ####-####")
            : Faker.Phone.PhoneNumber("(##) #####-####");

        return phoneNumber;
    }

    public string GetValidPassword()
        => "ValidPassword123!";

    public bool GetRandomBoolean()
    => new Random().NextDouble() < 0.5;

    public User GetExampleUser()
        => new(
            GetValidUserName(),
            GetValidEmail(),
            GetValidPassword()
        );

    public List<User> GetExampleUserList(int lenght = 10)
        => Enumerable.Range(1, lenght)
            .Select(_ => GetExampleUser()).ToList();
}
