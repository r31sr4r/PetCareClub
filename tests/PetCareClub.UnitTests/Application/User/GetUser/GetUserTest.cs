using FluentAssertions;
using Moq;
using PetCareClub.Application.Exceptions;
using PetCareClub.Application.UseCases.User.GetUser;
using UseCase = PetCareClub.Application.UseCases.User.GetUser;

namespace PetCareClub.UnitTests.Application.User.GetUser;

[Collection(nameof(GetUserTestFixture))]
public class GetUserTest
{
    private readonly GetUserTestFixture _fixture;

    public GetUserTest(GetUserTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetUser))]
    [Trait("Application", "GetUser - Use Cases")]
    public async Task GetUser()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleUser = _fixture.GetValidUser();

        repositoryMock.Setup(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleUser);

        var input = new GetUserInput(exampleUser.Id);
        var useCase = new UseCase.GetUser(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(exampleUser.Name);
        output.Email.Should().Be(exampleUser.Email);
        output.IsActive.Should().Be(exampleUser.IsActive);
        output.Id.Should().Be(exampleUser.Id);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenUserDoesntExist))]
    [Trait("Application", "GetUser - Use Cases")]
    public async Task NotFoundExceptionWhenUserDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();

        repositoryMock.Setup(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(
            new NotFoundException($"User '{exampleGuid}' not found")
        );

        var input = new GetUserInput(exampleGuid);
        var useCase = new UseCase.GetUser(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
