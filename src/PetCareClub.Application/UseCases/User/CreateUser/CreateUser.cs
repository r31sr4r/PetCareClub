using PetCareClub.Application.Interfaces;
using PetCareClub.Application.UseCases.User.Common;
using PetCareClub.Domain.Repository;
using DomainEntity = PetCareClub.Domain.Entity;

namespace PetCareClub.Application.UseCases.User.CreateUser
{
    public class CreateUser : ICreateUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUser(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserModelOutput> Handle(CreateUserInput request, CancellationToken cancellationToken)
        {
            var user = new DomainEntity.User(
                request.Name,
                request.Email,
                request.Password
            );

            await _userRepository.Insert(user, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return UserModelOutput.FromUser(user);
        }
    }
}
