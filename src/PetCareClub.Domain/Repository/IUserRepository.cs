using PetCareClub.Domain.Entity;
using PetCareClub.Domain.SeedWork;
using PetCareClub.Domain.SeedWork.SearchableRepository;

namespace PetCareClub.Domain.Repository;
public interface IUserRepository
    : IGenericRepository<User>,
    ISearchableRepository<User>
{
    Task<User> GetByEmail(string email, CancellationToken cancellationToken);

}
