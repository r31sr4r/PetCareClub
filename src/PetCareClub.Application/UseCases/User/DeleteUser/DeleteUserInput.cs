namespace PetCareClub.Application.UseCases.User.DeleteUser
{
    public class DeleteUserInput
    {
        public DeleteUserInput(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
