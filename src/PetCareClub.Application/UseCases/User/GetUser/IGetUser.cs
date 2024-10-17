using PetCareClub.Application.UseCases.User.Common;

namespace PetCareClub.Application.UseCases.User.GetUser
{
    public interface IGetUser
    {
        Task<UserModelOutput> Handle(GetUserInput request, CancellationToken cancellationToken);
    }
}
