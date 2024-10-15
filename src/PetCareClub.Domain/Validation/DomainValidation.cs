using PetCareClub.Domain.Exceptions;

namespace PetCareClub.Domain.Validation;
public class DomainValidation
{
    public static void NotNull(object? target, string name)
    {
        if (target is null)
            throw new EntityValidationException($"{name} cannot be null.");
    }

    public static void NotNullOrEmpty(string? target, string name)
    {
        if (string.IsNullOrWhiteSpace(target))
            throw new EntityValidationException($"{name} cannot be empty or null.");
    }

    public static void Length(string target, string name, int minLength, int maxLength)
    {
        NotNullOrEmpty(target, name);
        if (target.Length < minLength)
            throw new EntityValidationException($"{name} should be greater than {minLength} characters.");
        if (target.Length > maxLength)
            throw new EntityValidationException($"{name} should be less than {maxLength} characters.");
    }

    public static void IsValidEmail(string email, string name)
    {
        NotNullOrEmpty(email, name);
        if (!ValidationHelper.IsValidEmail(email))
        {
            throw new EntityValidationException($"{name} is not in a valid format.");
        }
    }

    public static void DateNotInFuture(DateTime target, string name)
    {
        if (target > DateTime.Now)
            throw new EntityValidationException($"{name} date cannot be in the future.");
    }
}
