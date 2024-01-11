namespace Tellurian.Trains.MeetingApp.Contracts;

public class LanguageUtility
{
    public static IEnumerable<CultureInfo> Cultures => SupportedCultures;

    public static readonly CultureInfo[] SupportedCultures =
    [
        new CultureInfo("en"),
        new CultureInfo("cs"),
        new CultureInfo("da"),
        new CultureInfo("de"),
        new CultureInfo("fi"),
        new CultureInfo("fr"),
        new CultureInfo("hu"),
        new CultureInfo("it"),
        new CultureInfo("nb"),
        new CultureInfo("nl"),
        new CultureInfo("nn"),
        new CultureInfo("pl"),
        new CultureInfo("sk"),
        new CultureInfo("sv"),
    ];

    public static string[] Languages => SupportedCultures.Select(c => c.TwoLetterISOLanguageName).ToArray();
    public static readonly string DefaultLanguage = SupportedCultures[0].TwoLetterISOLanguageName;
}