using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ClientCriptografia
{
    public class ClCryptoService
    {
        private RSA _rsaPropi = RSA.Create(2048); // Clau RSA pròpia
        private RSAParameters? _clauPublicaRemota = null;

        public byte[] ClauAES { get; private set; } = null;
        public byte[] IvAES { get; private set; } = null;

        // --- MÈTODES RSA ---
        public byte[] ExportarClauPublicaRSA() => _rsaPropi.ExportRSAPublicKey();

        public void ImportarClauPublicaRemota(byte[] dadesClau)
        {
            using RSA rsa = RSA.Create();
            rsa.ImportRSAPublicKey(dadesClau, out _);
            _clauPublicaRemota = rsa.ExportParameters(false);
        }

        public byte[] XifrarAmbRSARemota(byte[] dades)
        {
            if (_clauPublicaRemota == null) throw new InvalidOperationException("No es disposa de la clau pública remota.");
            using RSA rsa = RSA.Create();
            rsa.ImportParameters(_clauPublicaRemota.Value);
            return rsa.Encrypt(dades, RSAEncryptionPadding.OaepSHA256);
        }

        public byte[] DesxifrarAmbRSAPropia(byte[] dadesXifrades)
        {
            return _rsaPropi.Decrypt(dadesXifrades, RSAEncryptionPadding.OaepSHA256);
        }

        // --- MÈTODES AES ---
        public byte[] GenerarClauIVCombinadaAES()
        {
            ClauAES = RandomNumberGenerator.GetBytes(32); // AES-256
            IvAES = RandomNumberGenerator.GetBytes(16);  // Bloque de 16 bytes

            byte[] combinada = new byte[48];
            Buffer.BlockCopy(ClauAES, 0, combinada, 0, 32);
            Buffer.BlockCopy(IvAES, 0, combinada, 32, 16);
            return combinada;
        }

        public void EstablirClauIVAES(byte[] combinada)
        {
            if (combinada.Length < 48) throw new ArgumentException("Mida de clau combinada incorrecta.");
            ClauAES = combinada[0..32];
            IvAES = combinada[32..48];
        }

        public byte[] XifrarAES(string text)
        {
            using Aes aes = Aes.Create();
            aes.Key = ClauAES;
            aes.IV = IvAES;
            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(text);
            return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public string DesxifrarAES(byte[] dadesXifrades)
        {
            using Aes aes = Aes.Create();
            aes.Key = ClauAES;
            aes.IV = IvAES;
            using var decryptor = aes.CreateDecryptor();
            byte[] plainBytes = decryptor.TransformFinalBlock(dadesXifrades, 0, dadesXifrades.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }

        public void NetejarClaus()
        {
            ClauAES = null;
            IvAES = null;
            _clauPublicaRemota = null;
        }
    }
}