using VidiView.Api.Helpers;

namespace System.Security.Cryptography.X509Certificates;

public static class X509CertificateExtension
{
    /// <summary>
    /// Returns true if the certificate is issued by the VidiView License authority
    /// </summary>
    /// <param name="certificate"></param>
    /// <returns></returns>
    public static bool IsVidiViewLicenseCertificate(this X509Certificate2 certificate)
    {
        using var chain = new X509Chain();
        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
        chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
        chain.ChainPolicy.CustomTrustStore.Add(VidiViewAuthority.Certificate()); // Add our own root CA here...
        
        bool success = chain.Build(certificate);
        return success;
    }
}