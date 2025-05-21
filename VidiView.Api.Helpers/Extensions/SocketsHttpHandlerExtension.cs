using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using VidiView.Api.Helpers;

namespace System.Net.Http;

public static class SocketsHttpHandlerExtension
{
    public static void AcceptLegacyLicenseCertificate(this SocketsHttpHandler handler, bool accept)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(handler));

        if (accept)
        {
            handler.SslOptions.RemoteCertificateValidationCallback = RemoteCertificateValidationCallback;
        }
        else
        {
            handler.SslOptions.RemoteCertificateValidationCallback = null;
        }
    }

    private static bool RemoteCertificateValidationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            return true;
        }

        return certificate is X509Certificate2 x2
               && x2.IsLegacyLicenseCertificate();
    }
}
