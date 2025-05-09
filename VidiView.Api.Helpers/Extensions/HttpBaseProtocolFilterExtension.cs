#if (WINRT)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http.Filters;

namespace VidiView.Api.Helpers;

public static class HttpBaseProtocolFilterExtension
{
    private static readonly Certificate AuthorityCertificate;
    static HttpBaseProtocolFilterExtension()
    {
        AuthorityCertificate = VidiViewAuthority.Certificate2();
    }

    public static void AcceptLegacyLicenseCertificate(this HttpBaseProtocolFilter filter, bool accept)
    {
        filter.ServerCustomValidationRequested -= HttpFilter_ServerCustomValidationRequested;
        filter.IgnorableServerCertificateErrors.Remove(ChainValidationResult.Untrusted);

        if (accept)
        {
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filter.ServerCustomValidationRequested += HttpFilter_ServerCustomValidationRequested;
        }
    }

    /// <summary>
    /// Custom certificate validation to ensure our license certificate that
    /// most VidiView Servers present is trusted by the application
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private static async void HttpFilter_ServerCustomValidationRequested(HttpBaseProtocolFilter sender, HttpServerCustomValidationRequestedEventArgs args)
    {
        if (args.ServerCertificateErrorSeverity == Windows.Networking.Sockets.SocketSslErrorSeverity.None
            && !args.ServerCertificateErrors.Any((e) => e != ChainValidationResult.Success))
        {
            // The certificate is OK
            return;
        }

        var def = args.GetDeferral();

        // We should accept our own certificate as issuer, but nothing else
        bool isIssuedByLicenseAuthority = await args.ServerCertificate.IsLegacyLicenseCertificateAsync();
        isIssuedByLicenseAuthority &= args.ServerCertificate.Issuer == AuthorityCertificate.Subject;

        if (!isIssuedByLicenseAuthority)
        {
            args.Reject();
        }

        def.Complete();
    }
}
#endif