namespace Tellurian.Trains.MeetingApp.Contracts.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string me) =>
        !string.IsNullOrWhiteSpace(me);

    public static TimeSpan? AsTimeSpanOrNull(this string time) =>
        TimeSpan.TryParse(time, out var value) ? value : (TimeSpan?)null;


    public static string Random(this string characters, int length)
    {
        var random = new Random();
        if (characters.Length == 0) return string.Empty;
        var text = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            var c = characters[random.Next(0, characters.Length - 1)];
            text.Append(c);
        }
        return text.ToString();
    }
}
