namespace ByteLink.Domain.Exceptions;

public class NotFoundException(string entityName, object key) : Exception($"{entityName} with value {key} was not found.") { }
