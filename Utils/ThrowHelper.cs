using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NerdCritica.Domain.Utils;

public class ThrowHelper
{
    public static void ThrowIfNull<T>([NotNull] T? value, string message = "",
        [CallerArgumentExpression("value")] string valueExpression = "Not provided")
    {
        _ = value ?? throw new ArgumentNullException(message ?? $"{valueExpression} cannot be null");
    }
 }
