using PetCareClub.Domain.Validation;

namespace PetCareClub.UnitTests.Domain.Validation;
public class ValidationHelperTest
{
    [Theory(DisplayName = nameof(CpfIsValid))]
    [Trait("Domain", "Helpers")]
    [InlineData("52998224725")]
    public void CpfIsValid(string validCpf)
    {
        bool result = ValidationHelper.IsCpfValid(validCpf);
        Assert.True(result);
    }

    [Theory(DisplayName = nameof(CpfIsInvalid))]
    [Trait("Domain", "Helpers")]
    [InlineData("52998224721")]
    [InlineData("11111111111")]
    [InlineData("123")]
    public void CpfIsInvalid(string invalidCpf)
    {
        bool result = ValidationHelper.IsCpfValid(invalidCpf);
        Assert.False(result);
    }

    [Theory(DisplayName = nameof(EmailIsValid))]
    [Trait("Domain", "Helpers")]
    [InlineData("example@example.com")]
    [InlineData("test.user+regexcheck@subdomain.example.com")]
    public void EmailIsValid(string validEmail)
    {
        bool result = ValidationHelper.IsValidEmail(validEmail);
        Assert.True(result);
    }

    [Theory(DisplayName = nameof(EmailIsInvalid))]
    [Trait("Domain", "Helpers")]
    [InlineData("plainaddress")]
    [InlineData("@missingusername.com")]
    [InlineData("username@.com")]
    public void EmailIsInvalid(string invalidEmail)
    {
        bool result = ValidationHelper.IsValidEmail(invalidEmail);
        Assert.False(result);
    }

    [Theory(DisplayName = nameof(PhoneIsValid))]
    [Trait("Domain", "Helpers")]
    [InlineData("(11) 1234-5678")]
    [InlineData("(11) 91234-5678")]
    public void PhoneIsValid(string validPhone)
    {
        bool result = ValidationHelper.IsValidPhone(validPhone);
        Assert.True(result);
    }

    [Theory(DisplayName = nameof(PhoneIsInvalid))]
    [Trait("Domain", "Helpers")]
    [InlineData("1111-1111")]
    [InlineData("(11)1234-5678")]
    [InlineData("(11) 912345678")]
    public void PhoneIsInvalid(string invalidPhone)
    {
        bool result = ValidationHelper.IsValidPhone(invalidPhone);
        Assert.False(result);
    }

    [Theory(DisplayName = nameof(PasswordIsValid))]
    [Trait("Domain", "Helpers")]
    [InlineData("Password123!")]
    [InlineData("Str0ngP@ssword")]
    [InlineData("1Val!dPass")]
    public void PasswordIsValid(string validPassword)
    {
        bool result = ValidationHelper.IsValidPassword(validPassword);
        Assert.True(result);
    }

    [Theory(DisplayName = nameof(PasswordIsInvalid))]
    [Trait("Domain", "Helpers")]
    [InlineData("short1!")] // Too short
    [InlineData("password")] // No uppercase, no number, no symbol
    [InlineData("PASSWORD")] // No lowercase, no number, no symbol
    [InlineData("12345678")] // No letter, no symbol
    [InlineData("Password")] // No number, no symbol
    [InlineData("Password123")] // No symbol
    public void PasswordIsInvalid(string invalidPassword)
    {
        bool result = ValidationHelper.IsValidPassword(invalidPassword);
        Assert.False(result);
    }
}

