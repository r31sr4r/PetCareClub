namespace PetCareClub.UnitTests.Application.User.DeleteUser;

[CollectionDefinition(nameof(DeleteUserTestFixture))]
public class DeleteUserTestFixtureCollection
    : ICollectionFixture<DeleteUserTestFixture>
{ }

public class DeleteUserTestFixture : UserUseCasesBaseFixture
{ }
