namespace Tellurian.Trains.MeetingApp.Contract;

public record ErrorMessage(HttpStatusCode StatusCode, string DocumentationLink, string ErrorCode, params string[] Messages);
