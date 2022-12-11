using System;

namespace Tapper;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
internal class TranspilationSourceAttribute : Attribute
{
}
