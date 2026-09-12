using SupportCenter.Domain.Auditing;

namespace SupportCenter.UnitTests.Auditing;

public class AuditEntryTests
{
    [Fact]
    public void Create_Should_Create_AuditEntry()
    {
        // Arrange
        var entityId =
            Guid.NewGuid();

        var userId =
            Guid.NewGuid();


        // Act
        var audit =
            AuditEntry.Create(
                userId,
                "STATUS_CHANGED",
                "Ticket",
                entityId,
                "{\"Status\":\"Open\"}",
                "{\"Status\":\"InProgress\"}");


        // Assert

        Assert.NotEqual(
            Guid.Empty,
            audit.Id);


        Assert.Equal(
            userId,
            audit.UserId);


        Assert.Equal(
            "STATUS_CHANGED",
            audit.Action);


        Assert.Equal(
            "Ticket",
            audit.EntityName);


        Assert.Equal(
            entityId,
            audit.EntityId);


        Assert.NotEqual(
            default,
            audit.CreatedDate);
    }
}