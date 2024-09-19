namespace ModularMonolithModule.Application.Queries;

public static class ListWidgets
{
    public record Query : IRequest<IEnumerable<Response>>;

    public class Response
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public decimal Price { get; init; }
    }
    
    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
        }
    }

    public class Handler(IDbConnectionFactory connections) : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT * FROM {WidgetsTable}";
            var command = new CommandDefinition(sql, cancellationToken: token);
            return await connections.Create().QueryAsync<Response>(command);
        }
    }
}