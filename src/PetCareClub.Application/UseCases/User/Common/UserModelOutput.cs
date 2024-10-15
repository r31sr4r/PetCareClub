using DomainEntity = PetCareClub.Domain.Entity;

namespace PetCareClub.Application.UseCases.User.Common;
public class UserModelOutput
{
    public UserModelOutput(Guid id, string name, string email, bool isActive)
    {
        Id = id;
        Name = name;
        Email = email;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static UserModelOutput FromUser(DomainEntity.User user)
    {
        return new UserModelOutput(user.Id, user.Name, user.Email, user.IsActive);
    }
}
