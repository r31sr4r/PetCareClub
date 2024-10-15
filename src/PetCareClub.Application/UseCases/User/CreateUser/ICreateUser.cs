using PetCareClub.Application.UseCases.User.Common;

namespace PetCareClub.Application.UseCases.User.CreateUser
{
    public interface ICreateUser
    {
        Task<UserModelOutput> Handle(CreateUserInput request, CancellationToken cancellationToken);
    }
}
