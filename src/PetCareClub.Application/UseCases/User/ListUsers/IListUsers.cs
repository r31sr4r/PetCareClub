namespace PetCareClub.Application.UseCases.User.ListUsers
{
    public interface IListUsers
    {
        Task<ListUsersOutput> Handle(ListUsersInput request, CancellationToken cancellationToken);
    }
}
