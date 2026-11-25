namespace Tellurian.Trains.MeetingApp.Client.Services;

public class InteropService(IJSRuntime runtime)
{
    public IJSRuntime Runtime { get; } = runtime;

    /// <summary>
    /// Gets the browser window dimensions, or null if they cannot be obtained from JavaScript.
    /// </summary>
    public async ValueTask<WindowDimensions?> GetDimensionsAsync()
    {
        try
        {
            return await Runtime.InvokeAsync<WindowDimensions>("getDimensions");
        }
        catch (JSException)
        {
            return null;
        }
    }

}

public class WindowDimensions
{
    public int Width { get; set; }
    public int Height { get; set; }
}
