namespace ModularMonolithModule.Application.Queries;

public static class GetWidget
{
    public record Query(Guid Id) : IRequest<Response>;

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
            RuleFor(m => m.Id).NotEmpty();
        }
    }

    public class Handler(IDbConnectionFactory connections) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query query, CancellationToken token)
        {
            var id = query.Id;

            const string sql = $"SELECT * FROM {WidgetsTable} WHERE {IdColumn}=@id";
            var command = new CommandDefinition(sql, new { id }, cancellationToken: token);
            var result = await connections.Create().QuerySingleOrDefaultAsync<Response>(command);
            if (result == null)
            {
                throw new BusinessRuleValidationException($"Widget with id {id} not found");
            }

            return result;
        }
    }
}