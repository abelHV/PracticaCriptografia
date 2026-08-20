using System.Net.WebSockets;

namespace ServidorCriptografia
{
    public class ClClientConnection
    {
        public WebSocket Socket { get; set; }
        public string Nickname { get; set; }

        public ClClientConnection(WebSocket socket, string nickname)
        {
            Socket = socket;
            Nickname = nickname;
        }
    }
}