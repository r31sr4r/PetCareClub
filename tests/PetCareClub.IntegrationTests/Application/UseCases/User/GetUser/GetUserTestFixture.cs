using PetCareClub.IntegrationTests.Application.UseCases.User.Common;

namespace PetCareClub.IntegrationTests.Application.UseCases.User.GetUser;

[CollectionDefinition(nameof(GetUserTestFixture))]
public class GetUserTestFixtureCollection
    : ICollectionFixture<GetUserTestFixture>
{ }

public class GetUserTestFixture : UserUseCasesBaseFixture
{ }
