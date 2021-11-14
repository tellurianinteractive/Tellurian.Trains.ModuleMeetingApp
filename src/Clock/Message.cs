namespace Tellurian.Trains.MeetingApp.Clocks;

#pragma warning disable IDE0060 // Remove unused parameter: Skall vara denna signatur för framtida bruk.

public class Message
{
    public string DefaultText { get; set; } = string.Empty;

    public string ToString(CultureInfo culture) => DefaultText ?? string.Empty;
    public override string ToString() => DefaultText;
}
