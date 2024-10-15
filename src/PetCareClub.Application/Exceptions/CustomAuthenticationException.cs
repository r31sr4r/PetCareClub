namespace PetCareClub.Application.Exceptions;
public class CustomAuthenticationException : ApplicationException
{
    public CustomAuthenticationException(string message) : base(message) { }
}
