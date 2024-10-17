using PetCareClub.Domain.SeedWork.SearchableRepository;
using PetCareClub.IntegrationTests.Application.UseCases.User.Common;
using DomainEntity = PetCareClub.Domain.Entity;

namespace PetCareClub.IntegrationTests.Application.UseCases.User.ListUser;

[CollectionDefinition(nameof(ListUsersTestFixture))]
public class ListUsersTestFixtureCollection
    : ICollectionFixture<ListUsersTestFixture>
{ }

public class ListUsersTestFixture
    : UserUseCasesBaseFixture
{
    public List<DomainEntity.User> GetExampleUsersListWithNames(List<string> names)
    => names.Select(name => new DomainEntity.User(
        name,
        GetValidEmail(),
        GetValidPassword()
    )).ToList();

    public List<DomainEntity.User> SortList(
       List<DomainEntity.User> usersList,
       string orderBy,
       SearchOrder order
   )
    {
        var listClone = new List<DomainEntity.User>(usersList);
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
