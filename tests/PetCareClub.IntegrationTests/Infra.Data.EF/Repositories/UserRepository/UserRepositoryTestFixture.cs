using Bogus.Extensions.Brazil;
using PetCareClub.Domain.Entity;
using PetCareClub.Domain.SeedWork.SearchableRepository;
using PetCareClub.IntegrationTests.Base;

namespace PetCareClub.IntegrationTests.Infra.Data.EF.Repositories.UserRepository;


[CollectionDefinition(nameof(UserRepositoryTestFixture))]
public class UserRepositoryTestFixtureCollection
    : ICollectionFixture<UserRepositoryTestFixture>
{ }

public class UserRepositoryTestFixture
    : BaseFixture
{
    public string GetValidUserName()
        => Faker.Internet.UserName();


    public string GetValidEmail()
        => Faker.Internet.Email();

    public string GetValidPassword()
        => "ValidPassword123!";

    public bool GetRandomBoolean()
    => new Random().NextDouble() < 0.5;

    public string GetValidGroup()
        => Faker.Company.CompanyName();

    public string GetValidRole()
        => Faker.Name.JobTitle();

    public DateTime GetValidCreatedAt()
        => Faker.Date.Past(5); 

    public User GetExampleUser(bool? isActive = null)
        => new(
            GetValidUserName(),
            GetValidEmail(),
            GetValidPassword(),
            isActive ?? GetRandomBoolean(),  
            GetValidGroup(),
            GetValidRole(),
            GetValidCreatedAt()
        );

    public List<User> GetExampleUserList(int lenght = 10)
        => Enumerable.Range(1, lenght)
            .Select(_ => GetExampleUser()).ToList();

    public User GetValidUserWithoutPassword()
        => new(
            GetValidUserName(),
            GetValidEmail(),
            string.Empty
        );

    public List<User> GetExampleUsersListWithNames(List<string> names)
        => names.Select(name => new User(
            name,
            GetValidEmail(),
            GetValidPassword()
        )).ToList();

    public List<User> SortList(
        List<User> usersList,
        string orderBy,
        SearchOrder order
    )
    {
        var listClone = new List<User>(usersList);
        var orderedEnumerable = (orderBy, order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ToList(),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ToList(),
            ("createdAt", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt).ToList(),
            ("createdAt", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt).ToList(),
            _ => listClone.OrderBy(x => x.Name).ToList(),
        };

        return orderedEnumerable.ToList();
    }

}
