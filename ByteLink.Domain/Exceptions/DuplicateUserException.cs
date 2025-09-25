namespace ByteLink.Domain.Exceptions;

public class DuplicateUserException(string email) : Exception($"Given email '{email}' is already registered");