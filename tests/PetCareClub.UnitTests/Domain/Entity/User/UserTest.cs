using FluentAssertions;
using PetCareClub.Domain.Entity;
using PetCareClub.Domain.Exceptions;
using PetCareClub.Domain.Common.Security;

namespace PetCareClub.UnitTests.Domain.Entity;
[Collection(nameof(UserTestFixture))]
public class UserTest
{
    private readonly UserTestFixture _fixture;

    public UserTest(UserTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "User - Aggregates")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = _fixture.Faker.Name.FullName(),
            Email = _fixture.Faker.Internet.Email(),
            Password = "ValidPassword123!",
            IsActive = true,
            Group = _fixture.Faker.Commerce.Department(),
            Role = "Admin",
            CreatedAt = DateTime.Now
        };

        var user = new User(
            validData.Name,
            validData.Email,
            validData.Password,
            validData.IsActive,
            validData.Group,
            validData.Role
        );

        user.Should().NotBeNull();
        user.Name.Should().Be(validData.Name);
        user.Email.Should().Be(validData.Email);
        user.Password.Should().NotBeNull();
        user.Group.Should().Be(validData.Group);
        user.Role.Should().Be(validData.Role);
        user.IsActive.Should().BeTrue();
        user.CreatedAt.Should().BeCloseTo(validData.CreatedAt, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "User - Aggregates")]
    public void InstantiateErrorWhenNameIsEmpty()
    {
        Action action = () => new User("", _fixture.Faker.Internet.Email());

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name cannot be empty or null.");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenEmailIsInvalid))]
    [Trait("Domain", "User - Aggregates")]
    [InlineData("invalidEmail")]
    [InlineData("invalid.email@.com")]
    [InlineData("@invalidemail.com")]
    public void InstantiateErrorWhenEmailIsInvalid(string invalidEmail)
    {
        Action action = () => new User(_fixture.Faker.Name.FullName(), invalidEmail);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Email is not in a valid format.");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "User - Methods")]
    public void Update()
    {
        var user = _fixture.CreateValidUser();
        var newData = new
        {
            Name = _fixture.Faker.Name.FullName(),
            Email = _fixture.Faker.Internet.Email(),
            Group = _fixture.Faker.Commerce.Department(),
            Role = "User"
        };

        user.Update(newData.Name, newData.Email, newData.Group, newData.Role);

        user.Name.Should().Be(newData.Name);
        user.Email.Should().Be(newData.Email);
        user.Group.Should().Be(newData.Group);
        user.Role.Should().Be(newData.Role);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "User - Methods")]
    public void UpdateErrorWhenNameIsEmpty()
    {
        var user = _fixture.CreateValidUser();

        Action action = () => user.Update("", user.Email);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name cannot be empty or null.");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "User - Methods")]
    public void Activate()
    {
        var user = _fixture.CreateValidUser();
        user.Deactivate(); // To test the activation

        user.Activate();

        user.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "User - Methods")]
    public void Deactivate()
    {
        var user = _fixture.CreateValidUser();

        user.Deactivate();

        user.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(UpdatePasswordSuccessfully))]
    [Trait("Domain", "User - Methods")]
    public void UpdatePasswordSuccessfully()
    {
        var user = _fixture.CreateValidUser();
        var currentPasswordPlainText = "ValidPassword123!";
        var newPassword = _fixture.GenerateValidPassword();

        user.UpdatePassword(currentPasswordPlainText, newPassword);

        PasswordHasher.VerifyPasswordHash(newPassword, user.Password!).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(UpdatePasswordFailsWhenCurrentPasswordIsIncorrect))]
    [Trait("Domain", "User - Methods")]
    public void UpdatePasswordFailsWhenCurrentPasswordIsIncorrect()
    {
        var user = _fixture.CreateValidUser();
        var incorrectCurrentPassword = "WrongPassword!";
        var newPassword = _fixture.GenerateValidPassword();

        Action action = () => user.UpdatePassword(incorrectCurrentPassword, newPassword);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Current password is not valid");
    }

    [Theory(DisplayName = nameof(UpdatePasswordFailsWhenNewPasswordIsInvalid))]
    [Trait("Domain", "User - Methods")]
    [InlineData("short")]
    [InlineData("withoutdigits")]
    [InlineData("WITHOUTLOWERCASE")]
    [InlineData("withoutspecialchar123")]
    public void UpdatePasswordFailsWhenNewPasswordIsInvalid(string invalidNewPassword)
    {
        var user = _fixture.CreateValidUser();
        var currentPassword = "ValidPassword123!";

        Action action = () => user.UpdatePassword(currentPassword, invalidNewPassword);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Password does not meet complexity requirements");
    }

    [Fact(DisplayName = nameof(UpdateGroupSuccessfully))]
    [Trait("Domain", "User - Methods")]
    public void UpdateGroupSuccessfully()
    {
        var user = _fixture.CreateValidUser();
        var newGroup = _fixture.Faker.Commerce.Department();

        user.Update(user.Name, user.Email, newGroup, user.Role);

        user.Group.Should().Be(newGroup);
    }

    [Fact(DisplayName = nameof(UpdateRoleSuccessfully))]
    [Trait("Domain", "User - Methods")]
    public void UpdateRoleSuccessfully()
    {
        var user = _fixture.CreateValidUser();
        var newRole = "SuperAdmin";

        user.Update(user.Name, user.Email, user.Group, newRole);

        user.Role.Should().Be(newRole);
    }
}
