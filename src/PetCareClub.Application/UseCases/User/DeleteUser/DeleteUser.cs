using PetCareClub.Application.Exceptions;
using PetCareClub.Application.Interfaces;
using PetCareClub.Domain.Repository;

namespace PetCareClub.Application.UseCases.User.DeleteUser
{
    public class DeleteUser : IDeleteUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUser(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteUserInput request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.Id, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with id {request.Id} not found");
            }

            await _userRepository.Delete(user, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
    }
}
