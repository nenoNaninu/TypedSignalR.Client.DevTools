using System.Text;
using TypedSignalR.Client.CodeAnalysis;

namespace TypedSignalR.Client.DevTools;

// In Source Generator, we cannot use System.Text.Json and other nuget packages.
// https://github.com/dotnet/roslyn/discussions/47517

public static class PrimitiveJsonExtensions
{
    public static void SerializeToJson(this string value, StringBuilder stringBuilder)
    {
        stringBuilder.Append('\"');
        stringBuilder.Append(value);
        stringBuilder.Append('\"');
    }

    public static void SerializeToJson(this bool value, StringBuilder stringBuilder)
    {
        stringBuilder.Append(value ? "true" : "false");
    }

    public static void AppendAsJsonKey(this StringBuilder stringBuilder, string value)
    {
        stringBuilder.Append('\"');
        stringBuilder.Append(value);
        stringBuilder.Append("\":");
    }
}

public static class MetadataJsonExtensions
{
    public static void SerializeToJson(this SignalRServiceTypeMetadata metadata, StringBuilder stringBuilder)
    {
        stringBuilder.Append('{');

        stringBuilder.AppendAsJsonKey("name");
        metadata.Name.SerializeToJson(stringBuilder);
        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("path");
        metadata.Path.SerializeToJson(stringBuilder);
        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("isAuthRequired");
        metadata.IsAuthRequired.SerializeToJson(stringBuilder);
        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("authenticationSchemes");
        stringBuilder.Append('[');
        if (metadata.AuthenticationSchemes != null && metadata.AuthenticationSchemes.Count > 0)
        {
            metadata.AuthenticationSchemes[0].SerializeToJson(stringBuilder);
            for (int i = 1; i < metadata.AuthenticationSchemes.Count; i++)
            {
                stringBuilder.Append(',');
                metadata.AuthenticationSchemes[i].SerializeToJson(stringBuilder);
            }
        }
        stringBuilder.Append(']');
        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("hubType");
        metadata.HubType.SerializeToJson(stringBuilder);
        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("receiverType");
        metadata.ReceiverType.SerializeToJson(stringBuilder);

        stringBuilder.Append('}');
    }

    public static void SerializeToJson(this TypeMetadata metadata, StringBuilder stringBuilder)
    {
        stringBuilder.Append('{');

        stringBuilder.AppendAsJsonKey("interfaceName");
        metadata.InterfaceName.SerializeToJson(stringBuilder);
        stringBuilder.Append(",");

        stringBuilder.AppendAsJsonKey("interfaceFullName");
        metadata.InterfaceFullName.SerializeToJson(stringBuilder);
        stringBuilder.Append(",");

        stringBuilder.AppendAsJsonKey("collisionFreeName");
        metadata.CollisionFreeName.SerializeToJson(stringBuilder);
        stringBuilder.Append(",");

        stringBuilder.AppendAsJsonKey("methods");
        stringBuilder.Append('[');


        if (metadata.Methods.Count > 0)
        {
            metadata.Methods[0].SerializeToJson(stringBuilder);

            for (int i = 1; i < metadata.Methods.Count; i++)
            {
                stringBuilder.Append(',');
                metadata.Methods[i].SerializeToJson(stringBuilder);
            }
        }

        stringBuilder.Append("]}");
    }

    public static void SerializeToJson(this MethodMetadata metadata, StringBuilder stringBuilder)
    {
        stringBuilder.Append('{');

        stringBuilder.AppendAsJsonKey("methodName");
        metadata.MethodName.SerializeToJson(stringBuilder);
        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("returnType");
        metadata.ReturnType.SerializeToJson(stringBuilder);
        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("isGenericReturnType");
        metadata.IsGenericReturnType.SerializeToJson(stringBuilder);
        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("genericReturnTypeArgument");

        if (metadata.GenericReturnTypeArgument is not null)
        {
            metadata.GenericReturnTypeArgument.SerializeToJson(stringBuilder);
        }
        else
        {
            stringBuilder.Append("null");
        }

        stringBuilder.Append(',');

        stringBuilder.AppendAsJsonKey("parameters");
        stringBuilder.Append('[');

        if (metadata.Parameters.Count > 0)
        {
            metadata.Parameters[0].SerializeToJson(stringBuilder);

            for (int i = 1; i < metadata.Parameters.Count; i++)
            {
                stringBuilder.Append(',');
                metadata.Parameters[i].SerializeToJson(stringBuilder);
            }
        }

        stringBuilder.Append(']');
        stringBuilder.Append('}');
    }

    public static void SerializeToJson(this ParameterMetadata metadata, StringBuilder stringBuilder)
    {
        stringBuilder.Append('{');
        stringBuilder.AppendAsJsonKey("name");
        metadata.Name.SerializeToJson(stringBuilder);
        stringBuilder.Append(',');
        stringBuilder.AppendAsJsonKey("typeName");
        metadata.TypeName.SerializeToJson(stringBuilder);
        stringBuilder.Append("}");
    }
}
