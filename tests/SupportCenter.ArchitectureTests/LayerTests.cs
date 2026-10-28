using System.Reflection;
using SupportCenter.Api;
using SupportCenter.Domain.Common;
using Xunit;

namespace SupportCenter.ArchitectureTests;

public class LayerTests
{
    private static readonly Assembly Domain = typeof(Entity).Assembly;
    private static readonly Assembly Application =
        typeof(SupportCenter.Application.DependencyInjection).Assembly;
    private static readonly Assembly Infrastructure =
        typeof(SupportCenter.Infrastructure.DependencyInjection).Assembly;
    private static readonly Assembly Api = typeof(Program).Assembly;

    private static List<string> TypesReferencing(
        Assembly source,
        Assembly target)
    {
        var targetName = target.GetName().Name!;

        return source
            .DefinedTypes
            .Where(type => type
                .Assembly
                .GetReferencedAssemblies()
                .Any(reference => reference.Name == targetName))
            .Select(type => type.FullName ?? type.Name)
            .ToList();
    }

    private static List<string> TypesReferencingPersistenceFrameworks(
        Assembly source)
    {
        return source
            .DefinedTypes
            .Where(type => type
                .Assembly
                .GetReferencedAssemblies()
                .Any(reference =>
                    reference.Name!.Contains("EntityFrameworkCore")
                    || reference.Name!.Contains("Npgsql")))
            .Select(type => type.FullName ?? type.Name)
            .ToList();
    }

    [Fact]
    public void Domain_should_not_reference_other_layers()
    {
        Assert.Empty(TypesReferencing(Domain, Application));
        Assert.Empty(TypesReferencing(Domain, Infrastructure));
        Assert.Empty(TypesReferencing(Domain, Api));
    }

    [Fact]
    public void Application_should_not_reference_infrastructure_or_api()
    {
        Assert.Empty(TypesReferencing(Application, Infrastructure));
        Assert.Empty(TypesReferencing(Application, Api));
    }

    [Fact]
    public void Infrastructure_should_not_reference_api()
    {
        Assert.Empty(TypesReferencing(Infrastructure, Api));
    }

    [Fact]
    public void Domain_should_not_depend_on_persistence_frameworks()
    {
        Assert.Empty(TypesReferencingPersistenceFrameworks(Domain));
    }

    [Fact]
    public void Application_should_not_depend_on_persistence_frameworks()
    {
        Assert.Empty(TypesReferencingPersistenceFrameworks(Application));
    }
}
