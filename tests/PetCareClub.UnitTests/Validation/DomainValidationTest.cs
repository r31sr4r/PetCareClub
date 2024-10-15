using Bogus;
using FluentAssertions;
using PetCareClub.Domain.Validation;
using PetCareClub.Domain.Exceptions;

namespace PetCareClub.UnitTests.Domain.Validation;
public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker("pt_BR");

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Name.Random.String2(10);

        Action action =
            () => DomainValidation.NotNull(value, "Value");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowException))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowException()
    {
        string? value = null;

        Action action =
            () => DomainValidation.NotNull(value, "FieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName cannot be null.");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        var value = Faker.Name.Random.String2(10);

        Action action =
            () => DomainValidation.NotNullOrEmpty(value, "Value");

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowException))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NotNullOrEmptyThrowException(string? target)
    {
        Action action =
            () => DomainValidation.NotNullOrEmpty(target, "FieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName cannot be empty or null.");
    }

    [Fact(DisplayName = nameof(DateNotInFutureOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void DateNotInFutureOk()
    {
        var publishDate = Faker.Date.Past();

        Action action =
            () => DomainValidation.DateNotInFuture(publishDate, "FieldName");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(DateNotInFutureThrowException))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void DateNotInFutureThrowException()
    {
        var publishDate = Faker.Date.Future();

        Action action =
            () => DomainValidation.DateNotInFuture(publishDate, "FieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName date cannot be in the future.");
    }

    [Fact(DisplayName = nameof(LengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void LengthOk()
    {
        var value = Faker.Random.String(5, 5);
        int minLength = 3;
        int maxLength = 10;

        Action action =
            () => DomainValidation.Length(value, "FieldName", minLength, maxLength);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(LengthThrowsWhenTooShort))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("ab", 3, 10)]
    [InlineData("a", 2, 5)]
    public void LengthThrowsWhenTooShort(string value, int minLength, int maxLength)
    {
        Action action =
            () => DomainValidation.Length(value, "FieldName", minLength, maxLength);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"FieldName should be greater than {minLength} characters.");
    }

    [Theory(DisplayName = nameof(LengthThrowsWhenTooLong))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("abcdefghijklmnop", 3, 10)]
    [InlineData("this_is_a_very_long_string", 5, 20)]
    public void LengthThrowsWhenTooLong(string value, int minLength, int maxLength)
    {
        Action action =
            () => DomainValidation.Length(value, "FieldName", minLength, maxLength);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"FieldName should be less than {maxLength} characters.");
    }

    [Theory(DisplayName = nameof(IsValidEmailOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("example@example.com")]
    [InlineData("test.user+regexcheck@subdomain.example.com")]
    public void IsValidEmailOk(string validEmail)
    {
        Action action =
            () => DomainValidation.IsValidEmail(validEmail, "Email");

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(IsValidEmailThrowsException))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("plainaddress")]
    [InlineData("@missingusername.com")]
    [InlineData("username@.com")]
    public void IsValidEmailThrowsException(string invalidEmail)
    {
        Action action =
            () => DomainValidation.IsValidEmail(invalidEmail, "Email");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Email is not in a valid format.");
    }
}
