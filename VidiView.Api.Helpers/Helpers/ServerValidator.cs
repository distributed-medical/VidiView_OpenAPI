using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VidiView.Api.Helpers;

/// <summary>
/// Default HttpClient handler for accepting the VidiView Server certificate
/// </summary>
public static class ServerValidator
{
    static readonly ConcurrentDictionary<string, X509Certificate2> _acceptedServerCertificates = new();

    static ServerValidator()
    {
        Authority = VidiViewAuthority.Certificate();

        TrustVidiViewAuthorityHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = Validate_Callback,
            CheckCertificateRevocationList = false,
            ClientCertificateOptions = ClientCertificateOption.Manual,
            UseDefaultCredentials = true,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.Deflate,
            UseCookies = false,
        };
    }

    public static X509Certificate2 Authority { get; }

    public static HttpClientHandler TrustVidiViewAuthorityHandler { get; }

    /// <summary>
    /// Since the VidiView Server will present a custom License certificate, we should validate
    /// that this is valid, even though we might not have the root authority cert installed
    /// </summary>
    /// <param name="requestMessage"></param>
    /// <param name="certificate"></param>
    /// <param name="chain"></param>
    /// <param name="sslErrors"></param>
    /// <returns></returns>
    static bool Validate_Callback(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
    {
        if (certificate == null || chain == null)
            return false;
        if (_acceptedServerCertificates.ContainsKey(certificate.Thumbprint))
            return true;

        try
        {
            // Verify the server certificate, and accept our own authority as
            // well as any trusted root
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
            chain.ChainPolicy.CustomTrustStore.Add(Authority); // Add our own root CA here...
            bool isValid = chain.Build(certificate);

            if (!isValid)
            {
                // Try validate using the default system store
                chain.ChainPolicy.TrustMode = X509ChainTrustMode.System;
                chain.ChainPolicy.CustomTrustStore.Clear();
                isValid = chain.Build(certificate);
            }

            if (isValid)
                _acceptedServerCertificates.TryAdd(certificate.Thumbprint, certificate);

            return isValid;
        }
        catch
        {
            return false;
        }
    }

}
