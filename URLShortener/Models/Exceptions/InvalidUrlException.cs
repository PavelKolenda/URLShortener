namespace URLShortener.Models.Exceptions;

public class InvalidUrlException(string message) : Exception(message);