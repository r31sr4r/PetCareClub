using Bogus;
using DomainEntity = PetCareClub.Domain.Entity;
using PetCareClub.IntegrationTests.Base;

namespace PetCareClub.IntegrationTests.Application.UseCases.User.Common;
public class UserUseCasesBaseFixture
    : BaseFixture
{
    public string GetValidUserName()
    {
        var userName = "";
        while (userName.Length < 3)
            userName = Faker.Internet.UserName();
        if (userName.Length > 255)
            userName = userName[..255];
        return userName;
    }

    public string GetValidEmail() => Faker.Internet.Email();

    public string GetValidPassword() => "ValidPassword123!";

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public DomainEntity.User GetValidUser()
        => new(
            GetValidUserName(),
            GetValidEmail(),
            GetValidPassword(),
            GetRandomBoolean()
        );

    public DomainEntity.User GetValidUserWithoutPassword()
        => new(
            GetValidUserName(),
            GetValidEmail(),
            string.Empty,
            GetRandomBoolean()
        );

    public List<DomainEntity.User> GetUsersList(int length = 10)
        => Enumerable.Range(1, length)
            .Select(_ => GetValidUser())
            .ToList();
}
