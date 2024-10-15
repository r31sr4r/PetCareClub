using PetCareClub.Domain.Common.Security;
using PetCareClub.Domain.Exceptions;
using PetCareClub.Domain.SeedWork;
using PetCareClub.Domain.Validation;

namespace PetCareClub.Domain.Entity;

public class User : AggregateRoot
{
    public User() { }

    public User(
        string name,
        string email,
        string? password = null,
        bool isActive = true,
        string? group = null, 
        string? role = null,
        DateTime? createdAt = null
    ) : base()
    {
        Name = name;
        Email = email;
        Group = group;
        Role = role;
        Password = !string.IsNullOrEmpty(password)
            ? PasswordHasher.HashPassword(password!)
            : null;
        IsActive = isActive;
        CreatedAt = createdAt ?? DateTime.Now;
        Validate();
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Password { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? Group { get; private set; }
    public string? Role { get; private set; }

    public void Update(string name, string email, string? group = null, string? role = null)
    {
        Name = name;
        Email = email;
        Group = group;
        Role = role;
        Validate();
    }

    public void UpdatePassword(string currentPassword, string newPassword)
    {
        if (!PasswordHasher.VerifyPasswordHash(currentPassword, this.Password))
        {
            throw new EntityValidationException("Current password is not valid");
        }
        ValidatePassword(newPassword);
        this.Password = PasswordHasher.HashPassword(newPassword);
    }

    private void Validate()
    {
        DomainValidation.Length(Name, nameof(Name), 3, 255);
        DomainValidation.IsValidEmail(Email, nameof(Email));
        if (CreatedAt != default)
            DomainValidation.DateNotInFuture(CreatedAt, nameof(CreatedAt));
        if (!string.IsNullOrEmpty(Password))
            ValidatePassword(Password!);
    }

    private void ValidatePassword(string password)
    {
        if (!ValidationHelper.IsValidPassword(password))
        {
            throw new EntityValidationException($"{nameof(Password)} does not meet complexity requirements");
        }
    }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }
}
