using PetCareClub.Infra.Data.EF.Repositories;
using PetCareClub.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using UseCase = PetCareClub.Application.UseCases.User.DeleteUser;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetCareClub.Application.UseCases.User.DeleteUser;
using PetCareClub.Application.Exceptions;

namespace PetCareClub.IntegrationTests.Application.UseCases.User.DeleteUser;

[Collection(nameof(DeleteUserTestFixture))]
public class DeleteUserTest
{
    private readonly DeleteUserTestFixture _fixture;

    public DeleteUserTest(DeleteUserTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteUser))]
    [Trait("Integration/Application", "DeleteUser - Use Cases")]
    public async Task DeleteUser()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(
            dbContext
        );

        var userExample = _fixture.GetValidUser();
        await dbContext.AddRangeAsync(_fixture.GetUsersList());
        var tracking = await dbContext.AddAsync(userExample);
        await dbContext.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        var useCase = new UseCase.DeleteUser(repository, unitOfWork);
        var input = new DeleteUserInput(userExample.Id);

        await useCase.Handle(input, CancellationToken.None);

        var dbUser = await (_fixture.CreateDbContext(true))
            .Users
            .FindAsync(userExample.Id);

        dbUser.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ThrowWhenUserNotFound))]
    [Trait("Integration/Application", "DeleteUser - Use Cases")]
    public async Task ThrowWhenUserNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWork(
            dbContext
        );

        var userRepository = new UserRepository(dbContext);
        var input = new DeleteUserInput(Guid.NewGuid());
        var useCase = new UseCase.DeleteUser(userRepository, unitOfWork);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"User with id {input.Id} not found");
    }
}
