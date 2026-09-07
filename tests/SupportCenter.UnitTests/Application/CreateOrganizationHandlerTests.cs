using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Organizations.CreateOrganization;
using SupportCenter.Domain.Organizations;

namespace SupportCenter.UnitTests.Application;

public class CreateOrganizationHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Organization()
    {
        // Arrange
        var repository = Substitute.For<IOrganizationRepository>();

        var handler = new CreateOrganizationCommandHandler(repository);

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
    }
}