using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Manager
{
    public static partial class CertificateManager
    {
        /// <summary>
        /// Generate serial number for new certificates
        /// </summary>
        /// <returns></returns>
        private static byte[] GenerateSerialNumber()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToByteArray();
        }

        /// <summary>
        /// Return the certificate from remote location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static X509Certificate2? GetCertificateFromHost(string location)
        {
            try
            {
                using (TcpClient tcpClient = new TcpClient(location, 443))
                {
                    using (SslStream sslStream = new SslStream(tcpClient.GetStream(), false, null))
                    {
                        sslStream.AuthenticateAsClient(location);

                        var cert = sslStream.RemoteCertificate;

                        if (cert == null)
                            return null;

                        return new X509Certificate2(cert.GetRawCertData());
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        
        /// <summary>
        /// Printa certificado junto com sua cadeia de certificados
        /// </summary>
        /// <param name="certificate"></param>
        public static void PrintCertificateChain(X509Certificate2 certificate)
        {
            X509Chain chain = new X509Chain();

            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            if (chain.Build(certificate))
            {
                foreach (X509ChainElement chainElement in chain.ChainElements)
                {
                    Console.WriteLine("----------------------------------");
                    PrintCertificate(chainElement.Certificate);
                }
            }
            else
            {
                Console.WriteLine("Chain building failed.");
            }
        }

        /// <summary>
        /// Printa infos do certificado
        /// </summary>
        /// <param name="certificate"></param>
        public static void PrintCertificate(X509Certificate2 certificate)
        {
            Console.WriteLine("Main Certificate:");
            Console.WriteLine($"Version: {certificate.Version}");
            Console.WriteLine($"Serial Number: {certificate.SerialNumber}");
            Console.WriteLine($"Signature Algorithm: {certificate.SignatureAlgorithm.FriendlyName}");

            Console.WriteLine($"Not Before: {certificate.NotBefore}");
            Console.WriteLine($"Not After: {certificate.NotAfter}");

            Console.WriteLine($"Public Key Algorithm: {certificate.PublicKey.Oid.FriendlyName}");
            Console.WriteLine($"Public Key Format: {certificate.PublicKey.EncodedKeyValue.Format(true)}");
            Console.WriteLine($"Public Key: {Convert.ToBase64String(certificate.PublicKey.EncodedKeyValue.RawData)}");

            Console.WriteLine($"Has Private Key: {certificate.HasPrivateKey}");
           
            Console.WriteLine($"Issuer: {certificate.Issuer}");
            Console.WriteLine($"Subject: {certificate.Subject}");

            Console.WriteLine($"Thumbprint: {certificate.Thumbprint}");
            Console.WriteLine($"Serial Number: {certificate.SerialNumber}");
        }
    }
}
