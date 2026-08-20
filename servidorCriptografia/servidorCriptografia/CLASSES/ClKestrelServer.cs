using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ServidorCriptografia
{
    public class ClKestrelServer
    {
        private WebApplication _app;
        private readonly ClWebSocketHandler _wsHandler;

        public event Action<string> OnLog;
        public event Action<string> OnStatusChange;

        public ClKestrelServer(ClWebSocketHandler wsHandler)
        {
            _wsHandler = wsHandler;
        }

        public async Task StartAsync(int port, string certPath, string certPassword)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(port, listenOptions =>
                {
                    listenOptions.UseHttps(certPath, certPassword);
                });
            });

            _app = builder.Build();
            _app.UseWebSockets();

            _app.Map("/", async context =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("ERROR: No és una petició de WebSocket.");
                }
                else
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    string clientIP = context.Connection.RemoteIpAddress?.ToString() ?? "desconeguda";
                    OnLog?.Invoke($"[Connexió] Nova petició des de l'IP {clientIP}");

                    await _wsHandler.HandleClientAsync(webSocket);
                }
            });

            _ = _app.RunAsync();

            OnStatusChange?.Invoke($"Servidor actiu (Port {port})");
            OnLog?.Invoke($"[Sistema] Servidor Kestrel iniciat correctament.");
        }

        public async Task StopAsync()
        {
            if (_app != null)
            {
                await _wsHandler.CloseAllConnectionsAsync();
                await _app.StopAsync();
                await _app.DisposeAsync();
                _app = null;

                OnStatusChange?.Invoke("Servidor aturat");
                OnLog?.Invoke("[Sistema] Servidor Kestrel completament aturat.");
            }
        }
    }
}