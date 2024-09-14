namespace ModularMonolithModule.Domain;

public class BusinessRuleValidationException(string message) : Exception(message)
{
    public static void ThrowAlreadyExists<T>(Guid id) =>
        throw new BusinessRuleValidationException($"{nameof(T)} '{id}' already exists.");

    public static void ThrowAlreadyExists<T>(string value)=>
        throw new BusinessRuleValidationException($"{typeof(T).Name} '{value}' already exists.");
}