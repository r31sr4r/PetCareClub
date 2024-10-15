namespace PetCareClub.UnitTests.Application.User.CreateUser;
public class CreateUserTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int numberOfIterations = 12)
    {
        var fixture = new CreateUserTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3; 

        for (int index = 0; index < numberOfIterations; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInvalidInputShortName(),
                        "Name should be greater than 3 characters."
                    });
                    break;
                case 1:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInvalidInputTooLongName(),
                        "Name should be less than 255 characters."
                    });
                    break;
                case 2:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithInvalidEmail(),
                        "Email is not in a valid format."                        
                    });
                    break;
            }
        }
        return invalidInputsList;
    }
}
