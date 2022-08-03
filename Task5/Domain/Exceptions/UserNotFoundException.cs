namespace Task5.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User not found")
    {
    }
}