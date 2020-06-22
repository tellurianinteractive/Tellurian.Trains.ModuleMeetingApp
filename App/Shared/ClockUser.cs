using System;
using System.Net;

namespace Tellurian.Trains.MeetingApp.Shared
{
    public class ClockUser
    {
        public string IPAddress { get; set; } = string.Empty;
        public string? UserName { get; set; } = string.Empty;
        public string LastUsedTime { get; set; } = string.Empty;
    }
}
