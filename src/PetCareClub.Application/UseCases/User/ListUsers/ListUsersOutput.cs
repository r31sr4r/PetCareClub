using PetCareClub.Application.Common;
using PetCareClub.Application.UseCases.User.Common;

namespace PetCareClub.Application.UseCases.User.ListUsers
{
    public class ListUsersOutput : PaginatedListOutput<UserModelOutput>
    {
        public ListUsersOutput(int page, int perPage, int total, IReadOnlyList<UserModelOutput> items)
            : base(page, perPage, total, items)
        { }
    }
}
