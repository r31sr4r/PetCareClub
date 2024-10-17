using PetCareClub.Application.Common;
using PetCareClub.Domain.SeedWork.SearchableRepository;

namespace PetCareClub.Application.UseCases.User.ListUsers
{
    public class ListUsersInput : PaginatedListInput
    {
        public ListUsersInput(
            int page = 1,
            int perPage = 15,
            string search = "",
            string sort = "",
            SearchOrder dir = SearchOrder.Asc
        ) : base(page, perPage, search, sort, dir)
        { }
    }
}
