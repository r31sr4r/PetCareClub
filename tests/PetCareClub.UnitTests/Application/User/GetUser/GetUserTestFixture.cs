namespace PetCareClub.UnitTests.Application.User.GetUser;

[CollectionDefinition(nameof(GetUserTestFixture))]
public class GetUserTestFixtureCollection :
    ICollectionFixture<GetUserTestFixture>
{ }

public class GetUserTestFixture : UserUseCasesBaseFixture
{ }
