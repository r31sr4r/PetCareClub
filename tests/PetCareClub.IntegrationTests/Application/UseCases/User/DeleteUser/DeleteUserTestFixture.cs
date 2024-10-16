using PetCareClub.IntegrationTests.Application.UseCases.User.Common;

namespace PetCareClub.IntegrationTests.Application.UseCases.User.DeleteUser;

[CollectionDefinition(nameof(DeleteUserTestFixture))]
public class DeleteUserTestFixtureCollection
    : ICollectionFixture<DeleteUserTestFixture>
{ }

public class DeleteUserTestFixture : UserUseCasesBaseFixture
{ }
