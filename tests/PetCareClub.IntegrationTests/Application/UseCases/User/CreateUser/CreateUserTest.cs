using UseCase = PetCareClub.Application.UseCases.User.CreateUser;
using PetCareClub.Domain.Exceptions;
using PetCareClub.Infra.Data.EF;
using PetCareClub.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetCareClub.Application.UseCases.User.CreateUser;

namespace PetCareClub.IntegrationTests.Application.UseCases.User.CreateUser;

[Collection(nameof(CreateUserTestFixture))]
public class CreateUserTest
{
    private readonly CreateUserTestFixture _fixture;

    public CreateUserTest(CreateUserTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateUser))]
    [Trait("Integration/Application", "Create User - Use Cases")]
    public async void CreateUser()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(
            dbContext
        );

        var useCase = new UseCase.CreateUser(repository, unitOfWork);
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbUser = await _fixture.CreateDbContext(true)
            .Users.FindAsync(output.Id);

        dbUser.Should().NotBeNull();
        dbUser!.Name.Should().Be(input.Name);
        dbUser.Email.Should().Be(input.Email);
        dbUser.IsActive.Should().BeTrue();

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Email.Should().Be(input.Email);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateUserWithPassword))]
    [Trait("Integration/Application", "Create User - Use Cases")]
    public async void CreateUserWithPassword()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(
            dbContext
        );

        var useCase = new UseCase.CreateUser(repository, unitOfWork);
        var input = _fixture.GetInputWithPassword();

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbUser = await _fixture.CreateDbContext(true)
            .Users.FindAsync(output.Id);

        dbUser.Should().NotBeNull();
        dbUser!.Name.Should().Be(input.Name);
        dbUser.Email.Should().Be(input.Email);
        dbUser.IsActive.Should().BeTrue();

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Email.Should().Be(input.Email);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateUser))]
    [Trait("Integration/Application", "Create User - Use Cases")]
    [MemberData(
        nameof(CreateUserTestDataGenerator.GetInvalidInputs),
        parameters: 6,
        MemberType = typeof(CreateUserTestDataGenerator)
    )]
    public async void ThrowWhenCantInstantiateUser(
        CreateUserInput input,
        string expectedMessage
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new UserRepository(dbContext);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var unitOfWork = new UnitOfWork(
            dbContext
        );

        var useCase = new UseCase.CreateUser(repository, unitOfWork);

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        var dbUser = _fixture.CreateDbContext(true)
            .Users.AsNoTracking()
            .ToList();

        dbUser.Should().HaveCount(0);
    }
}
