namespace Tellurian.Trains.MeetingApp.Contracts.Extensions;

public static class LanguageExtensions
{
    public static bool IsLanguage(this string languague) => CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals(languague, StringComparison.OrdinalIgnoreCase);

    public static bool IsLanguage(this Language language) => LanguageToTwoLetterISOLanguageNameMap[language].IsLanguage();

    public static bool IsDefaultLanguage => DefaultLanguage.IsLanguage();
    public static string DefaultLanguage => "en";

    private static IDictionary<Language, string> LanguageToTwoLetterISOLanguageNameMap =>
         new Dictionary<Language, string>() {
                 { Language.Default, DefaultLanguage},
                 { Language.English, "en" },
                 { Language.Swedish, "sv" },
                 { Language.Danish, "da" },
                 { Language.Norwegian, "no" },
                 { Language.German, "de" },
                 { Language.Polish, "pl" },
                 { Language.Dutch, "nl" },
                 { Language.Czech, "cs" },
                 { Language.Slovenian, "sl" }
         };
    public static string ToLower(this LocalizedString me) => me?.Value?.ToLowerInvariant() ?? string.Empty;

    public async static Task<string> GetMarkdownAsync(this CultureInfo culture, string path, string pageName)
    {
        var specificCutureFileName = $"{path}/{pageName}.{culture.TwoLetterISOLanguageName}.md";
        if (File.Exists(specificCutureFileName)) return await File.ReadAllTextAsync(specificCutureFileName);
        var defaultCultureFileName = $"{path}/{pageName}.md";
        if (File.Exists(defaultCultureFileName)) return await File.ReadAllTextAsync(defaultCultureFileName);
        return string.Empty;
    }
}

public enum Language
{
    Default,
    English,
    Swedish,
    Danish,
    Norwegian,
    German,
    Polish,
    Dutch,
    Czech,
    Slovenian
}
