using ModularMonolithModule.Domain;

namespace ModularMonolithModule.Application;

public interface IWidgetRepository
{
    Task<bool> Exists(Guid id);
    Task<bool> Exists(string name);
    Task Add(Widget widget);
}