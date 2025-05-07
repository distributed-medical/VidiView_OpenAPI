using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using VidiView.Api.Helpers;

namespace VidiView.Example.HttpHandlers;
public static class HttpClientHandlerFactory
{
    /// <summary>
    /// Create a new HttpClientHandler that will accept our License certificate
    /// as well as provide some default configuration appropriate for calling
    /// the VidiView Server API
    /// </summary>
    /// <returns></returns>
    public static HttpClientHandler Create(bool checkCertificateRevocation = false)
    {
        return new HttpClientHandler()
        {
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.Deflate,
            CheckCertificateRevocationList = checkCertificateRevocation,
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = Validate_Callback,
            UseCookies = false,
            UseDefaultCredentials = true,
        };
    }

    static bool Validate_Callback(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
    {
        if (certificate == null || chain == null)
            return false;

        try
        {
            // Verify the server certificate, and accept our own authority as
            // well as any trusted root
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
            chain.ChainPolicy.CustomTrustStore.Add(VidiViewAuthority.Certificate()); // Add our own root CA here...
            bool isValid = chain.Build(certificate);

            if (!isValid)
            {
                // Try validate using the default system store
                chain.ChainPolicy.TrustMode = X509ChainTrustMode.System;
                chain.ChainPolicy.CustomTrustStore.Clear();
                isValid = chain.Build(certificate);
            }

            return isValid;
        }
        catch
        {
            return false;
        }
    }
}
