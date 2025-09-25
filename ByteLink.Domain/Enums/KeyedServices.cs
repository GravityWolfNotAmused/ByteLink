namespace ByteLink.Domain.Enums;

public enum GeneratorKeyedServices
{
    ShortCodeGenerator,
    ShortCodeUrlGenerator,
    PasswordHashGenerator,
    JwtTokenGenerator,
    UserDatabaseConnectionStringGenerator,
    DatabasePwdGenerator,
    DatabaseUserNameGenerator,
    DatabaseNameGenerator,
    UserIdGenerator,
    UserSqidGenerator
}

public enum ComparatorKeyedServices
{
    PasswordValidator
}