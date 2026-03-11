namespace Hei_Hei_Api.Exceptions;

public class EmailNotVerifiedException : UnauthorizedAccessException
{
    public EmailNotVerifiedException(string message) : base(message) { }
}
