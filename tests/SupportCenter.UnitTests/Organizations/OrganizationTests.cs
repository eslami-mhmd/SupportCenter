using SupportCenter.Domain.Organizations;

namespace SupportCenter.UnitTests.Organizations;

public class OrganizationTests
{
    [Fact]
    public void Create_should_create_active_organization()
    {
        var organization =
            Organization.Create(
                "Acme Software",
                "acme");


        Assert.Equal(
            "Acme Software",
            organization.Name);


        Assert.Equal(
            OrganizationStatus.Active,
            organization.Status);
    }


    [Fact]
    public void Suspend_should_change_status()
    {
        var organization =
            Organization.Create(
                "Acme Software",
                "acme");


        organization.Suspend();


        Assert.Equal(
            OrganizationStatus.Suspended,
            organization.Status);
    }
}