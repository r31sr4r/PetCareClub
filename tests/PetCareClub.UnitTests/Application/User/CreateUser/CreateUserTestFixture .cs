using PetCareClub.Application.UseCases.User.CreateUser;

namespace PetCareClub.UnitTests.Application.User.CreateUser;

[CollectionDefinition(nameof(CreateUserTestFixture))]
public class CreateUserTestFixtureCollection
    : ICollectionFixture<CreateUserTestFixture>
{ }

public class CreateUserTestFixture 
    : UserUseCasesBaseFixture
{
    public CreateUserInput GetInput()
    {
        var user = GetValidUserWithoutPassword();
        return new CreateUserInput(
            user.Name,
            user.Email,
            user.Password,
            user.IsActive
        );
    }

    public CreateUserInput GetInputWithPassword()
    {
        var user = GetValidUser();
        return new CreateUserInput(
            user.Name,
            user.Email,
            user.Password,
            user.IsActive
        );        
    }

    public CreateUserInput GetInputWithInvalidEmail()
    {
        var user = GetInput();
        user.Email = "invalid-email";
        return user;
    }

    public CreateUserInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];
        return invalidInputShortName;
    }

    public CreateUserInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetInput();
        while (invalidInputTooLongName.Name.Length <= 255)
            invalidInputTooLongName.Name = $"{invalidInputTooLongName.Name} TooLongName";
        return invalidInputTooLongName;
    }
}
