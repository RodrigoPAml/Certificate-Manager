using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertificateManager
{
    public static partial class CertificateManager
    {
        /// <summary>
        /// Format the private key of RSA to PEM format
        /// </summary>
        /// <param name="rsa"></param>
        /// <returns></returns>
        private static string FormatPrivateKeyPem(RSA rsa)
        {
            var privateKeyBytes = rsa.ExportRSAPrivateKey();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("-----BEGIN PRIVATE KEY-----");
            builder.AppendLine(Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END PRIVATE KEY-----");
            return builder.ToString();
        }

        /// <summary>
        /// Format the certificate to PEM format (remove the private key)
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        private static string FormatCertificateCrt(X509Certificate2 certificate)
        {
            var certificateBytes = certificate.Export(X509ContentType.Cert);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(certificateBytes, Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");
            return builder.ToString();
        }
           
        /// <summary>
        /// Open certificate in PEM format (.crt)
        /// </summary>
        /// <param name="certificatePem"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static X509Certificate2 ParseCertificateCrt(string certificatePem)
        {
            const string beginMarker = "-----BEGIN CERTIFICATE-----";
            const string endMarker = "-----END CERTIFICATE-----";

            if (!certificatePem.Contains(beginMarker) || !certificatePem.Contains(endMarker))
                throw new ArgumentException("Invalid format for certificate.");

            int startIndex = certificatePem.IndexOf(beginMarker) + beginMarker.Length;
            int endIndex = certificatePem.IndexOf(endMarker, startIndex);

            if (startIndex < 0 || endIndex < 0)
                throw new ArgumentException("Invalid format for certificate.");

            string base64Certificate = certificatePem.Substring(startIndex, endIndex - startIndex).Replace("\r", "").Replace("\n", "");

            byte[] certificateBytes = Convert.FromBase64String(base64Certificate);
            return new X509Certificate2(certificateBytes);
        }

        /// <summary>
        /// Parse a private key in .PEM format
        /// </summary>
        /// <param name="privateKeyPem"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static RSA ParsePrivateKeyPem(string privateKeyPem)
        {
            const string beginMarker = "-----BEGIN PRIVATE KEY-----";
            const string endMarker = "-----END PRIVATE KEY-----";

            if (!privateKeyPem.Contains(beginMarker) || !privateKeyPem.Contains(endMarker))
                throw new ArgumentException("Invalid format for private key.");

            int startIndex = privateKeyPem.IndexOf(beginMarker) + beginMarker.Length;
            int endIndex = privateKeyPem.IndexOf(endMarker, startIndex);

            if (startIndex < 0 || endIndex < 0)
                throw new ArgumentException("Invalid format for private key.");

            string base64PrivateKey = privateKeyPem.Substring(startIndex, endIndex - startIndex).Replace("\r", "").Replace("\n", "");

            byte[] privateKeyBytes = Convert.FromBase64String(base64PrivateKey);

            RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

            return rsa;
        }
    }
}
