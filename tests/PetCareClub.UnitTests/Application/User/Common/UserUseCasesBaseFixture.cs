using Moq;
using PetCareClub.Application.Interfaces;
using PetCareClub.Domain.Entity;
using PetCareClub.Domain.Repository;
using PetCareClub.UnitTests.Common;

public class UserUseCasesBaseFixture : BaseFixture
{
    public Mock<IUserRepository> GetRepositoryMock() => new();
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public string GetValidUserName()
    {
        var userName = "";
        while (userName.Length < 3)
            userName = Faker.Person.FullName;
        if (userName.Length > 255)
            userName = userName[..255];
        return userName;
    }

    public string GetValidEmail() => Faker.Internet.Email();
    public string GetValidPassword() => "ValidPassword123!";
    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public User GetValidUser() => new(
        GetValidUserName(),
        GetValidEmail(),
        GetValidPassword(),
        GetRandomBoolean()
    );

    public User GetValidUserWithoutPassword() => new(
        GetValidUserName(),
        GetValidEmail(),
        string.Empty,
        GetRandomBoolean()
    );
}
