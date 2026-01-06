using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypedSignalR.Client.CodeAnalysis;
using TypedSignalR.Client.DevTools.Templates;

namespace TypedSignalR.Client.DevTools;

[Generator]
public sealed class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            ctx.CancellationToken.ThrowIfCancellationRequested();

            ctx.AddSource("TypedSignalR.Client.DevTools.Specification.Generated.cs", NormalizeNewLines(new CoreTemplate().TransformText()));
        });

        var specialSymbols = context.CompilationProvider
            .Select(static (compilation, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                return GetSpecialSymbols(compilation);
            });

        var mapHubMethodSymbols = context.SyntaxProvider
            .CreateSyntaxProvider(WhereMapHubMethod, TransformToSourceSymbol)
            .Combine(specialSymbols)
            .Select(ValidateMapHubMethodSymbol)
            .Where(static x => x.IsValid())
            .Collect();

        context.RegisterSourceOutput(mapHubMethodSymbols.Combine(specialSymbols), GenerateSource);
    }

    private static bool WhereMapHubMethod(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (syntaxNode is InvocationExpressionSyntax invocationExpressionSyntax)
        {
            if (invocationExpressionSyntax.Expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
            {
                if (memberAccessExpressionSyntax.Name.Identifier.ValueText is "MapHub")
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static SourceSymbol TransformToSourceSymbol(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var node = context.Node as InvocationExpressionSyntax;
        var target = node?.Expression as MemberAccessExpressionSyntax;

        if (target is null)
        {
            return default;
        }

        var arguments = node!.ArgumentList.Arguments;

        // Assumes MapHub is called as an extension method
        //     MapHub<THub>(this IEndpointRouteBuilder, string) -> 1
        //     MapHub<THub>(this IEndpointRouteBuilder, string, Action<HttpConnectionDispatcherOptions>) -> 2
        if (arguments.Count is not (1 or 2))
        {
            return default;
        }

        var path = GetPath(context, arguments[0].Expression);

        if (string.IsNullOrEmpty(path))
        {
            return default;
        }

        var methodSymbol = context.SemanticModel.GetSymbolInfo(target).Symbol as IMethodSymbol;

        if (methodSymbol is null)
        {
            return default;
        }

        return new SourceSymbol(methodSymbol, target.GetLocation(), path!);
    }

    private static string? GetPath(GeneratorSyntaxContext context, ExpressionSyntax syntax)
    {
        if (syntax.Kind() == SyntaxKind.StringLiteralExpression
            && syntax is LiteralExpressionSyntax literal)
        {
            return literal.Token.ValueText;
        }

        var symbol = context.SemanticModel.GetSymbolInfo(syntax).Symbol;

        if (symbol is IFieldSymbol field
            && field.IsConst
            && field.ConstantValue is string fieldValue)
        {
            return fieldValue;
        }

        if (symbol is ILocalSymbol local
            && local.IsConst
            && local.ConstantValue is string localValue)
        {
            return localValue;
        }

        return null;
    }

    private static ValidatedSourceSymbol ValidateMapHubMethodSymbol((SourceSymbol, SpecialSymbols) pair, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var sourceSymbol = pair.Item1;

        if (sourceSymbol.IsNone())
        {
            return default;
        }

        var methodSymbol = sourceSymbol.MethodSymbol;
        var location = sourceSymbol.Location;
        var path = sourceSymbol.Path;
        var specialSymbols = pair.Item2;

        var extensionMethodSymbol = methodSymbol.ReducedFrom ?? methodSymbol.ConstructedFrom;

        foreach (var mapHubMethodSymbol in specialSymbols.MapHubMethodSymbols)
        {
            if (SymbolEqualityComparer.Default.Equals(extensionMethodSymbol, mapHubMethodSymbol))
            {
                return new ValidatedSourceSymbol(methodSymbol, location, path);
            }
        }

        return default;
    }

    private static void GenerateSource(SourceProductionContext context, (ImmutableArray<ValidatedSourceSymbol>, SpecialSymbols) data)
    {
        var sourceSymbols = data.Item1;
        var specialSymbols = data.Item2;

        try
        {
            var serviceTypes = ExtractSignalRServiceTypesFromMapHubMethods(context, sourceSymbols, specialSymbols);

            var template = new SpecificationTemplate(serviceTypes);
            var source = NormalizeNewLines(template.TransformText());

            Debug.WriteLine(source);

            context.AddSource("TypedSignalR.Client.DevTools.Specification.Core.Generated.cs", source);
        }
        catch (Exception exception)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptorItems.UnexpectedException,
                Location.None,
                exception));
        }
    }

    private static SpecialSymbols GetSpecialSymbols(Compilation compilation)
    {
        var taskSymbol = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task");
        var genericTaskSymbol = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1");
        var cancellationTokenSymbol = compilation.GetTypeByMetadataName("System.Threading.CancellationToken");
        var asyncEnumerableSymbol = compilation.GetTypeByMetadataName("System.Collections.Generic.IAsyncEnumerable`1");
        var channelReaderSymbol = compilation.GetTypeByMetadataName("System.Threading.Channels.ChannelReader`1");

        var hubEndpointRouteBuilderExtensions = compilation.GetTypesByMetadataName("Microsoft.AspNetCore.Builder.HubEndpointRouteBuilderExtensions");
        var authorizeAttributes = compilation.GetTypesByMetadataName("Microsoft.AspNetCore.Authorization.AuthorizeAttribute");
        var hubAttributes = compilation.GetTypesByMetadataName("TypedSignalR.Client.HubAttribute");
        var receiverAttributes = compilation.GetTypesByMetadataName("TypedSignalR.Client.ReceiverAttribute");

        var mapHubMethodSymbols = ImmutableArray<IMethodSymbol>.Empty;

        foreach (var builderExtensions in hubEndpointRouteBuilderExtensions)
        {
            foreach (var memberSymbol in builderExtensions.GetMembers())
            {
                if (memberSymbol is not IMethodSymbol methodSymbol)
                {
                    continue;
                }

                if (methodSymbol.Name is "MapHub" && methodSymbol.MethodKind is MethodKind.Ordinary)
                {
                    mapHubMethodSymbols = mapHubMethodSymbols.Add(methodSymbol);
                }
            }
        }

        return new SpecialSymbols(
            taskSymbol!,
            genericTaskSymbol!,
            cancellationTokenSymbol!,
            asyncEnumerableSymbol!,
            channelReaderSymbol!,
            hubAttributes,
            receiverAttributes,
            authorizeAttributes,
            mapHubMethodSymbols
        );
    }

    private static IReadOnlyList<SignalRServiceTypeMetadata> ExtractSignalRServiceTypesFromMapHubMethods(
        SourceProductionContext context,
        IReadOnlyList<ValidatedSourceSymbol> mapHubMethodSymbols,
        SpecialSymbols specialSymbols)
    {
        var serviceTypeList = new List<SignalRServiceTypeMetadata>(mapHubMethodSymbols.Count);

        foreach (var mapHubMethod in mapHubMethodSymbols)
        {
            var methodSymbol = mapHubMethod.MethodSymbol;
            var location = mapHubMethod.Location;

            var serviceTypeSymbol = methodSymbol.TypeArguments[0];

            if (!serviceTypeList.Contains(serviceTypeSymbol))
            {
                var hubType = ExtractHubTypesFromSignalRServiceType(context, serviceTypeSymbol, specialSymbols, location);
                var receiverType = ExtractReceiverTypeTypesFromSignalRServiceType(context, serviceTypeSymbol, specialSymbols, location);

                if (hubType is null || receiverType is null)
                {
                    continue;
                }

                var (isAuthRequired, authenticationSchemes) = AnalyzeRequiredAuth(context, serviceTypeSymbol, specialSymbols);

                serviceTypeList.Add(new SignalRServiceTypeMetadata(
                    serviceTypeSymbol,
                    hubType,
                    receiverType,
                    mapHubMethod.Path,
                    isAuthRequired,
                    authenticationSchemes
                ));
            }
        }

        return serviceTypeList;
    }

    private static TypeMetadata? ExtractHubTypesFromSignalRServiceType(
        SourceProductionContext context,
        ITypeSymbol serviceType,
        SpecialSymbols specialSymbols,
        Location location)
    {
        foreach (var interfaceSymbol in serviceType.Interfaces)
        {
            foreach (var attributeData in interfaceSymbol.GetAttributes())
            {
                foreach (var hubAttributeSymbol in specialSymbols.HubAttributeSymbols)
                {
                    if (!SymbolEqualityComparer.Default.Equals(hubAttributeSymbol, attributeData.AttributeClass))
                    {
                        continue;
                    }
                }

                var isValid = TypeValidator.ValidateHubTypeRule(context, interfaceSymbol, specialSymbols, location);

                return isValid ? new TypeMetadata(interfaceSymbol) : null;
            }
        }

        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptorItems.HubAttributeAnnotationRule,
            location,
            serviceType.ToDisplayString()));

        return null;
    }


    private static TypeMetadata? ExtractReceiverTypeTypesFromSignalRServiceType(
        SourceProductionContext context,
        ITypeSymbol serviceType,
        SpecialSymbols specialSymbols,
        Location location)
    {
        var baseType = serviceType.BaseType;

        if (!baseType?.IsGenericType ?? true)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptorItems.BaseHubTypeRule,
                serviceType.Locations[0],
                serviceType.ToDisplayString()));

            return null;
        }

        var receiverType = baseType!.TypeArguments[0];

        bool annotated = false;

        foreach (var attributeData in receiverType.GetAttributes())
        {
            foreach (var receiverAttributeSymbol in specialSymbols.ReceiverAttributeSymbols)
            {
                if (SymbolEqualityComparer.Default.Equals(receiverAttributeSymbol, attributeData.AttributeClass))
                {
                    annotated = true;
                }
            }
        }

        if (!annotated)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptorItems.ReceiverAttributeAnnotationRule,
                location,
                serviceType.ToDisplayString()));

            return null;
        }

        var isValid = TypeValidator.ValidateReceiverTypeRule(context, receiverType, specialSymbols, location);

        return isValid ? new TypeMetadata(receiverType) : null;
    }

    private static (bool isAuthRequired, List<string> authenticationSchemes) AnalyzeRequiredAuth(
        SourceProductionContext context,
        ITypeSymbol serviceType,
        SpecialSymbols specialSymbols)
    {
        var schemes = new List<string>();
        bool isAuthRequired = false;

        void CheckAttributes(ImmutableArray<AttributeData> attributes)
        {
            foreach (var attribute in attributes)
            {
                foreach (var authorizeAttributeSymbol in specialSymbols.AuthorizeAttributeSymbols)
                {
                    if (SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, authorizeAttributeSymbol))
                    {
                        isAuthRequired = true;
                        bool hasScheme = false;
                        if (attribute.NamedArguments is { Length: > 0 })
                        {
                            foreach (var arg in attribute.NamedArguments)
                            {
                                if (arg.Key == "AuthenticationSchemes" && arg.Value.Value is string value && !string.IsNullOrWhiteSpace(value))
                                {
                                    // Split by comma in case multiple schemes are specified
                                    foreach (var scheme in value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        if (!schemes.Contains(scheme))
                                            schemes.Add(scheme);
                                    }
                                    hasScheme = true;
                                }
                            }
                        }

                        // If [Authorize] is present but no AuthenticationSchemes specified, assume JWT
                        if (!hasScheme)
                        {
                            if (!schemes.Contains("Bearer"))
                                schemes.Add("Bearer");
                        }
                    }
                }
            }
        }

        CheckAttributes(serviceType.GetAttributes());

        foreach (var member in serviceType.GetMembers())
        {
            if (member is IMethodSymbol method)
            {
                CheckAttributes(method.GetAttributes());
            }
        }

        return (isAuthRequired, schemes);
    }


    private static string NormalizeNewLines(string source)
    {
        return source.Replace("\r\n", "\n");
    }

    private readonly record struct SourceSymbol
    {
        public static SourceSymbol None => default;

        public readonly IMethodSymbol MethodSymbol;
        public readonly Location Location;
        public readonly string Path;

        public SourceSymbol(IMethodSymbol methodSymbol, Location location, string path)
        {
            MethodSymbol = methodSymbol;
            Location = location;
            Path = path;
        }

        public bool IsNone()
        {
            return this == default;
        }
    }

    private readonly record struct ValidatedSourceSymbol
    {
        public static ValidatedSourceSymbol None => default;

        public readonly IMethodSymbol MethodSymbol;
        public readonly Location Location;
        public readonly string Path;

        public ValidatedSourceSymbol(IMethodSymbol methodSymbol, Location location, string path)
        {
            MethodSymbol = methodSymbol;
            Location = location;
            Path = path;
        }

        public bool IsValid()
        {
            return this != default;
        }
    }
}
