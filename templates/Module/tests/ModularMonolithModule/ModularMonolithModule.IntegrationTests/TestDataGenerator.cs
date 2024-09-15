namespace ModularMonolithModule.IntegrationTests;

public static class TestDataGenerator
{
    public static string UniqueValidName => RandomWord(20);
    public static decimal ValidPrice => new Random().Next(1, 100);
    
    private static string RandomWord(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}