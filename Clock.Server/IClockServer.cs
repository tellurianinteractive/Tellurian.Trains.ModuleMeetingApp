using System;
using System.Collections.Generic;
using System.Net;

namespace Tellurian.Trains.Clocks.Server
{
    public interface IClockServer
    {
        string AdministratorPassword { get; }
        IEnumerable<ClockUser> ClockUsers { get; }
        TimeSpan Duration { get; set; }
        TimeSpan Elapsed { get; set; }
        TimeSpan? ExpectedResumeTime { get; }
        TimeSpan FastEndTime { get; }
        TimeSpan FastTime { get; }
        bool IsCompleted { get; }
        bool IsPaused { get; }
        bool IsRealtime { get; }
        bool IsRunning { get; }
        DateTimeOffset LastUsedTime { get; }
        string Level1Message { get; }
        string Level2Message { get; }
        ClockMessage Message { get; set; }
        string MulticastMessage { get; }
        string Name { get; }
        PauseReason PauseReason { get; }
        TimeSpan? PauseTime { get; }
        TimeSpan RealEndTime { get; }
        bool ShowRealTimeWhenPaused { get; set; }
        double Speed { get; set; }
        TimeSpan StartDayAndTime { get; set; }
        TimeSpan StartTime { get; }
        string? StoppingUser { get; set; }
        StopReason StopReason { get; set; }
        string TcpMessage { get; }
        TimeSpan Time { get; }
        string UserPassword { get; }
        TimeSpan UtcOffset { get; }
        Weekday Weekday { get; }

        void Dispose();
        void RemoveInactiveUsers(TimeSpan age);
        ClockServer StartServer(ClockSettings settings);
        bool StartTick(string? user, string? password);
        void StopServer();
        void StopTick();
        void StopTick(StopReason reason, string user);
        bool Update(ClockSettings settings);
        bool Update(ClockSettings settings, IPAddress ipAddress, string? userName);
        bool UpdateUser(IPAddress ipAddress, string? userName = "", string? clientVersion = "");
    }
}