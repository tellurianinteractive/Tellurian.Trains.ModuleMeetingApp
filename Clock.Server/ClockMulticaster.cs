using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Tellurian.Trains.Clocks.Server
{
    internal class ClockMulticaster : IDisposable
    {
        public ClockMulticaster(MulticastOptions options, ClockServer server)
        {
            Options = options;
            Server = server;
        }

        private readonly MulticastOptions Options;
        private readonly ClockServer Server;
        private IPEndPoint? MulticastEndpoint;
        private Timer? MulticastTimer;
        private UdpClient? Multicaster;
        private readonly UTF8Encoding Encoding = new UTF8Encoding();


        internal void TryStartMulticast()
        {

            if (Options.IsEnabled)
            {
                Multicaster = new UdpClient(Options.LocalPortNumber)
                {
                    EnableBroadcast = true
                };
                MulticastEndpoint = new IPEndPoint(IPAddress.Parse(Options.IPAddress), Options.PortNumber);
                MulticastTimer = new Timer(Options.IntervalSeconds * 1000);
                MulticastTimer.Elapsed += Multicast;
                MulticastTimer.Start();
            }
        }

        internal void StopMulticast()
        {
            MulticastTimer?.Stop();
            Multicaster?.Close();
        }

        private void Multicast(object me, ElapsedEventArgs args)
        {
            if (Multicaster == null || MulticastTimer == null) return;
            try
            {
                var bytes = Encoding.GetBytes(Server.MulticastMessage);
                Multicaster.Send(bytes, bytes.Length, MulticastEndpoint);
            }
            catch (ObjectDisposedException) { MulticastTimer.Stop(); }
            catch (SocketException) { MulticastTimer.Stop(); }
        }

        #region Disposable suppport

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StopMulticast();
                    MulticastTimer?.Dispose();
                    Multicaster?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}
