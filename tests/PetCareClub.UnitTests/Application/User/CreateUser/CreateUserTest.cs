using Moq;
using UseCase = PetCareClub.Application.UseCases.User.CreateUser;
using DomainEntity = PetCareClub.Domain.Entity;
using PetCareClub.Domain.Exceptions;
using FluentAssertions;
using PetCareClub.Application.UseCases.User.CreateUser;

namespace PetCareClub.UnitTests.Application.User.CreateUser;

[Collection(nameof(CreateUserTestFixture))]
public class CreateUserTest
{
    private readonly CreateUserTestFixture _fixture;

    public CreateUserTest(CreateUserTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateUser))]
    [Trait("Application", "Create User - Use Cases")]
    public async void CreateUser()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCase.CreateUser(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.User>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Email.Should().Be(input.Email);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateUser))]
    [Trait("Application", "Create User - Use Cases")]
    [MemberData(
    nameof(CreateUserTestDataGenerator.GetInvalidInputs),
    parameters: 12,
    MemberType = typeof(CreateUserTestDataGenerator)
    )]
    public async void ThrowWhenCantInstantiateUser(CreateUserInput input, string expectedMessage)
    {
        var useCase = new UseCase.CreateUser(
            _fixture.GetRepositoryMock().Object,
            _fixture.GetUnitOfWorkMock().Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);
    }
}
