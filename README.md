# Certificate Manager

This GitHub repository hosts a digital certificate generation tool in .CRT and .PEM format for studies purposes.

Digital certificates play a crucial role in establishing trust and secure communication over networks (aka SSL protocol over TCP or HTTP). 

Whether you're securing web applications, implementing secure email communication, or ensuring the integrity of software updates.

# SSL Context

Digital certificates play a crucial role in ensuring secure communication over the internet, particularly in the context of SSL/TLS (Secure Sockets Layer/Transport Layer Security) protocols. 

* Authentication:
Digital certificates are used to authenticate the identity of entities involved in a communication. 
In the case of SSL/TLS, this primarily involves authenticating the identity of web servers to web browsers (clients).
The digital certificate is issued by a trusted Certificate Authority (CA), which verifies the legitimacy of the server's identity.
Usually the CA Certificate is already pre installed in your computer or broswer.
Since the CA Certificate is already safe in your computer or transported in a safe way to your computer, all the child certificates created
are signed by CA (RSA) and can be verified using the CA certificate.

* Encryption with Key Exchange:
When a client connects to a server secured with SSL/TLS, the server presents its digital certificate to the client.
The client, in turn, checks the certificate's authenticity by verifying its digital signature using the public key of the CA. 
Once the certificate is validated, the client and server exchange encryption keys (usually uses AES), allowing them to establish a secure communication channel.

* Data Integrity:
SSL/TLS uses digital signatures in certificates to ensure the integrity of the transmitted data. 
Digital signatures help verify that the data has not been tampered with during transit (Man in the Middle). 
If the data is altered in any way, the signature verification will fail, alerting both the client and server to potential security threats.

* Trust Hierarchy:
The concept of a Certificate Authority (CA) and the trust hierarchy it establishes is fundamental to SSL/TLS. 
Web browsers and operating systems come pre-installed with a list of trusted CAs. 
When a client encounters a digital certificate, it checks whether it was signed by a trusted CA. 
This trust chain ensures that users can trust the legitimacy of the server's identity.

# Creating the root certificate (Our certification authority)

Code

```C#
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
```

Output 

```
Version: 3
Serial Number: 5E250EE112B3BE1C
Signature Algorithm: sha256RSA
Not Before: 01/02/2024 08:29:23
Not After: 01/02/2025 08:29:23
Public Key Algorithm: RSA
Public Key Format: 30 82 01 0a 02 82 01 01 00 c2 0e 7f 39 b3 23 bb 84 ab 9d 62 2b 2e 32 fb e3 6a d2 32 9d 03 ea 67 d6 0c 07 d3 a1 ef 56 b5 47 ec f2 f0 e5 98 e2 87 34 28 c1 37 10 fc b0 80 ec b8 24 ba 70 fb 01 cd ee af d6 2b d2 b3 cb c3 db 02 2e f5 f3 74 c7 8f 59 35 f3 43 c9 cf 25 31 70 7a bf 93 01 0a 89 40 12 58 84 43 7e 9b 27 c3 dc bc ab 34 f8 98 97 cf 47 41 d1 e9 e3 f9 b1 8a 9b 3d e9 2a 69 9c e8 6e d3 ea b9 64 f5 3a aa 2c 0b f7 ba f5 19 0b c3 05 6f af 66 41 aa 21 38 56 cb b9 51 68 77 0c 00 65 f0 dd 38 6f 72 98 70 d0 c1 7f 34 fc ce 74 c0 3a 6d d2 6e d0 21 77 f7 fd e6 8e 42 52 6f 3a 9b 68 46 c3 09 cc e5 4b ca 2b 74 84 38 67 31 69 03 df 21 11 2a fa 2c 08 c0 52 d3 85 1c 25 06 4c d1 2d e0 70 61 6e 87 71 ea aa 6f 47 19 8b 56 de 7e b6 af 10 ed 49 d7 09 d2 58 99 95 66 37 66 ae 41 36 21 34 4c 7b fa 8e 9a 93 31 02 03 01 00 01
Public Key: MIIBCgKCAQEAwg5/ObMju4SrnWIrLjL742rSMp0D6mfWDAfToe9WtUfs8vDlmOKHNCjBNxD8sIDsuCS6cPsBze6v1ivSs8vD2wIu9fN0x49ZNfNDyc8lMXB6v5MBColAEliEQ36bJ8PcvKs0+JiXz0dB0enj+bGKmz3pKmmc6G7T6rlk9TqqLAv3uvUZC8MFb69mQaohOFbLuVFodwwAZfDdOG9ymHDQwX80/M50wDpt0m7QIXf3/eaOQlJvOptoRsMJzOVLyit0hDhnMWkD3yERKvosCMBS04UcJQZM0S3gcGFuh3Hqqm9HGYtW3n62rxDtSdcJ0liZlWY3Zq5BNiE0THv6jpqTMQIDAQAB
Has Private Key: False
Issuer: CN=rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, S=RS, C=BR
Subject: CN=rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, S=RS, C=BR
Thumbprint: 366A8B0CE3EF00D23F1C7B2DE83F6EDDC787F8D9
Serial Number: 5E250EE112B3BE1C
```

CRT File

```
-----BEGIN CERTIFICATE-----
MIIDjDCCAnSgAwIBAgIIXiUO4RKzvhwwDQYJKoZIhvcNAQELBQAwdTELMAkGA1UEBhMCQlIxCzAJ
BgNVBAgTAlJTMQswCQYDVQQHEwJTTTEbMBkGA1UECxMSUm9kcmlnbyBEZXBhcnRtZW50MR0wGwYD
VQQKExRSb2RyaWdvIE9yZ2FuaXphdGlvbjEQMA4GA1UEAxMHcm9kcmlnbzAeFw0yNDAyMDExMTI5
MjNaFw0yNTAyMDExMTI5MjNaMHUxCzAJBgNVBAYTAkJSMQswCQYDVQQIEwJSUzELMAkGA1UEBxMC
U00xGzAZBgNVBAsTElJvZHJpZ28gRGVwYXJ0bWVudDEdMBsGA1UEChMUUm9kcmlnbyBPcmdhbml6
YXRpb24xEDAOBgNVBAMTB3JvZHJpZ28wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDC
Dn85syO7hKudYisuMvvjatIynQPqZ9YMB9Oh71a1R+zy8OWY4oc0KME3EPywgOy4JLpw+wHN7q/W
K9Kzy8PbAi7183THj1k180PJzyUxcHq/kwEKiUASWIRDfpsnw9y8qzT4mJfPR0HR6eP5sYqbPekq
aZzobtPquWT1OqosC/e69RkLwwVvr2ZBqiE4Vsu5UWh3DABl8N04b3KYcNDBfzT8znTAOm3SbtAh
d/f95o5CUm86m2hGwwnM5UvKK3SEOGcxaQPfIREq+iwIwFLThRwlBkzRLeBwYW6Hceqqb0cZi1be
fravEO1J1wnSWJmVZjdmrkE2ITRMe/qOmpMxAgMBAAGjIDAeMA8GA1UdEwEB/wQFMAMBAf8wCwYD
VR0PBAQDAgEGMA0GCSqGSIb3DQEBCwUAA4IBAQCk8iRbr9TepgoZFeuH2Cu/KAVqDjzVa/rT8QDD
OnB79Xiuxn475WCbwZFchPyf2bRSG8Zmb+H+v1dJrXlcbBaAsdcZZrZso7sOvc/SQGdHR0C0aB0h
cIXUGzDeLIbOxRCuwhYkZDfHnhODm1o86t99b0Tr5QtkTpWbNSmM1lyn5qt+CQYiqu/EawB0XMlN
UKQj8Hm4tu2K5tHq36pZQ17uUPXBnAxUj5I+X9FWa4Dk+hALoqDnXmCsCXpcmePJ0aUZzP8Yt+ld
gTmm5WytWfUxPRyB8GbXU/t1U+KvI0jq5kiExyj5bgfK4JfT5kap7C1T573aqmEY4hKiJBmALVbO
-----END CERTIFICATE-----
```

Private key file

```
-----BEGIN PRIVATE KEY-----
MIIEpAIBAAKCAQEAwg5/ObMju4SrnWIrLjL742rSMp0D6mfWDAfToe9WtUfs8vDlmOKHNCjBNxD8
sIDsuCS6cPsBze6v1ivSs8vD2wIu9fN0x49ZNfNDyc8lMXB6v5MBColAEliEQ36bJ8PcvKs0+JiX
z0dB0enj+bGKmz3pKmmc6G7T6rlk9TqqLAv3uvUZC8MFb69mQaohOFbLuVFodwwAZfDdOG9ymHDQ
wX80/M50wDpt0m7QIXf3/eaOQlJvOptoRsMJzOVLyit0hDhnMWkD3yERKvosCMBS04UcJQZM0S3g
cGFuh3Hqqm9HGYtW3n62rxDtSdcJ0liZlWY3Zq5BNiE0THv6jpqTMQIDAQABAoIBAH5R+4Vu1DLL
sP75GUywiYFHVt71kMf0ocQhECVYbCQDVv3pzDtSMlrEsMmMuECZG/7egbLr4gfQeQu8aSL64Fpf
OHyH5xvEd3kECruCqR5errioE8RtBZUro6Vf89XVzInQFOJCSof2RaVEUE+Iv8ZAbdha+XXgNviT
iGGNt5qFZvMIXNMWIvaqFc9O44tLk4D4wtYqtrEfRlwezbvuvtiOCVvzN1zvj00Jwy1NGJD7m06w
OMLeqaUUTV3JhRqjhl3d3OO1p3WyZ5R7VliHAeeXTUpOQpKNgG4eOtU1VoCkSD+cG9XFIv9srDrl
iz45Sh1tdll0BEKLsMPBYAL7T0UCgYEAwnlf0DlacC9pzKe2n+XQrFd9Fjv4LlZsBbLFYbpaIJMM
bUMpfY5LKZ8fdIvWaPlv4cjpt9vAh01XzgbFymdiotAwAo05ZPMIit8sP8eL7fAaHHA6rfHhoEcI
klTGLtMtRtzoZc9mQq/445e7lfUbm096D8ggfcLyjcljbzi3XisCgYEA/3NPU/mFnt2Tk9ggk/pM
qomPHwU5FJGFN5AAsbuD4UpH3/+cOHb+BVzMkzk3mO4mzScfeU9K2cA5wRZLXasN5PCAXlfUICjI
XUHhKyA438TiebX2VcJ5v6/NkefKt7TYM5ei7U2eNM4A297HTXXjq1sW/uwl2vtPg9Mm09vmwhMC
gYEAm9UsfKromEq16aNeiKRb4S298yocxezEZJCK77Om8sFCTO9reMuj2PdD+lzvc3ClQXSAK67w
iLrmKBZvObA8bqCKSBEoM4c3iigoyfa5XvFtyun3a1kOZiNlb/R0ViMr/cOYibR2iet+ccktLI2b
EInQEoNX3c5wFEOS5RW6gb8CgYANm3PXUkxW2RC6aGHb4BJjZy5zhZCz0siY3BfdX9K3loBboz8M
FvKTEI1pLjwS7mSsuu/HQ30GOmIZMfnTxH/Z+SGtZNpM2D9MEG1RIcdFzDQQLawwh0p69n9pzUIU
JRL7NJGeOGs7/tk23RgavTPiddi0vZmKn6ZtrDq+QFF7dwKBgQC1rkVI8gu9n+66GTK3hI7K/f61
624aWkJXki/DQnxF/8XiwtCwEr/BgQNyGH421qmK8xgaLSS9lgU/8CV5w+xCrjdI3eAMfs/iBXB2
V87yB8C5/89oMKfv56BH18UoOLNWuq5oAK54ZE2PFwvW3V/1tdK9/ZtRRv5sAddnhYnkXg==
-----END PRIVATE KEY-----
```

# Creating the child certificate (Can be use with a website for example)

Code

```C#
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
        new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.NonRepudiation, false)
    }
);

// Save child certificate
CertificateManager.WriteCertificate(childCertificate, childPrivateKey, "child", projectPath);

// Print child certificate
CertificateManager.PrintCertificate(childCertificate);
```

Output 

```
Version: 3
Serial Number: 00CC7B8748E6F645952E005CBA0B783F
Signature Algorithm: sha256RSA
Not Before: 01/02/2024 08:29:33
Not After: 01/02/2025 08:29:23
Public Key Algorithm: RSA
Public Key Format: 30 82 01 0a 02 82 01 01 00 bb cd fd 03 48 65 5b e4 73 55 ff ba 8a 98 91 64 66 a6 e7 85 fc b5 fd f0 ac cd 12 5e 7a da a6 d7 5e ed b9 38 63 65 7c a9 81 02 39 67 76 29 49 b1 9c 93 c6 23 6e 99 9c 71 0a 36 77 a7 2b 30 87 9b 72 bf 9a 0c b0 6b 2c c3 d9 d9 f5 63 04 f0 48 28 fb 3c ac 20 58 b9 84 a1 b9 32 7b 9a a5 8b 5b dc 32 4d 5f 6d 85 64 94 60 94 95 28 6a 3e 29 a2 e0 f6 4d 53 fb 0b a7 99 09 92 5d 69 7f 52 05 09 10 d6 3b bc ac d5 6e 86 db 34 3f 0c 05 3f ba 4f 5e a5 b4 67 72 a6 4f 75 f0 49 69 db ba d2 42 16 8f 79 fa 4e 40 60 1a b6 b8 e7 0b 3f 43 64 c9 50 44 5e 10 76 58 e3 a9 b4 76 df a3 81 f7 2a ed bc 73 d1 ec 55 33 aa c9 ff 01 24 7b 00 98 9c cf 71 4e 0f c3 d4 ed 00 97 28 23 63 44 ac 03 33 a9 93 50 c1 ab 10 b8 2f 8c 5e 85 79 45 bf c0 7a ab ed 6b e8 d9 2e d0 86 17 90 a1 35 5c 8e 65 fd 64 e2 29 02 03 01 00 01
Public Key: MIIBCgKCAQEAu839A0hlW+RzVf+6ipiRZGam54X8tf3wrM0SXnraptde7bk4Y2V8qYECOWd2KUmxnJPGI26ZnHEKNnenKzCHm3K/mgywayzD2dn1YwTwSCj7PKwgWLmEobkye5qli1vcMk1fbYVklGCUlShqPimi4PZNU/sLp5kJkl1pf1IFCRDWO7ys1W6G2zQ/DAU/uk9epbRncqZPdfBJadu60kIWj3n6TkBgGra45ws/Q2TJUEReEHZY46m0dt+jgfcq7bxz0exVM6rJ/wEkewCYnM9xTg/D1O0AlygjY0SsAzOpk1DBqxC4L4xehXlFv8B6q+1r6Nku0IYXkKE1XI5l/WTiKQIDAQAB
Has Private Key: False
Issuer: CN=rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, S=RS, C=BR
Subject: CN=child.rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, S=RS, C=BR
Thumbprint: EA6CE531CEB297FA8D9ECD2E6C8C69FF4D41EB9F
Serial Number: 00CC7B8748E6F645952E005CBA0B783F
```

CTR FILE 

```
-----BEGIN CERTIFICATE-----
MIIDmjCCAoKgAwIBAgIQAMx7h0jm9kWVLgBcugt4PzANBgkqhkiG9w0BAQsFADB1MQswCQYDVQQG
EwJCUjELMAkGA1UECBMCUlMxCzAJBgNVBAcTAlNNMRswGQYDVQQLExJSb2RyaWdvIERlcGFydG1l
bnQxHTAbBgNVBAoTFFJvZHJpZ28gT3JnYW5pemF0aW9uMRAwDgYDVQQDEwdyb2RyaWdvMB4XDTI0
MDIwMTExMjkzM1oXDTI1MDIwMTExMjkyM1owezELMAkGA1UEBhMCQlIxCzAJBgNVBAgTAlJTMQsw
CQYDVQQHEwJTTTEbMBkGA1UECxMSUm9kcmlnbyBEZXBhcnRtZW50MR0wGwYDVQQKExRSb2RyaWdv
IE9yZ2FuaXphdGlvbjEWMBQGA1UEAxMNY2hpbGQucm9kcmlnbzCCASIwDQYJKoZIhvcNAQEBBQAD
ggEPADCCAQoCggEBALvN/QNIZVvkc1X/uoqYkWRmpueF/LX98KzNEl562qbXXu25OGNlfKmBAjln
dilJsZyTxiNumZxxCjZ3pyswh5tyv5oMsGssw9nZ9WME8Ego+zysIFi5hKG5MnuapYtb3DJNX22F
ZJRglJUoaj4pouD2TVP7C6eZCZJdaX9SBQkQ1ju8rNVuhts0PwwFP7pPXqW0Z3KmT3XwSWnbutJC
Fo95+k5AYBq2uOcLP0NkyVBEXhB2WOOptHbfo4H3Ku28c9HsVTOqyf8BJHsAmJzPcU4Pw9TtAJco
I2NErAMzqZNQwasQuC+MXoV5Rb/Aeqvta+jZLtCGF5ChNVyOZf1k4ikCAwEAAaMgMB4wDwYDVR0T
AQH/BAUwAwEB/zALBgNVHQ8EBAMCBFAwDQYJKoZIhvcNAQELBQADggEBAFSoP3S5V20lJeK2jn81
F2jm3vS1kOBcl7H4oL+XuobTAZASgI1LnWctVtE/mhMn4V3GUzU+/zSOhjuImPLqvtogg+3Dg8P/
bgPe/08LRd8MZxmDdxwJORf5dBsvlYPzcFsJ64kGL0/wsa8/407+E5Pnmid9FuUP3ZWV+ikxEzrR
FC8gCICMBpaO+QnwJdnQ4+24izvj2mnc1BLAOTp9WVpZIRE8eEEUKwy6UOFq/PkKvUQ0gNDg1rSW
fOrK7lpc4hPdH0K+FhSCb9hCqArU7YLA3Fx75NBOx/lt+YS0kFelspoV9VQGWVBlsqduZdZu+nEX
J6T53izx5FCYQqBEFAY=
-----END CERTIFICATE-----
```

Private Key file

```
-----BEGIN PRIVATE KEY-----
MIIEpAIBAAKCAQEAu839A0hlW+RzVf+6ipiRZGam54X8tf3wrM0SXnraptde7bk4Y2V8qYECOWd2
KUmxnJPGI26ZnHEKNnenKzCHm3K/mgywayzD2dn1YwTwSCj7PKwgWLmEobkye5qli1vcMk1fbYVk
lGCUlShqPimi4PZNU/sLp5kJkl1pf1IFCRDWO7ys1W6G2zQ/DAU/uk9epbRncqZPdfBJadu60kIW
j3n6TkBgGra45ws/Q2TJUEReEHZY46m0dt+jgfcq7bxz0exVM6rJ/wEkewCYnM9xTg/D1O0Alygj
Y0SsAzOpk1DBqxC4L4xehXlFv8B6q+1r6Nku0IYXkKE1XI5l/WTiKQIDAQABAoIBADPEfxa1kQL4
gPZWrIPhtEoZpa4yF8vOuNXZHVQfxz+wJEGhCbMUWX2byxIpu797ydKgRUClDwC5hBgpeVIFaYvm
4HvFPaf9Ses2Elb+GIG7ki1SQQRT9wRhOVIq+bDlXFgdTc2xtFzWFn7bChYigfxCyGP60mabX7ER
wK/Q2wR5P70c1dtV8Qy8Osfb1BIUbmVcQ6ifF1oJPJzRprJjkardmrLSOq8w1SXWFrxiHFPvu+as
SihNKfeOmGSEEIkBrZS6562kU4e44EHdRlBVcueuVCkQoZNx1p9ZT7+SYR+NFFqzqdVunCxkOhF1
x8XIlnkQXEE5EAgjzq97EpJJ9RUCgYEA1yae43naOQglo2Y8b13rZbm1QOzp8ZmTQOUqLMuLEc3w
D8owpG/Hvj63WwJ98m9ApQklkGqr8NtTW/5KOZEPd2uiq/3JTzlTKcWGAsfnYunXZZmV8VTU9xoy
nE3+2tPDTHoPfUGzNeYvGh+772z9JBEt/+5eEKez+NbC3tBrcYcCgYEA33Y1T5j/3vgKH2UDnE/b
L8fsCJEBXakc0Ke/vzL+5LHnesHHA6aTX2d6Rdtli/v7Lg++oBOmSEuLX4S+/vq48B8kZkXZ+ww+
5IyPOt+NYu8iuyyTqPxMxEgNGaEU4nJuTAcSiiTW/wB4F563VjW1SG2r7gAGQPc94uaddQu4us8C
gYEAuej/ZuTGMn3duCIBmuH3JIGiI6YUSNZBrdPX67k+RHZFN/+opI6KdNPQsG2o0zJbE8Chmvbx
+EAs+dLz5GZ6jOCPQjscFr8cI34w3HpMcOZLX9mk/VImbBdRIEstK33Mxmq6s6tv5eUvUk2Vob/1
xZKqwvoAgjfHhgBdhxdloEUCgYEAzm8GWs0t+05LE6uexUVkaGSZYWSLzXmP/BYr5jb3EFBlVpsy
/eVhma4fX++JhOM4rNmwsXLVl03qqEgapbT7KiCq4KxA7lHmu/45xxqEI9Rk0mMg2eiSBnXFXFrZ
z0QFLskN0H6t+w704mUmW0bsfMeMICb3oAaHrlh7N4inlDMCgYAf3OfuXDs1nov4wYygaV45Ctka
YEhKdqMY/7yROmw2uAG1LkTGwImB7JL72jVvN29Q7MJIewENq4hCr3hMyyg8JUVBV5D0dBef+0+U
a/yRetHAxfuihoUFYpassftJYtPkPHQOEuh9c2dEb3SwGxA+wyXLCgMGgKed4jB/HGqKkQ==
-----END PRIVATE KEY-----
```
