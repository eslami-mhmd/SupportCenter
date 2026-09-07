using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace SupportCenter.Application.Abstractions.Messaging;

public sealed class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        await ValidateAsync(
            command,
            cancellationToken);

        var handlerType =
            typeof(ICommandHandler<,>)
                .MakeGenericType(
                    command.GetType(),
                    typeof(TResponse));

        var handler =
            _serviceProvider.GetRequiredService(handlerType);

        return await ((dynamic)handler).Handle(
            (dynamic)command,
            cancellationToken);
    }

    public async Task<TResponse> Send<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken = default)
    {
        var handlerType =
            typeof(IQueryHandler<,>)
                .MakeGenericType(
                    query.GetType(),
                    typeof(TResponse));

        var handler =
            _serviceProvider.GetRequiredService(handlerType);

        return await ((dynamic)handler).Handle(
            (dynamic)query,
            cancellationToken);
    }

    private async Task ValidateAsync<TCommand>(
        TCommand command,
        CancellationToken cancellationToken)
    {
        var commandType = command!.GetType();

        var validatorType =
            typeof(IValidator<>)
                .MakeGenericType(commandType);

        var validators =
            _serviceProvider
                .GetServices(validatorType)
                .ToArray();

        if (validators.Length == 0)
            return;

        var context =
            new ValidationContext<object>(command);

        var errors = new Dictionary<string, string[]>();

        foreach (var validator in validators)
        {
            if (validator is not IValidator nonGenericValidator)
                continue;

            var result =
                await nonGenericValidator.ValidateAsync(
                    context,
                    cancellationToken);

            foreach (var error in result.Errors)
            {
                if (!errors.ContainsKey(error.PropertyName))
                {
                    errors[error.PropertyName] =
                        new[] { error.ErrorMessage };
                }
            }
        }

        if (errors.Count > 0)
        {
            throw new SupportCenter.Application.Exceptions.ValidationException(
                errors);
        }
    }
}