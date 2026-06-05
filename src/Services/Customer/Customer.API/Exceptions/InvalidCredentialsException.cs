namespace Customer.API.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() 
        : base("Invalid credentials")
    {
    }
}
