using PetCareClub.Application.UseCases.User.Common;
using PetCareClub.Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Moq;
using DomainEntity = PetCareClub.Domain.Entity;
using UseCase = PetCareClub.Application.UseCases.User.ListUsers;

namespace PetCareClub.UnitTests.Application.User.ListUsers;

[Collection(nameof(ListUsersTestFixture))]
public class ListUsersTest
{
    private readonly ListUsersTestFixture _fixture;

    public ListUsersTest(ListUsersTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ShouldReturnUsers))]
    [Trait("Application", "ListUsers - Use Cases")]
    public async Task ShouldReturnUsers()
    {
        var usersList = _fixture.GetUsersList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetInput();
        var outputRepositorySearch = new SearchOutput<DomainEntity.User>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.User>)usersList,
            total: new Random().Next(50, 200)
        );

        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListUsers(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        foreach (var outputItem in output.Items)
        {
            var repositoryUser = outputRepositorySearch.Items
                .FirstOrDefault(u => u.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryUser!.Name);
            outputItem.Email.Should().Be(repositoryUser!.Email);
            outputItem.IsActive.Should().Be(repositoryUser!.IsActive);
            outputItem.Id.Should().Be(repositoryUser!.Id);
        }

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
    [Trait("Application", "ListUsers - Use Cases")]
    [MemberData(nameof(ListUsersTestDataGenerator.GetInputWithoutAllParameters),
        parameters: 18,
        MemberType = typeof(ListUsersTestDataGenerator)
    )]
    public async Task ListInputWithoutAllParameters(UseCase.ListUsersInput input)
    {
        var usersList = _fixture.GetUsersList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var outputRepositorySearch = new SearchOutput<DomainEntity.User>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.User>)usersList,
            total: new Random().Next(50, 200)
        );

        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListUsers(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        foreach (var outputItem in output.Items)
        {
            var repositoryUser = outputRepositorySearch.Items
                .FirstOrDefault(u => u.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryUser!.Name);
            outputItem.Email.Should().Be(repositoryUser!.Email);
            outputItem.IsActive.Should().Be(repositoryUser!.IsActive);
            outputItem.Id.Should().Be(repositoryUser!.Id);
        }


        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(ListOkWhenEmpty))]
    [Trait("Application", "ListUsers - Use Cases")]
    public async Task ListOkWhenEmpty()
    {
        var input = _fixture.GetInput();
        var repositoryMock = _fixture.GetRepositoryMock();
        var outputRepositorySearch = new SearchOutput<DomainEntity.User>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<DomainEntity.User>().AsReadOnly(),
            total: 0
        );

        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListUsers(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
