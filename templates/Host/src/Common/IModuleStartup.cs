namespace Common;

public interface IModuleStartup
{
    Task DestroyAsync();
    Task InitializeAsync();
}