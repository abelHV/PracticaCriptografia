using ClientCriptografia;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientCriptografia
{
    public class ClWebSocketClient
    {
        private ClientWebSocket _ws;
        public event Action<string> OnLog;
        public event Action<ClPacket> OnPacketReceived;
        public event Action OnDisconnected;

        public bool IsConnected => _ws != null && _ws.State == WebSocketState.Open;

        public async Task ConnectAsync(string ip, int port)
        {
            _ws = new ClientWebSocket();

            // Ignorem la validació estricta de firmes del certificat per treballar amb localhost
            _ws.Options.RemoteCertificateValidationCallback = (sender, cert, chain, errors) => true;

            Uri uri = new Uri($"wss://{ip}:{port}/");
            OnLog?.Invoke($"Connectant a {uri} ...");

            await _ws.ConnectAsync(uri, CancellationToken.None);
            OnLog?.Invoke("Connexió establerta de forma segura amb l'extrem remot.");

            // Iniciem el fil de recepció de paquets en segon pla
            _ = Task.Run(() => EscoltatDeXarxaAsync());
        }

        public async Task EnviarPaquetAsync(ClPacket paquet)
        {
            if (!IsConnected) return;

            string json = paquet.ToJson();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task EscoltatDeXarxaAsync()
        {
            byte[] chunk = new byte[4096];

            try
            {
                while (_ws.State == WebSocketState.Open)
                {
                    List<byte> buffer = new List<byte>();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(chunk), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close) break;

                        for (int i = 0; i < result.Count; i++) buffer.Add(chunk[i]);
                    }
                    while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        OnLog?.Invoke("El remot ha sol·licitat tancar la connexió.");
                        break;
                    }

                    string jsonRaw = Encoding.UTF8.GetString(buffer.ToArray());
                    ClPacket paquet = ClPacket.FromJson(jsonRaw);
                    OnPacketReceived?.Invoke(paquet);
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Desconnexió o error de xarxa: {ex.Message}");
            }
            finally
            {
                await DisconnectAsync();
                OnDisconnected?.Invoke();
            }
        }

        public async Task DisconnectAsync()
        {
            if (_ws != null)
            {
                try
                {
                    if (_ws.State == WebSocketState.Open)
                    {
                        await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Tancant", CancellationToken.None);
                    }
                }
                catch { }
                finally
                {
                    _ws.Dispose();
                    _ws = null;
                }
            }
        }
    }
}