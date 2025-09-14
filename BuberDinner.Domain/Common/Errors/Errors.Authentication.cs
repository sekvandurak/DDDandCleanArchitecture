using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Authentication.InvalidCredentials",
            description: "The provided credentials are invalid."
        );
    }
   
}