using PetCareClub.Application.Exceptions;
using PetCareClub.Domain.Entity;
using PetCareClub.Domain.Repository;
using PetCareClub.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace PetCareClub.Infra.Data.EF.Repositories;
public class UserRepository
    : IUserRepository
{
    private readonly PetCareClubDbContext _context;
    private DbSet<User> _users => _context.Set<User>();

    public UserRepository(PetCareClubDbContext context)
    {
        _context = context;
    }

    public async Task Insert(User aggregate, CancellationToken cancellationToken)
        => await _users.AddAsync(aggregate, cancellationToken);

    public async Task<User> Get(Guid id, CancellationToken cancellationToken)
    {
        var user = await _users.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == id, 
            cancellationToken
        );                
        NotFoundException.ThrowIfNull(user, $"User with id {id} not found");
        return user!;
    }

    public Task Update(User aggregate, CancellationToken _)
        => Task.FromResult(_users.Update(aggregate));

    public Task Delete(User aggregate, CancellationToken _)
        => Task.FromResult(_users.Remove(aggregate));

    public async Task<SearchOutput<User>> Search(
        SearchInput input, 
        CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _users.AsNoTracking();
        query = AddSorting(query, input.OrderBy, input.Order);
        if (!string.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Name.Contains(input.Search));

        var total = await query.CountAsync();
        var items = await query.AsNoTracking()
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync();

        return new SearchOutput<User>(
            currentPage: input.Page,
            perPage: input.PerPage,
            total: total,
            items: items
        );
    }

    private IQueryable<User> AddSorting(
        IQueryable<User> query, 
        string orderProperty,
        SearchOrder order
    )
    {
        var orderedEnumerable = (orderProperty, order) switch
        {
            ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
            ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
            ("createdAt", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdAt", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.Name)
        };

        return orderedEnumerable
            .ThenBy(x => x.CreatedAt);

    }


    public async Task<User> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var user = await _users.AsNoTracking().FirstOrDefaultAsync(
            x => x.Email == email,
            cancellationToken
        );
        NotFoundException.ThrowIfNull(user, $"User with email {email} not found");
        return user!;
    }
}
