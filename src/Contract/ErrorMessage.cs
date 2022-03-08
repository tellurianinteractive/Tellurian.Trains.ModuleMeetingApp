namespace Tellurian.Trains.MeetingApp.Contracts;

public record ErrorMessage(HttpStatusCode StatusCode, string DocumentationLink, string ErrorCode, params string[] Messages);
