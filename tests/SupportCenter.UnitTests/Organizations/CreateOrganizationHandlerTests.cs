using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Organizations.CreateOrganization;
using SupportCenter.Domain.Organizations;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Auditing.CreateAuditEntry;

namespace SupportCenter.UnitTests.Organizations;

public class CreateOrganizationHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Organization()
    {
        // Arrange
        var repository = Substitute.For<IOrganizationRepository>();

        var dispatcher = Substitute.For<IDispatcher>();

        var handler = new CreateOrganizationCommandHandler(repository, dispatcher);

        var command = new CreateOrganizationCommand(
            "Acme Software",
            "acme"
        );

        Organization? savedOrganization = null;

        repository
            .AddAsync(
                Arg.Do<Organization>(x => savedOrganization = x),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);


        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);


        // Assert
        Assert.NotEqual(Guid.Empty, result);

        Assert.NotNull(savedOrganization);

        Assert.Equal(
            "Acme Software",
            savedOrganization.Name);

        Assert.Equal(
            "acme",
            savedOrganization.Slug);

        await repository
            .Received(1)
            .AddAsync(
                Arg.Any<Organization>(),
                Arg.Any<CancellationToken>());

        await dispatcher
            .Received(1)
            .Send<Guid>(
                Arg.Is<CreateAuditEntryCommand>(
                command =>
                command.Action == "ORGANIZATION_CREATED"
                &&
                command.EntityName == "Organization"),
                Arg.Any<CancellationToken>());
    }
}