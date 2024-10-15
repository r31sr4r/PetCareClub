using PetCareClub.Domain.Entity;
using PetCareClub.Domain.SeedWork;
using PetCareClub.Domain.SeedWork.SearchableRepository;

namespace PetCareClub.Domain.Repository;
public interface IUserRepository
    : IGenericRepository<User>,
    ISearchableRepository<User>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );

    Task<User> GetByEmail(string email, CancellationToken cancellationToken);

}
