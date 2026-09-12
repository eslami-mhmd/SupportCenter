using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Auditing.CreateAuditEntry;
using SupportCenter.Domain.Auditing;

namespace SupportCenter.UnitTests.Auditing;

public class CreateAuditEntryHandlerTests
{
    [Fact]
    public async Task Should_create_audit_entry()
    {
        // Arrange

        var repository =
            Substitute.For<IAuditRepository>();


        AuditEntry? createdAudit =
            null;


        repository
            .AddAsync(
                Arg.Do<AuditEntry>(
                    x => createdAudit = x),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);


        var handler =
            new CreateAuditEntryCommandHandler(
                repository);


        var entityId =
            Guid.NewGuid();


        var command =
            new CreateAuditEntryCommand(
                null,
                "STATUS_CHANGED",
                "Ticket",
                entityId,
                "{\"Status\":\"Open\"}",
                "{\"Status\":\"InProgress\"}");



        // Act

        var result =
            await handler.Handle(
                command,
                CancellationToken.None);



        // Assert

        Assert.NotEqual(
            Guid.Empty,
            result);


        Assert.NotNull(
            createdAudit);


        Assert.Equal(
            "STATUS_CHANGED",
            createdAudit.Action);


        Assert.Equal(
            "Ticket",
            createdAudit.EntityName);


        Assert.Equal(
            entityId,
            createdAudit.EntityId);


        await repository
            .Received(1)
            .AddAsync(
                Arg.Any<AuditEntry>(),
                Arg.Any<CancellationToken>());
    }
}