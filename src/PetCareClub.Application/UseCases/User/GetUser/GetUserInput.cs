namespace PetCareClub.Application.UseCases.User.GetUser
{
    public class GetUserInput
    {
        public GetUserInput(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
