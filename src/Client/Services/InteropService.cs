namespace Tellurian.Trains.MeetingApp.Client.Services;

public class InteropService(IJSRuntime runtime)
{
    public IJSRuntime Runtime { get; } = runtime;

    public async ValueTask<WindowDimensions> GetDimensionsAsync() => await Runtime.InvokeAsync<WindowDimensions>("getDimensions");

}

public class WindowDimensions
{
    public int Width { get; set; }
    public int Height { get; set; }
}
