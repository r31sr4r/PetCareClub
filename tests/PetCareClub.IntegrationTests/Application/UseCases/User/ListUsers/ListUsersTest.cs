using PetCareClub.Infra.Data.EF.Repositories;
using PetCareClub.Application.UseCases.User.ListUsers;
using UseCase = PetCareClub.Application.UseCases.User.ListUsers;
using FluentAssertions;
using PetCareClub.Domain.SeedWork.SearchableRepository;

namespace PetCareClub.IntegrationTests.Application.UseCases.User.ListUser;

[Collection(nameof(ListUsersTestFixture))]
public class ListUsersTest
{
    private readonly ListUsersTestFixture _fixture;

    public ListUsersTest(ListUsersTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "SearchReturnsListAndTotal")]
    [Trait("Integration/Application", "ListUsers - Use Cases")]
    public async Task SearchReturnsListAndTotal()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleUserList = _fixture.GetUsersList(15);
        await dbContext.AddRangeAsync(exampleUserList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var userRepository = new UserRepository(dbContext);
        var searchInput = new ListUsersInput(page: 1, perPage: 10);
        var useCase = new UseCase.ListUsers(userRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleUserList.Count);
        output.Items.Should().HaveCount(10);
        foreach (var outputItem in output.Items)
        {
            var exampleItem = exampleUserList.Find(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem!.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Email.Should().Be(exampleItem.Email);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Fact(DisplayName = "SearchReturnsEmpty")]
    [Trait("Integration/Application", "ListUsers - Use Cases")]
    public async Task SearchReturnsEmpty()
    {
        var dbContext = _fixture.CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var searchInput = new ListUsersInput(page: 1, perPage: 10);
        var useCase = new UseCase.ListUsers(userRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = "SearchReturnsPaginated")]
    [Trait("Integration/Application", "ListUsers - Use Cases")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnsPaginated(
        int itemsToGenerate,
        int page,
        int perPage,
        int expectedTotal
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleUserList = _fixture.GetUsersList(itemsToGenerate);
        await dbContext.AddRangeAsync(exampleUserList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var userRepository = new UserRepository(dbContext);
        var searchInput = new ListUsersInput(page, perPage);
        var useCase = new UseCase.ListUsers(userRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleUserList.Count);
        output.Items.Should().HaveCount(expectedTotal);
        foreach (var outputItem in output.Items)
        {
            var exampleItem = exampleUserList.Find(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem!.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Email.Should().Be(exampleItem.Email);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Theory(DisplayName = "SearchByText")]
    [Trait("Integration/Application", "ListUsers - Use Cases")]
    [InlineData("John", 1, 5, 1, 1)]
    [InlineData("Doe", 1, 5, 2, 2)]
    [InlineData("Example", 1, 5, 3, 3)]
    public async Task SearchByText(
        string search,
        int page,
        int perPage,
        int expectedTotalResult,
        int expectedTotalItems
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleUserList = _fixture.GetExampleUsersListWithNames(
            new List<string>()
            {
                "Example User 1",
                "Example User 2",
                "John Doe",
                "Jane Doe",
                "Example User 3",
            }
        );
        await dbContext.AddRangeAsync(exampleUserList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var userRepository = new UserRepository(dbContext);
        var searchInput = new ListUsersInput(page, perPage, search);
        var useCase = new UseCase.ListUsers(userRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectedTotalResult);
        output.Items.Should().HaveCount(expectedTotalItems);
        foreach (var outputItem in output.Items)
        {
            var exampleItem = exampleUserList.Find(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem!.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Email.Should().Be(exampleItem.Email);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Theory(DisplayName = "SearchOrdered")]
    [Trait("Integration/Application", "ListUsers - Use Cases")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    public async Task SearchOrdered(
        string orderBy,
        string order
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleUserList = _fixture.GetUsersList(10);
        await dbContext.AddRangeAsync(exampleUserList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var userRepository = new UserRepository(dbContext);
        var searchOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchInput = new ListUsersInput(1, 20, "", orderBy, searchOrder);
        var useCase = new UseCase.ListUsers(userRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        var expectOrdered = _fixture.SortList(exampleUserList, orderBy, searchOrder);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleUserList.Count);
        output.Items.Should().HaveCount(exampleUserList.Count);

        for (int i = 0; i < output.Items.Count; i++)
        {
            var outputItem = output.Items[i];
            var exampleItem = expectOrdered[i];
            outputItem.Should().NotBeNull();
            outputItem!.Id.Should().Be(exampleItem.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Email.Should().Be(exampleItem.Email);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }
}
