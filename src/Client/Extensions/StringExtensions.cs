using Microsoft.AspNetCore.Components;

namespace Tellurian.Trains.MeetingApp.Client.Extensions;

public static class StringExtensions
{
    public static MarkupString ToMarkup(this string? me) =>
       me is null ? new MarkupString(string.Empty) : new MarkupString(me);

    public static bool HasText(this string? value, int minLenght = 1) =>
        ! string.IsNullOrWhiteSpace(value) && value.Length >= minLenght;  
}
