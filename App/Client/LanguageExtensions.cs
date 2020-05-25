using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

#pragma warning disable CA1308 // Normalize strings to uppercase

namespace Tellurian.Trains.MeetingApp.Client
{
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
                 { Language.Dutch, "nl" }
             };
        public static string ToLower(this LocalizedString me) => me?.Value?.ToLowerInvariant() ?? string.Empty;
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
        Dutch
    }
}
