#if (WINRT)
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;

namespace Windows.Security.Cryptography.Certificates;

public static class CertificateExtension
{
    /// <summary>
    /// Convert to new certificate type
    /// </summary>
    /// <param name="certificate"></param>
    /// <returns></returns>
    public static Certificate AsWinRtCertificate(this X509Certificate2 certificate)
    {
        if (certificate == null)
        {
            return null!;
        }

        var blob = certificate.RawData;
        return new Certificate(blob.AsBuffer());
    }

    public static X509Certificate2 AsX509Certificate(this Certificate certificate)
    {
        if (certificate == null)
        {
            return null!;
        }

        var blob = certificate.GetCertificateBlob();
        return new X509Certificate2(blob.ToArray());
    }

}
#endif