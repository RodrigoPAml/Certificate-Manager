using System.Security.Cryptography.X509Certificates;

namespace CertificateManager
{
    class Program
    {
        static void Main()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;

            // Create a root certificate (aka our CA)
            var (rootCertificate, rootPrivateKey) = CertificateManager.CreateSelfSignedCertificate(
                "CN=rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, ST=RS, C=BR", 
                DateTimeOffset.Now,
                DateTimeOffset.Now.AddYears(1),
                new List<X509Extension>()
                {
                    new X509BasicConstraintsExtension(true, false, 0, true),
                    new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign, false)
                }
            );

            // Save root certificate
            CertificateManager.WriteCertificate(rootCertificate, rootPrivateKey, "root", projectPath);

            // Print root certificate
            CertificateManager.PrintCertificate(rootCertificate);

            // ******* Create children certificate emited by root certificate (CA)
            var (childCertificate, childPrivateKey) = CertificateManager.CreateSignedCertificate(
                rootCertificate,
                rootPrivateKey,
                "CN=child.rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, ST=RS, C=BR",
                DateTimeOffset.Now.AddSeconds(10),
                DateTimeOffset.Now.AddYears(1),
                new List<X509Extension>()
                {
                    new X509BasicConstraintsExtension(true, false, 0, true),
                    new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign, false)
                }
            );

            // Save child certificate
            CertificateManager.WriteCertificate(childCertificate, childPrivateKey, "child", projectPath);

            // Print child certificate
            CertificateManager.PrintCertificate(childCertificate);
        }
    }
}
