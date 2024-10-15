using PetCareClub.Application.Interfaces;

namespace PetCareClub.Infra.Data.EF;
public class UnitOfWork
    : IUnitOfWork
{
    private readonly PetCareClubDbContext _context;

    public UnitOfWork(PetCareClubDbContext context)
    {
        _context = context;
    }

    public Task Commit(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}