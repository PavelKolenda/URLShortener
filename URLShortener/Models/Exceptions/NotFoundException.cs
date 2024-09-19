namespace URLShortener.Models.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}
