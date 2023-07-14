using Microsoft.AspNetCore.Components;

namespace Tellurian.Trains.MeetingApp.Client.Extensions;

public static class StringExtensions
{
    public static MarkupString ToMarkup(this string? me) =>
       me is null ? new MarkupString(string.Empty) : new MarkupString(me);

    public static bool HasText(this string? value, int minLenght = 1) =>
        ! string.IsNullOrWhiteSpace(value) && value.Length >= minLenght;

    public static string UncapitalizeFirstLetter(this string? value)
    {
        if (value is null || value.Length == 0) return string.Empty;
            var v = value.AsSpan();
        return string.Concat( char.ToLowerInvariant(v[0]).ToString(), v[1..^0]);
    }

    public static bool EqualsCaseInsensitive(this  string? value, string? other) =>
        value is not null && other is not null && value.Equals(other, StringComparison.OrdinalIgnoreCase);
}
