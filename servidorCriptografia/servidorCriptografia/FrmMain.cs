using ServidorCriptografia;
using System;
using System.Windows.Forms;

namespace servidorCriptografia
{
    public partial class FrmMain : Form
    {

        private readonly ClCryptoService _cryptoService;
        private readonly ClWebSocketHandler _wsHandler;
        private ClKestrelServer _kestrelServer;

        // Configuración de las rutas del certificado digital
        private string fitxerCertificat = "C:\\work\\certLocalhost.pfx";
        private string pswCertificat = "lamevapassword";
        public FrmMain()
        {
            InitializeComponent();

            _cryptoService = new ClCryptoService();
            _wsHandler = new ClWebSocketHandler();
            _kestrelServer = new ClKestrelServer(_wsHandler);

            _wsHandler.OnLog += InserirLog;
            _kestrelServer.OnLog += InserirLog;
            _kestrelServer.OnStatusChange += CambiarEstat;
        }

        private async void btStart_Click(object sender, EventArgs e)
        {
            if (!_cryptoService.ValidarCertificado(fitxerCertificat, pswCertificat, out string errorMsg))
            {
                MessageBox.Show(errorMsg, "ERROR CERTIFICAT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            llPeticions.Items.Clear();
            btStart.Visible = false;
            btStop.Visible = true;

            int puerto = (int)nupPort.Value;
            try
            {
                await _kestrelServer.StartAsync(puerto, fitxerCertificat, pswCertificat);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No s'ha pogut iniciar el servidor: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btStart.Visible = true;
                btStop.Visible = false;
            }
        }

        private async void btStop_Click(object sender, EventArgs e)
        {
            await _kestrelServer.StopAsync();

            btStop.Visible = false;
            btStart.Visible = true;
        }

        private void InserirLog(string mensaje)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => InserirLog(mensaje)));
            }
            else
            {
                llPeticions.Items.Add($"[{DateTime.Now:HH:mm:ss}] {mensaje}");
                llPeticions.SelectedIndex = llPeticions.Items.Count - 1;
            }
        }

        private void CambiarEstat(string estado)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => CambiarEstat(estado)));
            }
            else
            {
                lbEstat.Text = estado;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
