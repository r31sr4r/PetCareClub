using PetCareClub.Application.UseCases.User.Common;
using PetCareClub.Domain.Repository;
using PetCareClub.Domain.SeedWork.SearchableRepository;

namespace PetCareClub.Application.UseCases.User.ListUsers
{
    public class ListUsers : IListUsers
    {
        private readonly IUserRepository _userRepository;

        public ListUsers(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ListUsersOutput> Handle(ListUsersInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _userRepository.Search(
                new SearchInput(
                    request.Page,
                    request.PerPage,
                    request.Search,
                    request.Sort,
                    request.Dir
                ),
                cancellationToken
            );

            return new ListUsersOutput(
                searchOutput.CurrentPage,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items.Select(UserModelOutput.FromUser).ToList()
            );
        }
    }
}
