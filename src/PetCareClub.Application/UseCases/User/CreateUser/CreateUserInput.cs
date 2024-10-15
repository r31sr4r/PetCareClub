namespace PetCareClub.Application.UseCases.User.CreateUser;
public class CreateUserInput
{
    public CreateUserInput(
        string name,
        string email,
        string? password = null,
        bool isActive = true
    )
    {
        Name = name;
        Email = email;
        Password = password ?? string.Empty;
        IsActive = isActive;
    }

    public string Name { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; }
}
