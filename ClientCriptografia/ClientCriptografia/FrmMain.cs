using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Windows.Forms;

namespace ClientCriptografia
{
    public partial class FrmMain : Form
    {

        private readonly ClCryptoService _cryptoService;
        private readonly ClWebSocketClient _webSocketClient;

        public FrmMain()
        {
            InitializeComponent();

            _cryptoService = new ClCryptoService();
            _webSocketClient = new ClWebSocketClient();

            // Subscripció de fluxos mitjançant esdeveniments (Events)
            _webSocketClient.OnLog += InserirLog;
            _webSocketClient.OnPacketReceived += ProcesarPaquetEntrant;
            _webSocketClient.OnDisconnected += ResetInterficieGrafica;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ActivarZonaXat(false);
        }

        private async void btParlar_Click(object sender, EventArgs e)
        {
            // Validación previa: El campo de Nombre no puede estar vacío
            if (string.IsNullOrWhiteSpace(tbNom.Text))
            {
                MessageBox.Show("Siusplau, introdueix el teu nom (Nickname) abans de connectar.", "Avís", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IPAddress.TryParse(tbIpremota.Text.Trim(), out IPAddress ipRemota))
            {
                MessageBox.Show("L'adreça IP introduïda no és vàlida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btParlar.Enabled = false;
            tbNom.Enabled = false; // Bloqueamos el TextBox para que no se cambie el nombre a mitad de sesión

            try
            {
                int portServidor = (int)nupPortParlar.Value;

                // Conectamos al Servidor Central a través de WSS (WebSocket Seguro)
                await _webSocketClient.ConnectAsync(ipRemota.ToString(), portServidor);

                // =========================================================================
                //  PROTOCOL DE HANDSHAKE - FASE 1
                //  Enviamos nuestra clave pública RSA propia para que el servidor la reciba.
                // =========================================================================
                byte[] rsaPublicaPropia = _cryptoService.ExportarClauPublicaRSA();
                ClPacket packPublico = new ClPacket("RSA_PUB", rsaPublicaPropia);

                await _webSocketClient.EnviarPaquetAsync(packPublico);

                InserirLog("[Handshake] Clau pública RSA pròpia enviada al servidor central.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al connectar amb el servidor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btParlar.Enabled = true;
                tbNom.Enabled = true;
            }
        }

        private void ProcesarPaquetEntrant(ClPacket paquet)
        {
            Invoke((MethodInvoker)(async () =>
            {
                byte[] dadesRaw = null;
                try
                {
                    if (!string.IsNullOrEmpty(paquet.DadesBase64))
                    {
                        dadesRaw = Convert.FromBase64String(paquet.DadesBase64);
                    }
                    else
                    {
                        dadesRaw = Array.Empty<byte>();
                    }
                }
                catch (FormatException)
                {
                    InserirLog("[Error] S'ha rebut un paquet amb dades Base64 corruptes.");
                    return;
                }

                // =========================================================================
                //  FASE INTERCAMBIO: ESCUCHAMOS LA RESPUESTA EXCLUSIVA DEL SERVIDOR
                // =========================================================================
                if (paquet.Tipus == "RSA_SERVER_RESP")
                {
                    // El servidor nos responde enviando SU clave pública RSA
                    _cryptoService.ImportarClauPublicaRemota(dadesRaw);
                    InserirLog("[Handshake] S'ha rebut la clau pública RSA del servidor.");

                    // Generamos la clave de sesión AES definitiva
                    byte[] clauIvCombinadaAES = _cryptoService.GenerarClauIVCombinadaAES();

                    // Ciframos la clave AES usando la clave pública RSA del servidor
                    byte[] combinadaXifrada = _cryptoService.XifrarAmbRSARemota(clauIvCombinadaAES);

                    // Enviamos la clave simétrica protegida al servidor
                    ClPacket respuestaAES = new ClPacket("AES_KEY", combinadaXifrada);
                    await _webSocketClient.EnviarPaquetAsync(respuestaAES);
                    InserirLog("[Handshake] Clau de sessió AES enviada de forma segura.");

                    // REGISTRO DE NICKNAME (FASE 3)
                    byte[] nickBytes = Encoding.UTF8.GetBytes(tbNom.Text.Trim());
                    ClPacket paquetRegistre = new ClPacket("LOGIN", nickBytes);
                    await _webSocketClient.EnviarPaquetAsync(paquetRegistre);

                    InserirLog($"[Registre] S'ha enviat el Nickname '{tbNom.Text}' al servidor central.");
                    ActivarZonaXat(true);
                }
                // =========================================================================
                //  FASE LLISTA: RECIBIR LISTA ACTUALIZADA DE USUARIOS
                // =========================================================================
                else if (paquet.Tipus == "LLISTA_USUARIS")
                {
                    string usuariosConectados = Encoding.UTF8.GetString(dadesRaw);
                    InserirLog($"[Actualització] Usuaris en línia: {usuariosConectados}");

                    // Aquí puedes volcar los nicks si tienes un ListBox/ListView para usuarios:
                    // lbUsuaris.Items.Clear();
                    // foreach(var user in usuariosConectados.Split(',')) { lbUsuaris.Items.Add(user); }
                }
                else if (paquet.Tipus == "MSG_GENERAL")
                {
                    // Si tus mensajes van cifrados en AES, primero tendrías que pasarle el _cryptoService.DesxifrarAES(dadesRaw)
                    // Si de momento los pruebas en texto plano (Base64), lo decodificas así:
                    string textoMensaje = Encoding.UTF8.GetString(dadesRaw);

                    // Lo pintas en la caja de texto grande del chat
                    InserirMissatgePantalla(textoMensaje);
                }
            }));
        }
        private async void btEnviar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbEnviar.Text))
            {
                return;
            }

            try
            {
                // 2. Preparamos los bytes del mensaje juntando el Nombre de usuario y el texto
                byte[] msgBytes = Encoding.UTF8.GetBytes($"{tbNom.Text}: {tbEnviar.Text}");

                // 3. Empaquetamos bajo la etiqueta "MSG_GENERAL"
                ClPacket paquetGeneral = new ClPacket("MSG_GENERAL", msgBytes);

                // 4. Lo enviamos de forma asíncrona al servidor a través del WebSocket
                await _webSocketClient.EnviarPaquetAsync(paquetGeneral);

                // 5. Limpiamos la caja de texto para que el usuario pueda escribir el siguiente mensaje
                tbEnviar.Text = "";
            }
            catch (Exception ex)
            {
                InserirLog($"[Error] No s'ha pogut enviar el missatge: {ex.Message}");
            }
        }

        private async void btStop_Click(object sender, EventArgs e)
        {
            await _webSocketClient.DisconnectAsync();
        }

        private void ActivarZonaXat(bool activa)
        {
            btEnviar.Enabled = activa;
            tbEnviar.Enabled = activa;
            lbPersonas.Enabled = activa;
            btStop.Visible = activa;
        }

        private void ResetInterficieGrafica()
        {
            _cryptoService.NetejarClaus();

            btParlar.Enabled = true;
            tbNom.Enabled = true; // Volvemos a permitir editar el nombre al desconectar

            lbPersonas.Items.Clear(); // Vaciamos la lista de personas ya que no estamos en el servidor
            ActivarZonaXat(false);

            InserirLog("S'ha tancat la connexió amb el servidor. Comunicacions netejades.");
        }

        private void InserirLog(string mensaje)
        {
            lbLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {mensaje}");
            lbLog.SelectedIndex = lbLog.Items.Count - 1;
        }

        private void InserirMissatgePantalla(string textComplet)
        {
            tbRebre.AppendText($"[{DateTime.Now:HH:mm:ss}] {textComplet}\r\n");
            tbRebre.SelectionStart = tbRebre.Text.Length;
            tbRebre.ScrollToCaret();
        }

       
    }
}
