using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
#if NET6_0
using NullabilityInfoContext = Nullability.NullabilityInfoContextEx;
using NullabilityInfo = Nullability.NullabilityInfoEx;
using NullabilityState = Nullability.NullabilityStateEx;
#endif

namespace JanHafner.TypeNameR.Tests;

public sealed class Explorations
{
    private readonly ITestOutputHelper testOutputHelper;

    public Explorations(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GenericPropertiesOfClosedGenericType()
    {
        // Arrange
        var closedGenericType =
            typeof(GenericTestClass<string[,,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<KeyValuePair<string, double>>, object[,][]>);

        // Act, Assert
        closedGenericType.ContainsGenericParameters.Should().BeFalse();
        closedGenericType.IsGenericParameter.Should().BeFalse();
        closedGenericType.IsGenericType.Should().BeTrue();
        closedGenericType.IsConstructedGenericType.Should().BeTrue();
        closedGenericType.IsGenericMethodParameter.Should().BeFalse();
        closedGenericType.IsGenericTypeDefinition.Should().BeFalse();
        closedGenericType.IsGenericTypeParameter.Should().BeFalse();
        closedGenericType.GenericTypeArguments.Should().HaveCount(3);
        closedGenericType.GetGenericArguments().Should().HaveCount(3);
    }

    [Fact]
    public void GenericPropertiesOfClosedGenericInheritedType()
    {
        // Arrange
        var inheritedClosedGenericType = typeof(InheritedGenericTestClass);

        // Act, Assert
        inheritedClosedGenericType.ContainsGenericParameters.Should().BeFalse();
        inheritedClosedGenericType.IsGenericParameter.Should().BeFalse();
        inheritedClosedGenericType.IsGenericType.Should().BeFalse();
        inheritedClosedGenericType.IsConstructedGenericType.Should().BeFalse();
        inheritedClosedGenericType.IsGenericMethodParameter.Should().BeFalse();
        inheritedClosedGenericType.IsGenericTypeDefinition.Should().BeFalse();
        inheritedClosedGenericType.IsGenericTypeParameter.Should().BeFalse();
        inheritedClosedGenericType.GenericTypeArguments.Should().BeEmpty();
        inheritedClosedGenericType.GetGenericArguments().Should().BeEmpty();
    }

    [Fact]
    public void GenericPropertiesOfOpenGenericType()
    {
        // Arrange
        var openGenericType = typeof(Nullable<>);

        // Act, Assert
        openGenericType.ContainsGenericParameters.Should().BeTrue();
        openGenericType.IsGenericParameter.Should().BeFalse();
        openGenericType.IsGenericType.Should().BeTrue();
        openGenericType.IsConstructedGenericType.Should().BeFalse();
        openGenericType.IsGenericMethodParameter.Should().BeFalse();
        openGenericType.IsGenericTypeDefinition.Should().BeTrue();
        openGenericType.IsGenericTypeParameter.Should().BeFalse();
        openGenericType.GenericTypeArguments.Should().BeEmpty();
        openGenericType.GetGenericArguments().Should().HaveCount(1);
    }

    [Fact]
    public void GenericPropertiesOfGenericMethod()
    {
        // Arrange
        var genericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.GenericMethod));

        // Act, Assert
        genericMethod.ContainsGenericParameters.Should().BeTrue();
        genericMethod.IsGenericMethod.Should().BeTrue();
        genericMethod.IsConstructedGenericMethod.Should().BeFalse();
        genericMethod.IsGenericMethodDefinition.Should().BeTrue();
        genericMethod.GetGenericArguments().Should().HaveCount(2);
    }

    [Fact]
    public void GenericPropertiesOfNonGenericMethod()
    {
        // Arrange
        var genericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.NonGenericMethod));

        // Act, Assert
        genericMethod.ContainsGenericParameters.Should().BeFalse();
        genericMethod.IsGenericMethod.Should().BeFalse();
        genericMethod.IsConstructedGenericMethod.Should().BeFalse();
        genericMethod.IsGenericMethodDefinition.Should().BeFalse();
        genericMethod.GetGenericArguments().Should().BeEmpty();
    }

    [Fact]
    public void NullabilityInfoOfNullableOfInt()
    {
        // Arrange
        var nullabilityInfoContext = new NullabilityInfoContext();
        var parameter = typeof(ExtensionMethodsClass).GetParameter(nameof(ExtensionMethodsClass.This), 0);

        // Act
        var nullabilityInfo = nullabilityInfoContext.Create(parameter);

        // Assert
        nullabilityInfo.Type.Should().BeSameAs(parameter.ParameterType);
        nullabilityInfo.GenericTypeArguments.Should().BeEmpty();
        nullabilityInfo.ReadState.Should().Be(NullabilityState.Nullable);
        nullabilityInfo.WriteState.Should().Be(NullabilityState.Nullable);
    }

    [Fact]
    public void EnumerationOfHashSetReturnsElements()
    {
        // Arrange
        IReadOnlySet<string> hashSet = new[] { "System" }.ToHashSet();

        // Act
        foreach (var item in hashSet)
        {
            // Assert
            item.Should().Be("System");
        }
    }

    [Fact]
    public void FrequencyOfPrimitiveTypes()
    {
        // Arrange
        const int take = 100000;

        var primitiveTypesLookup = new HashSet<Type>(
        [
            typeof(string),
            typeof(object),
            typeof(bool),
            typeof(double),
            typeof(decimal),
            typeof(float),
            typeof(sbyte),
            typeof(byte),
            typeof(void),
            typeof(ushort),
            typeof(short),
            typeof(uint),
            typeof(int),
            typeof(ulong),
            typeof(long),
            typeof(char),
            typeof(nint),
            typeof(nuint),
        ]);

        var types = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).SelectMany(a => a.GetTypes()).ToArray();

        var methods = types.SelectMany(t => t.GetMethods(BindingFlags.Instance
                                                         | BindingFlags.Public
                                                         | BindingFlags.Static
                                                         | BindingFlags.NonPublic
                                                         | BindingFlags.DeclaredOnly)).Take(take)
            .Cast<MethodBase>()
            .Concat(types.SelectMany(t => t.GetConstructors(BindingFlags.Instance
                                                            | BindingFlags.Public
                                                            | BindingFlags.Static
                                                            | BindingFlags.NonPublic
                                                            | BindingFlags.DeclaredOnly)).Take(take))
            .ToArray();

        // Act
        var groupedByFrequency = methods.SelectMany(m => m.GetParameters())
            .Select(p => p.ParameterType)
            .Concat(methods.OfType<MethodInfo>().Select(m => m.ReturnType))
            .Where(t => primitiveTypesLookup.Contains(t))
            .GroupBy(t => t, (k, t) => new { Type = k, Count = t.Count(), })
            .OrderByDescending(f => f.Count)
            .ToArray();

        // Assert
        groupedByFrequency.Should().NotBeEmpty();

        testOutputHelper.WriteLine("Frequency of primitive types");
        testOutputHelper.WriteLine(new string('-', 28));
        testOutputHelper.WriteLine($"{"Type",-15}Count");
        testOutputHelper.WriteLine(new string('_', 28));

        foreach (var frequency in groupedByFrequency)
        {
            testOutputHelper.WriteLine($"{frequency.Type.Name,-15}{frequency.Count}");
        }
    }
}