#if NETSTANDARD2_1
namespace System.Runtime.CompilerServices;

/// <summary>
/// Enables records and init-only properties when targeting .NET Standard 2.1.
/// </summary>
internal static class IsExternalInit { }
#endif
