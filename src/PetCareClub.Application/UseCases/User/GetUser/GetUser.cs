using PetCareClub.Application.Exceptions;
using PetCareClub.Application.UseCases.User.Common;
using PetCareClub.Domain.Repository;

namespace PetCareClub.Application.UseCases.User.GetUser
{
    public class GetUser : IGetUser
    {
        private readonly IUserRepository _repository;

        public GetUser(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserModelOutput> Handle(GetUserInput request, CancellationToken cancellationToken)
        {
            var user = await _repository.Get(request.Id, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with id {request.Id} not found");
            }

            return UserModelOutput.FromUser(user);
        }
    }
}
