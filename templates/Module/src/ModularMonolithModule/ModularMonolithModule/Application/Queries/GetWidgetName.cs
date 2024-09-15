using Dapper;
using FluentValidation;
using MediatR;
using ModularMonolithModule.Application.Commands;
using ModularMonolithModule.Domain;

namespace ModularMonolithModule.Application.Queries;

public static class GetWidgetName
{
    public record Query(Guid Id) : IRequest<string>;

    public class Validator : AbstractValidator<CreateWidget.Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }

    public class Handler(IDbConnectionFactory connections) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query query, CancellationToken token)
        {
            var id = query.Id;

            const string sql = "SELECT name FROM widgets WHERE id=@id";
            var command = new CommandDefinition(sql, new { id }, cancellationToken: token);
            var result = await connections.Create().ExecuteScalarAsync<string>(command);
            if (result == null)
            {
                throw new BusinessRuleValidationException($"Widget with id {id} not found");
            }

            return result;
        }
    }
}

public static class ListWidgets
{
    public record Query : IRequest<IEnumerable<Response>>;

    public class Response
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
    
    public class Validator : AbstractValidator<CreateWidget.Command>
    {
        public Validator()
        {
        }
    }

    public class Handler(IDbConnectionFactory connections) : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query query, CancellationToken token)
        {
            const string sql = "SELECT * FROM widgets";
            var command = new CommandDefinition(sql, cancellationToken: token);
            return await connections.Create().QueryAsync<Response>(command);
        }
    }
}