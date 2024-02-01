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
are signed by the CA (RSA) and can be verified using the CA certificate.

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
Serial Number: 432F35B4FF3517A1
Signature Algorithm: sha256RSA
Not Before: 01/02/2024 19:44:57
Not After: 01/02/2025 19:44:57
Public Key Algorithm: RSA
Public Key Format: 30 82 01 0a 02 82 01 01 00 c3 5d 9c af e7 b0 61 f2 4e 3d 10 f0 82 b9 91 14 6b f8 46 e2 10 b4 c5 46 ee 57 bb 9d 19 21 47 8a 1e b8 53 09 d8 83 cc 83 95 54 95 b9 5c 4d 89 74 63 1f c0 e0 f4 88 48 66 6d fc bd 1e fc b0 b0 68 66 45 14 1e 10 28 e3 a3 35 ca bc 52 ce 5d da c7 99 87 11 94 36 c3 2c 20 68 04 3b 72 ae ce 4e 72 f3 a7 aa a2 70 a7 e2 04 ff 44 f4 0b f6 3f 87 72 67 d0 46 77 11 ef 02 c4 99 8f de 2e a8 16 04 f2 f7 ab 88 5c 60 9e 0b 19 e1 ee dc 91 86 90 b4 c3 cb 0a 9c 23 35 c5 f4 08 38 b8 28 82 cf da 05 d3 5d 51 68 e8 fe ab a6 d7 b8 29 88 ea 4f f2 63 ac 00 61 ee e9 da 90 61 93 e7 da 4b 63 59 92 fb da 87 10 df f2 1a 1c 89 9b ff aa d9 c9 32 a4 af ec 18 3e b2 a8 16 53 bb f1 fb c5 b2 79 b9 d6 c6 ad 26 b9 c7 ab e2 07 74 a1 9f 7e 15 41 ae 14 68 69 bb 2d 10 09 fd 5f e4 b7 79 c4 49 38 15 97 41 49 02 03 01 00 01
Public Key: MIIBCgKCAQEAw12cr+ewYfJOPRDwgrmRFGv4RuIQtMVG7le7nRkhR4oeuFMJ2IPMg5VUlblcTYl0Yx/A4PSISGZt/L0e/LCwaGZFFB4QKOOjNcq8Us5d2seZhxGUNsMsIGgEO3Kuzk5y86eqonCn4gT/RPQL9j+HcmfQRncR7wLEmY/eLqgWBPL3q4hcYJ4LGeHu3JGGkLTDywqcIzXF9Ag4uCiCz9oF011RaOj+q6bXuCmI6k/yY6wAYe7p2pBhk+faS2NZkvvahxDf8hociZv/qtnJMqSv7Bg+sqgWU7vx+8WyebnWxq0mucer4gd0oZ9+FUGuFGhpuy0QCf1f5Ld5xEk4FZdBSQIDAQAB
Has Private Key: False
Issuer: CN=rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, S=RS, C=BR
Subject: CN=rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, S=RS, C=BR
Thumbprint: 58B2EF49D6D1BEBA57620692D1D1CB437AFE6680
Serial Number: 432F35B4FF3517A1
```

CRT File

```
-----BEGIN CERTIFICATE-----
MIIDjDCCAnSgAwIBAgIIQy81tP81F6EwDQYJKoZIhvcNAQELBQAwdTELMAkGA1UEBhMCQlIxCzAJ
BgNVBAgTAlJTMQswCQYDVQQHEwJTTTEbMBkGA1UECxMSUm9kcmlnbyBEZXBhcnRtZW50MR0wGwYD
VQQKExRSb2RyaWdvIE9yZ2FuaXphdGlvbjEQMA4GA1UEAxMHcm9kcmlnbzAeFw0yNDAyMDEyMjQ0
NTdaFw0yNTAyMDEyMjQ0NTdaMHUxCzAJBgNVBAYTAkJSMQswCQYDVQQIEwJSUzELMAkGA1UEBxMC
U00xGzAZBgNVBAsTElJvZHJpZ28gRGVwYXJ0bWVudDEdMBsGA1UEChMUUm9kcmlnbyBPcmdhbml6
YXRpb24xEDAOBgNVBAMTB3JvZHJpZ28wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDD
XZyv57Bh8k49EPCCuZEUa/hG4hC0xUbuV7udGSFHih64UwnYg8yDlVSVuVxNiXRjH8Dg9IhIZm38
vR78sLBoZkUUHhAo46M1yrxSzl3ax5mHEZQ2wywgaAQ7cq7OTnLzp6qicKfiBP9E9Av2P4dyZ9BG
dxHvAsSZj94uqBYE8veriFxgngsZ4e7ckYaQtMPLCpwjNcX0CDi4KILP2gXTXVFo6P6rpte4KYjq
T/JjrABh7unakGGT59pLY1mS+9qHEN/yGhyJm/+q2ckypK/sGD6yqBZTu/H7xbJ5udbGrSa5x6vi
B3Shn34VQa4UaGm7LRAJ/V/kt3nESTgVl0FJAgMBAAGjIDAeMA8GA1UdEwEB/wQFMAMBAf8wCwYD
VR0PBAQDAgEGMA0GCSqGSIb3DQEBCwUAA4IBAQANHnnHYzkI0FaWamOagI9+8ht22jSjlyidAH8L
YdDtaSutPhUa3eHQovveWkh4ch1q+Qs6lhvlU7E/nKSQSGwxQy/y0pPt7IkdY5v1oEvvGQIa0wCk
14uw1y2tzJ+Fc3FdVQxDLdpViaZe6IbZyxazpznOnf50ICl18Fu+NIn4nBlvx6kYOFfbQRVXCwnd
zPlxuSkFEQFBargps8LHacdOIDXPgMDPFG+UUdddimgOWa52e7cYUVgCUTXNisIwD96+y2PRek3h
hE6KBvRsaNi9ERtrEK2XCUp6ETMnxv/5DXFcZU2EpXTyiKInCMVxMyfa1EFMINSV92E1nsKF2pnJ
-----END CERTIFICATE-----
```

Private key file

```
-----BEGIN PRIVATE KEY-----
MIIEpgIBAAKCAQEAw12cr+ewYfJOPRDwgrmRFGv4RuIQtMVG7le7nRkhR4oeuFMJ2IPMg5VUlblc
TYl0Yx/A4PSISGZt/L0e/LCwaGZFFB4QKOOjNcq8Us5d2seZhxGUNsMsIGgEO3Kuzk5y86eqonCn
4gT/RPQL9j+HcmfQRncR7wLEmY/eLqgWBPL3q4hcYJ4LGeHu3JGGkLTDywqcIzXF9Ag4uCiCz9oF
011RaOj+q6bXuCmI6k/yY6wAYe7p2pBhk+faS2NZkvvahxDf8hociZv/qtnJMqSv7Bg+sqgWU7vx
+8WyebnWxq0mucer4gd0oZ9+FUGuFGhpuy0QCf1f5Ld5xEk4FZdBSQIDAQABAoIBAQCXLmrhKN6y
fQqUcPZdgvJFJmRDng8lhIAmUtgJcvfw3250XzYAScXKkZWaI43NJrBdQKZGrpSDylgUu9kt3CE3
OOT982G38qhLKS7guXehpNIm1rCmTBRqo3Oa+V1SoxyLdtUxzRHtcsaDeoODNJhBLRrcXKoRFkhD
OTTLUCFyA6Sanw6XrVhkgwcp3XR9rHTUkRIvRZJovkmyJTrrsG9Ib+JarCF9U3PCtUkdigx8ZElf
ro1bkSSUlipipkGNC6oyvE4o+E608kC6evn5+LwhqHxO1Nscqs1cDH9NXbsp3xclUcZBNunSqpwo
Yy/LuyfU45zPnXY2+QIrjiCM6y7tAoGBANSfvrpWiEAz2y5dXGf92YVVRjQsBY2o5kx3K255RZtr
lb3DmlYK/EubL+CmFebIvo4pmbSFkZAGPkbF7XgVFIEiXKNbgCc1EqWyOqCFPbzqEg3hIAUz34VK
ASCdvl4972xnVpjUOdyQ0Zkdf6kRYJcNhGhc+ZhEdc4BHDSI4jvDAoGBAOs4janwySlMaLtQNtla
MJ164ZLa6rFhAE1cj2UmK12daCDuaY7FN71F5xZXDy8nnH7a2ks2tLnZpgHxeGyFtN/2CcmfdTyx
6UbYqlzcp/1mPYuTnTpFiIedez0FdxBekgTJDPaW3uM8G3hanNJAXJ0DyZaksxtAQEB1SA89eloD
AoGBAMday9IcZf2EYWIUaRqbuUI5VOx/xekvTlateUxIakox0iEtb9xyymD9LSO9tVVALOJQAOxw
O63FHMjwtRlc8m+tTxxfI0Yfjc9Lz4pHwz4IXUskJAicvVtUqr/xBteHzJ9gOKJsgZ/bLJoaPWLA
h0a5hsbvPZpVvfqbsd1T5oJPAoGBAMrFhdnp6H/hjqdrDt1wElivhaXDcm5W4JvQHw1jSh75grpg
zD53ZQWVLRDlHAHac94na+dK0aElLyadsuJaoQIwZ1+YRsh71k1smfBTh726VXPtXKzPJFNEPujr
vSgvXlEhwDsS9DGPr16hPm9BbJUunqlucbgcO0dV0zjQFplhAoGBAM/n6bJwsNBAqmgUyZh4G81H
3kdhADIMuT/gfOaee/1OwE4Gv+SBVg5VRAbh+JzAQCVjpFhAxqkkcqr20bLJhMvBrXgIUvh1LxIL
/Bsr9EkJ8INN3VGYI4EvU8I6+kZYICMAWuhjYC4Jx4+2CNjQA/3EX1rWZE2rcSxxNwmVl43G
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
        new X509BasicConstraintsExtension(false, false, 0, true),
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
Serial Number: 00BBEEC8927ED03E4B8A8FB936EAFF6B65
Signature Algorithm: sha256RSA
Not Before: 01/02/2024 19:45:07
Not After: 01/02/2025 19:44:57
Public Key Algorithm: RSA
Public Key Format: 30 82 01 0a 02 82 01 01 00 e8 26 75 49 97 e1 8f 8e f8 35 29 b4 85 c0 82 86 8f 0a f0 01 ff 00 14 1a 85 5b 77 37 45 e5 b4 26 fd 4b e9 31 87 bf 71 92 b9 1a 61 38 13 95 6b e6 14 d9 c0 b1 69 39 34 91 6f 34 42 51 c1 f8 5a 3a 06 8a ac 35 db a5 4d e8 62 cd a9 f6 0c d4 d8 09 fd 1d 0a 2d fa 29 5c 9b 1c aa 6b 32 e6 0e 7a 45 45 25 a1 2b 97 27 cd 8a 3e 08 a6 6e d7 3b 3d 69 94 6f 91 b1 00 34 20 ae b2 cb e0 b2 0a a1 35 93 32 6b 42 ed b0 93 03 5e c2 77 73 c4 cc c9 34 53 da df 15 13 44 14 2a 3f 29 0d 8e 8e 47 65 8e 44 18 dd c2 76 9f d2 f1 80 9d 35 d9 fa af 08 ee 20 ee 63 a6 e5 68 fb 3c ba 77 da d9 dc 48 0a 63 84 f9 e8 49 76 5a ed 5f 10 78 b0 70 9c 61 54 be c7 c5 9e 0e 1f 03 60 31 f8 53 0f ab c7 65 0a 4f 28 68 96 4e ec b6 70 f7 8f a7 ec a4 00 89 d3 84 7c 59 0a 03 bf 82 13 7e e4 8a 76 1a 89 74 a9 50 75 02 03 01 00 01
Public Key: MIIBCgKCAQEA6CZ1SZfhj474NSm0hcCCho8K8AH/ABQahVt3N0XltCb9S+kxh79xkrkaYTgTlWvmFNnAsWk5NJFvNEJRwfhaOgaKrDXbpU3oYs2p9gzU2An9HQot+ilcmxyqazLmDnpFRSWhK5cnzYo+CKZu1zs9aZRvkbEANCCussvgsgqhNZMya0LtsJMDXsJ3c8TMyTRT2t8VE0QUKj8pDY6OR2WORBjdwnaf0vGAnTXZ+q8I7iDuY6blaPs8unfa2dxICmOE+ehJdlrtXxB4sHCcYVS+x8WeDh8DYDH4Uw+rx2UKTyholk7stnD3j6fspACJ04R8WQoDv4ITfuSKdhqJdKlQdQIDAQAB
Has Private Key: False
Issuer: CN=rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, S=RS, C=BR
Subject: CN=child.rodrigo, O=Rodrigo Organization, OU=Rodrigo Department, L=SM, S=RS, C=BR
Thumbprint: CC4CA530CB62F9C384D3B06364B9DDB4C011CE0F
Serial Number: 00BBEEC8927ED03E4B8A8FB936EAFF6B65
```

CTR FILE 

```
-----BEGIN CERTIFICATE-----
MIIDmDCCAoCgAwIBAgIRALvuyJJ+0D5Lio+5Nur/a2UwDQYJKoZIhvcNAQELBQAwdTELMAkGA1UE
BhMCQlIxCzAJBgNVBAgTAlJTMQswCQYDVQQHEwJTTTEbMBkGA1UECxMSUm9kcmlnbyBEZXBhcnRt
ZW50MR0wGwYDVQQKExRSb2RyaWdvIE9yZ2FuaXphdGlvbjEQMA4GA1UEAxMHcm9kcmlnbzAeFw0y
NDAyMDEyMjQ1MDdaFw0yNTAyMDEyMjQ0NTdaMHsxCzAJBgNVBAYTAkJSMQswCQYDVQQIEwJSUzEL
MAkGA1UEBxMCU00xGzAZBgNVBAsTElJvZHJpZ28gRGVwYXJ0bWVudDEdMBsGA1UEChMUUm9kcmln
byBPcmdhbml6YXRpb24xFjAUBgNVBAMTDWNoaWxkLnJvZHJpZ28wggEiMA0GCSqGSIb3DQEBAQUA
A4IBDwAwggEKAoIBAQDoJnVJl+GPjvg1KbSFwIKGjwrwAf8AFBqFW3c3ReW0Jv1L6TGHv3GSuRph
OBOVa+YU2cCxaTk0kW80QlHB+Fo6BoqsNdulTehizan2DNTYCf0dCi36KVybHKprMuYOekVFJaEr
lyfNij4Ipm7XOz1plG+RsQA0IK6yy+CyCqE1kzJrQu2wkwNewndzxMzJNFPa3xUTRBQqPykNjo5H
ZY5EGN3Cdp/S8YCdNdn6rwjuIO5jpuVo+zy6d9rZ3EgKY4T56El2Wu1fEHiwcJxhVL7HxZ4OHwNg
MfhTD6vHZQpPKGiWTuy2cPePp+ykAInThHxZCgO/ghN+5Ip2Gol0qVB1AgMBAAGjHTAbMAwGA1Ud
EwEB/wQCMAAwCwYDVR0PBAQDAgRQMA0GCSqGSIb3DQEBCwUAA4IBAQDDKHd4jxKuf/BBATQaeOrL
JJzxUDUZBa+XXrtDdBagYyWpJAjk6aeN+l9X0RLFq2/QiCO6OQlfqtK0qwaFqvw+V1F+tOU4z+p+
DaWqPK6/LfW/zZ6uzv0+aE1qcRLkhMifNe+n00dxsAIp+Wq/jQ1vyBk71MQmJzOIeO4QIHZeqoM2
Y0FE2oOkUpC9LV5YWJXX3OW4wB4oTljSIq2nYkM4dnI2poFUPhUoGagzrJEDVJO4z4X76wFER/eE
nAwDMKLms0EGU3nr8rcKnD3hK+CyokR5kpODbZJ/yGtLzq8VWTrYE1o+18kNWLHjvMywvQbYiQdg
yjcuudjsqQDp4JOP
-----END CERTIFICATE-----
```

Private Key file

```
-----BEGIN PRIVATE KEY-----
MIIEpAIBAAKCAQEA6CZ1SZfhj474NSm0hcCCho8K8AH/ABQahVt3N0XltCb9S+kxh79xkrkaYTgT
lWvmFNnAsWk5NJFvNEJRwfhaOgaKrDXbpU3oYs2p9gzU2An9HQot+ilcmxyqazLmDnpFRSWhK5cn
zYo+CKZu1zs9aZRvkbEANCCussvgsgqhNZMya0LtsJMDXsJ3c8TMyTRT2t8VE0QUKj8pDY6OR2WO
RBjdwnaf0vGAnTXZ+q8I7iDuY6blaPs8unfa2dxICmOE+ehJdlrtXxB4sHCcYVS+x8WeDh8DYDH4
Uw+rx2UKTyholk7stnD3j6fspACJ04R8WQoDv4ITfuSKdhqJdKlQdQIDAQABAoIBAQCayImK9+k8
fmafi58BTsm4TcdNHweWVdEY+VMTV1dANn7MDj8n800WNrSP/YORLM/LNyRzJWUu14j9nvpECgKe
jZ4IFCDOppM6zUV0+DkxkfdhVVgyY0GLaiF2OAmLvZpR8DT1i2LOeE3EFrPwbV8U9hMtYIZyOxg/
vg+ipdO2k5kD+rrRQ8RoV2gTE8xtQrPFL7J5NnnEpIuEqFR+ldfzOBjOsYPgZi7tydKiSh6GsfW8
FJFbZMrCM+7f2sv+Hxts2MAEuMjIv8O60ILcXT9xS2Cy2u1nI6T5o55kL0a3BUtzs5shd2ygVml6
cosU9N2qf1+MCTbdh3S0Bbbo0cpBAoGBAPLNRiGrzNl6jjiXawTJTqanOCAB1U0L53KITXqHwrin
8BTVMHmBRb/3R8WjM1zocsaO4ufnwePM65PuCrPUKLDgcrPsRMiIiYHQ106VxZE/8DUa4thnEBbb
0cVcRtm7v5qDNKmySzVpsATZvtK5NWJDHw0EoHo1mhbNgCB1jtY/AoGBAPTE9fYJeCPSOxYF5qxX
LEFzL8VNj3mgFjBTpNrnChiUePRhuwbPyLjAOCFk6B9o2lPOVLzTTY8RXR7HZDXbzQE2Hmz3JYgZ
IA9e9vZa+oh3LbMzkLfxoItWAgw+OxdSwM+dj1kb2S9m6VGFqdOOK49DNf1Rj799+1LIacoeRHRL
AoGAAa6WW8la4+7LaWzzGtdiKhlidCZPGfeJJOv4zW5/VNQk/5/ydAUHPKsz3hShWHVrxiBbRgv+
9dsHjsDJZjGEDqWfZcuvNkxr13Bg7XRwJ+9vdI65H5KnM+FV9K4M95krXKCoDrzjdH1E8OOpRMBv
tDyOzIJJsk8IwDuwNYla8fUCgYEAv5MA54bVz0OWF1CbELxR2RAsjYJ9Dzaq5zwwRVybXE2ota1g
Up6CfYeE5y4xN5Q7fWh6jifcBmQSvCpXVogoVhBjk+mEKJdxrlYneP4QJIsS67UqSa4Gd8fjZm22
ljSg0F6Deb0AKcekSbgSCZC2qzRxp0kU903FTS+rDRTPgy0CgYBt+kkO/KNTtntzdE9oZua7BYLA
Wbqhr/9N0ZZdhyOHyu8NTlWfvRRoJJJNlM9VlQPYjfZ+n9/WsAzu2XfCRzvXgAtmdrvlbK1M6PSE
rGC12Ays2Mkz+siA0vW3bxOwBPFbaboXEWTPTMbcs4AqmWOORnszF01zvPHk62Suv+hz7Q==
-----END PRIVATE KEY-----
```
