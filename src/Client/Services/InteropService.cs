namespace Tellurian.Trains.MeetingApp.Client.Services;

public class InteropService
{
    public InteropService(IJSRuntime runtime)
    {
        Runtime = runtime;
    }

    public IJSRuntime Runtime { get; }

    public async ValueTask<WindowDimensions> GetDimensionsAsync() => await Runtime.InvokeAsync<WindowDimensions>("getDimensions");

}

public class WindowDimensions
{
    public int Width { get; set; }
    public int Height { get; set; }
}
