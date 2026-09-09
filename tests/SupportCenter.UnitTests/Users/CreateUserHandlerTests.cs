using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Users.CreateUser;
using SupportCenter.Domain.Users;

namespace SupportCenter.UnitTests.Users;

public class CreateUserHandlerTests
{
    [Fact]
    public async Task Should_create_user()
    {
        var repository =
            Substitute.For<IUserRepository>();


        var handler =
            new CreateUserCommandHandler(repository);


        var command =
            new CreateUserCommand(
                Guid.NewGuid(),
                "agent@test.com",
                "John Agent",
                UserRole.Agent);


        var result =
            await handler.Handle(
                command,
                CancellationToken.None);


        Assert.NotEqual(
            Guid.Empty,
            result);


        await repository
            .Received(1)
            .AddAsync(
                Arg.Any<User>(),
                Arg.Any<CancellationToken>());
    }
}