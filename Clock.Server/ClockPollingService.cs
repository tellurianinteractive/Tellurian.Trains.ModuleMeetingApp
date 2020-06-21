using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tellurian.Trains.Clocks.Server
{
    internal class ClockPollingService : IDisposable
    {
        public ClockPollingService(PollingOptions options, ClockServer server)
        {
            Options = options;
            Server = server;
        }
        private readonly PollingOptions Options;
        private readonly ClockServer Server;
        private TcpListener? Listener;
        private Task? TcpListenerTask;
        private readonly UTF8Encoding Encoding = new UTF8Encoding();

        public void TryStartPolling()
        {
            if (Options.IsEnabled)
            {
                Listener = new TcpListener(ClockServer.GetLocalIPAddress(), Options.PortNumber);
                TcpListenerTask = Task.Factory.StartNew(() => ProcessTcpRequests(), System.Threading.CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public void StopPolling()
        {
            if (Listener == null) return;
            Listener.Stop();
            Listener = null;
            try
            {
                //TcpListenerTask?.GetAwaiter().GetResult();
            }
            finally
            {
            }
        }

        private void ProcessTcpRequests()
        {
            if (Listener == null) return;
            Listener.Start();
            while (true)
            {
                try
                {
                    using var client = Listener.AcceptTcpClient();
                    var bytes = Encoding.GetBytes(Server.TcpMessage);
                    var stream = client.GetStream();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    client.Close();
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    TcpListenerTask?.Wait();
                    TcpListenerTask?.Dispose();
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
    }
}
