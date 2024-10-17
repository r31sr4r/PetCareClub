using PetCareClub.Application.Exceptions;
using PetCareClub.Infra.Data.EF.Repositories;
using FluentAssertions;
using UseCase = PetCareClub.Application.UseCases.User.GetUser;

namespace PetCareClub.IntegrationTests.Application.UseCases.User.GetUser;

[Collection(nameof(GetUserTestFixture))]
public class GetUserTest
{
    private readonly GetUserTestFixture _fixture;

    public GetUserTest(GetUserTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetUser))]
    [Trait("Integration/Application", "GetUser - Use Cases")]
    public async Task GetUser()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleUser = _fixture.GetValidUser();
        await dbContext.AddAsync(exampleUser);
        await dbContext.SaveChangesAsync();
        var userRepository = new UserRepository(dbContext);

        var input = new UseCase.GetUserInput(exampleUser.Id);
        var useCase = new UseCase.GetUser(userRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbUser = await (_fixture.CreateDbContext(true))
            .Users
            .FindAsync(exampleUser.Id);

        dbUser.Should().NotBeNull();
        dbUser!.Name.Should().Be(exampleUser.Name);
        dbUser.Email.Should().Be(exampleUser.Email);
        dbUser.IsActive.Should().Be(exampleUser.IsActive);
        dbUser.Id.Should().Be(exampleUser.Id);

        output.Should().NotBeNull();
        output!.Name.Should().Be(exampleUser.Name);
        output.Email.Should().Be(exampleUser.Email);
        output.IsActive.Should().Be(exampleUser.IsActive);
        output.Id.Should().Be(exampleUser.Id);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenUserDoesntExist))]
    [Trait("Integration/Application", "GetUser - Use Cases")]
    public async Task NotFoundExceptionWhenUserDoesntExist()
    {
        var dbContext = _fixture.CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var input = new UseCase.GetUserInput(Guid.NewGuid());
        var useCase = new UseCase.GetUser(userRepository);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"User with id {input.Id} not found");
    }
}
