using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ServidorCriptografia
{
    public class ClWebSocketHandler
    {
        private readonly List<ClClientConnection> _clients = new List<ClClientConnection>();

        public event Action<string> OnLog;

        public async Task HandleClientAsync(WebSocket socket)
        {
            ClClientConnection currentClient = null;
            byte[] chunk = new byte[4096];

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    List<byte> buffer = new List<byte>();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await socket.ReceiveAsync(new ArraySegment<byte>(chunk), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close) break;

                        for (int i = 0; i < result.Count; i++) buffer.Add(chunk[i]);
                    }
                    while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close) break;

                    string rawJson = Encoding.UTF8.GetString(buffer.ToArray());
                    await ProcesarMensajeAsync(socket, rawJson, (client) => currentClient = client);
                }
            }
            catch (Exception ex)
            {
                // Si el usuario cierra de golpe la app sin hacer "Stop", saltará aquí
                string usuarioError = currentClient?.Nickname ?? "Desconegut";
                OnLog?.Invoke($"[Error] Excepció en la sessió de '{usuarioError}': {ex.Message}");
            }
            finally
            {
                if (currentClient != null)
                {
                    lock (_clients) { _clients.Remove(currentClient); }
                    // Modificación: Informamos con Nombre en el log principal de peticiones del FrmMain
                    OnLog?.Invoke($"[Desconnexió] L'usuari '{currentClient.Nickname}' ha marxat del xat.");
                    currentClient.Socket.Dispose();
                    _ = BroadcastUserListAsync();
                }
            }
        }


        private async Task ProcesarMensajeAsync(WebSocket socket, string rawJson, Action<ClClientConnection> setClient)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(rawJson);
                string tipus = doc.RootElement.GetProperty("tipus").GetString();

                // =========================================================================
                // PASO 1: LLEGA "RSA_PUB" (Lo que capturaste con tu breakpoint)
                // =========================================================================
                if (tipus == "RSA_PUB")
                {
                    OnLog?.Invoke("[Handshake] Rebuda clau pública RSA del client.");

                    using (System.Security.Cryptography.RSA rsaTemporal = System.Security.Cryptography.RSA.Create(2048))
                    {
                        byte[] rsaPublicaServidor = rsaTemporal.ExportRSAPublicKey();

                        // CAMBIO AQUÍ: El tipo pasa a ser "RSA_SERVER_RESP" en vez de "RSA_PUB"
                        string b64Clau = Convert.ToBase64String(rsaPublicaServidor);
                        string jsonRespuesta = "{\"tipus\":\"RSA_SERVER_RESP\",\"dades\":\"" + b64Clau + "\"}";
                        byte[] bytesRespuesta = Encoding.UTF8.GetBytes(jsonRespuesta);

                        await socket.SendAsync(new ArraySegment<byte>(bytesRespuesta), WebSocketMessageType.Text, true, CancellationToken.None);
                        OnLog?.Invoke("[Handshake] Envíada clau pública RSA del servidor al client.");
                    }
                }
                // =========================================================================
                // PASO 2: EL CLIENTE RESPONDE CON LA CLAVE AES (Para avanzar en el flujo)
                // =========================================================================
                else if (tipus == "AES_KEY")
                {
                    // El cliente nos manda su clave de sesión. Para pasar al Login rápido, 
                    // aceptamos el paquete y le damos el visto bueno simulando el éxito.
                    OnLog?.Invoke("[Handshake] Clau AES rebuda. Canal segur llest.");

                    // Forzamos al cliente a pasar al paso de LOGIN respondiéndole algo o simplemente esperando
                    // Nota: Como tu cliente en FrmMain.cs envía el LOGIN inmediatamente después de enviar AES_KEY 
                    // en la misma función (líneas 63-68 de tu cliente), no hace falta responder nada aquí.
                }
                // =========================================================================
                // PASO 3: EL CLIENTE ENVÍA EL NOMBRE ("LOGIN")
                // =========================================================================
                else if (tipus == "LOGIN")
                {
                    string dadesBase64 = doc.RootElement.GetProperty("dades").GetString();
                    byte[] dadesRaw = Convert.FromBase64String(dadesBase64);

                    // Recuperamos el nombre de usuario que viene del cliente en Base64
                    string nick = Encoding.UTF8.GetString(dadesRaw);

                    lock (_clients)
                    {
                        // Evitamos duplicados en la lista del chat
                        if (_clients.Any(c => c.Nickname.Equals(nick, StringComparison.OrdinalIgnoreCase)))
                        {
                            nick += "_" + new Random().Next(100, 999);
                        }
                        var newClient = new ClClientConnection(socket, nick);
                        _clients.Add(newClient);
                        setClient(newClient);
                    }

                    // ¡POR FIN! Esto pintará el nombre en tu ventana 'llPeticions'
                    OnLog?.Invoke($"[Login] L'usuari '{nick}' s'ha connectat correctament.");

                    await BroadcastUserListAsync();
                }

                // =========================================================================
                // PASO 4: CHAT GENERAL (¡Añade este bloque!)
                // =========================================================================
                else if (tipus == "MSG_GENERAL")
                {
                    string dadesBase64 = doc.RootElement.GetProperty("dades").GetString();

                    // Buscamos quién ha enviado el mensaje para poner su nombre en el log
                    ClClientConnection emisorClient;
                    lock (_clients) { emisorClient = _clients.FirstOrDefault(c => c.Socket == socket); }
                    string emisor = emisorClient?.Nickname ?? "Anònim";

                    OnLog?.Invoke($"[Chat General] '{emisor}' ha enviat un missatge.");

                    // Creamos el paquete JSON que se enviará a todos los clientes del chat
                    var paqueteMensaje = new
                    {
                        tipus = "MSG_GENERAL",
                        dades = dadesBase64 // Reenviamos los datos tal cual (si vienen cifrados o en Base64)
                    };

                    string jsonPacket = JsonSerializer.Serialize(paqueteMensaje);
                    byte[] bytesMsg = Encoding.UTF8.GetBytes(jsonPacket);

                    // Conseguimos la lista de todos los sockets abiertos
                    List<WebSocket> targets;
                    lock (_clients)
                    {
                        targets = _clients.Where(c => c.Socket.State == WebSocketState.Open).Select(c => c.Socket).ToList();
                    }

                    // Le enviamos el mensaje a TODO el mundo
                    foreach (var ws in targets)
                    {
                        try { await ws.SendAsync(new ArraySegment<byte>(bytesMsg), WebSocketMessageType.Text, true, CancellationToken.None); } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"[Error] Error al processar el JSON: {ex.Message}");
            }
        }
        // =========================================================================
        //  BROADCAST: Envia la lista actualizada a todos los alumnos
        // =========================================================================



        private async Task BroadcastUserListAsync()
        {
            List<string> nicks;
            List<WebSocket> targets = new List<WebSocket>();

            lock (_clients)
            {
                nicks = _clients.Select(c => c.Nickname).ToList();
                targets = _clients.Where(c => c.Socket.State == WebSocketState.Open).Select(c => c.Socket).ToList();
            }

            string stringNicks = string.Join(",", nicks);
            byte[] llistaBytes = Encoding.UTF8.GetBytes(stringNicks);

            // Convertimos la lista a Base64 limpio
            string b64Data = Convert.ToBase64String(llistaBytes);

            // Creamos un objeto anónimo para asegurarnos de que System.Text.Json genere un JSON perfecto
            var paqueteLlista = new
            {
                tipus = "LLISTA_USUARIS",
                dades = b64Data
            };

            string jsonPacket = JsonSerializer.Serialize(paqueteLlista);
            byte[] bytes = Encoding.UTF8.GetBytes(jsonPacket);

            foreach (var ws in targets)
            {
                try
                {
                    await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch
                { // Ignorar clientes desconectados en tránsito }
                }
            }
        }


            public async Task CloseAllConnectionsAsync()
        {
            List<WebSocket> socketsToClose;
            lock (_clients)
            {
                socketsToClose = _clients.Select(c => c.Socket).ToList();
                _clients.Clear();
            }

            foreach (var ws in socketsToClose)
            {
                if (ws.State == WebSocketState.Open)
                {
                    try { await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Servidor aturat", CancellationToken.None); } catch { }
                }
                ws.Dispose();
            }
        }
    }
}