namespace Hei_Hei_Api.Exceptions;

public class InvalidCredentialsException : UnauthorizedAccessException
{
    public InvalidCredentialsException(string message) : base(message) { }
}
