using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CertificateManager
{
    public static partial class CertificateManager
    {
        /// <summary>
        /// Load a certificate
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static X509Certificate2 LoadCertificate(string filePath)
        {
            return new X509Certificate2(filePath);
        }

        /// <summary>
        /// Load private key
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static RSA LoadPrivateKey(string filePath)
        {
            return ParsePrivateKeyPem(File.ReadAllText(filePath));  
        }

        /// <summary>
        /// Create a self signed certificate and return it along with private key
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="notBefore"></param>
        /// <param name="notAfter"></param>
        /// <param name="extensions"></param>
        /// <returns></returns>
        public static (X509Certificate2, string) CreateSelfSignedCertificate(
            string subject,
            DateTimeOffset notBefore,
            DateTimeOffset notAfter,
            List<X509Extension> extensions
        )
        {
            using (RSA rsa = RSA.Create(2048))
            {
                CertificateRequest certificateRequest = new CertificateRequest(subject, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                extensions.ForEach(x => certificateRequest.CertificateExtensions.Add(x));

                var certificate = certificateRequest.CreateSelfSigned(notBefore, notAfter);
                var privateKeyPem = FormatPrivateKeyPem(rsa);

                var cleanCert = new X509Certificate2(certificate.Export(X509ContentType.Cert));
                return (cleanCert, privateKeyPem);
            }
        }

        /// <summary>
        /// Create child certificate signed by a root certificate
        /// </summary>
        /// <param name="rootCertificate"></param>
        /// <param name="rootPrivateKeyPem"></param>
        /// <param name="subject"></param>
        /// <param name="notBefore"></param>
        /// <param name="notAfter"></param>
        /// <param name="extensions"></param>
        /// <returns></returns>
        public static (X509Certificate2, string) CreateSignedCertificate(
            X509Certificate2 rootCertificate,
            string rootPrivateKeyPem,
            string subject,
            DateTimeOffset notBefore,
            DateTimeOffset notAfter,
            List<X509Extension> extensions
        )
        {
            RSA rootPrivateKey = ParsePrivateKeyPem(rootPrivateKeyPem);
            rootCertificate = rootCertificate.CopyWithPrivateKey(rootPrivateKey);

            using (RSA rsa = RSA.Create())
            {
                var request = new CertificateRequest(
                    subject,
                    rsa,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                );

                extensions.ForEach(x => request.CertificateExtensions.Add(x));

                X509Certificate2 certificate = request.Create(
                    rootCertificate,
                    notBefore,
                    notAfter,
                    GenerateSerialNumber()
                );

                var cleanCert = new X509Certificate2(certificate.Export(X509ContentType.Cert));
                var privateKeyPem = FormatPrivateKeyPem(rsa);

                return (cleanCert, privateKeyPem);
            }
        }

        /// <summary>
        /// Save a .crt file and its private key in .pem format
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="privateKey"></param>
        /// <param name="name"></param>
        /// <param name="outputPath"></param>
        public static void WriteCertificate(
            X509Certificate2 certificate,
            string privateKey, 
            string name,
            string outputPath
        )
        {
            var certificateCrt = FormatCertificateCrt(certificate);
            var privateKeyPem = FormatPrivateKeyPem(ParsePrivateKeyPem(privateKey));

            File.WriteAllText($"{outputPath}{name}_key.pem", privateKeyPem);
            File.WriteAllText($"{outputPath}{name}_cert.crt", certificateCrt);
        }
    }
}
