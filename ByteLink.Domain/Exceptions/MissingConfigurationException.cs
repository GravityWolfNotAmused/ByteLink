namespace ByteLink.Domain.Exceptions;

public class MissingConfigurationException(string key) : Exception($"Configuration key '{key}' is missing from appsetings.json.") { }
