namespace PetCareClub.Application.UseCases.User.DeleteUser
{
    public interface IDeleteUser
    {
        Task Handle(DeleteUserInput request, CancellationToken cancellationToken);
    }
}
