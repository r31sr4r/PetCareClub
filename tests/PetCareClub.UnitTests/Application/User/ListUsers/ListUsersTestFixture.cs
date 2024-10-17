using PetCareClub.Application.UseCases.User.ListUsers;
using PetCareClub.Domain.SeedWork.SearchableRepository;
using DomainEntity = PetCareClub.Domain.Entity;

namespace PetCareClub.UnitTests.Application.User.ListUsers;

[CollectionDefinition(nameof(ListUsersTestFixture))]
public class ListUsersTestFixtureCollection : ICollectionFixture<ListUsersTestFixture> { }

public class ListUsersTestFixture : UserUseCasesBaseFixture
{
    public List<DomainEntity.User> GetUsersList(int length = 10)
    {
        var list = new List<DomainEntity.User>();
        for (int i = 0; i < length; i++)
        {
            list.Add(GetValidUser());
        }
        return list;
    }

    public ListUsersInput GetInput()
    {
        var random = new Random();
        return new ListUsersInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}
