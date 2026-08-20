using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ServidorCriptografia
{
    public class ClCryptoService
    {
        public bool ValidarCertificado(string certPath, string password, out string error)
        {
            error = string.Empty;

            if (!File.Exists(certPath))
            {
                error = $"No s'ha trobat el fitxer de certificat a: {certPath}";
                return false;
            }

            try
            {
                using var cert = new X509Certificate2(certPath, password);
                return true;
            }
            catch (Exception ex)
            {
                error = $"Error al carregar el certificat (Contrasenya incorrecta?): {ex.Message}";
                return false;
            }
        }
    }
}